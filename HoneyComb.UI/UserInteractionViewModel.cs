using System;
using System.Threading.Tasks;
using HoneyComb.Core.Lifecycles;
using HoneyComb.Core.Lifecycles.Threading.Tasks;

namespace HoneyComb.UI
{
    public sealed class UserInteractionViewModel : ILifecycleOwner
    {
        public UserInteractionViewModel()
        {
            Lifecycle = new LifecycleOfLifecycles(this);
        }
        
        public async Task<T> InteractAsync<T>(Func<TaskCompletionSource<T>, Task> interactAsync)
        {
            await Lifecycle.WhenActiveAsync();
            var interactionTaskSource = new TaskCompletionSource<T>();
            
            EventHandler<EventArgs> onLifecycleRenewed = async (_, __) =>
            {
                try
                {
                    await Lifecycle.WhenActiveAsync();
                    await interactAsync(interactionTaskSource);
                }
                catch (OperationCanceledException)
                {
                    interactionTaskSource.TrySetCanceled();
                }
                catch (Exception e)
                {
                    interactionTaskSource.TrySetException(e);
                }
            };
            EventHandler<EventArgs> onLifecycleDisposed = (_, __) => interactionTaskSource.TrySetCanceled();

            try
            {
                Lifecycle.Renewed += onLifecycleRenewed;
                Lifecycle.OnDisposed += onLifecycleDisposed;

                try
                {
                    await interactAsync(interactionTaskSource);
                }
                catch (OperationCanceledException)
                {
                    interactionTaskSource.TrySetCanceled();
                }
                catch (Exception e)
                {
                    interactionTaskSource.TrySetException(e);
                }
                
                return await interactionTaskSource.Task;
            }
            finally
            {
                Lifecycle.Renewed -= onLifecycleRenewed;
                Lifecycle.OnDisposed -= onLifecycleDisposed;
            }
        }
        
        public LifecycleOfLifecycles Lifecycle { get; }

        Lifecycle ILifecycleOwner.Lifecycle => Lifecycle;
    }
}
