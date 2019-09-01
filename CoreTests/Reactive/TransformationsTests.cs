using Core.Reactive;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CoreTests.Reactive
{
    public class TransformationsTests
    {
        protected const int DifferentValue = 2;
        protected const int SameValue = 1;

        public class MapTests : TransformationsTests
        {
            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenAnReturnedLiveData_ShouldEmitResultingValue_WhenSourceIsChanged()
            {
                var source = new MutableLiveData<int>(SameValue);
                var returned = Transformations.Map(source, input => input.ToString());

                source.Value = DifferentValue;

                Assert.Equal(DifferentValue.ToString(), returned.Value);
            }
        }
    }
}
