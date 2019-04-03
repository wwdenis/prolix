// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Autofac;
using Autofac.Core.Lifetime;

using System;
using System.Collections.Generic;
using System.Linq;

using Prolix.Core.Ioc;

namespace Prolix.Ioc.Autofac
{
    /// <summary>
    /// Autofac generic Ioc Container
    /// </summary>
    public class AutofacResolver : Resolver
    {
        ILifetimeScope _container;
        readonly ContainerBuilder _builder;

        public AutofacResolver()
        {
            _builder = new ContainerBuilder();
        }

        AutofacResolver(ILifetimeScope container)
        {
            _container = container ?? throw new ArgumentNullException("container");
        }

        ~AutofacResolver()
        {
            Dispose(false);
        }

        public override void Register<ConcreteType>(DepedencyLifetime lifetime = DepedencyLifetime.PerDependency)
        {
            switch (lifetime)
            {
                case DepedencyLifetime.PerDependency:
                    _builder.RegisterType<ConcreteType>().InstancePerDependency();
                    break;
                case DepedencyLifetime.PerLifetime:
                    _builder.RegisterType<ConcreteType>().InstancePerLifetimeScope();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("lifetime");
            }
        }

        public override void Register<AbstractType>(AbstractType instance, DepedencyLifetime lifetime = DepedencyLifetime.PerDependency)
        {
            switch (lifetime)
            {
                case DepedencyLifetime.PerDependency:
                    _builder.RegisterInstance(instance).InstancePerDependency();
                    break;
                case DepedencyLifetime.PerLifetime:
                    _builder.RegisterInstance(instance).SingleInstance();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("lifetime");
            }
        }

        public override void Register<AbstractType>(Func<AbstractType> builder)
        {
            _builder
                .Register<AbstractType>(context => builder())
                .InstancePerLifetimeScope();
        }

        public override void Register<ConcreteType, AbstractType>(DepedencyLifetime lifetime = DepedencyLifetime.PerDependency)
        {
            switch (lifetime)
            {
                case DepedencyLifetime.PerDependency:
                    _builder.RegisterType<ConcreteType>().As<AbstractType>().InstancePerDependency();
                    break;
                case DepedencyLifetime.PerLifetime:
                    _builder.RegisterType<ConcreteType>().As<AbstractType>().InstancePerLifetimeScope();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("lifetime");
            }
        }

        public override void Register(Type concreteType, Type abstractType, DepedencyLifetime lifetime = DepedencyLifetime.PerDependency, string name = null)
        {
            switch (lifetime)
            {
                case DepedencyLifetime.PerDependency:
                    if (string.IsNullOrWhiteSpace(name))
                        _builder.RegisterType(concreteType).As(abstractType).InstancePerDependency();
                    else
                        _builder.RegisterType(concreteType).Named(name, abstractType).As(abstractType).InstancePerDependency();
                    break;
                case DepedencyLifetime.PerLifetime:
                    if (string.IsNullOrWhiteSpace(name))
                        _builder.RegisterType(concreteType).As(abstractType).InstancePerLifetimeScope();
                    else
                        _builder.RegisterType(concreteType).Named(name, abstractType).As(abstractType).InstancePerLifetimeScope();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("lifetime");
            }
        }

        public override void Register(Type concreteType, DepedencyLifetime lifetime = DepedencyLifetime.PerDependency)
        {
            switch (lifetime)
            {
                case DepedencyLifetime.PerDependency:
                    _builder.RegisterType(concreteType).InstancePerDependency();
                    break;
                case DepedencyLifetime.PerLifetime:
                    _builder.RegisterType(concreteType).InstancePerLifetimeScope();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("lifetime");
            }
        }

        public override void Register(Type abstractType, Func<object> builder)
        {
            _builder
                .Register(context => builder())
                .As(abstractType)
                .InstancePerLifetimeScope();
        }

        public override AbstractType Resolve<AbstractType>()
        {
            if (!IsRegistered<AbstractType>())
                return null;

            return _container.Resolve<AbstractType>();
        }

        public override IEnumerable<AbstractType> ResolveAll<AbstractType>()
        {
            if (!IsRegistered<AbstractType>())
                return Enumerable.Empty<AbstractType>();

            var enumerableType = typeof(IEnumerable<AbstractType>);
            var all = _container.Resolve(enumerableType);
            return all as IEnumerable<AbstractType>;
        }

        public override object Resolve(Type abstractType)
        {
            if (!IsRegistered(abstractType))
                return null;

            return _container.Resolve(abstractType);
        }

        public override IEnumerable<object> ResolveAll(Type abstractType)
        {
            var enumerableType = typeof(IEnumerable<>).MakeGenericType(abstractType);
            var all = _container.Resolve(enumerableType);
            return all as IEnumerable<object>;
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
            _container = _builder.Build();
        }

        public override void Release()
        {
            _container?.Dispose();
        }

        public override Resolver CreateChild()
        {
            var child = _container.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag);
            return new AutofacResolver(child);
        }
    }
}
