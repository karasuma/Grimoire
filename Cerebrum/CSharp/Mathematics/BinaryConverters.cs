using System;
using System.Linq;

namespace Crowolf.Cerebrum.Mathematics
{
	public static class BinaryConverters
	{
		/// <summary>
		/// Converts from a number to binary string.
		/// </summary>
		/// <param name="number">Specifies a integer number.</param>
		/// <param name="signed">Specities a flag to handle positive number or not. default is True.</param>
		/// <returns>Represents binary</returns>
		/// <exception cref="ArgumentException">Throws if 'number' is negative numbers but 'signed' is False.</exception>
		public static string ToBits( this int number, bool signed = true )
		{
			var expandNumber = (long)number;
			var isNegative = expandNumber < 0;
			if ( !signed && isNegative )
				throw new ArgumentException( "A parameter 'number' must be positive number when a parameter 'signed' is False." );

			if ( signed && isNegative ) expandNumber *= -1;
			var bits = "";
			while ( expandNumber != 1 )
			{
				bits = $"{expandNumber % 2}{bits}";
				expandNumber /= 2;
			}
			bits = $"1{bits}";

			if ( signed ) bits = ( isNegative ? "1" : "0" ) + bits;
			return bits;
		}

		/// <summary>
		/// Converts from binary string to a number.
		/// </summary>
		/// <param name="binary">Specifies binary represents as string.</param>
		/// <param name="signed">Specifies a flag to handle positive number or not. default is True.</param>
		/// <returns>Represents a number</returns>
		/// <exception cref="ArgumentException">Throws if 'binary' contains other than 0 or 1.</exception>
		public static decimal FromBits( string binary, bool signed = true )
		{
			if ( binary.Length == 0 ) return 0m;
			if ( signed && binary.Length == 1 )
				throw new ArgumentException( "" );
			if ( binary.Any( b => !"01".Contains( b ) ) )
				throw new ArgumentException( "" );

			var sign = (signed && binary[0] == '1') ? -1 : 1;
			ulong digitDec = 1ul << (binary.Length - (signed ? 1 : 0) - 1);
			decimal number = 0;
			foreach ( var bit in binary.Skip( signed ? 1 : 0 ) )
			{
				number += ulong.Parse( $"{bit}" ) * digitDec;
				digitDec >>= 1;
			}

			return number * sign;
		}
	}
}
