using Crowolf.Artem.Utilities;
using System;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Crowolf.Artem.InputTools
{
	/// <summary>
	/// Representation of mouse moving distance and magnitude of distance.
	/// </summary>
	public sealed class MouseMovement
	{
		/// <summary>
		/// 2D vector of mouse movement between previous position and next position
		/// </summary>
		public Vector2 Distance => _distance;
		private Vector2 _distance = new Vector2();

		/// <summary>
		/// Magnitude of mouse movement distance 
		/// </summary>
		public float Magnitude => _magnitude;
		private float _magnitude = 0f;

		public MouseMovement( Vector2 distance, float magnitude )
		{
			_distance = distance;
			_magnitude = magnitude;
		}
		public MouseMovement() : this( Vector2.zero, 0f ) { }
	}

	public class MouseInputProvider : GlobalInputProvider
	{
		public IObservable<MouseMovement> OnDragging => _onDragging;
		private Subject<MouseMovement> _onDragging = new Subject<MouseMovement>();

		public IObservable<float> MouseWheel => _mouseWheel;
		private Subject<float> _mouseWheel = new Subject<float>();

		public MouseInputProvider( Component component, MouseCode mouseCode ) : base( component, mouseCode )
		{
			// Dragging
			component.UpdateAsObservable()
				.Where( _ => IsPushing.Value )
				.Select( _ => Input.mousePosition.ToVector2() )
				.Pairwise()
				.Select( pos =>
				{
					var distance = pos.Current - pos.Previous;
					var power = distance.magnitude;
					return new MouseMovement( distance, power );
				} )
				.Subscribe( _onDragging )
				.AddTo( component );
			component.UpdateAsObservable()
				.Where( _ => !IsPushing.Value )
				.Select( _ => new MouseMovement( Vector2.zero, 0f ) )
				.Subscribe( _onDragging )
				.AddTo( component );

			// Mouse wheel
			component.UpdateAsObservable()
				.Select( _ => Input.mouseScrollDelta.y )
				.Subscribe( _mouseWheel )
				.AddTo( component );
		}

		protected override ReadOnlyReactiveProperty<float> InitializePushPowerSource( Component component )
		{
			return component.UpdateAsObservable()
				.Select( _ => Enable && Input.GetMouseButton( (int)Mouse ) )
				.DistinctUntilChanged()
				.Select( pushed => pushed.BoolTo01() )
				.ToReadOnlyReactiveProperty()
				.AddTo( component );
		}
	}
}
