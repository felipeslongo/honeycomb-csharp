using System;
using System.Threading.Tasks;
using HoneyComb.Core.Lifecycles;
using HoneyComb.Core.Lifecycles.Threading.Tasks;

namespace HoneyComb.UI
{
    public sealed class UserInteractionViewModel
    {
        private readonly LifecycleOfLifecycles lifecycle;
        
        public UserInteractionViewModel(LifecycleOfLifecycles lifecycleOfLifecycles)
        {
            lifecycle = lifecycleOfLifecycles;
        }
        
        public async Task<T> InteractAsync<T>(Func<TaskCompletionSource<T>, Task> interactAsync)
        {
            await lifecycle.WhenActiveAsync();
            var interactionTaskSource = new TaskCompletionSource<T>();
            
            EventHandler<EventArgs> onLifecycleRenewed = async (_, __) =>
            {
                try
                {
                    await lifecycle.WhenActiveAsync();
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
                lifecycle.Renewed += onLifecycleRenewed;
                lifecycle.OnDisposed += onLifecycleDisposed;

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
                lifecycle.Renewed -= onLifecycleRenewed;
                lifecycle.OnDisposed -= onLifecycleDisposed;
            }
        }
    }
}
