using System;

namespace Crowolf.Cerebrum
{
	public static class Extensions
	{
		/// <summary>
		/// Convert boolean to float value.
		/// </summary>
		/// <param name="self"></param>
		/// <returns>true: 1f / false: 0f</returns>
		public static float BoolTo01( this bool self ) => self ? 1f : 0f;

		/// <summary>
		/// Descrease current value. If current value has already 0, the value changes Max - 1.
		/// </summary>
		/// <param name="current"></param>
		/// <param name="max"></param>
		/// <returns></returns>
		public static int Decrease( this int current, int max ) => ( current + ( max - 1 ) ) % max;

		/// <summary>
		/// Increase current value. If current value has already Max - 1, the value changes 0.
		/// </summary>
		/// <param name="current"></param>
		/// <param name="max"></param>
		/// <returns></returns>
		public static int Increase( this int current, int max ) => ( current + 1 ) % max;

		/// <summary>
		/// Check value is between min and max.
		/// </summary>
		/// <param name="current"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <param name="includeEdge">Set flag to include Min and Max</param>
		/// <returns></returns>
		public static bool IsInRange( this double current, double min, double max, bool includeEdge = true )
		{
			if ( min > max )
			{
				var temp = min;
				min = max;
				max = temp;
			}
			if ( includeEdge )
				return min <= current && current <= max;
			return min < current && current < max;
		}

		/// <summary>
		/// Check value is between min and max.
		/// </summary>
		/// <param name="current"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <param name="includeEdge">Set flag to include Min and Max</param>
		/// <returns></returns>
		public static bool IsInRange( this float current, float min, float max, bool includeEdge = true )
			=> ( (double)current ).IsInRange( min, max, includeEdge );

		/// <summary>
		/// Check value is between min and max.
		/// </summary>
		/// <param name="current"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <param name="includeEdge">Set flag to include Min and Max</param>
		/// <returns></returns>
		public static bool IsInRange( this int current, int min, int max, bool includeEdge = true )
			=> ( (double)current ).IsInRange( min, max, includeEdge );

		/// <summary>
		/// Indicates whether a specified string is empty approximately.
		/// </summary>
		/// <param name="self"></param>
		/// <returns>True if the string is empty approximately; otherwise, false.</returns>
		public static bool IsEmptyApprox( this string self )
			=> string.IsNullOrEmpty( self.Trim() ) || string.IsNullOrWhiteSpace( self.Trim() );
	}

	public static class Definitions
	{
		/// <summary>
		/// Gets Environment.NewLine
		/// </summary>
		public static string NewLine => Environment.NewLine;
	}
}
