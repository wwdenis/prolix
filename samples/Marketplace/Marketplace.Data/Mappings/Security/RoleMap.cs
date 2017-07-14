// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Marketplace.Domain.Models.Security;

namespace Marketplace.Data.Mappings.Security
{
    internal sealed class RoleMap : EntityTypeConfiguration<Role>
    {
        public RoleMap()
        {
            ToTable("Roles");

            #region Key

            HasKey(i => i.Id);

            Property(i => i.Id)
                .HasColumnName("Id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(i => i.Active)
                .HasColumnName("Active");

            #endregion

            #region Fields

            Property(i => i.Name)
                .HasColumnName("Name");

            Property(i => i.IsAdmin)
                .HasColumnName("IsAdmin");

            #endregion

            #region Relationships

            HasMany(s => s.Permissions)
                .WithMany(d => d.Roles)
                .Map(cs => {
                    cs.MapLeftKey("RoleId");
                    cs.MapRightKey("FeatureId");
                    cs.ToTable("Permissions");
                });

            #endregion
        }
    }
}
