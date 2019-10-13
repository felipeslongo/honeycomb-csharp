using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Strings;
using Xunit;

namespace CoreTests
{
    public class IEnumerableStringJoinTests
    {
        private const string Separator = ", ";
        
        [Fact]
        public void ShouldJoinStringsUsingTheSeparator()
        {
            var strings = new[] {"first", "second", "third"};

            var actual = strings.StringJoin(Separator);

            var expected = "first, second, third";
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ShouldReturnFirstStringUnmodifiedGivenAnIEnumerableWithOneString()
        {
            var strings = new[] {"first"};

            var actual = strings.StringJoin(Separator);

            var expected = "first";
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ShouldReturnEmptyGivenEmptyIEnumerable()
        {
            var strings = Enumerable.Empty<string>();

            var actual = strings.StringJoin(Separator);

            var expected = string.Empty;
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ShouldThrowArgumentNullExceptionGivenNullIEnumerable()
        {
            IEnumerable<string>? strings = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Assert.Throws<ArgumentNullException>(() => strings!.StringJoin(Separator));
        }
    }
}
