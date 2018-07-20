using Restful.Core.Entities.Milk;
using System.Collections.Generic;
using Restful.UnitTests.Core.Entities.Milk.Data;
using Xunit;

namespace Restful.UnitTests.Core.Entities.Milk
{
    public class OrderValidatorShould
    {
        private readonly OrderValidator _sut;

        public OrderValidatorShould()
        {
            _sut = new OrderValidator();
        }

        [Fact]
        public void RejectWhenQuantityIsZero()
        {
            var dummyProduct = new Product();

            var result = _sut.Validate(0, dummyProduct);

            Assert.Equal(OrderResult.Rejected, result);
        }

        [Theory]
        [ProductByBoxWithInvalidBoxQuantityData]
        public void RejectedWhenOrderByBoxButQuantityNotDividedByBox(int quantity, Product product)
        {
            var result = _sut.Validate(quantity, product);

            Assert.Equal(OrderResult.Rejected, result);
        }

        [Theory]
        [ProductByBoxWithInvalidMinQuantityData]
        public void RejectedWhenOrderByBoxLessThanMinimum(int quantity, Product product)
        {
            var result = _sut.Validate(quantity, product);

            Assert.Equal(OrderResult.Rejected, result);
        }

        [Theory]
        [ProductByBoxWithValidQuantityData]
        public void AcceptedWhenOrderByWithGoodQuantity(int quantity, Product product)
        {
            var result = _sut.Validate(quantity, product);

            Assert.Equal(OrderResult.Accepted, result);
        }

        [Theory]
        [ProductByOneWithInvalidQuantityData]
        public void RejectedWhenOrderByOneLessThanMinimum(int quantity, Product product)
        {
            var result = _sut.Validate(quantity, product);

            Assert.Equal(OrderResult.Rejected, result);
        }

        [Theory]
        [ProductByOneWithValidQuantityData]
        public void AcceptedWhenOrderByOneNotLessThanMinimum(int quantity, Product product)
        {
            var result = _sut.Validate(quantity, product);

            Assert.Equal(OrderResult.Accepted, result);
        }

        [Theory]
        [MemberData(nameof(OrderTestData.MixOfValidAndInvalidOrders), MemberType = typeof(OrderTestData))]
        public void RejectedWhenSomeOrdersContainsInvalidQuantity(IEnumerable<(Product product, int quantity)> orders)
        {
            var result = _sut.Validate(orders);

            Assert.Equal(OrderResult.Rejected, result);
        }

        [Theory]
        [MemberData(nameof(OrderTestData.ValidOrders), MemberType = typeof(OrderTestData))]
        public void AcceptedWhenOrdersOnlyContainsValidQuantity(IEnumerable<(Product product, int quantity)> orders)
        {
            var result = _sut.Validate(orders);

            Assert.Equal(OrderResult.Accepted, result);
        }

    }
}
