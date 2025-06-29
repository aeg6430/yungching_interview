using NUnit.Framework;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yungching.Application.Services;
using Yungching.Infrastructure.Contexts;
using Yungching.Infrastructure.IRepositories;
using Yungching.Domain.Models;
using Yungching.Application.DTOs;
using Microsoft.Extensions.Logging;
using Yungching.Infrastructure.Entities.Stores;

namespace Yungching.UnitTests.Services
{
    public class StoreServiceTests
    {
        private Mock<TransactionContext> _contextMock;
        private Mock<ILogger<StoreService>> _loggerMock = null!;
        private Mock<IStoreRepository> _storeRepositoryMock = null!;
        private StoreService _storeService = null!;

        [SetUp]
        public void Setup()
        {
            _contextMock = new Mock<TransactionContext>();
            _loggerMock = new Mock<ILogger<StoreService>>();
            _storeRepositoryMock = new Mock<IStoreRepository>();

            _storeService = new StoreService(
                _contextMock .Object,
                _loggerMock.Object,
                _storeRepositoryMock.Object
            );
        }

        [Test]
        public async Task GetAllAsync_ReturnsMappedStoreDtos()
        {
            // Arrange
            var stores = new List<StoreEntity>
            {
                new StoreEntity
                {
                    StoreId = Guid.NewGuid(),
                    Name = "Caffe A",
                    Latitude = 25.03m,
                    Longitude = 121.56m,
                    Address = "Taipei",
                    BusinessHours = "9am - 6pm"
                },
                new StoreEntity
                {
                    StoreId = Guid.NewGuid(),
                    Name = "Caffe B",
                    Latitude = 24.99m,
                    Longitude = 121.54m,
                    Address = "New Taipei",
                    BusinessHours = "10am - 7pm"
                }
            };

            _storeRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(stores);

            // Act
            var result = await _storeService.GetAllAsync();

            // Assert
            var resultList = result.ToList();
            Assert.That(resultList.Count, Is.EqualTo(2));
            Assert.That(resultList[0].Name, Is.EqualTo("Caffe A"));
            Assert.That(resultList[1].Address, Is.EqualTo("New Taipei"));
        }
    }
}
