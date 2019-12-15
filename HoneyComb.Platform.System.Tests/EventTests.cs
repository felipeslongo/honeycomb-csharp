using HoneyComb.TestChamber;
using Xunit;

namespace HoneyComb.Core.Tests
{
    public class EventTests
    {
        public class ExecuteIfUnhandled : EventTests
        {
            [Fact, Trait(nameof(Category), Category.Unit)]
            public void GivenAHandledEvent_WhenInvoked_ShouldNotExecuteAction()
            {
                var wasExecuted = false;
                var Event = new Event();
                Event.ExecuteIfUnhandled(() => { });

                Event.ExecuteIfUnhandled(() => wasExecuted = true);

                Assert.False(wasExecuted);
            }

            [Fact, Trait(nameof(Category), Category.Unit)]
            public void GivenAnUnhandledEvent_WhenInvoked_ShouldExecuteAction()
            {
                var wasExecuted = false;
                var Event = new Event();

                Event.ExecuteIfUnhandled(() => wasExecuted = true);

                Assert.True(wasExecuted);
            }
        }

        public class IsHandledTests : EventTests
        {
            [Fact, Trait(nameof(Category), Category.Unit)]
            public void GivenAHandledEvent_WhenInvoked_ShouldBeTrue()
            {
                var Event = new Event();
                Event.ExecuteIfUnhandled(() => { });

                var @return = Event.IsHandled;

                Assert.True(@return);
            }

            [Fact, Trait(nameof(Category), Category.Unit)]
            public void GivenAnUnhandledEvent_WhenInvoked_ShouldBeFalse()
            {
                var Event = new Event();

                var @return = Event.IsHandled;

                Assert.False(@return);
            }
        }
    }

    public class EventTests_Generic : EventTests
    {
        public class ExecuteIfUnhandledTests_Generic : EventTests_Generic
        {
            [Fact, Trait(nameof(Category), Category.Unit)]
            public void GivenAHandledEvent_WhenInvoked_ShouldNotExecuteAction()
            {
                var wasExecuted = false;
                var Event = new Event<bool>(true);
                Event.ExecuteIfUnhandled(content => { });

                Event.ExecuteIfUnhandled(content => wasExecuted = content);

                Assert.False(wasExecuted);
            }

            [Fact, Trait(nameof(Category), Category.Unit)]
            public void GivenAnUnhandledEvent_WhenInvoked_ShouldExecuteAction()
            {
                var wasExecuted = false;
                var Event = new Event<bool>(true);

                Event.ExecuteIfUnhandled(content => wasExecuted = content);

                Assert.True(wasExecuted);
            }

            [Fact, Trait(nameof(Category), Category.Unit)]
            public void GivenAnUnhandledEvent_WhenInvoked_ShouldPassContentAsParameterOfTheAction()
            {
                const int expectedArgument = int.MaxValue;
                var actualArgument = 0;
                var Event = new Event<int>(expectedArgument);

                Event.ExecuteIfUnhandled(content => actualArgument = content);

                Assert.Equal(expectedArgument, actualArgument);
            }
        }

        public class PeekContentTests : EventTests_Generic
        {
            [Fact, Trait(nameof(Category), Category.Unit)]
            public void GivenAHandledEvent_WhenInvoked_ShouldReturnTheContent()
            {
                const int expectedContent = int.MaxValue;
                var Event = new Event<int>(expectedContent);
                Event.ExecuteIfUnhandled(content => { });

                var actualContent = Event.PeekContent;

                Assert.Equal(expectedContent, actualContent);
            }

            [Fact, Trait(nameof(Category), Category.Unit)]
            public void GivenAnUnhandledEvent_WhenInvoked_ShouldReturnTheContent()
            {
                const int expectedContent = int.MaxValue;
                var Event = new Event<int>(expectedContent);

                var actualContent = Event.PeekContent;

                Assert.Equal(expectedContent, actualContent);
            }
        }
    }
}