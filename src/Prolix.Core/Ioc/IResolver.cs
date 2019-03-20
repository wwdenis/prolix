// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prolix.Core.Ioc
{
	/// <summary>
	/// Dependency resolver specification
	/// </summary>
	public interface IResolver : IDisposable
	{
		/// <summary>
		/// Registers a dependency
		/// </summary>
		/// <typeparam name="ConcreteType">The implemented type</typeparam>
		/// <typeparam name="AbstractType">The interface type</typeparam>
        /// <param name="lifetime">The lifetime type</param>
		void Register<ConcreteType, AbstractType>(DepedencyLifetime lifetime = DepedencyLifetime.PerDependency)
			where ConcreteType : class, AbstractType
			where AbstractType : class;

        /// <summary>
        /// Registers a dependency
        /// </summary>
        /// <typeparam name="ConcreteType">The implemented type</typeparam>
        /// <param name="lifetime">The lifetime type</param>
        void Register<ConcreteType>(DepedencyLifetime lifetime = DepedencyLifetime.PerDependency)
			where ConcreteType : class;

        /// <summary>
        /// Registers an instance
        /// </summary>
        /// <typeparam name="AbstractType">The interface type</typeparam>
        /// <param name="instance">The dependency instance</param>
        /// <param name="lifetime">The lifetime type</param>
        void Register<AbstractType>(AbstractType instance, DepedencyLifetime lifetime = DepedencyLifetime.PerDependency)
			where AbstractType : class;

        /// <summary>
        /// Registers a dependency through an initialiser
        /// </summary>
        /// <typeparam name="AbstractType">The interface type</typeparam>
        /// <param name="builder">The expression initialiser</param>
        void Register<AbstractType>(Func<AbstractType> builder)
			where AbstractType : class;

        /// <summary>
        /// Registers a dependency
        /// </summary>
        /// <param name="concreteType">The concrete type</param>
        /// <param name="abstractType">The interface type</param>
        /// <param name="lifetime">The lifetime type</param>
        /// <param name="name">The name for a named dependency</param>
        void Register(Type concreteType, Type abstractType, DepedencyLifetime lifetime = DepedencyLifetime.PerDependency, string name = null);

        /// <summary>
        /// Register a dependency
        /// </summary>
        /// <param name="concreteType">The concrete type</param>
        /// <param name="lifetime">The lifetime type</param>
        void Register(Type concreteType, DepedencyLifetime lifetime = DepedencyLifetime.PerDependency);

		/// <summary>
		/// Registers a dependency through an initialiser
		/// </summary>
		/// <param name="abstractType">The interface type</param>
		/// <param name="builder">The expression initialiser</param>
		void Register(Type abstractType, Func<object> builder);

		/// <summary>
		/// Resolves a dependency
		/// </summary>
		/// <typeparam name="AbstractType">The interface type</typeparam>
		/// <returns>The concrete instance</returns>
		AbstractType Resolve<AbstractType>()
			where AbstractType : class;

        /// <summary>
		/// Resolves all dependencies
		/// </summary>
		/// <typeparam name="AbstractType">The interface type</typeparam>
		/// <returns>The concrete instances</returns>
        IEnumerable<AbstractType> ResolveAll<AbstractType>()
            where AbstractType : class;

        /// <summary>
        /// Resolves a dependency
        /// </summary>
        /// <param name="abstractType">The interface type</param>
        /// <returns>The boxed instance</returns>
        object Resolve(Type abstractType);

        /// <summary>
		/// Resolves all dependencies
		/// </summary>
		/// <param name="abstractType">The interface type</param>
		/// <returns>All boxed instances</returns>
		IEnumerable<object> ResolveAll(Type abstractType);

        /// <summary>
		/// Check if a type is registered
		/// </summary>
		/// <typeparam name="AbstractType">The interface type</typeparam>
		/// <returns>TRUE is the type is registered</returns>
		bool IsRegistered<AbstractType>()
            where AbstractType : class;

        /// <summary>
		/// Check if a type is registered
		/// </summary>
		/// <param name="abstractType">The interface type</param>
		/// <returns>TRUE is the type is registered</returns>
		bool IsRegistered(Type abstractType);

        /// <summary>
        /// Finish the dependency registration
        /// </summary>
        void Finish();

        /// <summary>
        /// Creates a child container
        /// </summary>
        /// <returns>
        /// A copy of the parent container
        /// </returns>
        IResolver CreateChild();
    }
}
