// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Prolix.Domain;

namespace Marketplace.Domain.Models.Security
{
    /// <summary>
    /// Audit Log Change
    /// </summary>
    public class AuditChange : Model
    {
        public AuditChange()
        {
        }

        public AuditChange(string fieldName, string newValue, string oldValue)
        {
            FieldName = fieldName;
            NewValue = newValue;
            OldValue = oldValue;
        }

        /// <summary>
        /// Id
        /// </summary>
        public int LogId { get; set; }
        
        /// <summary>
        /// Field Name
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// New value
        /// </summary>
        public string NewValue { get; set; }

        /// <summary>
        /// Old value
        /// </summary>
        public string OldValue { get; set; }

        /// <summary>
        /// Audito Log
        /// </summary>
        public virtual AuditLog Log { get; set; }
    }
}
