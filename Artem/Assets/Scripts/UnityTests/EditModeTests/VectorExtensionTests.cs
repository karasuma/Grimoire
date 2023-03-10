using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using Crowolf.Artem.Utilities;

namespace Crowolf.EditModeTests.ExtensionTests
{
	public class VectorExtensionTests
	{
		[Test]
		public void Vector2ReplaceTest()
		{
			// Arrange
			var bvec = new Vector2(3, 5f); // base vector
			var expects = new List<KeyValuePair<string, Vector2>>()
			{
				new KeyValuePair<string, Vector2>("xy", new Vector2(bvec.x, bvec.y)),
				new KeyValuePair<string, Vector2>("yx", new Vector2(bvec.y, bvec.x)),
				new KeyValuePair<string, Vector2>("yy", new Vector2(bvec.y, bvec.y))
			};

			// Act
			var actuals = new List<bool>();
			var msg = "";
			foreach ( var expect in expects )
			{
				var actual = bvec.Replace(expect.Key);
				actuals.Add( actual == expect.Value );
				msg += $"{expect.Key} -> {actual}\n";
			}

			// Assert
			Assert.IsTrue( actuals.All( passed => passed ), msg );
		}

		[Test]
		public void Vector3ReplaceTest()
		{
			// Arrange
			var bvec = new Vector3(3f, 5f, 7f);
			var expects = new List<KeyValuePair<string, Vector3>>()
			{
				new KeyValuePair<string, Vector3>("xyz", new Vector3(bvec.x, bvec.y, bvec.z)),
				new KeyValuePair<string, Vector3>("xyz", new Vector3(bvec.x, bvec.y, bvec.z)),
				new KeyValuePair<string, Vector3>("yyx", new Vector3(bvec.y, bvec.y, bvec.x)),
				new KeyValuePair<string, Vector3>("zzz", new Vector3(bvec.z, bvec.z, bvec.z))
			};

			// Act
			var actuals = new List<bool>();
			var msg = "";
			foreach ( var expect in expects )
			{
				var actual = bvec.Replace(expect.Key);
				actuals.Add( actual == expect.Value );
				msg += $"{expect.Key} -> {actual}\n";
			}

			// Assert
			Assert.IsTrue( actuals.All( passed => passed ), msg );
		}

		[Test]
		public void Vector4ReplaceTest()
		{
			// Arrange
			var bvec = new Vector4(3f, 5f, 7f, 9f);
			var expects = new List<KeyValuePair<string, Vector4>>()
			{
				new KeyValuePair<string, Vector4>("xyzw", new Vector4(bvec.x, bvec.y, bvec.z, bvec.w)),
				new KeyValuePair<string, Vector4>("wzyx", new Vector4(bvec.w, bvec.z, bvec.y, bvec.x)),
				new KeyValuePair<string, Vector4>("yxxz", new Vector4(bvec.y, bvec.x, bvec.x, bvec.z)),
				new KeyValuePair<string, Vector4>("zzzz", new Vector4(bvec.z, bvec.z, bvec.z, bvec.z))
			};

			// Act
			var actuals = new List<bool>();
			var msg = "";
			foreach ( var expect in expects )
			{
				var actual = bvec.Replace(expect.Key);
				actuals.Add( actual == expect.Value );
				msg += $"{expect.Key} -> {actual}\n";
			}

			// Assert
			Assert.IsTrue( actuals.All( passed => passed ), msg );
		}
	}
}
