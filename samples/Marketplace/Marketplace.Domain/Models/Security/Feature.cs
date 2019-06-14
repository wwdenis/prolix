// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System.Collections.Generic;
using Prolix.Domain;

namespace Marketplace.Domain.Models.Security
{
    public class Feature : DetailModel
    {
        /// <summary>
        /// Parent Feature ID 
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// Path of the feature (e.g. /Customer/Get) 
        /// </summary>
        public string Path { get; set; }

        public virtual Feature Parent { get; set; }

        public virtual IList<Feature> Children { get; } = new List<Feature>();

        public virtual IList<Role> Roles { get; set; } = new List<Role>();
    }
}
