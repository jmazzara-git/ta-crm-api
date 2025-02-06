using Microsoft.EntityFrameworkCore;
using Moq;

namespace TACRM.Tests.Extensions
{
	public static class DbSetMockExtensions
	{
		public static Mock<DbSet<T>> ReturnsDbSet<T>(this Mock<DbSet<T>> mockSet, IEnumerable<T> sourceList) where T : class
		{
			var queryable = sourceList.AsQueryable();
			mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
			mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
			mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
			mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
			return mockSet;
		}
	}
}
