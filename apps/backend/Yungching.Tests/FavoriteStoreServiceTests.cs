using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yungching.Application.Contracts.FavoriteStores;
using Yungching.Application.Services;
using Yungching.Infrastructure.Contexts;
using Yungching.Infrastructure.Entities.FavoriteStores;
using Yungching.Infrastructure.Entities.Stores;
using Yungching.Infrastructure.IRepositories;

namespace Yungching.UnitTests.Services
{
    [TestFixture]
    public class FavoriteStoreServiceTests
    {
        private Mock<TransactionContext> _mockContext;
        private Mock<ILogger<FavoriteStoreService>> _mockLogger;
        private Mock<IFavoriteStoreRepository> _mockFavoriteRepo;
        private Mock<IStoreRepository> _mockStoreRepo;
        private FavoriteStoreService _service;

        [SetUp]
        public void SetUp()
        {
            _mockContext = new Mock<TransactionContext>();
            _mockLogger = new Mock<ILogger<FavoriteStoreService>>();
            _mockFavoriteRepo = new Mock<IFavoriteStoreRepository>();
            _mockStoreRepo = new Mock<IStoreRepository>();

            _mockContext.Setup(c => c.Connection).Returns(Mock.Of<IDbConnection>());
            _mockContext.Setup(c => c.Transaction).Returns(Mock.Of<IDbTransaction>());

            _service = new FavoriteStoreService(
                _mockContext.Object,
                _mockLogger.Object,
                _mockFavoriteRepo.Object,
                _mockStoreRepo.Object
            );
        }

        [Test]
        public async Task AddAsync_Should_Call_Repository_And_Commit()
        {
            var request = new AddFavoriteStoreRequest
            {
                UserId = Guid.NewGuid(),
                StoreId = Guid.NewGuid()
            };

            await _service.AddAsync(request);

            _mockContext.Verify(c => c.Begin(), Times.Once);
            _mockFavoriteRepo.Verify(r => r.AddFavoriteStoreAsync(
                It.Is<FavoriteStoreCriteria>(f => f.AppUserId == request.UserId && f.StoreId == request.StoreId),
                It.IsAny<IDbConnection>(), It.IsAny<IDbTransaction>()), Times.Once);
            _mockContext.Verify(c => c.Commit(), Times.Once);
        }

        [Test]
        public async Task RemoveAsync_Should_Call_Repository_And_Commit()
        {
            var request = new RemoveFavoriteStoreRequest
            {
                UserId = Guid.NewGuid(),
                StoreId = Guid.NewGuid()
            };

            await _service.RemoveAsync(request);

            _mockContext.Verify(c => c.Begin(), Times.Once);
            _mockFavoriteRepo.Verify(r => r.RemoveFavoriteStoreAsync(
                It.Is<FavoriteStoreCriteria>(f => f.AppUserId == request.UserId && f.StoreId == request.StoreId),
                It.IsAny<IDbConnection>(), It.IsAny<IDbTransaction>()), Times.Once);
            _mockContext.Verify(c => c.Commit(), Times.Once);
        }

        [Test]
        public async Task GetFavoriteStoresAsync_With_Specific_StoreId_Should_Return_One_Store()
        {
            var userId = Guid.NewGuid();
            var storeId = Guid.NewGuid();

            _mockFavoriteRepo.Setup(r => r.GetStoreAsync(It.IsAny<FavoriteStoreCriteria>()))
                .ReturnsAsync(new FavoriteStoreEntity { StoreId = storeId });

            _mockStoreRepo.Setup(r => r.GetByIdsAsync(It.IsAny<IEnumerable<Guid>>()))
                .ReturnsAsync(new List<StoreEntity>
                {
                    new StoreEntity
                    {
                        StoreId = storeId,
                        Name = "Test Cafe",
                        Latitude = 25.0m,
                        Longitude = 121.0m,
                        Address = "Taipei",
                        BusinessHours = "9am-5pm"
                    }
                });

            var result = await _service.GetFavoriteStoresAsync(new GetFavoriteStoreRequest
            {
                UserId = userId,
                StoreId = storeId
            });

            var store = result.Single();
            Assert.AreEqual(storeId, store.StoreId);
            Assert.AreEqual("Test Cafe", store.Name);
        }

        [Test]
        public async Task GetFavoriteStoresAsync_With_Empty_StoreId_Should_Return_All()
        {
            var userId = Guid.NewGuid();
            var storeId1 = Guid.NewGuid();
            var storeId2 = Guid.NewGuid();

            _mockFavoriteRepo.Setup(r => r.GetStoresAsync(userId))
                .ReturnsAsync(new List<FavoriteStoreEntity>
                {
                    new FavoriteStoreEntity { StoreId = storeId1 },
                    new FavoriteStoreEntity { StoreId = storeId2 }
                });

            _mockStoreRepo.Setup(r => r.GetByIdsAsync(It.IsAny<IEnumerable<Guid>>()))
                .ReturnsAsync(new List<StoreEntity>
                {
                    new StoreEntity { StoreId = storeId1, Name = "Cafe A", Latitude = 25.0m, Longitude = 121.0m, Address = "Taipei", BusinessHours = "9am-5pm" },
                    new StoreEntity { StoreId = storeId2, Name = "Cafe B", Latitude = 26.0m, Longitude = 122.0m, Address = "Tainan", BusinessHours = "10am-6pm" }
                });

            var result = await _service.GetFavoriteStoresAsync(new GetFavoriteStoreRequest
            {
                UserId = userId,
                StoreId = Guid.Empty
            });

            Assert.AreEqual(2, result.Count());
        }
    }
}
