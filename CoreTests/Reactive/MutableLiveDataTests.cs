using Core.Reactive;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CoreTests.Reactive
{
    public class MutableLiveDataTests
    {
        protected const int DifferentValue = 2;
        protected const int SameValue = 1;

        public class TwoWayBind : MutableLiveDataTests
        {
            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenFooAndBarTwoWayBinded_ShouldFooHaveTheSameValueAsBar_WhenBarValueChanges()
            {
                var foo = new MutableLiveData<int>(SameValue);
                var bar = new MutableLiveData<int>(SameValue);
                foo.TwoWayBind(bar);

                bar.Value = DifferentValue;

                Assert.Equal(bar.Value, foo.Value);
            }

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenFooAndBarTwoWayBinded_ShouldBarHaveTheSameValueAsFoo_WhenFooValueChanges()
            {
                var foo = new MutableLiveData<int>(SameValue);
                var bar = new MutableLiveData<int>(SameValue);
                foo.TwoWayBind(bar);

                foo.Value = DifferentValue;

                Assert.Equal(foo.Value, bar.Value);
            }

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenFooAndBarTwoWayBinded_ShouldFooValueBeUnchanged_WhenBarValueChanges()
            {
                var foo = new MutableLiveData<int>(SameValue);
                var bar = new MutableLiveData<int>(SameValue);
                var bind = foo.TwoWayBind(bar);
                bind.Dispose();

                bar.Value = DifferentValue;

                Assert.Equal(SameValue, foo.Value);

            }

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenFooAndBarTwoWayBinded_ShouldBarValueBeUnchanged_WhenFooValueChanges()
            {
                var foo = new MutableLiveData<int>(SameValue);
                var bar = new MutableLiveData<int>(SameValue);
                var bind = foo.TwoWayBind(bar);
                bind.Dispose();

                foo.Value = DifferentValue;

                Assert.Equal(SameValue, bar.Value);

            }
        }
    }
}
