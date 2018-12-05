using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;

namespace GameEngine.V2
{
	public abstract class NetworkedGame : GameWindow
	{
		private readonly long _serverTickRateMillis;
		private readonly Stopwatch _serverTickWatch;

		protected NetworkedGame(int width, int height, GraphicsMode mode, string title, long serverTickRateMillis)
			: base(width, height, mode, title)
		{
			_serverTickRateMillis = serverTickRateMillis;
			_serverTickWatch = new Stopwatch();
		}

		private void StartServerLoop()
		{
			Task.Factory.StartNew(() =>
			{
				_serverTickWatch.Start();
				while (true)
				{
					CallServerTick();
				}
			}, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
		}

		private void CallServerTick()
		{
			if (_serverTickWatch.ElapsedMilliseconds < _serverTickRateMillis) return;
			OnServerTick();
			_serverTickWatch.Restart();
		}
		
		protected override void OnLoad(EventArgs e)
		{
			StartServerLoop();
			base.OnLoad(e);
		}

		protected abstract void OnServerTick();
		
	}
}