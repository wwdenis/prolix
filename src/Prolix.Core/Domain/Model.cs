// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.ComponentModel;

namespace Prolix.Core.Domain
{
    /// <summary>
    /// Model with generic Id.
    /// This is the most basic model in the architecture hierarchy.
    /// </summary>
    /// <typeparam name="KeyType">Id type</typeparam>
    public abstract class Model<KeyType> : Observable, IIdentifiable<KeyType>
        where KeyType : IComparable<KeyType>, IEquatable<KeyType>
    {
        #region Fields

        private KeyType _id;
        private bool _isDirty;

        #endregion

        #region Constructor

        public Model()
        {
            PropertyChanged += Model_PropertyChanged;
        }

        #endregion

        #region Properties

        /// <summary>
		/// The unique identifier
		/// </summary>
		public virtual KeyType Id
        {
            get { return _id; }
            set { Set(ref _id, value); }
        }

        /// <summary>
        /// Returns true if any property has changed
        /// </summary>
        public bool IsDirty() =>_isDirty;

        #endregion

        #region Private Methods

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _isDirty = true;
        }

        #endregion

        #region Overriden Methods

        /// <summary>
        /// Generic text representation
        /// </summary>
        /// <returns>Model Type and Id.</returns>
        public override string ToString()
        {
            return string.Format("{0}: {1}", GetType().Name, Id);
        }

        /// <summary>
        /// Compares two models
        /// </summary>
        /// <param name="obj">The comparing model</param>
        /// <returns>TRUE if the two models are the same type and have the same Id.</returns>
        public override bool Equals(object obj)
        {
            if (obj?.GetType() != this.GetType())
                return false;

            var entity = obj as Model<KeyType>;

            return Equals(entity);
        }

        /// <summary>
        /// Compares two models
        /// </summary>
        /// <param name="model">The comparing model</param>
        /// <returns>TRUE if the two models are the same type and have the same Id.</returns>
        public bool Equals(Model<KeyType> model)
        {
            return model?.Id?.Equals(Id) ?? false;
        }

        /// <summary>
        /// Gets the model hash code
        /// </summary>
        /// <returns>The hash code based on the Id</returns>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        #endregion

        #region Overloaded Operators

        public static bool operator ==(Model<KeyType> first, Model<KeyType> second)
        {
            if (object.ReferenceEquals(first, second))
                return true;
            
            if (((object)first == null) || ((object)second == null))
                return false;
            
            return first.Equals(second);
        }

        public static bool operator !=(Model<KeyType> first, Model<KeyType> second)
        {
            return !(first == second);
        }

        #endregion
    }

    /// <summary>
    /// Model with numeric Id.
    /// </summary>
    public abstract class Model : Model<int>, IIdentifiable
    {
    }
}
