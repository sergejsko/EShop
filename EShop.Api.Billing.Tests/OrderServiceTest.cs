using EShop.Api.Billing.Contracts.Builders;
using EShop.Api.Billing.Contracts.Factory;
using EShop.Api.Billing.Contracts.Gateway;
using EShop.Api.Billing.Contracts.Validators;
using EShop.Api.Billing.Models;
using EShop.Api.Billing.Services;
using EShop.Api.Billing.Tests.Mocks.Gateways;
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
            _paymentFactoryMock = new Mock<IPaymentFactory>(MockBehavior.Strict);
            _receiptBuilderMock = new Mock<IReceiptBuilder>(MockBehavior.Strict);
            _paymentOrderBuilder = new Mock<IPaymentOrderBuilder>(MockBehavior.Strict);
            _orderValidatorMock = new Mock<IOrderValidator>(MockBehavior.Strict);
            _orderService = new OrderService(_paymentFactoryMock.Object, _receiptBuilderMock.Object, _paymentOrderBuilder.Object, _orderValidatorMock.Object);
        }
        #endregion

        [Test]
        [TestCaseSource(nameof(ValidPaymentOrderTestCases))]
        public async Task OrderService_ProcessOrderAsync_ReturnsSuccessResult(
            Receipt receipt,
            Order order,
            Payment payment,
            bool expected)
        {
            // Arrange
            var paymentResponse = new PaymentRespose() { Code = "000", Result = "OK" };
            _orderValidatorMock.Setup(m => m.Validate(order));
            _paymentFactoryMock.Setup(m => m.CreatePaymentGateway(order.Gateway)).Returns((PaymentGatewayType type) =>
            {
                switch (type)
                {
                    case PaymentGatewayType.FirstData:
                        return new FirstDataMock(paymentResponse);
                    case PaymentGatewayType.PayPal:
                        return new PayPalMock(paymentResponse);
                }
                return null;
            });

            _paymentOrderBuilder.Setup(m => m.Build(It.IsAny<Order>(), It.IsAny<System.Collections.Specialized.NameValueCollection>())).Returns(payment);
            _receiptBuilderMock.Setup(m => m.Build(It.IsAny<Order>())).Returns(receipt);

            // Act
            var actual = await _orderService.ProcessOrderAsync(order);

            // Assert                
            Assert.AreEqual(expected, actual.IsSuccess);

            // Expected
            _orderValidatorMock.Verify(m => m.Validate(order), Times.Once);
            _paymentFactoryMock.Verify(m => m.CreatePaymentGateway(order.Gateway), Times.Once);
            _paymentOrderBuilder.Verify(m => m.Build(It.IsAny<Order>(), It.IsAny<System.Collections.Specialized.NameValueCollection>()), Times.Once);
            _receiptBuilderMock.Verify(m => m.Build(It.IsAny<Order>()), Times.Once);
        }

        [Test]
        [TestCaseSource(nameof(FailPaymentOrderTestCases))]
        public async Task OrderService_ProcessOrderAsync_ReturnsFailResult(
           Receipt receipt,
           Order order,
           Payment payment,
           bool expected)
        {
            // Arrange
            var paymentResponse = new PaymentRespose() { Code = "400", Result = "FAIL", Status = "FAIL" };
            _orderValidatorMock.Setup(m => m.Validate(order));
            _paymentFactoryMock.Setup(m => m.CreatePaymentGateway(order.Gateway)).Returns((PaymentGatewayType type) =>
            {
                switch (type)
                {
                    case PaymentGatewayType.FirstData:
                        return new FirstDataMock(paymentResponse);
                    case PaymentGatewayType.PayPal:
                        return new PayPalMock(paymentResponse);
                }
                return null;
            });
            _paymentOrderBuilder.Setup(m => m.Build(It.IsAny<Order>(), It.IsAny<System.Collections.Specialized.NameValueCollection>())).Returns(payment);
            _receiptBuilderMock.Setup(m => m.Build(It.IsAny<Order>())).Returns(receipt);

            // Act
            var actual = await _orderService.ProcessOrderAsync(order);

            // Assert                
            Assert.AreEqual(expected, actual.IsSuccess);
            Assert.AreEqual(paymentResponse.Status, actual.ErrorMessage);

            // Expected
            _orderValidatorMock.Verify(m => m.Validate(order), Times.Once);
            _paymentFactoryMock.Verify(m => m.CreatePaymentGateway(order.Gateway), Times.Once);
            _paymentOrderBuilder.Verify(m => m.Build(It.IsAny<Order>(), It.IsAny<System.Collections.Specialized.NameValueCollection>()), Times.Once);
            _receiptBuilderMock.Verify(m => m.Build(It.IsAny<Order>()), Times.Never);
        }

        [Test]
        [TestCaseSource(nameof(InvalidArgumentsTestCases))]
        public async Task OrderService_ProcessOrderAsync_InvalidArgumentsResult(
           Order order,
           string expected)
        {
            // Arrange
            _orderValidatorMock.Setup(m => m.Validate(order)).Callback((Order o) =>
            {
                if (o.Amount <= 0)
                {
                    throw new ArgumentException("Invalid amount");
                }
            });

            _paymentFactoryMock.Setup(m => m.CreatePaymentGateway(order.Gateway)).Returns(It.IsAny<IPaymentGateway>());
            _paymentOrderBuilder.Setup(m => m.Build(It.IsAny<Order>(), It.IsAny<System.Collections.Specialized.NameValueCollection>())).Returns(It.IsAny<Payment>());
            _receiptBuilderMock.Setup(m => m.Build(It.IsAny<Order>())).Returns(It.IsAny<Receipt>());

            // Act
            var actual = await _orderService.ProcessOrderAsync(order);

            // Assert
            Assert.AreEqual(expected, actual.ErrorMessage);

            // Expected
            _orderValidatorMock.Verify(m => m.Validate(order), Times.Once);
            _paymentFactoryMock.Verify(m => m.CreatePaymentGateway(order.Gateway), Times.Never);
            _paymentOrderBuilder.Verify(m => m.Build(It.IsAny<Order>(), It.IsAny<System.Collections.Specialized.NameValueCollection>()), Times.Never);
            _receiptBuilderMock.Verify(m => m.Build(It.IsAny<Order>()), Times.Never);
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
                        new Payment()
                        {
                             Amount = "12.12",
                             Description = "",
                             Metadata = new System.Collections.Specialized.NameValueCollection()
                             {
                                 { "test", "test" }
                             },
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
                        new Payment(),
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
                        new Order()
                        {
                            Amount = 0,
                            Description = "",
                            Gateway = PaymentGatewayType.FirstData,
                            OrderNumber = "Test111",
                            UserId = 123456789
                        },
                        "Invalid amount")
                };
            }
        }

        private Mock<IPaymentFactory> _paymentFactoryMock;
        private Mock<IReceiptBuilder> _receiptBuilderMock;
        private Mock<IPaymentOrderBuilder> _paymentOrderBuilder;
        private Mock<IOrderValidator> _orderValidatorMock;
        private OrderService _orderService;
    }
}