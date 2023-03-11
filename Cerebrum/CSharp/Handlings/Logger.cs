using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Crowolf.Cerebrum.Handlings
{
	public static class Logger
	{
		public struct LogFile
		{
			/// <summary>
			/// Path that places a log file
			/// </summary>
			public string Path => _path;
			private string _path;

			/// <summary>
			/// Message in a log file
			/// </summary>
			public string Message => _message;
			private string _message;

			/// <summary>
			/// Provides a return value of logging structure
			/// </summary>
			/// <param name="path"></param>
			/// <param name="message"></param>
			public LogFile( string path, string message )
			{
				_path = path;
				_message = message;
			}
		}

		public enum LogStates
		{
			Debug, Info, Warn, Error, Fatal
		}

		public static LogFile Log( string message, LogStates state,
			string logFileDirectory = "",
			[CallerMemberName] string memberName = "",
			[CallerFilePath] string sourceFilePath = "",
			[CallerLineNumber] int sourceLineNumber = -1 )
		{
			// Create logging sentence
			var newline = Definitions.NewLine;
			var logstr =
				$"[{state.ToString().ToUpper(),-5}] {DateTime.Now.ToString( "yyyy-MM-dd HH:mm:ss" )}{newline}" +
				$"{memberName} at [{sourceFilePath}:{sourceLineNumber}]{newline}" +
				$"{message}{newline}" +
				$"{newline}";

			// Create log file
			var fileDir = logFileDirectory.IsEmptyApprox()
				? Path.GetDirectoryName( System.Reflection.Assembly.GetExecutingAssembly().Location )
				: logFileDirectory;
			var filepath =
				Path.Combine(
					fileDir,
					DateTime.Now.ToString( "yyyyMMdd" ) + ".log"
					);

			return new LogFile( filepath, logstr );
		}
	}
}
