using HoneyComb.TestChamber;
using Xunit;

namespace HoneyComb.LiveDataNet.Tests
{
    public class LiveData_CombineTests
    {
        public class CombineTests : LiveData_CombineTests
        {
            [Fact, Trait(nameof(Category), Category.Unit)]
            public void GivenTwoSourcesWithEmittedValuesCombined_WhenCombined_ShouldInvokeOnChange()
            {
                var source1 = new MutableLiveData<int>();
                var source2 = new MutableLiveData<int>();
                source1.Value = int.MaxValue;
                source2.Value = int.MinValue;
                var isOnChangeInvoked = false;

                source1.Combine(source2, (_, __) =>
                {
                    isOnChangeInvoked = true;
                    return int.MaxValue;
                });

                Assert.True(isOnChangeInvoked);
            }

            [Fact, Trait(nameof(Category), Category.Unit)]
            public void GivenTwoSourcesWithoutEmittedValues_WhenCombined_ShouldNotInvokeOnChange()
            {
                var source1 = new MutableLiveData<int>();
                var source2 = new MutableLiveData<int>();
                var isOnChangeInvoked = false;

                source1.Combine(source2, (_, __) =>
                {
                    isOnChangeInvoked = true;
                    return int.MaxValue;
                });

                Assert.False(isOnChangeInvoked);
            }

            [Fact, Trait(nameof(Category), Category.Unit)]
            public void GivenTwoSourcesWithoutEmittedValuesCombined_WhenBothSourcesChanges_ShouldInvokeOnChange()
            {
                var source1 = new MutableLiveData<int>();
                var source2 = new MutableLiveData<int>();
                var isOnChangeInvoked = false;
                source1.Combine(source2, (_, __) =>
                {
                    isOnChangeInvoked = true;
                    return int.MaxValue;
                });

                source1.Value = int.MaxValue;
                source2.Value = int.MinValue;

                Assert.True(isOnChangeInvoked);
            }

            [Fact, Trait(nameof(Category), Category.Unit)]
            public void GivenTwoSourcesWithoutEmittedValuesCombined_WhenBothSourcesChanges_ShouldInvokeOnChangePassingBothSourcesValues()
            {
                var source1 = new MutableLiveData<int>();
                var source2 = new MutableLiveData<int>();
                var source1Value = 0;
                var source2Value = 0;
                source1.Combine(source2, (source1ValueOnChange, source2ValueOnChange) =>
                {
                    source1Value = source1ValueOnChange;
                    source2Value = source2ValueOnChange;
                    return int.MaxValue;
                });

                source1.Value = int.MaxValue;
                source2.Value = int.MinValue;

                Assert.Equal(source1.Value, source1Value);
                Assert.Equal(source2.Value, source2Value);
            }

            [Fact, Trait(nameof(Category), Category.Unit)]
            public void GivenTwoSourcesWithoutEmittedValuesCombined_WhenOnlyOneSourceChanges_ShouldNotInvokeOnChange()
            {
                var source1 = new MutableLiveData<int>();
                var source2 = new MutableLiveData<int>();
                var isOnChangeInvoked = false;
                source1.Combine(source2, (_, __) =>
                {
                    isOnChangeInvoked = true;
                    return int.MaxValue;
                });

                source1.Value = int.MaxValue;

                Assert.False(isOnChangeInvoked);
            }
        }
    }
}
