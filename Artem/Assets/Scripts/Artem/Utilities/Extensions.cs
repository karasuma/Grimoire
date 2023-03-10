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
		public static Vector2 ToVector2( this Vector3 self )
			=> new Vector2( self.x, self.y );
	}
}
