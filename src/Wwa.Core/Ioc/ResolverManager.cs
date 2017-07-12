// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Reflection;

using Wwa.Core.Collections;
using Wwa.Core.Data;
using Wwa.Core.Extensions.Collections;
using Wwa.Core.Extensions.Reflection;
using Wwa.Core.Logic;

namespace Wwa.Core.Ioc
{
    /// <summary>
    /// Application Bootstrapper
    /// </summary>
    public abstract class ResolverManager : IResolverManager
    {
        #region Constructors

        public ResolverManager(IResolver resolver)
        {
            Resolver = resolver ?? throw new ArgumentNullException("resolver");
        }
        
        #endregion

        #region Properties

        public IResolver Resolver { get; }
        public List<Assembly> Assemblies { get; } = new List<Assembly>();

        public IDictionary<Type, Type> Contexts { get; } = new WeakDictionary<Type, Type>();
        public IDictionary<Type, Type> Services { get; } = new WeakDictionary<Type, Type>();
        public IDictionary<Type, Type> SharedServices { get; } = new WeakDictionary<Type, Type>();
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

        /// <summary>
        /// Initializes the application
        /// </summary>
        public virtual IResolver Build()
		{
            var coreAssembly = GetType().GetAssembly();
            MapAssembly(coreAssembly);

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

        void RegisterContext()
        {
            foreach (var type in Contexts)
            {
                Resolver.Register(type.Key, type.Value, DepedencyLifetime.PerLifetime);

                // Map the generic IDbContext to make it acessible through all assemblies
                if (type.Key.ImplementsInterface(typeof(IDbContext)))
                    Resolver.Register(type.Key, typeof(IDbContext), DepedencyLifetime.PerLifetime);
            }
        }

        void RegisterServices()
        {
            foreach (var type in Services)
            {
                Resolver.Register(type.Key, type.Value, DepedencyLifetime.PerDependency);
            }
        }

        void RegisterSharedServices()
        {
            foreach (var type in SharedServices)
            {
                Resolver.Register(type.Key, type.Value, DepedencyLifetime.PerDependency, type.Key.Name);
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
