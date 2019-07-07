// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Prolix.Extensions.Reflection
{
    public static class ReflectionExtensions
    {
        public static Type[] FindTypes<T>(this Assembly assembly)
        {
            var types = from i in assembly.DefinedTypes
                        where i.IsSubclassOf(typeof(T))
                        select i.AsType();

            return types.ToArray();
        }

        public static Type[] FindInterfaces<T>(this Assembly assembly, bool isInstantiable = false)
        {
            var types = from t in assembly?.DefinedTypes
                        where t.IsClass
                        && !t.IsAbstract
                        && t.ImplementedInterfaces.Contains(typeof(T))
                        && (!isInstantiable || t.DeclaredConstructors.Any(c => !c.GetParameters().Any()))
                        select t.AsType();

            return types?.ToArray() ?? new Type[0];
        }

        public static IDictionary<Type, Type> MapTypes<T>(this Assembly assembly)
        {
            var types = from i in assembly.FindInterfaces<T>()
                        where !i.GetTypeInfo().IsAbstract
                        select i;

            var mappings = types.ToDictionary(i => i, i => i.GetFirstInterface());
            return mappings;
        }

        public static IDictionary<Type, Type> MapGenericTypes<T>(this Assembly assembly, bool fromBase = false)
        {
            var types = from i in assembly.FindInterfaces<T>()
                        where !i.GetTypeInfo().IsAbstract
                        select i;

            var mappings = types.ToDictionary(i => i, i => i.GetFirstGenericChild(fromBase));
            return mappings;
        }

        public static MethodInfo MakeGenericMethod(this Type type, string name, params Type[] args)
        {
            var all = from i in type.GetRuntimeMethods()
                      where i.Name == name
                      && i.GetGenericArguments().Count() == args.Count()
                      select i;

            var method = all.FirstOrDefault();

            if (method == null)
                return null;

            var call = method.MakeGenericMethod(args);

            return call;
        }

        public static MethodInfo MakeGenericMethod<T>(this T instance, Expression<Func<T, object>> expression, params Type[] args)
        {
            var method = expression.GetMethod();

            if (instance == null || method == null)
                return null;

            return MakeGenericMethod(instance.GetType(), method.Name, args);
        }

        public static Type GetFirstInterface(this Type type)
        {
            if (type == null)
                return null;

            var typeInfo = type?.GetTypeInfo();
            var baseType = typeInfo?.BaseType;
            var baseInfo = baseType?.GetTypeInfo();

            var typeInterfaces = typeInfo?.ImplementedInterfaces ?? new Type[0];
            var baseInterfaces = baseInfo?.ImplementedInterfaces ?? new Type[0];

            var query = typeInterfaces.Except(baseInterfaces);
            var specificInterfaces = new List<Type>();

            foreach (var contract in query)
            {
                bool isParent = query.Any(i => i.ImplementsInterface(contract));
                if (!isParent)
                    specificInterfaces.Add(contract);
            }

            var result = specificInterfaces.FirstOrDefault();

            if (result != null)
                return result;

            return baseType.GetFirstInterface();
        }

        public static void PopulateProperties(this Type type, IDictionary<string, string> source = null)
        {
            if (type == null)
                return;

            var props = from i in type.GetRuntimeProperties()
                        where i.CanWrite
                        select i;

            foreach (var prop in props)
            {
                string value = string.Empty;

                if (source == null || !source.TryGetValue(prop.Name, out value))
                    value = prop.Name;

                prop.SetValue(null, value);
            }
        }

        public static T GetAttribute<T>(this Type type) where T : Attribute
        {
            return type?.GetTypeInfo()?.GetCustomAttribute<T>();
        }

        public static bool ContainsAttribute<T>(this Type type) where T : Attribute
        {
            return type.GetAttribute<T>() != null;
        }

        public static bool ImplementsInterface(this Type type, Type interfaceType)
        {
            return type?.GetTypeInfo()?.ImplementedInterfaces?.Contains(interfaceType) ?? false;
        }

        public static void CopyValues<T>(this T target, T source, bool ignoreNulls = false) where T : class
        {
            var props = from i in target.GetType().GetRuntimeProperties()
                        where i.CanWrite
                        select i;

            foreach (var prop in props)
            {
                var value = prop.GetValue(source);
                if (!ignoreNulls || value != null)
                    prop.SetValue(target, value);
            }
        }

        public static void CopyValues<T>(this T target, T source, IEnumerable<string> ignoredProps, bool ignoreNulls = false) where T : class
        {
            var props = from i in target.GetType().GetRuntimeProperties()
                        where i.CanWrite && !ignoredProps.Contains(i.Name)
                        select i;

            foreach (var prop in props)
            {
                var value = prop.GetValue(source);
                if (!ignoreNulls || value != null)
                    prop.SetValue(target, value);
            }
        }

        public static bool SetValue(this object obj, string propertyName, object value)
        {
            if (obj == null || !string.IsNullOrWhiteSpace(propertyName))
                return false;

            var prop = obj.GetType().GetRuntimeProperty(propertyName);

            if (prop == null || !prop.CanWrite || value.GetType() != prop.PropertyType)
                return false;

            prop.SetValue(obj, value);
            return true;
        }

        public static Assembly GetAssembly(this Type type)
        {
            return type.GetTypeInfo().Assembly;
        }

        public static object Instantiate(this Type type, params object[] args)
        {
            return Activator.CreateInstance(type, args);
        }

        public static T Instantiate<T>(this Type type, params object[] args)
            where T : class
        {
            if (type == null)
                return null;

            args = args ?? new object[0];

            return type.Instantiate(args) as T;
        }

        public static Assembly GetAssembly(this object obj)
        {
            return obj?.GetType().GetTypeInfo().Assembly;
        }

        public static string GetAssemblyVersion(this object obj, int detail = 0)
        {
            var ver = obj?.GetAssembly()?.GetName()?.Version;

            if (ver == null)
                return "0.0.0.0";

            if (detail == 0)
                return ver.ToString();

            var info = Enumerable.Range(0, detail);
            var formatted = info.Select(i => string.Format("{{{0}}}", i));
            string template = string.Join(".", formatted);

            return string.Format(template, ver.Major, ver.Minor, ver.Build);
        }

        public static PropertyInfo GetProperty<TS, TP>(this Expression<Func<TS, TP>> expression)
        {
            var body = expression.Body;
            
            if (body.NodeType == ExpressionType.Convert && body is UnaryExpression unary)
            {
                body = unary.Operand;
            }

            var member = body as MemberExpression;

            if (member?.Member is PropertyInfo prop)
                return prop;

            return null;
        }

        public static MethodInfo GetMethod(this LambdaExpression expression)
        {
            var unaryExp = expression?.Body as UnaryExpression;
            var methodCallExp = unaryExp?.Operand as MethodCallExpression;
            var methodCallObj = methodCallExp?.Object as ConstantExpression;
            var methodInfo = methodCallObj?.Value as MethodInfo;

            return methodInfo;
        }

        public static PropertyInfo[] GetAllProperties(this Type type, bool searchAll = false)
        {
            if (type == null)
                return new PropertyInfo[0];

            var info = type.GetTypeInfo();
            var baseProps = info.BaseType.GetRuntimeProperties();
            var concreteProps = type.GetRuntimeProperties();

            var props = from i in concreteProps
                        where searchAll || !baseProps.Any(c => c.Name == i.Name)
                        select i;

            return props.ToArray();
        }

        public static bool IsEmpty<T>(this T model)
        {
            if (model == null)
                return true;

            var props = model.GetType().GetAllProperties();

            foreach (var prop in props)
            {
                var value = prop.GetValue(model);

                if (prop.PropertyType == typeof(string))
                {
                    var parsed = (string)value;
                    if (!string.IsNullOrWhiteSpace(parsed))
                        return false;
                }
                else if (prop.PropertyType.GetTypeInfo().IsValueType)
                {
                    var propType = Nullable.GetUnderlyingType(prop.PropertyType);
                    var defaultValue = Activator.CreateInstance(propType);
                    if (value != null && value != defaultValue)
                        return false;
                }
                else if (value != null)
                {
                    return false;
                }
            }

            return true;
        }

        public static string[] GetPropertyNames(this Type type, bool? canRead = null, bool? canWrite = null)
        {
            var props =
                from i in type.GetRuntimeProperties()
                where (canRead == null || i.CanRead) && (canWrite == null || i.CanWrite)
                select i;

            if (!props.Any())
                return new string[0];

            var names = from i in props
                        select i.Name;

            return names.ToArray();
        }

        public static object[] GetPropertyValues<T>(this T model)
            where T : class
        {
            if (model == null)
                return new object[0];

            var type = model.GetType();

            var props =
                from i in type.GetRuntimeProperties()
                where i.CanRead
                select i;

            if (!props.Any())
                return new string[0];

            var values = from i in props
                         select i.GetValue(model);

            return values.ToArray();
        }

        public static Type GetGenericChild(this Type type, Type expectedBaseType = null)
        {
            Type compositeType = expectedBaseType == null ? type : type.GetTypeInfo().BaseType;

            Type genericType = compositeType?.GetGenericTypeDefinition();
            Type genericChild = compositeType?.GenericTypeArguments?.FirstOrDefault();

            if (genericChild != null)
            {
                if (expectedBaseType == null || expectedBaseType == genericType)
                    return genericChild;
            }

            return null;
        }

        public static Type GetFirstGenericChild(this Type type, bool fromBase = false)
        {
            Type compositeType = fromBase ? type.GetTypeInfo().BaseType : type;
            Type genericChild = compositeType?.GenericTypeArguments?.FirstOrDefault();

            return genericChild;
        }
    }
}
