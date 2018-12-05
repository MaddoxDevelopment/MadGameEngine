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
		private readonly long _tickRateMillis;
		private readonly Stopwatch _serverTickWatch;
		private readonly Stopwatch _tickRateWatch;

		protected NetworkedGame(int width, int height, GraphicsMode mode, string title, 
			long tickRateMillis,
			long serverTickRateMillis)
			: base(width, height, mode, title)
		{
			_serverTickRateMillis = serverTickRateMillis;
			_tickRateMillis = tickRateMillis;
			_serverTickWatch = new Stopwatch();
			_tickRateWatch = new Stopwatch();
		}

		private void CallServerTick()
		{
			if (_serverTickWatch.ElapsedMilliseconds < _serverTickRateMillis) return;
			OnServerTick();
			_serverTickWatch.Restart();
		}
		
		protected override void OnLoad(EventArgs e)
		{
			_tickRateWatch.Start();
			base.OnLoad(e);
		}

		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			if (_tickRateWatch.ElapsedMilliseconds >= _tickRateMillis)
			{
				OnTick();
				_tickRateWatch.Restart();
			}
			base.OnUpdateFrame(e);
		}

		protected abstract void OnServerTick();

		protected abstract void OnTick();

	}
}