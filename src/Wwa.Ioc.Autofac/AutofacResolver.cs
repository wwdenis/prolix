// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Autofac;
using Autofac.Core.Lifetime;

using System;
using System.Collections.Generic;
using System.Linq;

using Wwa.Core.Ioc;

namespace Wwa.Ioc.Autofac
{
    /// <summary>
    /// Autofac generic Ioc Container
    /// </summary>
    public class AutofacResolver : IResolver
    {
        bool _disposed;
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

        public void Register<ConcreteType>(DepedencyLifetime lifetime = DepedencyLifetime.PerDependency)
            where ConcreteType : class
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

        public void Register<AbstractType>(AbstractType instance, DepedencyLifetime lifetime = DepedencyLifetime.PerDependency)
            where AbstractType : class
        {
            switch (lifetime)
            {
                case DepedencyLifetime.PerDependency:
                    _builder.RegisterInstance(instance).InstancePerDependency();
                    break;
                case DepedencyLifetime.PerLifetime:
                    _builder.RegisterInstance(instance).InstancePerLifetimeScope();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("lifetime");
            }
        }

        public void Register<AbstractType>(Func<AbstractType> builder)
            where AbstractType : class
        {
            _builder
                .Register<AbstractType>(context => builder())
                .InstancePerLifetimeScope();
        }

        public void Register<ConcreteType, AbstractType>(DepedencyLifetime lifetime = DepedencyLifetime.PerDependency)
            where ConcreteType : class, AbstractType
            where AbstractType : class
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

        public void Register(Type concreteType, Type abstractType, DepedencyLifetime lifetime = DepedencyLifetime.PerDependency, string name = null)
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

        public void Register(Type concreteType, DepedencyLifetime lifetime = DepedencyLifetime.PerDependency)
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

        public void Register(Type abstractType, Func<object> builder)
        {
            _builder
                .Register(context => builder())
                .As(abstractType)
                .InstancePerLifetimeScope();
        }

        public AbstractType Resolve<AbstractType>()
            where AbstractType : class
        {
            if (!IsRegistered<AbstractType>())
                return null;

            return _container.Resolve<AbstractType>();
        }

        public IEnumerable<AbstractType> ResolveAll<AbstractType>()
            where AbstractType : class
        {
            if (!IsRegistered<AbstractType>())
                return Enumerable.Empty<AbstractType>();

            var enumerableType = typeof(IEnumerable<AbstractType>);
            var all = _container.Resolve(enumerableType);
            return all as IEnumerable<AbstractType>;
        }

        public object Resolve(Type abstractType)
        {
            if (!IsRegistered(abstractType))
                return null;

            return _container.Resolve(abstractType);
        }

        public IEnumerable<object> ResolveAll(Type abstractType)
        {
            var enumerableType = typeof(IEnumerable<>).MakeGenericType(abstractType);
            var all = _container.Resolve(enumerableType);
            return all as IEnumerable<object>;
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
            _container = _builder.Build();
        }

        public IResolver CreateChild()
        {
            var child = _container.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag);
            return new AutofacResolver(child);
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
