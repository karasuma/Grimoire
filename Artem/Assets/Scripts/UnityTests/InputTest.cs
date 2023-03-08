using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using Crowolf.Artem.InputTools;
using UniRx;
using System.Linq;
using UniRx.Triggers;
using Crowolf.Artem.Utilities;
using System;

namespace Crowolf.UnityTests
{
	#region Modules for this test ...
	public class InputProvider : GlobalInputProvider
	{
		public bool VirtualInput { get; set; } = false;

		public InputProvider( Component component ) : base( component )
		{
			// Do nothing
		}

		protected override void InitializePushPowerSource( Component component )
		{
			_pushPower = component.UpdateAsObservable()
				.Select( _ => Enable && VirtualInput )
				.DistinctUntilChanged()
				.Select( pushed => pushed.BoolTo01() )
				.ToReadOnlyReactiveProperty( 0f )
				.AddTo( component );
		}
	}

	public class InputTestComponent : MonoBehaviour
	{
		public InputProvider Provider { get; private set; }
	}
	#endregion

	public class InputTest
	{
		private GameObject _attachedGameObject;
		private InputTestComponent _inputComponent;
		private InputProvider _provider;

		[UnitySetUp]
		public IEnumerator UnitySetUp()
		{
			_attachedGameObject = new GameObject( "Input Test" );
			_attachedGameObject.AddComponent<InputTestComponent>();
			_inputComponent = _attachedGameObject.GetComponent<InputTestComponent>();
			_provider = new InputProvider( _inputComponent );
			yield break;
		}

		[UnityTearDown]
		public IEnumerator UnityTearDown()
		{
			GameObject.Destroy( _attachedGameObject );
			yield break;
		}

		[UnityTest]
		public IEnumerator OnPushed()
		{
			// Arrange
			var pushed = false;
			_provider.OnPushed
				.Subscribe( _ => pushed = true )
				.AddTo( _inputComponent );

			// Act
			_provider.VirtualInput = true;
			yield return new WaitForSeconds( 0.01f ); // Wait for 10ms
			_provider.VirtualInput = false;

			// Assert
			Assert.IsTrue( pushed );
		}

		[UnityTest]
		public IEnumerator OnReleased()
		{
			// Arrange
			var released = false;
			_provider.OnReleased
				.Subscribe( _ => released = true )
				.AddTo( _inputComponent );

			// Act
			_provider.VirtualInput = true;
			yield return new WaitForSeconds( 0.01f ); // Wait for 10ms
			_provider.VirtualInput = false;
			yield return new WaitForFrames( 1 );

			// Assert
			Assert.IsTrue( released );
		}

		[UnityTest]
		public IEnumerator OnDoublePushed()
		{
			// Arrange
			var doublePushSucceeded = false;
			var doublePushFailed = true;
			var failedTest = true;
			_provider.OnDoublePushed
				.Subscribe( _ =>
				{
					if ( failedTest ) doublePushFailed = false;
					else doublePushSucceeded = true;
				} )
				.AddTo( _inputComponent );
			_provider.DoublePushSpanMilliseconds = 200; // 200ms

			// Act
			/* failed */
			var boreingTimeSec = 0.25f; // Larger than double push span (250ms)
			yield return new WaitForSeconds( boreingTimeSec );
			_provider.VirtualInput = true;
			yield return new WaitForSeconds( boreingTimeSec );
			_provider.VirtualInput = false;
			yield return new WaitForSeconds( boreingTimeSec );
			_provider.VirtualInput = true; // <- approx. 500ms passed
			yield return new WaitForSeconds( boreingTimeSec );
			_provider.VirtualInput = false;
			/* succeeded */
			failedTest = false;
			boreingTimeSec = 0.05f; // Lower than double push span (50ms)
			yield return new WaitForSeconds( boreingTimeSec );
			_provider.VirtualInput = true;
			yield return new WaitForSeconds( boreingTimeSec );
			_provider.VirtualInput = false;
			yield return new WaitForSeconds( boreingTimeSec );
			_provider.VirtualInput = true; // <- approx. 100ms passed
			yield return new WaitForSeconds( boreingTimeSec );
			_provider.VirtualInput = false;
			yield return new WaitForFrames( 1 );

			// Assert
			Assert.IsTrue( doublePushSucceeded );
			Assert.IsTrue( doublePushFailed );
		}

		[UnityTest]
		public IEnumerator OnLongPushed()
		{
			// Arrange
			var longPushSucceeded = false;
			var longPushFailed = true;
			var failedTest = true;
			_provider.OnLongPushed
				.Subscribe( _ =>
				{
					if ( failedTest ) longPushFailed = false;
					else longPushSucceeded = true;
				} )
				.AddTo( _inputComponent );
			_provider.LongPushSpanMilliseconds = 200; // 200ms

			// Act
			/* failed */
			var boreingTimeSec = 0.05f; // Larger than double push span (50ms)
			_provider.VirtualInput = true;
			yield return new WaitForSeconds( boreingTimeSec );
			_provider.VirtualInput = false;
			/* succeeded */
			failedTest = false;
			boreingTimeSec = 0.3f; // Lower than double push span (300ms)
			_provider.VirtualInput = true;
			yield return new WaitForSeconds( boreingTimeSec );
			_provider.VirtualInput = false;

			// Assert
			Assert.IsTrue( longPushSucceeded );
			Assert.IsTrue( longPushFailed );
		}
	}
}