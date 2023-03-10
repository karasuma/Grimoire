using System;

namespace Crowolf.Artem.Utilities
{
	/// <summary>
	/// https://easings.net/
	/// </summary>
	public static class Easings
	{
		/// <summary>
		/// Moves value between 0 and 1 smoothly.
		/// </summary>
		/// <param name="time">Current moving time between 0 and 1</param>
		/// <param name="easingType">Moving types</param>
		/// <param name="easeInOutType">Moving priority types</param>
		/// <returns>Moved value between 0 and 1</returns>
		public static double Ease(double time, EasingType easingType, EaseInOutType easeInOutType = EaseInOutType.Both)
		{
			time = time < 0.0 ? 0.0 : time > 1.0 ? 1.0 : time;

			// Ease!
			switch( easingType )
			{
				case EasingType.Sine:
					return Sine( time, easeInOutType );
				case EasingType.Quad:
					return Quad( time, easeInOutType );
				case EasingType.Cubic:
					return Cubic( time, easeInOutType );
				case EasingType.Quart:
					return Quart( time, easeInOutType );
				case EasingType.Quint:
					return Quint( time, easeInOutType );
				case EasingType.Expo:
					return Expo( time, easeInOutType );
				case EasingType.Circ:
					return Circ( time, easeInOutType );
				case EasingType.Back:
					return Back( time, easeInOutType );
				case EasingType.Elastic:
					return Elastic( time, easeInOutType );
				case EasingType.Bounce:
					return Bounce( time, easeInOutType );
			}
			return time;
		}

		private static double Sine(double t, EaseInOutType type)
		{
			switch( type )
			{
				case EaseInOutType.In:
					return 1.0 - Math.Cos( ( t * Math.PI ) / 2.0 );
				case EaseInOutType.Out:
					return Math.Sin( ( t * Math.PI ) / 2.0 );
				default:
					return -( Math.Cos( Math.PI * t ) - 1.0 ) / 2.0;
			}
		}

		private static double Quad(double t, EaseInOutType type)
		{
			switch( type )
			{
				case EaseInOutType.In:
					return t * t;
				case EaseInOutType.Out:
					return 1.0 - ( 1.0 - t ) * ( 1.0 - t );
				default:
					return t < .5 ? 2.0 * t * t : 1.0 - Math.Pow( -2.0 * t + 2.0, 2.0 ) / 2.0;
			}
		}

		private static double Cubic(double t, EaseInOutType type)
		{
			switch( type )
			{
				case EaseInOutType.In:
					return t * t * t;
				case EaseInOutType.Out:
					return 1.0 - Math.Pow( 1.0 - t, 3.0 );
				default:
					return t < .5 ? 4.0 * t * t * t : 1.0 - Math.Pow( -2.0 * t + 2.0, 3.0 ) / 2.0;
			}
		}

		private static double Quart(double t, EaseInOutType type)
		{
			switch( type )
			{
				case EaseInOutType.In:
					return t * t * t * t;
				case EaseInOutType.Out:
					return 1.0 - Math.Pow( 1.0 - t, 4.0 );
				default:
					return t < .5 ? 8.0 * t * t * t * t : 1.0 - Math.Pow( -2.0 * t + 2.0, 4.0 ) / 2.0;
			}
		}

		private static double Quint(double t, EaseInOutType type)
		{
			switch( type )
			{
				case EaseInOutType.In:
					return t * t * t * t * t;
				case EaseInOutType.Out:
					return 1.0 - Math.Pow( 1.0 - t, 5.0 );
				default:
					return t < .5 ? 16.0 * t * t * t * t * t : 1.0 - Math.Pow( -2.0 * t + 2.0, 5.0 ) / 2.0;
			}
		}

		private static double Expo(double t, EaseInOutType type)
		{
			switch( type )
			{
				case EaseInOutType.In:
					return t == 0.0 ? 0.0 : Math.Pow( 2.0, 10.0 * t - 10.0 );
				case EaseInOutType.Out:
					return t == 1.0 ? 1.0 : 1.0 - Math.Pow( 2.0, -10.0 * t );
				default:
					return t == 0
						? 0
						: t == 1.0
							? 1f
							: t < .5
								? Math.Pow( 2.0, 20.0 * t - 10.0 ) / 2.0
								: ( 2.0 - Math.Pow( 2.0, -20.0 * t + 10.0 ) ) / 2.0;
			}
		}

		private static double Circ(double t, EaseInOutType type)
		{
			switch( type )
			{
				case EaseInOutType.In:
					return 1.0 - Math.Sqrt( 1.0 - Math.Pow( t, 2.0 ) );
				case EaseInOutType.Out:
					return Math.Sqrt( 1.0 - Math.Pow( t - 1.0, 2.0 ) );
				default:
					return t < .5
						? ( 1.0 - Math.Sqrt( 1.0 - Math.Pow( 2.0 * t, 2.0 ) ) ) / 2.0
						: ( Math.Sqrt( 1.0 - Math.Pow( -2.0 * t + 2.0, 2.0 ) ) + 1.0 ) / 2.0;
			}
		}

		private static double Back(double t, EaseInOutType type)
		{
			var c1 = 1.70158;
			var c2 = c1 * 1.525;
			var c3 = c1 + 1.0;
			switch( type )
			{
				case EaseInOutType.In:
					return c3 * t * t * t - c1 * t * t;
				case EaseInOutType.Out:
					return 1.0 + c3 * Math.Pow( t - 1.0, 3.0 ) + c1 * Math.Pow( t - 1.0, 2.0 );
				default:
					return t < .5
						? ( Math.Pow( 2.0 * t, 2.0 ) * ( ( c2 + 1.0 ) * 2.0 * t - c2 ) ) / 2.0
						: ( Math.Pow( 2.0 * t - 2.0, 2.0 ) * ( ( c2 + 1.0 ) * ( t * 2.0 - 2.0 ) + c2 ) + 2.0 ) / 2.0;
			}
		}

		private static double Elastic(double t, EaseInOutType type)
		{
			var c4 = ( 2.0 * Math.PI ) / 3.0;
			var c5 = ( 2.0 * Math.PI ) / 4.5;
			switch( type )
			{
				case EaseInOutType.In:
					return t == 0.0 ? 0.0 : t == 1.0 ? 1.0 : -Math.Pow( 2.0, 10.0 * t - 10.0 ) * Math.Sin( ( t * 10.0 - 10.75 ) * c4 );
				case EaseInOutType.Out:
					return t == 0.0 ? 0 : t == 1.0 ? 1.0 : Math.Pow( 2.0, -10.0 * t ) * Math.Sin( ( t * 10.0 - 0.75 ) * c4 ) + 1.0;
				default:
					return t == 0.0 ? 0.0 : t == 1.0 ? 1.0 : t < .5
						? -( Math.Pow( 2.0, 20.0 * t - 10.0 ) * Math.Sin( ( 20.0 * t - 11.125 ) * c5 ) ) / 2.0
						: ( Math.Pow( 2.0, -20.0 * t + 10.0 ) * Math.Sin( ( 20.0 * t - 11.125 ) * c5 ) ) / 2.0 + 1.0;
			}
		}

		private static double Bounce(double t, EaseInOutType type)
		{
			var n1 = 7.5625;
			var d1 = 2.75;
			var easeOutBounce = new Func<double, double>( t2 =>
			{
				if( t2 < 1.0 / d1 )
					return n1 * t2 * t2;
				else if( t2 < 2.0 / d1 )
					return n1 * ( t2 -= 1.5 / d1 ) * t2 + 0.75;
				else if( t2 < 2.5 / d1 )
					return n1 * ( t2 -= 2.25 / d1 ) * t2 + 0.9375;
				else
					return n1 * ( t2 -= 2.625 / d1 ) * t2 + 0.984375;
			} );
			switch( type )
			{
				case EaseInOutType.In:
					return 1.0 - easeOutBounce( 1.0 - t );
				case EaseInOutType.Out:
					return easeOutBounce( t );
				default:
					return t < .5f
						? ( 1.0 - easeOutBounce( 1.0 - 2.0 * t ) ) / 2.0
						: ( 1.0 + easeOutBounce( 2.0 * t - 1.0 ) ) / 2.0;
			}
		}
	}

	public enum EasingType
	{
		Normal,
		Sine,
		Quad,
		Cubic,
		Quart,
		Quint,
		Expo,
		Circ,
		Back,
		Elastic,
		Bounce
	}

	public enum EaseInOutType
	{
		In, Out, Both
	}
}
