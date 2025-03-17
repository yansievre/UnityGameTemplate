#if R3
using System;
using System.Collections.Generic;
using System.Linq;
using R3;

namespace Utility.ProgressTracker
{
	public class CompositeProgress : IReadableProgress, IDisposable
	{
		private readonly List<IReadableProgress> _progresses = new List<IReadableProgress>();
		private readonly ReactiveProperty<float> _progress;
		private int _maxContribution;
		private DisposableBag _disposableBag;

		public CompositeProgress()
		{
			_progress = new ReactiveProperty<float>(0);
		}

		public IReadableProgress NewProgress(int contribution)
		{
			var reactiveProgress = new ReactiveProgress(contribution);
            
			RegisterReadableProcess(contribution, reactiveProgress);

			return reactiveProgress;
		}

		public CompositeProgress NewSubProgress(int contribution)
		{
			var subProcess = new CompositeProgress();
            
			RegisterReadableProcess(contribution, subProcess);

			return subProcess;
		}
        
		private void RegisterReadableProcess(int contribution, IReadableProgress readableProgress)
		{
			_maxContribution += contribution;
			_progresses.Add(readableProgress);
			readableProgress.Progress.SubscribeBlind(Recalculate).AddTo(ref _disposableBag);
		}
        
		private void Recalculate()
		{
			_progress.Value = _progresses.Sum(x => x.Progress.CurrentValue) / _maxContribution;
		}

		public ReadOnlyReactiveProperty<float> Progress => _progress;
        
		public void Report(float value)
		{
			_progress.Value = value;
		}

		public void Report(int currentContribution)
		{
			_progress.Value = (float)currentContribution / _maxContribution;
		}

		public void Dispose()
		{
			foreach (var progress in _progresses)
			{
				if (progress is CompositeProgress compositeProgress)
					compositeProgress.Dispose();
			}
			_disposableBag.Dispose();
			_progress?.Dispose();
		}
	}
}
#endif