// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Marketplace.Data;
using Marketplace.Domain.Models.Configuration;
using Marketplace.Domain.Security;
using Marketplace.Logic.Contracts.Configuration;
using Marketplace.Logic.Services.Configuration;
using Marketplace.Tests.Mock;

using Prolix.Core.Logic;
using Prolix.Core.Ioc;
using Prolix.Ioc.Autofac;

namespace Marketplace.Tests
{
    [TestClass]
    public class CategoryServiceTest
    {
        Resolver _resolver = null;

        [TestInitialize]
        public void Initialize()
        {
            var resolver = new AutofacResolver();

            // Map all assemblies
            resolver.ScanAssembly<CategoryService>(); // Services
            resolver.ScanAssembly<SecurityContext>(); // Domain

            // Mocking the data context
            resolver.ScanAssembly<MockDataContext>(); // Mock: Data

            // Builds the IoC container
            resolver.Build();

            this._resolver = resolver;
        }
        
        [TestMethod]
        public void CategoryService_SaveOneCategory()
        {
            // Gets the ioc container
            var service = _resolver.Resolve<ICategoryService>();

            var item = new Category { Id = 1, Name = "Electronics" };
            service.Add(item);

            var existing = service.Get(item.Id);

            Assert.IsNotNull(existing);
            Assert.AreEqual(existing.Name, item.Name);
        }

        [TestMethod]
        public void CategoryService_SaveInvalid()
        {
            // Gets the ioc container
            var service = _resolver.Resolve<ICategoryService>();

            // Attempt to save invalida model (Name: required)
            var item = new Category { Id = 1, Name = "" };
            
            // Asserts broken rule exception
            Assert.ThrowsExceptionAsync<RuleException>(() => service.Add(item));
        }
    }
}
