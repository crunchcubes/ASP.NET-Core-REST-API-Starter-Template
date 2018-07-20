using Restful.Core.Helpers;
using Restful.Core.Interfaces;
using Xunit;

namespace Restful.UnitTests.Core.Helpers
{
    public class PaginationBaseShould
    {
        private readonly PaginationBase _paginationBase;

        public PaginationBaseShould()
        {
            _paginationBase = new PaginationBase();
        }

        [Theory]
        [InlineData(100, 100)]
        [InlineData(2, 1)]
        [InlineData(500, 10)]
        public void PageSizeEqualToMaxWhenTooBigSetPageSizeFirst(int pageSize, int maxSize)
        {
            _paginationBase.PageSize = pageSize;
            _paginationBase.MaxPageSize = maxSize;

            var result = _paginationBase.PageSize;

            Assert.Equal(maxSize, result);
        }

        [Theory]
        [InlineData(100, 100)]
        [InlineData(2, 1)]
        [InlineData(500, 10)]
        public void PageSizeEqualToMaxWhenTooBigSetMaxPageSizeFirst(int pageSize, int maxSize)
        {
            _paginationBase.MaxPageSize = maxSize;
            _paginationBase.PageSize = pageSize;

            var result = _paginationBase.PageSize;

            Assert.Equal(maxSize, result);
        }

        [Theory]
        [InlineData(0, 100)]
        [InlineData(-1, 100)]
        [InlineData(-10, 100)]
        public void SetDefaultPageSizeWhenPageSizeLessThanOne(int pageSize, int maxSize)
        {
            _paginationBase.PageSize = pageSize;
            _paginationBase.MaxPageSize = maxSize;

            var result = _paginationBase.PageSize;

            Assert.Equal(10, result);
        }

        [Theory]
        [InlineData(0, 5)]
        [InlineData(-1, 7)]
        [InlineData(-10, 1)]
        public void SetPageSizeToMaxWhenPageSizeLessThanOneAndMaxSizeLessThanDefaultPageSize(int pageSize, int maxSize)
        {
            _paginationBase.PageSize = pageSize;
            _paginationBase.MaxPageSize = maxSize;

            var result = _paginationBase.PageSize;

            Assert.Equal(maxSize, result);
        }

        [Theory]
        [InlineData(10, -100)]
        [InlineData(10, 0)]
        [InlineData(10, -1)]
        public void SetDefaultMaxPageSizeWhenMaxPageSizeLessThanOne(int pageSize, int maxSize)
        {
            _paginationBase.PageSize = pageSize;
            _paginationBase.MaxPageSize = maxSize;

            var result = _paginationBase.MaxPageSize;

            Assert.Equal(100, result);
        }

        [Theory]
        [InlineData(-100)]
        [InlineData(-1)]
        public void PageIndexEqualsToZeroWhenSetNegative(int pageIndex)
        {
            _paginationBase.PageIndex = pageIndex;

            Assert.Equal(0, _paginationBase.PageIndex);
        }

        [Fact]
        public void OrderByEqualsToIdWhenNull()
        {
            _paginationBase.OrderBy = null;

            Assert.Equal(nameof(IEntity.Id), _paginationBase.OrderBy);
        }
    }
}
