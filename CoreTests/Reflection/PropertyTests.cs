using Core.Reflection;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CoreTests.Reflection
{
    public class PropertyTests
    {
        private int PrivateProperty { get; set; }

        public class GetSetterTests: PropertyTests
        {
            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenAPrivateProperty_ShouldBeSetWithThePassedValue_WhenSetterInvoked()
            {
                var value = 1;
                var setter = Property.GetSetter(this, @this => @this.PrivateProperty);

                setter(value);

                Assert.Equal(value, PrivateProperty);
            }
        }
    }
}
