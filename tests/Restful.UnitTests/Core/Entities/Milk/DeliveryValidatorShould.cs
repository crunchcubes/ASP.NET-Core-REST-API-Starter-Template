using Moq;
using Restful.Core.Entities.Milk;
using Restful.Core.Interfaces.Milk;
using Restful.UnitTests.Core.Entities.Milk.Data;
using System.Collections.Generic;
using Xunit;

namespace Restful.UnitTests.Core.Entities.Milk
{
    public class DeliveryValidatorShould
    {
        private readonly DeliveryValidator _sut;

        public DeliveryValidatorShould()
        {
            var orderValidator = new Mock<IOrderValidator>();
            orderValidator.Setup(x => x.Validate(It.IsAny<IEnumerable<(Product, int)>>()))
                .Returns(OrderResult.Accepted);

            _sut = new DeliveryValidator(orderValidator.Object);
        }

        [Theory]
        [OrdersExceedBoxAreaData]
        public void RejectWhenOrdersByBoxExceedBoxArea(IEnumerable<(Product product, int quantity)> orders)
        {
            var result = _sut.ValidateByTruck(orders);

            Assert.Equal(DeliveryResult.Rejected, result);
        }
        
        [Theory]
        [OrdersBoxOkButSingleExceedData]
        public void PartialAcceptedWhenOrdersByBoxNotExceedBoxAreaButOrdersByOneExceedTotal(IEnumerable<(Product product, int quantity)> orders)
        {
            var result = _sut.ValidateByTruck(orders);

            Assert.Equal(DeliveryResult.PartialAccepted, result);
        }

        [Theory]
        [OrdersNotExceedItsAreaData]
        public void AcceptedWhenAllOrdersNotExceedItsArea(IEnumerable<(Product product, int quantity)> orders)
        {
            var result = _sut.ValidateByTruck(orders);

            Assert.Equal(DeliveryResult.Accepted, result);
        }

        [Theory]
        [OrdersBoxNotExceedSingleExceedTotalNotExceedData]
        public void AcceptedWhenBoxNotExceedSingleExceedButTotalOk(IEnumerable<(Product product, int quantity)> orders)
        {
            var result = _sut.ValidateByTruck(orders);

            Assert.Equal(DeliveryResult.Accepted, result);
        }
    }
}
