using BusinessLogicLayer;
using DataAccessLayer;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace xUnitTests.IntegrationTests
{
    public class SupplierIntegrationTests : IDisposable
    {
        private readonly SupplierRepository repo = new SupplierRepository();
        private readonly SupplierService service = new SupplierService();
        private readonly List<Supplier> insertedSuppliers = new List<Supplier>();


        [Fact]
        public void AddSupplier_ValidSupplier_PersistsToDB()
        {
            var supplier = new Supplier
            {
                Name = "Lidl",
                Email = "lidl@email.com",
                Phone = "0123456789",
                Address = "Lidladdress 130",
                CreatedAt = DateTime.Now
            };

            var result = service.AddSupplier(supplier);

            Assert.True(result);
            Assert.True(supplier.Id > 0);
            insertedSuppliers.Add(supplier);
        }

        [Fact]
        public void AddSupplier_InvalidSupplier_DoesNotPersistToDB()
        {
            var invalidSupplier = new Supplier
            {
                Email = "lidl@email.com",//invalid - nnema imena
                Phone = "0123456789",
                Address = "Lidladdress 130",
                CreatedAt = DateTime.Now
            };

            var result = service.AddSupplier(invalidSupplier);

            Assert.False(result);
            Assert.Equal(0, invalidSupplier.Id);
        }

        [Fact]
        public void GetSuppliers_ReturnsSupplierFromDB()
        {
            var supplier = new Supplier
            {
                Name = "Kaufland",
                Email = "kaufland@email.com",
                Phone = "0123456789",
                Address = "Kauflandaddress 130", 
                CreatedAt = DateTime.Now
            };

            service.AddSupplier(supplier);
            insertedSuppliers.Add(supplier);

            var result = service.GetSuppliers();

            Assert.NotEmpty(result);
            Assert.Contains(result, s => s.Id == supplier.Id);
        }

        [Fact]
        public void UpdateSupplier_ValidSupplier_UpdatesInDB()
        {
            var supplier = new Supplier
            {
                Name = "Spar",
                Email = "spar@email.com",
                Phone = "0123456789",
                Address = "Sparaddress 130",
                CreatedAt = DateTime.Now
            };

            service.AddSupplier(supplier);
            insertedSuppliers.Add(supplier);

            supplier.Name = "Spar Updated";
            var result = service.UpdateSupplier(supplier);

            Assert.True(result);
            var updated = service.GetSuppliers().FirstOrDefault(s => s.Id == supplier.Id);
            Assert.Equal("Spar Updated", updated.Name);
        }

        [Fact]
        public void UpdateSupplier_InvalidSupplier_DoesNotUpdateInDB()
        {
            var supplier = new Supplier
            {
                Name = "Billa",
                Email = "billa@email.com",
                Phone = "0123456789",
                Address = "Billaaddress 130",
                CreatedAt = DateTime.Now
            };
            service.AddSupplier(supplier);
            insertedSuppliers.Add(supplier);

            var originalName = supplier.Name;
            supplier.Name = null;
            var result = service.UpdateSupplier(supplier);

            Assert.False(result);

            supplier.Name = originalName;
            var unchanged = service.GetSuppliers().FirstOrDefault(s => s.Id == supplier.Id);
            Assert.Equal(originalName, unchanged.Name);
        }

        [Fact]
        public void RemoveSupplier_WhenSupplierHasNoProducts_DeletesFromDB()
        {
            var supplier = new Supplier
            {
                Name = "Konzum",
                Email = "konzum@email.com",
                Phone = "0123456789",
                Address = "Konzumaddress 130",
                CreatedAt = DateTime.Now
            };

            service.AddSupplier(supplier);

            var result = service.RemoveSupplier(supplier);

            Assert.True(result);
            var found = service.GetSuppliers().FirstOrDefault(s => s.Id == supplier.Id);
            Assert.Null(found);
        }

        [Fact]
        public void RemoveSupplier_WhenSupplierIsNull_ReturnsFalse()
        {
            var result = service.RemoveSupplier(null);

            Assert.False(result);
        }

        [Fact]
        public void RemoveSupplier_WhenSupplierIdIsZero_ReturnsFalse()
        {
            var supplier = new Supplier
            {
                Id = 0,
                Name = "Plodine",
                Email = "plodine@email.com",
                Phone = "0123456789",
                Address = "Plodineaddress 130",
                CreatedAt = DateTime.Now
            };

            var result = service.RemoveSupplier(supplier);

            Assert.False(result);
        }

        public void Dispose()
        {
            foreach (var supplier in insertedSuppliers)
            {
                repo.Remove(supplier);
            }
            repo.Dispose();
        }
    }
}
