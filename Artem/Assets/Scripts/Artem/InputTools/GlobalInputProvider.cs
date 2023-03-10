using System;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Crowolf.Artem.InputTools
{
	public enum MouseCode : int
	{
		None = -1,
		Left = 0,
		Right = 1,
		Middle = 2
	}

	public abstract class GlobalInputProvider : IInputProvider, IDisposable
	{
		public bool Enable { get; set; } = true;

		public IReadOnlyReactiveProperty<float> PushPower => _pushPower;
		protected ReadOnlyReactiveProperty<float> _pushPower = default;

		public IObservable<Unit> OnPushed => _onPushed;
		private Subject<Unit> _onPushed = new Subject<Unit>();

		public IObservable<Unit> OnDoublePushed => _onDoublePushed;
		private Subject<Unit> _onDoublePushed = new Subject<Unit>();

		public IObservable<Unit> OnLongPushed => _onLongPushed;
		private Subject<Unit> _onLongPushed = new Subject<Unit>();

		public IObservable<Unit> OnReleased => _onReleased;
		private Subject<Unit> _onReleased = new Subject<Unit>();

		public IReadOnlyReactiveProperty<bool> IsPushing => _isPushing.ToReadOnlyReactiveProperty();
		private ReactiveProperty<bool> _isPushing = new ReactiveProperty<bool>( false );

		public int DoublePushSpanMilliseconds
		{
			get => _doublePushSpanMilliseconds;
			set => _doublePushSpanMilliseconds = Math.Max( 0, value );
		}
		private int _doublePushSpanMilliseconds = 170;
		public int LongPushSpanMilliseconds
		{
			get => _longPushSpanMilliseconds;
			set => _longPushSpanMilliseconds = Math.Max( 0, value );
		}
		private int _longPushSpanMilliseconds = 600;
		public float PushDetectThreshold
		{
			get => _pushDetectThreshold;
			set => _pushDetectThreshold = Mathf.Clamp( value, 0.01f, 1f );
		}
		private float _pushDetectThreshold = 0.05f;

		public KeyCode Key => _keyCode;
		private KeyCode _keyCode = KeyCode.None;
		public MouseCode Mouse => _mouseCode;
		private MouseCode _mouseCode = MouseCode.None;

		protected abstract ReadOnlyReactiveProperty<float> InitializePushPowerSource( Component component );

		public GlobalInputProvider(
			Component component,
			KeyCode keyCode = KeyCode.None,
			MouseCode mouseCode = MouseCode.None )
		{
			if ( _pushPower != default ) // Dispose when _pushPower has initialized
			{
				_pushPower.Dispose();
				_pushPower = null;
			}
			_keyCode = keyCode;
			_mouseCode = mouseCode;
			_pushPower = InitializePushPowerSource( component );

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
				.Subscribe( _ => _isPushing.Value = true )
				.AddTo( component );

			// Released
			pushStateChangedSubject
				.Where( p => PushDetectThreshold < Math.Abs( p.Previous ) && Math.Abs( p.Current ) <= PushDetectThreshold )
				.AsUnitObservable()
				.Subscribe( _onReleased )
				.AddTo( component );
			OnReleased
				.Subscribe( _ => _isPushing.Value = false )
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

		public GlobalInputProvider( Component component, KeyCode keyCode )
			: this( component, keyCode, MouseCode.None ) { }

		public GlobalInputProvider( Component component, MouseCode mouseCode )
			: this( component, KeyCode.None, mouseCode ) { }

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
