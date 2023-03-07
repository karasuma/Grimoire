using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Crowolf.Artem.Utilities
{
	public static class Extensions
	{
		public static bool IsUnityNull( this Component self ) => self == null;
	}
}
