using Core.Reflection;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CoreTests.Reflection
{
    public class FieldTests
    {
        private int _privateField;
        private readonly int _privateReadonlyField = 0;

        public class GetSetterTests: FieldTests
        {
            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenAPrivateField_ShouldBeSetWithThePassedValue_WhenSetterInvoked()
            {
                var value = 1;
                var setter = Field.GetSetter(this, @this => @this._privateField);

                setter(value);

                Assert.Equal(value, _privateField);
            }

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenAPrivateReadonlyField_ShouldThrowException_WhenGetSetterCalled()
            {
                var exception = Assert.Throws<ArgumentException>(() => Field.GetSetter(this, @this => @this._privateReadonlyField));

                Assert.Contains(Field.MessageFieldIsReadonly, exception.Message);
            }
        }
    }
}
