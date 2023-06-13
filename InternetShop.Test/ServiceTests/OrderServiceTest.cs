using AutoFixture;
using InternetShop.Core.Domain.Entities;
using InternetShop.Core.Domain.RepositoryContracts;
using InternetShop.Core.DTO;
using InternetShop.Core.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetShop.Test.ServiceTests
{
    public class OrderServiceTest
    {
        private readonly Mock<IOrderHeaderRepository> _orderHeaderRepositoryMock;
        private readonly Mock<IOrderDetailsRepository> _orderDetailsRepositoryMock;
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly OrderService _orderService;
        private readonly Fixture _fixture;

        public OrderServiceTest()
        {
            //_orderHeaderRepositoryMock = new Mock<IOrderHeaderRepository>();
            //_orderDetailsRepositoryMock = new Mock<IOrderDetailsRepository>();
            //_productRepositoryMock = new Mock<IProductRepository>();
            //_orderService = new OrderService(null, null, null, null);
            //_fixture = new Fixture();
        }

        #region AddOrder
        [Fact]
        public async Task AddOrder_ValidOrderAddRequest_ReturnsOrderResponse()
        {
            // Arrange
            var orderAddRequest = _fixture.Create<OrderAddRequest>();
            var orderHeader = _fixture.Create<OrderHeader>();
            var orderDetails = _fixture.CreateMany<OrderDetails>().ToList();
            var expectedResponse = new OrderResponse
            {
                CustomerPhoneNumber = orderHeader.CustomerPhoneNumber,
                TimeToCall = orderHeader.TimeToCall
            };

            _productRepositoryMock
                .Setup(repo => repo.GetValueById(It.IsAny<Guid>()))
                .Returns(_fixture.Create<Product>());

            _orderHeaderRepositoryMock
                .Setup(repo => repo.Create(It.IsAny<OrderHeader>()))
                .Returns(orderHeader);
            _orderHeaderRepositoryMock
                .Setup(repo => repo.GetValueById(It.IsAny<Guid>()))
                .Returns(orderHeader);
            _orderHeaderRepositoryMock
                .Setup(repo => repo.Save())
                .Returns(Task.CompletedTask);

            _orderDetailsRepositoryMock
                .Setup(repo => repo.Create(It.IsAny<OrderDetails>()))
                .Returns(orderDetails.First());
            _orderDetailsRepositoryMock
                .Setup(repo => repo.GetByOrderHeaderId(It.IsAny<Guid>()))
                .Returns(orderDetails);
            _orderDetailsRepositoryMock
                .Setup(repo => repo.Save())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _orderService.AddOrder(orderAddRequest);

            // Assert
            Assert.Equal(expectedResponse.CustomerPhoneNumber, result.CustomerPhoneNumber);
            Assert.Equal(expectedResponse.TimeToCall, result.TimeToCall);
        }
        #endregion

        #region DeleteOrder
        [Fact]
        public async Task DeleteOrder_ExistingOrderId_ReturnsTrue()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            _orderHeaderRepositoryMock
                .Setup(repo => repo.Delete(orderId))
                .Returns(true);

            _orderHeaderRepositoryMock
                .Setup(repo => repo.Save())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _orderService.DeleteOrder(orderId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteOrder_NonExistingOrderId_ReturnsFalse()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            _orderHeaderRepositoryMock
                .Setup(repo => repo.Delete(orderId))
                .Returns(false);

            // Act
            var result = await _orderService.DeleteOrder(orderId);

            // Assert
            Assert.False(result);
        }
        #endregion

        #region DeleteRangeForDates
        [Fact]
        public async Task DeleteRangeForDates_ValidDates_CallsDeleteAndSaveMethods()
        {
            // Arrange
            var startDate = DateTime.Now.AddDays(-7);
            var endDate = DateTime.Now;
            var order1 = _fixture.Build<OrderHeader>()
                .With(x => x.OrderConfirmationTime, DateTime.Now.AddDays(-4))
                .Create();
            var order2 = _fixture.Build<OrderHeader>()
                .With(x => x.OrderConfirmationTime, DateTime.Now.AddDays(-3))
                .Create();
            var order3 = _fixture.Build<OrderHeader>()
                .With(x => x.OrderConfirmationTime, DateTime.Now.AddDays(-2))
                .Create();

            var orders = new List<OrderHeader>() { order1, order2, order3 };

            _orderHeaderRepositoryMock
                .Setup(repo => repo.GetAll())
                .Returns(orders.AsQueryable());

            _orderHeaderRepositoryMock
                .Setup(repo => repo.Delete(It.IsAny<Guid>()))
                .Returns(true);
            _orderHeaderRepositoryMock
                .Setup(repo => repo.Save())
                .Returns(Task.CompletedTask);

            // Act
            await _orderService.DeleteRangeForDates(startDate, endDate);

            // Assert
            _orderHeaderRepositoryMock.Verify(repo => repo.Delete(It.IsAny<Guid>()), Times.Exactly(orders.Count));
            _orderHeaderRepositoryMock.Verify(repo => repo.Save(), Times.Once);
        }
        #endregion

        #region GetAllOrders
        [Fact]
        public async Task GetAllOrders_ReturnsAllOrderResponses()
        {
            // Arrange
            var orderHeaders = _fixture.CreateMany<OrderHeader>().ToList();
            var orderDetails = _fixture.CreateMany<OrderDetails>().ToList();
            var expectedResponses = new List<OrderResponse>();

            foreach (var orderHeader in orderHeaders)
            {
                var detailsList = orderDetails
                    .Where(d => d.OrderHeaderId == orderHeader.Id)
                    .ToList();
                var response = new OrderResponse
                {
                    // Set response properties
                };
                expectedResponses.Add(response);
            }

            _orderHeaderRepositoryMock
                .Setup(repo => repo.GetAll())
                .Returns(orderHeaders.AsQueryable());

            _orderDetailsRepositoryMock
                .Setup(repo => repo.GetByOrderHeaderId(It.IsAny<Guid>()))
                .Returns((Guid id) => orderDetails.Where(d => d.OrderHeaderId == id));

            // Act
            var result = await _orderService.GetAllOrders();

            // Assert
            Assert.Equal(expectedResponses.Count, result.Count());
            // Assert other properties of each response
        }
        #endregion

        #region GetOrderById
        [Fact]
        public async Task GetOrderById_ExistingOrderId_ReturnsOrderResponse()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var orderHeader = _fixture.Create<OrderHeader>();
            var orderDetails = _fixture.CreateMany<OrderDetails>().ToList();
            var expectedResponse = new OrderResponse
            {
                // Set expected values
                CustomerPhoneNumber = orderHeader.CustomerPhoneNumber,
                TimeToCall = orderHeader.TimeToCall
            };

            _orderHeaderRepositoryMock
                .Setup(repo => repo.GetValueById(It.IsAny<Guid>()))
                .Returns(orderHeader);

            _orderDetailsRepositoryMock
                .Setup(repo => repo.GetByOrderHeaderId(It.IsAny<Guid>()))
                .Returns(orderDetails);

            // Act
            var result = await _orderService.GetOrderById(orderId);

            // Assert
            Assert.Equal(expectedResponse.CustomerPhoneNumber, result.CustomerPhoneNumber);
            Assert.Equal(expectedResponse.TimeToCall, result.TimeToCall);
            // Assert other properties
        }
        #endregion
    }
}
