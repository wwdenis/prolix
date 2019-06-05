// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

using Prolix.Core.Domain;

namespace Marketplace.Domain.Models.Security
{
    /// <summary>
    /// Audit Log
    /// </summary>
    public class AuditLog : ActiveModel, IDetailed
    {
        #region Constructors

        public AuditLog()
        {
            Date = DateTime.Now;
            Changes = new List<AuditChange>();
        }

        public AuditLog(Feature feature, User user, string detail, object entity)
        {
            if (string.IsNullOrWhiteSpace(detail))
                detail = null;

            Date = DateTime.Now;
            Feature = feature ?? throw new ArgumentOutOfRangeException(nameof(feature));
            User = user ?? throw new ArgumentNullException(nameof(user));
            Detail = detail ?? throw new ArgumentNullException(nameof(detail));
        }

        #endregion

        #region Properties

        /// <summary>
        /// User ID
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// Feature ID
        /// </summary>
        public int? FeatureId { get; set; }

        /// <summary>
        /// Audit Date
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Audit Detail
        /// </summary>
        public string Detail { get; set; }

        /// <summary>
        /// The ID of the auditable model
        /// </summary>
        public int? ModelId { get; set; }

        #endregion

        #region Relationships

        /// <summary>
        /// Audit User
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Feature
        /// </summary>
        public Feature Feature { get; set; }

        /// <summary>
        /// Audit Changes
        /// </summary>
        public virtual IList<AuditChange> Changes { get; set; }

        #endregion

        #region Overriden Methods

        public override string ToString()
        {
            return Detail;
        }

        #endregion
    }
}
