using System;

namespace Util {
	public class Disposable : IDisposable
	{
		public static IDisposable Create(Action action) {
			return new Disposable(action);
		}
		
		private Action action;

		public Disposable(Action action) {
			this.action = action;
		}

		public void Dispose() {
			action();
		}
	}
}