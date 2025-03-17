#if R3
using System;
using R3;

namespace Utility
{
    public static class ObservableExtensions
    {
        public static IDisposable SubscribeBlind<T>(this Observable<T> source, Action onNext) =>
            source.Subscribe(unit => onNext());
    }
}
#endif