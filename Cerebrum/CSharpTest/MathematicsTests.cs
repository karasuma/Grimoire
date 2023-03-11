using System.Linq;

namespace CSharpTest
{
	public class BinaryConvertersTests
	{
		[Test]
		public void CanConvertToBits()
		{
			// Arrange
			var expects = new List<Tuple<int, bool, string>>()
			{
				new Tuple<int, bool, string>(5, false, "101"),
				new Tuple<int, bool, string>(-11, true, "11011"),
				new Tuple<int, bool, string>(18, true, "010010"),
				new Tuple<int, bool, string>(int.MaxValue, true, "01111111111111111111111111111111"),
				new Tuple<int, bool, string>(int.MinValue, true, "110000000000000000000000000000000")
			};

			// Act
			var actuals = new List<KeyValuePair<bool, string>>();
			foreach ( var expect in expects )
			{
				var actual = expect.Item1.ToBits(expect.Item2);
				var message = $"{expect.Item1} -> {actual}";
				actuals.Add( new KeyValuePair<bool, string>( actual == expect.Item3, message ) );
			}

			// Assert
			Assert.IsTrue(
				actuals.Select( a => a.Key ).All( passed => passed ),
				string.Join( "\n", actuals.Select( a => a.Value ) )
			);
		}

		[Test]
		public void CannotConvertToBits()
		{
			// Arrange
			var test = new Tuple<int, bool, string>(-1, false, "100000000000000000000000000000001");

			// Act
			var actual = false;
			var equalizeTest = false;
			try
			{
				equalizeTest = test.Item1.ToBits( test.Item2 ) == test.Item3;
			}
			catch ( ArgumentException )
			{
				actual = true;
			}
			catch { /* Do nothing if exceptions throw other than ArgumentException */ }

			// Assert
			Assert.IsTrue( actual, "Test passed to convert from -1 to binary string as unsigned." );
			Assert.IsFalse( equalizeTest, "?" );
		}

		[Test]
		public void CanConvertFromBits()
		{
			// Arrange
			var expects = new List<Tuple<string, bool, decimal>>()
			{
				new Tuple<string, bool, decimal>("101", false, 5m),
				new Tuple<string, bool, decimal>("11011", true, -11m),
				new Tuple<string, bool, decimal>("010010", true, 18m),
				new Tuple<string, bool, decimal>("01111111111111111111111111111111", true, int.MaxValue),
				new Tuple<string, bool, decimal>("110000000000000000000000000000000", true, int.MinValue)
			};

			// Act
			var actuals = new List<KeyValuePair<bool, string>>();
			foreach ( var expect in expects )
			{
				var actual = BinaryConverters.FromBits(expect.Item1, expect.Item2);
				var message = $"{expect.Item1} -> {actual}";
				actuals.Add( new KeyValuePair<bool, string>( actual == expect.Item3, message ) );
			}

			// Assert
			Assert.IsTrue(
				actuals.Select( a => a.Key ).All( passed => passed ),
				string.Join( "\n", actuals.Select( a => a.Value ) )
			);
		}

		[Test]
		public void CannotConvertFromBits()
		{
			// Arrange
			var tests = new List<Tuple<string, bool, decimal>>()
			{
				new Tuple<string, bool, decimal>("0123", true, 4m),
				new Tuple<string, bool, decimal>("1", true, -1m),
				new Tuple<string, bool, decimal>("0", true, 0m)
			};

			// Act
			var actuals = new List<bool>();
			var passedConversionBits = new List<decimal>();
			var threwExceptions = new List<Exception>();
			foreach ( var test in tests )
			{
				try
				{
					var dec = BinaryConverters.FromBits( test.Item1, test.Item2 );
					passedConversionBits.Add( dec );
					actuals.Add( false );
				}
				catch ( ArgumentException )
				{
					actuals.Add( true );
				}
				catch ( Exception ex )
				{
					threwExceptions.Add( ex );
				}
			}

			// Assert
			var passedConversions = actuals.Contains( false );
			var throwsOtherExceptions = actuals.Count != tests.Count;
			Assert.IsFalse(
				passedConversions,
				$"Passed convertions: {string.Join( '\n', passedConversionBits )}"
			);
			Assert.IsFalse(
				throwsOtherExceptions,
				$"Throws other exceptions: {string.Join( '\n', threwExceptions )}"
			);
		}
	}
}