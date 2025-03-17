#if R3
using System;
using R3;

namespace Utility.ProgressTracker
{
	public interface IReadableProgress : IProgress<float>
	{
		ReadOnlyReactiveProperty<float> Progress { get;  }
		void Report(int currentContribution);
	}
}
#endif