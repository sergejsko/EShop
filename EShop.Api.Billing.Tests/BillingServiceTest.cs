using EShop.Api.Billing.Contracts.Builders;
using EShop.Api.Billing.Contracts.Providers;
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
    public class BillingServiceTest
    {
        #region Setup/Teardown
        [SetUp]
        public void Setup()
        {
            _paymentProviderMock = new Mock<IPaymentProvider>(MockBehavior.Strict);
            _receiptBuilderMock = new Mock<IReceiptBuilder>(MockBehavior.Strict);
            _paymentOrderBuilderMock = new Mock<IPaymentOrderBuilder>(MockBehavior.Strict);
            _orderValidatorMock = new Mock<IOrderValidator>(MockBehavior.Strict);
            _billingService = new BillingService(_paymentProviderMock.Object, _receiptBuilderMock.Object, _paymentOrderBuilderMock.Object, _orderValidatorMock.Object);
        }
        #endregion

        [Test]
        [TestCaseSource(nameof(ValidPaymentTestCases))]
        public async Task BillingService_ProcessOrderAsync_ReturnsSuccessResult(
            PaymentResult paymentResultValue, 
            Receipt receipt, 
            Order order, 
            PaymentOrder paymentOrder, 
            bool expected)
        {
            // Arrange
            var paymentResult = Task<PaymentResult>.Factory.StartNew(() => paymentResultValue);
            _orderValidatorMock.Setup(m => m.Validate(order));
            _paymentProviderMock.Setup(m => m.GetOrderPaymentInfoAsync(It.IsAny<PaymentOrder>())).Returns(paymentResult);
            _receiptBuilderMock.Setup(m => m.Build(It.IsAny<Order>())).Returns(receipt);
            _paymentOrderBuilderMock.Setup(m => m.Build(It.IsAny<Order>())).Returns(paymentOrder);

            // Act
            var actual = await _billingService.ProcessOrderAsync(order);

            // Assert                
            Assert.AreEqual(expected, actual.IsSuccess);

            // Expected
            _orderValidatorMock.Verify(m => m.Validate(order), Times.Once);
            _paymentProviderMock.Verify(m => m.GetOrderPaymentInfoAsync(It.IsAny<PaymentOrder>()), Times.Once);
            _receiptBuilderMock.Verify(m => m.Build(It.IsAny<Order>()), Times.Once);
            _paymentOrderBuilderMock.Verify(m => m.Build(It.IsAny<Order>()), Times.Once);
        }

        [Test]
        [TestCaseSource(nameof(FailPaymentTestCases))]
        public async Task BillingService_ProcessOrderAsync_ReturnsFailResult(
           PaymentResult paymentResultValue,
           Receipt receipt,
           Order order,
           PaymentOrder paymentOrder,
           bool expected)
        {
            // Arrange
            var paymentResult = Task<PaymentResult>.Factory.StartNew(() => paymentResultValue);
            _orderValidatorMock.Setup(m => m.Validate(order));
            _paymentProviderMock.Setup(m => m.GetOrderPaymentInfoAsync(It.IsAny<PaymentOrder>())).Returns(paymentResult);
            _receiptBuilderMock.Setup(m => m.Build(It.IsAny<Order>())).Returns(receipt);
            _paymentOrderBuilderMock.Setup(m => m.Build(It.IsAny<Order>())).Returns(paymentOrder);

            // Act
            var actual = await _billingService.ProcessOrderAsync(order);

            // Assert                
            Assert.AreEqual(expected, actual.IsSuccess);

            // Expected
            _orderValidatorMock.Verify(m => m.Validate(order), Times.Once);
            _paymentProviderMock.Verify(m => m.GetOrderPaymentInfoAsync(It.IsAny<PaymentOrder>()), Times.Once);
            _receiptBuilderMock.Verify(m => m.Build(It.IsAny<Order>()), Times.Never);
            _paymentOrderBuilderMock.Verify(m => m.Build(It.IsAny<Order>()), Times.Once);
        }

        [Test]
        [TestCaseSource(nameof(InvalidArgumentsTestCases))]
        public async Task BillingService_ProcessOrderAsync_InvalidArgumentsResult(
           Type exceptionType,
           PaymentResult paymentResultValue,
           Receipt receipt,
           Order order,
           PaymentOrder paymentOrder,
           string expected)
        {
            // Arrange
            var paymentResult = Task<PaymentResult>.Factory.StartNew(() => paymentResultValue);
            _orderValidatorMock.Setup(m => m.Validate(order)).Callback((Order o) =>
            {
                if (o.Amount <= 0)
                {
                    throw new ArgumentException("Invalid amount");
                }
            });
            _paymentProviderMock.Setup(m => m.GetOrderPaymentInfoAsync(It.IsAny<PaymentOrder>())).Returns(paymentResult);
            _receiptBuilderMock.Setup(m => m.Build(It.IsAny<Order>())).Returns(receipt);
            _paymentOrderBuilderMock.Setup(m => m.Build(It.IsAny<Order>())).Returns(paymentOrder);

            // Act
            var actual = await _billingService.ProcessOrderAsync(order);

            // Assert
            Assert.AreEqual(expected, actual.ErrorMessage);

            // Expected
            _orderValidatorMock.Verify(m => m.Validate(order), Times.Once);
            _paymentProviderMock.Verify(m => m.GetOrderPaymentInfoAsync(It.IsAny<PaymentOrder>()), Times.Never);
            _receiptBuilderMock.Verify(m => m.Build(It.IsAny<Order>()), Times.Never);
            _paymentOrderBuilderMock.Verify(m => m.Build(It.IsAny<Order>()), Times.Never);
        }

        public static TestCaseData[] ValidPaymentTestCases
        {
            get
            {
                return new[]
                {
                    new TestCaseData(
                        new PaymentResult()
                        {
                            Status = "Success",
                            Code = "000"
                        },
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

        public static TestCaseData[] FailPaymentTestCases
        {
            get
            {
                return new[]
                {
                    new TestCaseData(
                        new PaymentResult()
                        {
                            Status = "Fail",
                            Code = "400"
                        },
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
                        typeof(ArgumentException),
                        new PaymentResult()
                        {
                            Status = "Fail",
                            Code = "400"
                        },
                        new Receipt()
                        {
                             OrderNumber = "Test111",
                             ReceiptId = "R123"
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
                            OrderNumber = "Test111"
                        },
                        "Invalid amount")
                };
            }
        }

        private Mock<IPaymentProvider> _paymentProviderMock;
        private Mock<IReceiptBuilder> _receiptBuilderMock;
        private Mock<IPaymentOrderBuilder> _paymentOrderBuilderMock;
        private Mock<IOrderValidator> _orderValidatorMock;
        private BillingService _billingService;
    }
}