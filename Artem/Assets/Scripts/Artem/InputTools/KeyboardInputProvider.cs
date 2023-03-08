using Crowolf.Artem.Utilities;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Crowolf.Artem.InputTools
{
	public class KeyboardInputProvider : GlobalInputProvider
	{
		protected KeyCode _keyCode = default;

		public KeyboardInputProvider( Component component, KeyCode key ) : base( component )
			=> _keyCode = key;

		protected override void InitializePushPowerSource( Component component )
		{
			_pushPower = component.UpdateAsObservable()
				.Select( _ => Enable && Input.GetKey( _keyCode ) )
				.DistinctUntilChanged()
				.Select( pushed => pushed.BoolTo01() )
				.ToReadOnlyReactiveProperty()
				.AddTo( component );
		}
	}
}
