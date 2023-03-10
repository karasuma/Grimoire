using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Crowolf.Artem.Utilities
{
	public static class MiscExtensions
	{
		public static bool IsUnityNull( this Component self ) => self == null;

		public static float BoolTo01( this bool self ) => self ? 1f : 0f;
	}

	public static class ComponentExtensions
	{
		public static void RemoveComponent<T>( this T self ) where T : Component
			=> GameObject.Destroy( self.GetComponent<T>() );
		public static void RemoveComponent<T>( this GameObject self ) where T : Component
			=> GameObject.Destroy( self.GetComponent<T>() );
	}

	public static class VectorExtensions
	{
		#region Vector2
		public static Vector2 ToVector2( this Vector3 self )
			=> new Vector2( self.x, self.y );
		public static Vector2 ToVector2( this Vector4 self )
			=> new Vector2( self.x, self.y );
		public static Vector2 Replace( this Vector2 self, string pattern = "xy" )
		{
			const string defaultPattern = "xy";
			const int vectorLength = 2;
			var lowerPattern = pattern.ToLowerInvariant();
			// Error
			var incorrectLength = lowerPattern.Length != vectorLength;
			var incorrectPattern = lowerPattern.Any( p => !defaultPattern.Contains(p) );
			if ( incorrectLength || incorrectPattern )
				throw new ArgumentException( $"`{pattern}` received. 'pattern' needs only 2 characters, x and y. " );
			// Replace
			var vectorArray = new float[vectorLength] { self.x, self.y };
			var newVector2 = lowerPattern.Select(p => vectorArray[defaultPattern.IndexOf(p)]).ToArray();
			return new Vector2( newVector2[0], newVector2[1] );
		}
		#endregion

		#region Vector3
		public static Vector3 ToVector3( this Vector2 self, float z = 0f )
			=> new Vector3( self.x, self.y, z );
		public static Vector3 ToVector3( this Vector4 self )
			=> new Vector3( self.x, self.y, self.z );
		public static Vector3 Replace( this Vector3 self, string pattern = "xyz" )
		{
			const string defaultPattern = "xyz";
			const int vectorLength = 3;
			var lowerPattern = pattern.ToLowerInvariant();
			// Error
			var incorrectLength = lowerPattern.Length != vectorLength;
			var incorrectPattern = lowerPattern.Any( p => !defaultPattern.Contains(p) );
			if ( incorrectLength || incorrectPattern )
				throw new ArgumentException( $"`{pattern}` received. 'pattern' needs only 3 characters, x,y and z. " );
			// Replace
			var vectorArray = new float[vectorLength] { self.x, self.y, self.z };
			var newVector2 = lowerPattern.Select(p => vectorArray[defaultPattern.IndexOf(p)]).ToArray();
			return new Vector3( newVector2[0], newVector2[1], newVector2[2] );
		}
		#endregion

		#region Vector4
		public static Vector4 ToVector4( this Vector2 self, float z = 0f, float w = 0f )
			=> new Vector4( self.x, self.y, z, w );
		public static Vector4 ToVector4( this Vector3 self, float w = 0f )
			=> new Vector4( self.x, self.y, self.z, w );
		public static Vector4 Replace( this Vector4 self, string pattern = "xyzw" )
		{
			const string defaultPattern = "xyzw";
			const int vectorLength = 4;
			var lowerPattern = pattern.ToLowerInvariant();
			// Error
			var incorrectLength = lowerPattern.Length != vectorLength;
			var incorrectPattern = lowerPattern.Any( p => !defaultPattern.Contains(p) );
			if ( incorrectLength || incorrectPattern )
				throw new ArgumentException( $"`{pattern}` received. 'pattern' needs only 3 characters, x,y and z. " );
			// Replace
			var vectorArray = new float[vectorLength] { self.x, self.y, self.z, self.w };
			var newVector2 = lowerPattern.Select(p => vectorArray[defaultPattern.IndexOf(p)]).ToArray();
			return new Vector4( newVector2[0], newVector2[1], newVector2[2], newVector2[3] );
		}
		#endregion
	}
}
