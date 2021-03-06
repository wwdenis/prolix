// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Marketplace.Domain.Models.Trading;

namespace Marketplace.Data.Mappings.Trading
{
    internal sealed class ProductMap : EntityTypeConfiguration<Product>
    {
        public ProductMap()
        {
            ToTable("Products");

            #region Main

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

            Property(i => i.Detail)
                .HasColumnName("Detail");

            Property(i => i.Stock)
                .HasColumnName("Stock");

            Property(i => i.Price)
                .HasColumnName("Price");

            Property(i => i.CategoryId)
                .HasColumnName("CategoryId");

            Property(i => i.DealerId)
                .HasColumnName("DealerId");

            #endregion

            #region Relationships

            HasRequired(i => i.Category)
                .WithMany(m => m.Products)
                .HasForeignKey(c => c.CategoryId);

            HasRequired(i => i.Dealer)
                .WithMany(m => m.Products)
                .HasForeignKey(c => c.DealerId);

            #endregion
        }
    }
}
