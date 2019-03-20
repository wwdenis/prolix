// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using Prolix.Core.Ioc;

namespace Prolix.Ioc.Unity
{
    /// <summary>
    /// Unity generic Ioc Container
    /// </summary>
    internal class UnityResolver : IResolver
    {
        bool _disposed;
        IUnityContainer _container;

        public UnityResolver() : this(new UnityContainer())
        {
        }

        UnityResolver(IUnityContainer container)
        {
            _container = container ?? throw new ArgumentNullException("container");
        }

        ~UnityResolver()
        {
            Dispose(false);
        }

        public void Register<ConcreteType>(DepedencyLifetime lifetime = DepedencyLifetime.PerDependency)
            where ConcreteType : class
        {
            switch (lifetime)
            {
                case DepedencyLifetime.PerLifetime:
                    _container.RegisterType<ConcreteType>(new ContainerControlledLifetimeManager());
                    break;
                default:
                    _container.RegisterType<ConcreteType>();
                    break;
            }
        }

        public void Register<AbstractType>(AbstractType instance, DepedencyLifetime lifetime = DepedencyLifetime.PerDependency)
            where AbstractType : class
        {
            switch (lifetime)
            {
                case DepedencyLifetime.PerLifetime:
                    _container.RegisterInstance(instance, new ContainerControlledLifetimeManager());
                    break;
                default:
                    _container.RegisterInstance(instance);
                    break;
            }
        }

        public void Register<AbstractType>(Func<AbstractType> builder)
            where AbstractType : class
        {
            _container.RegisterType<AbstractType>(new InjectionFactory(c => builder()));
        }

        public void Register<ConcreteType, AbstractType>(DepedencyLifetime lifetime = DepedencyLifetime.PerDependency)
            where ConcreteType : class, AbstractType
            where AbstractType : class
        {
            switch (lifetime)
            {
                case DepedencyLifetime.PerLifetime:
                    _container.RegisterType<ConcreteType>(new ContainerControlledLifetimeManager());
                    break;
                default:
                    _container.RegisterType<AbstractType, ConcreteType>();
                    break;
            }
        }

        public void Register(Type concreteType, Type abstractType, DepedencyLifetime lifetime = DepedencyLifetime.PerDependency, string name = null)
        {
            switch (lifetime)
            {
                case DepedencyLifetime.PerLifetime:
                    if (string.IsNullOrWhiteSpace(name))
                        _container.RegisterType(abstractType, concreteType, new ContainerControlledLifetimeManager());
                    else
                        _container.RegisterType(abstractType, concreteType, name, new ContainerControlledLifetimeManager());
                    break;
                default:
                    if (string.IsNullOrWhiteSpace(name))
                        _container.RegisterType(abstractType, concreteType);
                    else
                        _container.RegisterType(abstractType, concreteType, name);
                    break;
            }
        }

        public void Register(Type concreteType, DepedencyLifetime lifetime = DepedencyLifetime.PerDependency)
        {
            switch (lifetime)
            {
                case DepedencyLifetime.PerLifetime:
                    _container.RegisterType(concreteType, new ContainerControlledLifetimeManager());
                    break;
                default:
                    _container.RegisterType(concreteType);
                    break;
            }
        }

        public void Register(Type abstractType, Func<object> builder)
        {
            _container.RegisterType(abstractType, new InjectionFactory(c => builder()));
        }

        public AbstractType Resolve<AbstractType>()
            where AbstractType : class
        {
            return _container.Resolve<AbstractType>();
        }

        public IEnumerable<AbstractType> ResolveAll<AbstractType>()
            where AbstractType : class
        {
            return _container.ResolveAll<AbstractType>();
        }

        public object Resolve(Type abstractType)
        {
            return _container.Resolve(abstractType);
        }

        public IEnumerable<object> ResolveAll(Type abstractType)
        {
            return _container.ResolveAll(abstractType);
        }

        public bool IsRegistered<AbstractType>()
            where AbstractType : class
        {
            return _container.IsRegistered<AbstractType>();
        }

        public bool IsRegistered(Type abstractType)
        {
            return _container.IsRegistered(abstractType);
        }

        public void Finish()
        {
            // Do nothing
        }

        public IResolver CreateChild()
        {
            return new UnityResolver(_container);
        }

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
                    _container.Dispose();

                _disposed = true;
            }
        }
    }
}
