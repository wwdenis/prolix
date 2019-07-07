// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Autofac;
using Autofac.Core.Lifetime;

using System;
using System.Collections.Generic;
using System.Linq;

using Prolix.Ioc;

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
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        ~AutofacResolver()
        {
            Dispose(false);
        }

        public override void Register<T>(DepedencyLifetime lifetime = DepedencyLifetime.PerDependency)
        {
            switch (lifetime)
            {
                case DepedencyLifetime.PerDependency:
                    _builder.RegisterType<T>().InstancePerDependency();
                    break;
                case DepedencyLifetime.PerLifetime:
                    _builder.RegisterType<T>().InstancePerLifetimeScope();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(lifetime));
            }
        }

        public override void Register<T>(T instance, DepedencyLifetime lifetime = DepedencyLifetime.PerDependency)
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
                    throw new ArgumentOutOfRangeException(nameof(lifetime));
            }
        }

        public override void Register<T>(Func<T> builder)
        {
            _builder
                .Register(context => builder())
                .InstancePerLifetimeScope();
        }

        public override void Register<TC, TA>(DepedencyLifetime lifetime = DepedencyLifetime.PerDependency)
        {
            switch (lifetime)
            {
                case DepedencyLifetime.PerDependency:
                    _builder.RegisterType<TC>().As<TA>().InstancePerDependency();
                    break;
                case DepedencyLifetime.PerLifetime:
                    _builder.RegisterType<TC>().As<TA>().InstancePerLifetimeScope();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(lifetime));
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
                    throw new ArgumentOutOfRangeException(nameof(lifetime));
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
                    throw new ArgumentOutOfRangeException(nameof(lifetime));
            }
        }

        public override void Register(Type abstractType, Func<object> builder)
        {
            _builder
                .Register(context => builder())
                .As(abstractType)
                .InstancePerLifetimeScope();
        }

        public override T Resolve<T>()
        {
            if (!IsRegistered<T>())
                return null;

            return _container.Resolve<T>();
        }

        public override IEnumerable<T> ResolveAll<T>()
        {
            if (!IsRegistered<T>())
                return Enumerable.Empty<T>();

            var enumerableType = typeof(IEnumerable<T>);
            var all = _container.Resolve(enumerableType);
            return all as IEnumerable<T>;
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

        public override bool IsRegistered<T>()
        {
            return _container.IsRegistered<T>();
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
