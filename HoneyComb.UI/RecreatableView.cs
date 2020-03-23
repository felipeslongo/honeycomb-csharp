using System;
using System.Threading.Tasks;
using HoneyComb.Core.Lifecycles;
using HoneyComb.Core.Lifecycles.Threading.Tasks;

namespace HoneyComb.UI
{
    public sealed class RecreatableView<TView> : ILifecycleOwner where TView : class, ILifecycleOwner
    {
        private readonly LifecycleOfLifecycles lifecycle;
        private TView? view;

        public RecreatableView()
        {
            lifecycle = new LifecycleOfLifecycles(this);
        }

        public Lifecycle Lifecycle => lifecycle;

        public TView? View
        {
            get => view;
            set
            {
                view = value;
                NotifyViewSet();
            }
        }
        
        public async Task<T> WhenActiveAsync<T>(Func<TaskCompletionSource<T>, Task> interactAsync)
        {
            await lifecycle.WhenActiveAsync();
            var interactionTaskSource = new TaskCompletionSource<T>();
            
            EventHandler<EventArgs> onLifecycleRenewed = async (_, __) =>
            {
                try
                {
                    if (interactionTaskSource.Task.IsCompleted)
                        return;
                    
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

        private void NotifyViewSet()
        {
            lifecycle.SetLifecycle(view!.Lifecycle);
        }
    }
}
