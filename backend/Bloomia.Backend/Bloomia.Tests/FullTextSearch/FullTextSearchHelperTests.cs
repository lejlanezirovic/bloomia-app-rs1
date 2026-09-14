using Bloomia.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Tests.FullTextSearch
{
    public class FullTextSearchHelperTests
    {
        [Fact]
        public void BuildContainsExpression_SplitsWordsIntoPrefixOrQuery()
        {
            var result = FullTextSearchHelper.BuildContainsExpression("anksioznost san");
            Assert.Equal("\"anksioznost*\" OR \"san*\"", result);
        }

        [Fact]
        public void BuildContainsExpression_ReturnsNullForEmptyInput()
        {
            Assert.Null(FullTextSearchHelper.BuildContainsExpression("   "));
        }

        [Fact]
        public void BuildContainsExpression_ReturnsNullForNullInput()
        {
            Assert.Null(FullTextSearchHelper.BuildContainsExpression(null));
        }
    }
}
