// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Reflection;

using Prolix.Core.Collections;
using Prolix.Core.Data;
using Prolix.Core.Extensions.Collections;
using Prolix.Core.Extensions.Reflection;
using Prolix.Core.Logic;

namespace Prolix.Core.Ioc
{
    /// <summary>
    /// Dependency resolver specification
    /// </summary>
    public abstract class Resolver : IDisposable
	{
        #region Fields

        bool _disposed = false;

        #endregion

        #region Public Properties

        public List<Assembly> Assemblies { get; } = new List<Assembly>();

        public IDictionary<Type, Type> Contexts { get; } = new WeakDictionary<Type, Type>();
        public IDictionary<Type, Type> Services { get; } = new WeakDictionary<Type, Type>();
        public IDictionary<Type, Type> SharedServices { get; } = new WeakDictionary<Type, Type>();
        public ICollection<Type> Instances { get; } = new HashSet<Type>();
        public ICollection<Type> Factories { get; } = new HashSet<Type>();
        public ICollection<Type> Types { get; } = new HashSet<Type>();
        public IDictionary<Type, Type> Descriptors { get; } = new WeakDictionary<Type, Type>();

        #endregion

        #region Abstract Methods

        /// <summary>
        /// Registers a dependency
        /// </summary>
        /// <typeparam name="ConcreteType">The implemented type</typeparam>
        /// <typeparam name="AbstractType">The interface type</typeparam>
        /// <param name="lifetime">The lifetime type</param>
        public abstract void Register<ConcreteType, AbstractType>(DepedencyLifetime lifetime = DepedencyLifetime.PerDependency)
			where ConcreteType : class, AbstractType
			where AbstractType : class;

        /// <summary>
        /// Registers a dependency
        /// </summary>
        /// <typeparam name="ConcreteType">The implemented type</typeparam>
        /// <param name="lifetime">The lifetime type</param>
        public abstract void Register<ConcreteType>(DepedencyLifetime lifetime = DepedencyLifetime.PerDependency)
			where ConcreteType : class;

        /// <summary>
        /// Registers an instance
        /// </summary>
        /// <typeparam name="AbstractType">The interface type</typeparam>
        /// <param name="instance">The dependency instance</param>
        /// <param name="lifetime">The lifetime type</param>
        public abstract void Register<AbstractType>(AbstractType instance, DepedencyLifetime lifetime = DepedencyLifetime.PerDependency)
			where AbstractType : class;

        /// <summary>
        /// Registers a dependency through an initialiser
        /// </summary>
        /// <typeparam name="AbstractType">The interface type</typeparam>
        /// <param name="builder">The expression initialiser</param>
        public abstract void Register<AbstractType>(Func<AbstractType> builder)
			where AbstractType : class;

        /// <summary>
        /// Registers a dependency
        /// </summary>
        /// <param name="concreteType">The concrete type</param>
        /// <param name="abstractType">The interface type</param>
        /// <param name="lifetime">The lifetime type</param>
        /// <param name="name">The name for a named dependency</param>
        public abstract void Register(Type concreteType, Type abstractType, DepedencyLifetime lifetime = DepedencyLifetime.PerDependency, string name = null);

        /// <summary>
        /// Register a dependency
        /// </summary>
        /// <param name="concreteType">The concrete type</param>
        /// <param name="lifetime">The lifetime type</param>
        public abstract void Register(Type concreteType, DepedencyLifetime lifetime = DepedencyLifetime.PerDependency);

        /// <summary>
        /// Registers a dependency through an initialiser
        /// </summary>
        /// <param name="abstractType">The interface type</param>
        /// <param name="builder">The expression initialiser</param>
        public abstract void Register(Type abstractType, Func<object> builder);

        /// <summary>
        /// Resolves a dependency
        /// </summary>
        /// <typeparam name="AbstractType">The interface type</typeparam>
        /// <returns>The concrete instance</returns>
        public abstract AbstractType Resolve<AbstractType>()
			where AbstractType : class;

        /// <summary>
		/// Resolves all dependencies
		/// </summary>
		/// <typeparam name="AbstractType">The interface type</typeparam>
		/// <returns>The concrete instances</returns>
        public abstract IEnumerable<AbstractType> ResolveAll<AbstractType>()
            where AbstractType : class;

        /// <summary>
        /// Resolves a dependency
        /// </summary>
        /// <param name="abstractType">The interface type</param>
        /// <returns>The boxed instance</returns>
        public abstract object Resolve(Type abstractType);

        /// <summary>
		/// Resolves all dependencies
		/// </summary>
		/// <param name="abstractType">The interface type</param>
		/// <returns>All boxed instances</returns>
		public abstract IEnumerable<object> ResolveAll(Type abstractType);

        /// <summary>
		/// Check if a type is registered
		/// </summary>
		/// <typeparam name="AbstractType">The interface type</typeparam>
		/// <returns>TRUE is the type is registered</returns>
		public abstract bool IsRegistered<AbstractType>()
            where AbstractType : class;

        /// <summary>
		/// Check if a type is registered
		/// </summary>
		/// <param name="abstractType">The interface type</param>
		/// <returns>TRUE is the type is registered</returns>
		public abstract bool IsRegistered(Type abstractType);

        /// <summary>
        /// Finishes the dependency registration
        /// </summary>
        public abstract void Finish();

        /// <summary>
        /// Releases the internal container
        /// </summary>
        public abstract void Release();

        /// <summary>
        /// Creates a child container
        /// </summary>
        /// <returns>
        /// A copy of the parent container
        /// </returns>
        public abstract Resolver CreateChild();

        #endregion

        #region Public Methods

        /// <summary>
        /// Builds the container
        /// </summary>
        /// <summary>
        /// Initializes the application
        /// </summary>
        public virtual void Build()
        {
            var coreAssembly = GetType().GetAssembly();
            ScanAssembly(coreAssembly);

            // Contexts
            RegisterContext();
            // Services
            RegisterServices();
            // Shared Services
            RegisterSharedServices();
            // Instances
            RegisterInstance();
            // Factories
            RegisterFactory();
            // Types
            RegisterTypes();
            // Model Descriptors
            RegisterDescriptors();

            // Finish the ioc container
            Finish();
        }

        /// <summary>
        /// Searchs an Assembly for registrable Services
        /// </summary>
        /// <typeparam name="AssemblyContainer">The class inside the Assembly that will be part of the search.</typeparam>
        public void ScanAssembly<AssemblyContainer>()
        {
            var assembly = typeof(AssemblyContainer).GetAssembly();
            ScanAssembly(assembly);
        }

        /// <summary>
        /// Searchs an Assembly for registrable Services
        /// </summary>
        /// <param name="assembly">The Assembly that will be part of the search.</param>
        public void ScanAssembly(Assembly assembly)
        {
            if (Assemblies.Contains(assembly))
                return;

            Assemblies.Add(assembly);

            var contextMappings = assembly.MapTypes<IContext>();
            var serviceMappings = assembly.MapTypes<IService>();
            var sharedServiceMappings = assembly.MapTypes<ISharedService>();
            var instanceTypes = assembly.FindInterfaces<IInstance>();
            var factoryTypes = assembly.FindInterfaces<IFactory>(true);
            var descriptorMappings = assembly.MapGenericTypes<IModelDescriptor>(true);

            Contexts.AddRange(contextMappings);
            Services.AddRange(serviceMappings);
            SharedServices.AddRange(sharedServiceMappings);
            Instances.AddRange(instanceTypes);
            Factories.AddRange(factoryTypes);
            Descriptors.AddRange(descriptorMappings);
        }

        /// <summary>
        /// Searchs an Assembly for a specific Services
        /// </summary>
        /// <typeparam name="AbstractType">The type that will be registered.</typeparam>
        public void ScanTypes<AbstractType>(Assembly assembly)
        {
            var types = assembly.FindInterfaces<AbstractType>();
            Types.AddRange(types);
        }

        #endregion

        #region Disposing Logic

        /// <summary>
        /// Free all resources
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); // Shut FxCop up
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                    Release();

                _disposed = true;
            }
        }

        #endregion

        #region Private Methods

        void RegisterContext()
        {
            foreach (var type in Contexts)
            {
                Register(type.Key, type.Value, DepedencyLifetime.PerLifetime);

                // Map the generic IDbContext to make it acessible through all assemblies
                if (type.Key.ImplementsInterface(typeof(IDbContext)))
                    Register(type.Key, typeof(IDbContext), DepedencyLifetime.PerLifetime);
            }
        }

        void RegisterServices()
        {
            foreach (var type in Services)
            {
                Register(type.Key, type.Value, DepedencyLifetime.PerDependency);
            }
        }

        void RegisterSharedServices()
        {
            foreach (var type in SharedServices)
            {
                Register(type.Key, type.Value, DepedencyLifetime.PerDependency, type.Key.Name);
            }
        }

        void RegisterInstance()
        {
            foreach (var type in Instances)
            {
                Register(type, DepedencyLifetime.PerLifetime);
            }
        }

        void RegisterFactory()
        {
            foreach (var type in Factories)
            {
                IFactory factory = type.Instantiate<IFactory>();

                if (factory != null)
                {
                    Register(factory.Type, factory.CreateInstance);
                }
            }
        }

        void RegisterTypes()
        {
            foreach (var type in Types)
            {
                Register(type, DepedencyLifetime.PerDependency);
            }
        }

        void RegisterDescriptors()
        {
            // Original descriptor search is Descriptor/Model (to avoid key collision)
            var mappings = new WeakDictionary<Type, Type>();

            // Creating a Model/Descriptor Weak Dictionary
            foreach (var map in Descriptors)
                mappings.Add(map.Value, map.Key);

            DescriptorManager.Configure(mappings);
        }

        #endregion
    }
}
