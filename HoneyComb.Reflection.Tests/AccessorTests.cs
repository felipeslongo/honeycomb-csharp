using HoneyComb.Reflection;
using HoneyComb.TestChamber;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace HoneyComb.Reflection.Tests
{
    public class AccessorTests
    {
        public const int AValue = 1;
        public int PublicProperty { get; set; }

        public class GetTests : AccessorTests
        {
            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenAPublicProperty_ShouldBeSetWithPassetValue_WhenAccessorSetIsInvoked()
            {
                var accessor = new Accessor<int>(() => PublicProperty);

                accessor.Set(AValue);

                Assert.Equal(AValue, PublicProperty);
            }
        }

        public class SetTests : AccessorTests
        {
            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenAPublicPropertyWithValue_ShouldTheValueBeRetrieved_WhenAccessorGetIsInvoked()
            {
                PublicProperty = AValue;
                var accessor = new Accessor<int>(() => PublicProperty);

                var actual = accessor.Get();

                Assert.Equal(AValue, actual);
            }
        }
    }
}
