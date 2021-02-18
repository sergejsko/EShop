using EShop.Api.Billing.Contracts.Builders;
using EShop.Api.Billing.Contracts.Services;
using EShop.Api.Billing.Contracts.Validators;
using EShop.Api.Billing.Models;
using EShop.Api.Billing.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace EShop.Api.Billing.Tests
{
    [TestFixture]
    public class OrderServiceTest
    {
        #region Setup/Teardown
        [SetUp]
        public void Setup()
        {
            _paymentServiceMock = new Mock<IPaymentService>(MockBehavior.Strict);
            _receiptBuilderMock = new Mock<IReceiptBuilder>(MockBehavior.Strict);
            _paymentOrderBuilderMock = new Mock<IPaymentOrderBuilder>(MockBehavior.Strict);
            _orderValidatorMock = new Mock<IOrderValidator>(MockBehavior.Strict);
            _orderService = new OrderService(_paymentServiceMock.Object, _receiptBuilderMock.Object, _paymentOrderBuilderMock.Object, _orderValidatorMock.Object);
        }
        #endregion

        [Test]
        [TestCaseSource(nameof(ValidPaymentOrderTestCases))]
        public async Task OrderService_ProcessOrderAsync_ReturnsSuccessResult(
            Receipt receipt,
            Order order,
            PaymentOrder paymentOrder,
            bool expected)
        {
            // Arrange
            var paymentResponse = Task<PaymentRespose>.Factory.StartNew(() => new PaymentRespose() { Result = "OK", Code = "000" });
            _orderValidatorMock.Setup(m => m.Validate(order));
            _paymentServiceMock.Setup(m => m.MakePaymentAsync(
                It.IsAny<PaymentGatewayType>(),
                It.IsAny<System.Collections.Specialized.NameValueCollection>())).Returns(paymentResponse);
            _receiptBuilderMock.Setup(m => m.Build(It.IsAny<Order>())).Returns(receipt);
            _paymentOrderBuilderMock.Setup(m => m.Build(It.IsAny<Order>())).Returns(paymentOrder);

            // Act
            var actual = await _orderService.ProcessOrderAsync(order);

            // Assert                
            Assert.AreEqual(expected, actual.IsSuccess);

            // Expected
            _orderValidatorMock.Verify(m => m.Validate(order), Times.Once);
            _paymentServiceMock.Verify(m => m.MakePaymentAsync(
                It.IsAny<PaymentGatewayType>(),
                It.IsAny<System.Collections.Specialized.NameValueCollection>()), Times.Once);
            _receiptBuilderMock.Verify(m => m.Build(It.IsAny<Order>()), Times.Once);
            _paymentOrderBuilderMock.Verify(m => m.Build(It.IsAny<Order>()), Times.Once);
        }

        [Test]
        [TestCaseSource(nameof(FailPaymentOrderTestCases))]
        public async Task OrderService_ProcessOrderAsync_ReturnsFailResult(
           Receipt receipt,
           Order order,
           PaymentOrder paymentOrder,
           bool expected)
        {
            // Arrange
            var paymentResponse = Task<PaymentRespose>.Factory.StartNew(() => new PaymentRespose() { Code = "400", Result = "FAIL" });
            _orderValidatorMock.Setup(m => m.Validate(order));
            _paymentServiceMock.Setup(m => m.MakePaymentAsync(
               It.IsAny<PaymentGatewayType>(),
               It.IsAny<System.Collections.Specialized.NameValueCollection>())).Returns(paymentResponse);
            _receiptBuilderMock.Setup(m => m.Build(It.IsAny<Order>())).Returns(receipt);
            _paymentOrderBuilderMock.Setup(m => m.Build(It.IsAny<Order>())).Returns(paymentOrder);

            // Act
            var actual = await _orderService.ProcessOrderAsync(order);

            // Assert                
            Assert.AreEqual(expected, actual.IsSuccess);

            // Expected
            _orderValidatorMock.Verify(m => m.Validate(order), Times.Once);
            _paymentServiceMock.Verify(m => m.MakePaymentAsync(
                It.IsAny<PaymentGatewayType>(),
                It.IsAny<System.Collections.Specialized.NameValueCollection>()), Times.Once);
            _receiptBuilderMock.Verify(m => m.Build(It.IsAny<Order>()), Times.Never);
            _paymentOrderBuilderMock.Verify(m => m.Build(It.IsAny<Order>()), Times.Once);
        }

        [Test]
        [TestCaseSource(nameof(InvalidArgumentsTestCases))]
        public async Task OrderService_ProcessOrderAsync_InvalidArgumentsResult(
           Receipt receipt,
           Order order,
           PaymentOrder paymentOrder,
           string expected)
        {
            // Arrange
            var paymentResponse = Task<PaymentRespose>.Factory.StartNew(() => new PaymentRespose());
            _orderValidatorMock.Setup(m => m.Validate(order)).Callback((Order o) =>
            {
                if (o.Amount <= 0)
                {
                    throw new ArgumentException("Invalid amount");
                }
            });

            _paymentServiceMock.Setup(m => m.MakePaymentAsync(
              It.IsAny<PaymentGatewayType>(),
              It.IsAny<System.Collections.Specialized.NameValueCollection>())).Returns(paymentResponse);
            _receiptBuilderMock.Setup(m => m.Build(It.IsAny<Order>())).Returns(receipt);
            _paymentOrderBuilderMock.Setup(m => m.Build(It.IsAny<Order>())).Returns(paymentOrder);

            // Act
            var actual = await _orderService.ProcessOrderAsync(order);

            // Assert
            Assert.AreEqual(expected, actual.ErrorMessage);

            // Expected
            _orderValidatorMock.Verify(m => m.Validate(order), Times.Once);
            _paymentServiceMock.Verify(m => m.MakePaymentAsync(
                It.IsAny<PaymentGatewayType>(),
                It.IsAny<System.Collections.Specialized.NameValueCollection>()), Times.Never);
            _receiptBuilderMock.Verify(m => m.Build(It.IsAny<Order>()), Times.Never);
            _paymentOrderBuilderMock.Verify(m => m.Build(It.IsAny<Order>()), Times.Never);
        }

        public static TestCaseData[] ValidPaymentOrderTestCases
        {
            get
            {
                return new[]
                {
                    new TestCaseData(
                        new Receipt()
                        {
                             OrderNumber = "Test111",
                             ReceiptId = "R123"
                        },
                        new Order()
                        {
                            Amount = 12.12,
                            Description = "",
                            Gateway = PaymentGatewayType.FirstData,
                            OrderNumber = "Test111",
                            UserId = 123456789
                        },
                        new PaymentOrder()
                        {
                            Amount = "1212",
                            Description = "",
                            Gateway = PaymentGatewayType.FirstData,
                            OrderNumber = "Test111"
                        },
                        true)
                };
            }
        }

        public static TestCaseData[] FailPaymentOrderTestCases
        {
            get
            {
                return new[]
                {
                    new TestCaseData(
                        new Receipt()
                        {
                             OrderNumber = "",
                             ReceiptId = ""
                        },
                        new Order()
                        {
                            Amount = 12.12,
                            Description = "",
                            Gateway = PaymentGatewayType.FirstData,
                            OrderNumber = "Test111",
                            UserId = 123456789
                        },
                        new PaymentOrder()
                        {
                            Amount = "",
                            Description = "",
                            Gateway = PaymentGatewayType.FirstData,
                            OrderNumber = ""
                        },
                        false)
                };
            }
        }


        public static TestCaseData[] InvalidArgumentsTestCases
        {
            get
            {
                return new[]
                {
                    new TestCaseData(
                        new Receipt()
                        {
                             OrderNumber = "",
                             ReceiptId = ""
                        },
                        new Order()
                        {
                            Amount = 0,
                            Description = "",
                            Gateway = PaymentGatewayType.FirstData,
                            OrderNumber = "Test111",
                            UserId = 123456789
                        },
                        new PaymentOrder()
                        {
                            Amount = "",
                            Description = "",
                            Gateway = PaymentGatewayType.FirstData,
                            OrderNumber = ""
                        },
                        "Invalid amount")
                };
            }
        }

        private Mock<IPaymentService> _paymentServiceMock;
        private Mock<IReceiptBuilder> _receiptBuilderMock;
        private Mock<IPaymentOrderBuilder> _paymentOrderBuilderMock;
        private Mock<IOrderValidator> _orderValidatorMock;
        private OrderService _orderService;
    }
}