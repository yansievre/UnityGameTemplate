#if R3
using System;
using R3;

namespace Utility.ProgressTracker
{
	public class ReactiveProgress : IReadableProgress
	{
		private readonly int _contribution;
		private readonly ReactiveProperty<float> _progress = new ReactiveProperty<float>();

		public ReactiveProgress(int contribution)
		{
			_contribution = contribution;
		}
		public ReadOnlyReactiveProperty<float> Progress => _progress;
        
		public void Report(float value)
		{
			_progress.Value = value;
		}

		public void Report(int currentContribution)
		{
			_progress.Value = (float)currentContribution / _contribution;
		}
	}
}
#endif