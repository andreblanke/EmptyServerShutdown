using System;
using System.Threading;
using System.Threading.Tasks;

using TShockAPI;

namespace EmptyServerShutdown
{
    public sealed class ScheduledServerShutdown : IDisposable
    {
        private CancellationTokenSource CancellationTokenSource { get; } = new CancellationTokenSource();

        public bool Save { get; set; }

        public string Reason { get; set; }

        public DateTime? ScheduledTime { get; private set; }

        public void Dispose() => CancellationTokenSource?.Dispose();

        internal void Initiate(TimeSpan timeSpan)
        {
            ScheduledTime = DateTime.Now.Add(timeSpan);

            Task.Delay(timeSpan, CancellationTokenSource.Token)
                .ContinueWith(task =>
                {
                    if (!task.IsCanceled)
                        TShock.Utils.StopServer(Save, Reason);
                });
        }

        internal void Cancel() => CancellationTokenSource.Cancel();
    }
}
