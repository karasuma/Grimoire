using Crowolf.Ars.Utilities;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Crowolf.Ars.InputTools
{
	public class KeyboardInputProvider : GlobalInputProvider
	{
		public KeyboardInputProvider( Component component, KeyCode keyCode) : base( component, keyCode )
		{
		}

		protected override ReadOnlyReactiveProperty<float> InitializePushPowerSource( Component component )
		{
			return component.UpdateAsObservable()
				.Select( _ => Enable && Input.GetKey( Key ) )
				.DistinctUntilChanged()
				.Select( pushed => pushed.BoolTo01() )
				.ToReadOnlyReactiveProperty()
				.AddTo( component );
		}
	}
}
