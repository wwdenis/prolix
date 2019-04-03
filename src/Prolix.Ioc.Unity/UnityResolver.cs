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
    public sealed class UnityResolver : Resolver
    {
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

        public override void Register<ConcreteType>(DepedencyLifetime lifetime = DepedencyLifetime.PerDependency)
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

        public override void Register<AbstractType>(AbstractType instance, DepedencyLifetime lifetime = DepedencyLifetime.PerDependency)
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

        public override void Register<AbstractType>(Func<AbstractType> builder)
        {
            _container.RegisterType<AbstractType>(new InjectionFactory(c => builder()));
        }

        public override void Register<ConcreteType, AbstractType>(DepedencyLifetime lifetime = DepedencyLifetime.PerDependency)
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

        public override void Register(Type concreteType, Type abstractType, DepedencyLifetime lifetime = DepedencyLifetime.PerDependency, string name = null)
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

        public override void Register(Type concreteType, DepedencyLifetime lifetime = DepedencyLifetime.PerDependency)
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

        public override void Register(Type abstractType, Func<object> builder)
        {
            _container.RegisterType(abstractType, new InjectionFactory(c => builder()));
        }

        public override AbstractType Resolve<AbstractType>()
        {
            return _container.Resolve<AbstractType>();
        }

        public override IEnumerable<AbstractType> ResolveAll<AbstractType>()
        {
            return _container.ResolveAll<AbstractType>();
        }

        public override object Resolve(Type abstractType)
        {
            return _container.Resolve(abstractType);
        }

        public override IEnumerable<object> ResolveAll(Type abstractType)
        {
            return _container.ResolveAll(abstractType);
        }

        public override bool IsRegistered<AbstractType>()
        {
            return _container.IsRegistered<AbstractType>();
        }

        public override bool IsRegistered(Type abstractType)
        {
            return _container.IsRegistered(abstractType);
        }

        public override void Finish()
        {
            // Do nothing
        }

        public override void Release()
        {
            _container?.Dispose();
        }

        public override Resolver CreateChild()
        {
            return new UnityResolver(_container);
        }
    }
}
