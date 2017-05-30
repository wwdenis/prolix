// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Wwa.Core.Logic;
using Wwa.Core.Extensions.Collections;
using Wwa.Core.Extensions.Reflection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Wwa.Core.Collections;
using Wwa.Core.Data;

namespace Wwa.Core.Ioc
{
    /// <summary>
    /// Application Bootstrapper
    /// </summary>
    public class ResolverManager<ResolverType> : IResolverManager
        where ResolverType : class, IResolver, new()
    {
        #region Constructors

        public ResolverManager(IResolver resolver)
        {
            Resolver = resolver ?? throw new ArgumentNullException("resolver");
        }

        public ResolverManager() : this(new ResolverType())
        {
        }

        #endregion

        #region Properties

        public IResolver Resolver { get; }
        public List<Assembly> Assemblies { get; } = new List<Assembly>();

        public IDictionary<Type, Type> Services { get; } = new WeakDictionary<Type, Type>();
        public IDictionary<Type, Type> Contexts { get; } = new WeakDictionary<Type, Type>();
        public ICollection<Type> Instances { get; } = new HashSet<Type>();
        public ICollection<Type> Factories { get; } = new HashSet<Type>();
        public ICollection<Type> Types { get; } = new HashSet<Type>();
        public IDictionary<Type, Type> Descriptors { get; } = new WeakDictionary<Type, Type>();

        #endregion

        #region Public Methods

        public void MapAssembly<AssemblyContainer>()
        {
            var assembly = typeof(AssemblyContainer).GetAssembly();
            MapAssembly(assembly);
        }

        public void MapType<AbstractType>(Assembly assembly)
        {
            var types = assembly.FindInterfaces<AbstractType>();
            Types.AddRange(types);
        }

        public void MapContext<AbstractType, ConcreteType>()
        {
            Contexts.Add(typeof(ConcreteType), typeof(AbstractType));
        }

        public void MapService<AbstractType, ConcreteType>()
        {
            Services.Add(typeof(ConcreteType), typeof(AbstractType));
        }

        /// <summary>
        /// Initializes the application
        /// </summary>
        public virtual IResolver Build()
		{
            var coreAssembly = GetType().GetAssembly();
            MapAssembly(coreAssembly);

            // Services
            RegisterServices();
            // Contexts
            RegisterContext();
            // Instances
            RegisterInstance();
            // Factories
            RegisterFactory();
            // Types
            RegisterTypes();
            // Model Descriptors
            RegisterDescriptors();
            
            // Finish the ioc container
            Resolver.Finish();

			return Resolver;
		}

        #endregion

        #region Private Methods

        void MapAssembly(Assembly assembly)
        {
            if (Assemblies.Contains(assembly))
                return;

            Assemblies.Add(assembly);

            var serviceMappings = assembly.MapTypes<IService>();
            var contextMappings = assembly.MapTypes<IContext>();
            var instanceTypes = assembly.FindInterfaces<IInstance>();
            var factoryTypes = assembly.FindInterfaces<IFactory>(true);
            var descriptorMappings = assembly.MapGenericTypes<IModelDescriptor>(true);

            Services.AddRange(serviceMappings);
            Contexts.AddRange(contextMappings);
            Instances.AddRange(instanceTypes);
            Factories.AddRange(factoryTypes);
            Descriptors.AddRange(descriptorMappings);
        }

        void RegisterServices()
        {
            foreach (var type in Services)
            {
                Resolver.Register(type.Key, type.Value, DepedencyLifetime.PerDependency);
            }
        }

        void RegisterContext()
        {
            foreach (var type in Contexts)
            {
                Resolver.Register(type.Key, type.Value, DepedencyLifetime.PerLifetime);

                // Map the generic IDbContext to make it acessible through all assemblies
                if (typeof(IDbContext).IsAssignableFrom(type.Key))
                    Resolver.Register(type.Key, typeof(IDbContext), DepedencyLifetime.PerLifetime);
            }
        }

        void RegisterInstance()
        {
            foreach (var type in Instances)
            {
                Resolver.Register(type, DepedencyLifetime.PerLifetime);
            }
        }
        
        void RegisterFactory()
		{
			foreach (var type in Factories)
            {
                IFactory factory = type.Instantiate<IFactory>();

                if (factory != null)
                {
                    Resolver.Register(factory.Type, factory.CreateInstance);
                }
			}
		}

        void RegisterTypes()
        {
            foreach (var type in Types)
            {
                Resolver.Register(type, DepedencyLifetime.PerDependency);
            }
        }

        private void RegisterDescriptors()
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
