using DSharpPlus;
using System.Threading;
using System.Threading.Tasks;

namespace Bot.Contracts
{
    public abstract class Functionality
    {
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        public abstract void Run(DiscordClient discord);
        public void Start(DiscordClient discord)
        {
            CancellationToken _cancellationToken = _cancellationTokenSource.Token;
            Task.Factory.StartNew(() =>
            {
                Run(discord);
            }, _cancellationToken);
        }
        public void Stop()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}
