using System;
using UniRx;

namespace Crowolf.Artem.InputTools
{
	public interface IInputProvider
	{
		bool Enable { get; set; }
		IReadOnlyReactiveProperty<float> PushPower { get; }
		IObservable<Unit> OnPushed { get; }
		IObservable<Unit> OnDoublePushed { get; }
		IObservable<Unit> OnLongPushed { get; }
		IObservable<Unit> OnReleased { get; }
		IReadOnlyReactiveProperty<bool> IsPushing { get; }
		int DoublePushSpanMilliseconds { get; set; }
		int LongPushSpanMilliseconds { get; set; }
		float PushDetectThreshold { get; set; }
	}
}