using System;
using UnityEngine;

namespace Crowolf.Ars.Utilities
{
	public sealed class WaitForFrames : CustomYieldInstruction
	{
		private int _targetFrameCount;

		public override bool keepWaiting => Time.frameCount < _targetFrameCount;

		/// <summary>
		/// Suspends the coroutine execution for the given amount of frames using scaled time.
		/// </summary>
		/// <param name="frames">Frame counts to suspend coroutine execution</param>
		public WaitForFrames( int frames )
		{
			if ( frames < 0 )
				throw new ArgumentException( $"Frames must be positive number but it's `{frames}`." );
			_targetFrameCount = Time.frameCount + frames;
		}
	}
}
