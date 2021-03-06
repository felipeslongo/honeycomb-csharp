﻿using HoneyComb.TestChamber;
using System;
using System.Collections.Generic;
using Xunit;
using HoneyComb.Core.Lifecycles;

namespace HoneyComb.Core.Tests.Lifecycles
{
    public class LifecycleState_ChangeStateTests
    {
        public static IEnumerable<object[]> EnumValues()
        {
            foreach (var state in Enum.GetValues(typeof(LifecycleState)))
            {
                yield return new object[] { state! };
            }
        }

        public class ChangeStateStrictTests : LifecycleState_ChangeStateTests
        {
            [Theory, Trait(nameof(Category), Category.Unit)]
            [MemberData(nameof(EnumValues))]
            public void GivenAnyState_WhenChangeStateToSame_ShouldSucceed(LifecycleState state)
            {
                var actual = state.ChangeStateStrict(state);
        
                Assert.Equal(state, actual);
            }
        
            [Theory, Trait(nameof(Category), Category.Unit)]
            [MemberData(nameof(EnumValues))]
            public void GivenAnyState_WhenChangeStateToInitialized_ShouldFail(LifecycleState state)
            {
                if (state == LifecycleState.Initialized)
                    return;
                Assert.Throws<InvalidOperationException>(() => state.ChangeStateStrict(LifecycleState.Initialized));
            }
        
            [Theory, Trait(nameof(Category), Category.Unit)]
            [MemberData(nameof(EnumValues))]
            public void GivenDisposedState_WhenChangeStateToAny_ShouldFail(LifecycleState state)
            {
                if (state == LifecycleState.Disposed)
                    return;
                Assert.Throws<InvalidOperationException>(() => LifecycleState.Disposed.ChangeStateStrict(state));
            }
        
            [Theory, Trait(nameof(Category), Category.Unit)]
            [InlineData(LifecycleState.Initialized, LifecycleState.Active)]
            [InlineData(LifecycleState.Active, LifecycleState.Inactive)]
            [InlineData(LifecycleState.Inactive, LifecycleState.Active)]
            [InlineData(LifecycleState.Inactive, LifecycleState.Disposed)]
            public void GivenValidTransitions_WhenChangeState_ShouldSucceed(LifecycleState fromState, LifecycleState toState)
            {
                var actual = fromState.ChangeStateStrict(toState);
        
                Assert.Equal(toState, actual);
            }
        
            [Theory, Trait(nameof(Category), Category.Unit)]
            [InlineData(LifecycleState.Initialized, LifecycleState.Inactive)]
            [InlineData(LifecycleState.Initialized, LifecycleState.Disposed)]
            [InlineData(LifecycleState.Active, LifecycleState.Initialized)]
            [InlineData(LifecycleState.Active, LifecycleState.Disposed)]
            [InlineData(LifecycleState.Inactive, LifecycleState.Initialized)]
            [InlineData(LifecycleState.Disposed, LifecycleState.Initialized)]
            [InlineData(LifecycleState.Disposed, LifecycleState.Active)]
            [InlineData(LifecycleState.Disposed, LifecycleState.Inactive)]
            public void GivenInvalidTransitions_WhenChangeState_ShouldFail(LifecycleState fromState, LifecycleState toState)
            {
                Assert.Throws<InvalidOperationException>(() => fromState.ChangeStateStrict(toState));
            }
        }
    }
}
