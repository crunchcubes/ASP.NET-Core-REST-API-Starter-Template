using Moq;
using Restful.Core.Helpers;
using System.Collections.Generic;
using Restful.Core.Interfaces;
using Xunit;

namespace Restful.UnitTests.Core.Helpers
{
    public abstract class PaginatedListShould<T> where T : class
    {
        protected PaginationBase PaginationBase;
        protected PaginatedList<T> PaginatedList;
        protected List<T> Items = new List<T>();
    }

    public class PaginatedObjectListShould : PaginatedListShould<IEntity>
    {
        public PaginatedObjectListShould()
        {
            PaginationBase = new PaginationBase();
        }

        private void MockItems(int pageSize)
        {
            for (int i = 0; i < pageSize; i++)
            {
                Items.Add(new Mock<IEntity>().Object);
            }
        }

        [Theory]
        [InlineData(0, 10)]
        [InlineData(1, 10)]
        [InlineData(1, 1)]
        [InlineData(20, 10)]
        public void HasZeroPageWhenNoItem(int pageIndex, int pageSize)
        {
            PaginatedList = new PaginatedList<IEntity>(pageIndex, pageSize, 0, Items);

            Assert.Equal(0, PaginatedList.PageCount);
        }

        [Theory]
        [InlineData(10, 0)]
        [InlineData(10, 10)]
        [InlineData(5, 10)]
        [InlineData(100, 200)]
        public void PageCountEqualToQuotentWhenItemsCountDivisibleByPageSize(int pageSize, int totalItemsCount)
        {
            MockItems(pageSize);

            PaginatedList = new PaginatedList<IEntity>(0, pageSize, totalItemsCount, Items);
            var quotient = totalItemsCount / pageSize;

            Assert.Equal(quotient, PaginatedList.PageCount);
        }

        [Theory]
        [InlineData(10, 1)]
        [InlineData(10, 7)]
        [InlineData(5, 9)]
        [InlineData(100, 150)]
        public void PageCountEqualToQuotentPlusOneWhenItemsCountCannotBeDivisibleByPageSize(int pageSize, int totalItemsCount)
        {
            MockItems(pageSize);

            PaginatedList = new PaginatedList<IEntity>(0, pageSize, totalItemsCount, Items);
            var quotient = totalItemsCount / pageSize;

            Assert.Equal(quotient + 1, PaginatedList.PageCount);
        }

        [Theory]
        [InlineData(10, 1, 15)]
        [InlineData(10, 7, 20)]
        [InlineData(5, 9, 1)]
        [InlineData(100, 150, 50)]
        public void PageCountChangedWhenTotalItemsCountChangedProperly(int pageSize, int totalItemsCountBefore, int totalItemsCountAfter)
        {
            MockItems(pageSize);

            PaginatedList = new PaginatedList<IEntity>(0, pageSize, totalItemsCountBefore, Items);
            var pageCountBefore = PaginatedList.PageCount;

            PaginatedList.TotalItemsCount = totalItemsCountAfter;
            var pageCountAfter = PaginatedList.PageCount;

            Assert.NotEqual(pageCountBefore, pageCountAfter);
        }

        [Theory]
        [InlineData(1, 10, 11)]
        [InlineData(2, 10, 47)]
        [InlineData(3, 5, 29)]
        [InlineData(4, 100, 250)]
        public void HasPreviousWhenPageIndexBiggerThanZero(int pageIndex, int pageSize, int totalItemsCount)
        {
            var temp = totalItemsCount - pageIndex * pageSize;
            var currentPageItemsCount = temp >= pageSize ? pageSize : temp;
            MockItems(currentPageItemsCount);

            PaginatedList = new PaginatedList<IEntity>(pageIndex, pageSize, totalItemsCount, Items);
            
            Assert.True(PaginatedList.HasPrevious);
        }

        [Theory]
        [InlineData(0, 10, 11)]
        [InlineData(3, 10, 47)]
        [InlineData(4, 5, 29)]
        [InlineData(1, 100, 250)]
        public void HasNextWhenPageIndexLessThanPageCountMinusOne(int pageIndex, int pageSize, int totalItemsCount)
        {
            MockItems(pageSize);

            PaginatedList = new PaginatedList<IEntity>(pageIndex, pageSize, totalItemsCount, Items);

            Assert.True(PaginatedList.HasNext);
        }
    }
}
