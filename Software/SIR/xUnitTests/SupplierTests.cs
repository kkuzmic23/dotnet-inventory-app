using BusinessLogicLayer;
using DataAccessLayer;
using EntityLayer.Entities;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace xUnitTests
{
    public class SupplierTests
    {

        [Fact]
        public void ValidateSupplier_ValidSupplier_ReturnsSuccess()
        {
            SupplierService service = new SupplierService();

            Supplier supplier = new Supplier
            {
                Id = 1,
                Name = "Supplier name",
                Email = "supplier@email.com",
                Phone = "0123456789",
                Address = "Supplieraddress 130",
                CreatedAt = DateTime.Now
            };

            ServiceResult result = service.ValidateSupplier(supplier);

            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void ValidateSupplier_NullSupplier_ReturnsFailure()
        {
            SupplierService service = new SupplierService();
            
            ServiceResult result = service.ValidateSupplier(null);

            Assert.False(result.IsSuccessful);
            Assert.Equal("Supplier is required", result.ErrorMessage);
        }

        [Fact]
        public void ValidateSupplier_SupplierWithNullName_ReturnsFailure()
        {
            SupplierService service = new SupplierService();

            Supplier supplier = new Supplier
            {
                Id = 1,
                Name = "",
                Email = "supplier@email.com",
                Phone = "0123456789",
                Address = "Supplieraddress 130",
                CreatedAt = DateTime.Now
            };

            ServiceResult result = service.ValidateSupplier(supplier);

            Assert.False(result.IsSuccessful);
            Assert.Equal("Name is required", result.ErrorMessage);
        }


        [Fact]
        public void GetSuppliers_ReturnsAllSuppliers()
        {
            var fakeRepo = A.Fake<ISupplierCRUDRepository>();
            var service = new SupplierService(fakeRepo);

            var suppliers = new List<Supplier>
            {
                new Supplier{Id = 1, Name = "Lidl", Email = "lidl@email.com", Phone = "0123456789", Address = "lidladdress 130", CreatedAt = DateTime.Now},
                new Supplier{Id = 2, Name = "Kaufland", Email = "kaufland@email.com", Phone = "0123456789", Address = "kauflandaddress 130", CreatedAt = DateTime.Now}
            }.AsQueryable();

            A.CallTo(() => fakeRepo.GetAll()).Returns(suppliers);

            var result = service.GetSuppliers();

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void GetSuppliers_WhenNoSuppliersExist_ReturnsEmptyList()
        {
            var fakeRepo = A.Fake<ISupplierCRUDRepository>();
            var service = new SupplierService(fakeRepo);

            A.CallTo(() => fakeRepo.GetAll()).Returns(new List<Supplier>().AsQueryable());

            var result = service.GetSuppliers();

            Assert.Empty(result);
        }

        [Fact]
        public void AddSupplier_WhenValidationFails_ReturnsFalse()
        {
            var fakeRepo = A.Fake<ISupplierCRUDRepository>();
            var service = new SupplierService(fakeRepo);

            var invalidSupplier = new Supplier();

            var result = service.AddSupplier(invalidSupplier);

            Assert.False(result);
            A.CallTo(() => fakeRepo.Add(A<Supplier>._, A<bool>._)).MustNotHaveHappened();
        }

        [Fact]
        public void AddSupplier_WhenSupplierIsValid_ReturnsTrue()
        {
            var fakeRepo = A.Fake<ISupplierCRUDRepository>();
            var service = new SupplierService(fakeRepo);

            var validSupplier = new Supplier
            {
                Id = 1,
                Name = "lidl",
                Email = "lidl@email.com",
                Phone = "0123456789",
                Address = "lidladdress 130",
                CreatedAt = DateTime.Now
            };

            A.CallTo(() => fakeRepo.Add(validSupplier, true)).Returns(1);

            var result = service.AddSupplier(validSupplier);

            Assert.True(result);
            A.CallTo(() => fakeRepo.Add(validSupplier, true)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void AddSupplier_WhenRepositoryAffectsNoRows_ReturnsFalse()
        {
            var fakeRepo = A.Fake<ISupplierCRUDRepository>();
            var service = new SupplierService(fakeRepo);

            var validSupplier = new Supplier
            {
                Id = 1,
                Name = "lidl",
                Email = "lidl@email.com",
                Phone = "0123456789",
                Address = "lidladdress 130",
                CreatedAt = DateTime.Now
            };

            A.CallTo(() => fakeRepo.Add(validSupplier, true)).Returns(0);

            var result = service.AddSupplier(validSupplier);

            Assert.False(result);
        }

        [Fact]
        public void UpdateSupplier_WhenValidationFails_ReturnsFalse()
        {
            var fakeRepo = A.Fake<ISupplierCRUDRepository>();
            var service = new SupplierService(fakeRepo);

            var invalidSupplier = new Supplier
            {
                Id = 1,
                Email = "kaufland@email.com", // nema imena
                Phone = "0123456789",
                Address = "kauflandaddress 130",
                CreatedAt = DateTime.Now
            };

            var result = service.UpdateSupplier(invalidSupplier);

            Assert.False(result);
            A.CallTo(() => fakeRepo.Update(A<Supplier>._, A<bool>._)).MustNotHaveHappened();
        }

        [Fact]
        public void UpdateSupplier_WhenSupplierIsValid_ReturnsTrue()
        {
            var fakeRepo = A.Fake<ISupplierCRUDRepository>();
            var service = new SupplierService(fakeRepo);

            var validSupplier = new Supplier
            {
                Id = 1,
                Name = "lidl",
                Email = "lidl@email.com",
                Phone = "0123456789",
                Address = "lidladdress 130",
                CreatedAt = DateTime.Now
            };

            A.CallTo(() => fakeRepo.Update(validSupplier, true)).Returns(1);

            var result = service.UpdateSupplier(validSupplier);

            Assert.True(result);
            A.CallTo(() => fakeRepo.Update(validSupplier, true)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void UpdateSupplier_WhenRepositoryAffectsNoRows_ReturnsFalse()
        {
            var fakeRepo = A.Fake<ISupplierCRUDRepository>();
            var service = new SupplierService(fakeRepo);

            var validSupplier = new Supplier
            {
                Id = 1,
                Name = "lidl",
                Email = "lidl@email.com",
                Phone = "0123456789",
                Address = "lidladdress 130",
                CreatedAt = DateTime.Now
            };

            A.CallTo(() => fakeRepo.Update(validSupplier, true)).Returns(0);

            var result = service.UpdateSupplier(validSupplier);

            Assert.False(result);
        }

        [Fact]
        public void RemoveSupplier_WhenSupplierIsNull_ReturnsFalse()
        {
            var fakeRepo = A.Fake<ISupplierCRUDRepository>();
            var service = new SupplierService(fakeRepo);

            var result = service.RemoveSupplier(null);

            Assert.False(result);
            A.CallTo(() => fakeRepo.Remove(A<Supplier>._, A<bool>._)).MustNotHaveHappened();
        }

        [Fact]
        public void RemoveSupplier_WhenSupplierHasProducts_ReturnsFalse()
        {
            var fakeRepo = A.Fake<ISupplierCRUDRepository>();
            var service = new SupplierService(fakeRepo);

            var supplier = new Supplier
            {
                Id = 1,
                Name = "lidl",
                Email = "lidl@email.com",
                Phone = "0123456789",
                Address = "lidladdress 130",
                CreatedAt = DateTime.Now
            };

            A.CallTo(() => fakeRepo.GetProductCount(supplier)).Returns(3);

            var result = service.RemoveSupplier(supplier);

            Assert.False(result);
            A.CallTo(() => fakeRepo.Remove(A<Supplier>._, A<bool>._)).MustNotHaveHappened();
        }

        [Fact]
        public void RemoveSupplier_WhenSupplierIdIsZero_ReturnsFalse()
        {
            var fakeRepo = A.Fake<ISupplierCRUDRepository>();
            var service = new SupplierService(fakeRepo);

            var supplier = new Supplier
            {
                Id = 0,
                Name = "lidl",
                Email = "lidl@email.com",
                Phone = "0123456789",
                Address = "lidladdress 130",
                CreatedAt = DateTime.Now
            };

            var result = service.RemoveSupplier(supplier);

            Assert.False(result);
            A.CallTo(() => fakeRepo.Remove(A<Supplier>._, A<bool>._)).MustNotHaveHappened();
        }

        [Fact]
        public void RemoveSupplier_WhenSupplierHasNoProducts_ReturnsTrue()
        {
            var fakeRepo = A.Fake<ISupplierCRUDRepository>();
            var service = new SupplierService(fakeRepo);

            var supplier = new Supplier
            {
                Id = 1,
                Name = "lidl",
                Email = "lidl@email.com",
                Phone = "0123456789",
                Address = "lidladdress 130",
                CreatedAt = DateTime.Now
            };

            A.CallTo(() => fakeRepo.GetProductCount(supplier)).Returns(0);
            A.CallTo(() => fakeRepo.Remove(supplier, true)).Returns(1);

            var result = service.RemoveSupplier(supplier);

            Assert.True(result);
            A.CallTo(() => fakeRepo.Remove(supplier, true)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void RemoveSupplier_WhenRepositoryAffectsNoRows_ReturnsFalse()
        {
            var fakeRepo = A.Fake<ISupplierCRUDRepository>();
            var service = new SupplierService(fakeRepo);

            var supplier = new Supplier
            {
                Id = 1,
                Name = "lidl",
                Email = "lidl@email.com",
                Phone = "0123456789",
                Address = "lidladdress 130",
                CreatedAt = DateTime.Now
            };

            A.CallTo(() => fakeRepo.GetProductCount(supplier)).Returns(0);
            A.CallTo(() => fakeRepo.Remove(supplier, true)).Returns(0);

            var result = service.RemoveSupplier(supplier);

            Assert.False(result);
        }
    }
}
