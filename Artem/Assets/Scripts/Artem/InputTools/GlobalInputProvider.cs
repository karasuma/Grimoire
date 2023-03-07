using System;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Crowolf.Artem.InputTools
{
	public enum MouseCode : int
	{
		Left = 0,
		Right = 1,
		Middle = 2
	}

	public abstract class GlobalInputProvider : IInputProvider, IDisposable
	{
		public bool Enable { get; set; } = true;

		public IReadOnlyReactiveProperty<float> PushPower => _pushPower;
		private ReactiveProperty<float> _pushPower = new ReactiveProperty<float>( 0f );

		public IObservable<Unit> OnPushed => _onPushed;
		private Subject<Unit> _onPushed = new Subject<Unit>();

		public IObservable<Unit> OnDoublePushed => _onDoublePushed;
		private Subject<Unit> _onDoublePushed = new Subject<Unit>();

		public IObservable<Unit> OnLongPushed => _onLongPushed;
		private Subject<Unit> _onLongPushed = new Subject<Unit>();

		public IObservable<Unit> OnReleased => _onReleased;
		private Subject<Unit> _onReleased = new Subject<Unit>();

		public IReadOnlyReactiveProperty<bool> IsPushing => _isPushing.ToReadOnlyReactiveProperty();
		private Subject<bool> _isPushing = new Subject<bool>();

		public abstract int DoublePushSpanMilliseconds { get; set; }
		public abstract int LongPushSpanMilliseconds { get; set; }
		public abstract float PushDetectThreshold { get; set; }

		protected abstract void InitializePushPowerSource();

		public GlobalInputProvider( Component component )
		{
			InitializePushPowerSource();

			var pushStateChangedSubject = new Subject<Pair<float>>();
			_pushPower
				.Pairwise()
				.Subscribe( pushStateChangedSubject )
				.AddTo( component );

			// Pushed
			pushStateChangedSubject
				.Where( p => Math.Abs( p.Previous ) <= PushDetectThreshold && PushDetectThreshold < Math.Abs( p.Current ) )
				.AsUnitObservable()
				.Subscribe( _onPushed )
				.AddTo( component );
			OnPushed
				.Select( _ => true )
				.Subscribe( _isPushing )
				.AddTo( component );

			// Released
			pushStateChangedSubject
				.Where( p => PushDetectThreshold < Math.Abs( p.Previous ) && Math.Abs( p.Current ) <= PushDetectThreshold )
				.AsUnitObservable()
				.Subscribe( _onReleased )
				.AddTo( component );
			OnReleased
				.Select( _ => false )
				.Subscribe( _isPushing )
				.AddTo( component );

			// Double pushed
			OnPushed
				.TimeInterval()
				.Where( t => t.Interval <= TimeSpan.FromMilliseconds( DoublePushSpanMilliseconds ) )
				.AsUnitObservable()
				.Subscribe( _onDoublePushed )
				.AddTo( component );

			// Long pushed
			OnPushed
				.SelectMany( _ => Observable.Timer( TimeSpan.FromMilliseconds( LongPushSpanMilliseconds ) ) )
				.TakeUntil( OnReleased )
				.RepeatUntilDestroy( component )
				.AsUnitObservable()
				.Subscribe( _onLongPushed )
				.AddTo( component );

			// Dispose
			component.OnDestroyAsObservable()
				.Subscribe( _ =>
				{
					pushStateChangedSubject.OnCompleted();
					Dispose();
				} );
		}

		#region Dispose...
		private bool _disposed = false;
		~GlobalInputProvider()
		{
			Dispose( false );
		}
		public void Dispose()
		{
			Dispose( true, () =>
			{
				_onPushed.OnCompleted();
				_onDoublePushed.OnCompleted();
				_onLongPushed.OnCompleted();
				_onReleased.OnCompleted();
				_isPushing.OnCompleted();
			} );
			GC.SuppressFinalize( this );
		}
		protected virtual void Dispose( bool disposing, Action freeManaged = null, Action freeHandle = null )
		{
			if ( _disposed ) return;
			if ( disposing ) freeManaged?.Invoke();
			freeHandle?.Invoke();
			_disposed = true;
		}
		#endregion
	}
}
