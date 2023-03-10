using Crowolf.Ars.InputTools;
using Crowolf.Ars.Sandbox;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Crowolf.Sandbox
{
	public class MouseInputCheck : MonoBehaviour
	{
		private void Start()
		{
			var input = new MouseInputProvider( this, MouseCode.Left );
			input.OnDragging
				.Select( move => $"{move.Distance} / {move.Magnitude}" )
				.Subscribe( txt => DebugDrawer.Log( txt ) )
				.AddTo( this );
		}
	}
}