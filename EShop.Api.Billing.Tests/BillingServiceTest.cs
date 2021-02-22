using EShop.Api.Billing.Contracts.Services;
using EShop.Api.Billing.Models;
using EShop.Api.Billing.Services;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace EShop.Api.Billing.Tests
{
    [TestFixture]
    public class BillingServiceTest
    {
        #region Setup/Teardown
        [SetUp]
        public void Setup()
        {
            _orderServiceMock = new Mock<IOrderService>(MockBehavior.Strict);
            _billingService = new BillingService(_orderServiceMock.Object);
        }
        #endregion

        [Test]
        [TestCaseSource(nameof(ValidProcessOrderTestCases))]
        public async Task BillingService_ProcessOrderAsync_ReturnsSuccessResult(
            Order order,
            OrderInfo expected)
        {
            // Arrange
            var orderResult = Task<OrderInfo>.Factory.StartNew(() => expected);
            _orderServiceMock.Setup(m => m.ProcessOrderAsync(order)).Returns(orderResult);

            // Act
            var actual = await _billingService.ProcessOrderAsync(order);

            // Assert                
            Assert.AreEqual(expected.IsSuccess, actual.IsSuccess);

            // Expected
            _orderServiceMock.Verify(m => m.ProcessOrderAsync(order), Times.Once);
        }

        public static TestCaseData[] ValidProcessOrderTestCases
        {
            get
            {
                return new[]
                {
                    new TestCaseData(
                        new Order()
                        {
                            Amount = 12.12,
                            Description = "",
                            Gateway = PaymentGatewayType.FirstData,
                            OrderNumber = "Test111",
                            UserId = 123456789
                        },
                        new OrderInfo(new Receipt()
                            {
                                 OrderNumber = "1111",
                                 ReceiptId = "1111"
                            }, true, null))
                };
            }
        }

        private Mock<IOrderService> _orderServiceMock;
        private BillingService _billingService;
    }
}