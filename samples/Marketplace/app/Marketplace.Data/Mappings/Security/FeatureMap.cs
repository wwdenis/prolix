// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Marketplace.Domain.Models.Security;

namespace Marketplace.Data.Mappings.Security
{
    internal sealed class FeatureMap : EntityTypeConfiguration<Feature>
    {
        public FeatureMap()
        {
            ToTable("Features");

            #region Key

            HasKey(i => i.Id);

            Property(i => i.Id)
                .HasColumnName("Id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            #endregion

            #region Fields

            Property(i => i.Name)
                .HasColumnName("Name");

            Property(i => i.Detail)
                .HasColumnName("Detail");

            Property(i => i.Path)
                .HasColumnName("Path");

            Property(i => i.ParentId)
                .HasColumnName("ParentId");

            #endregion

            #region Relationships

            HasOptional(s => s.Parent)
                .WithMany(d => d.Children)
                .HasForeignKey(i => i.ParentId);

            #endregion
        }
    }
}
