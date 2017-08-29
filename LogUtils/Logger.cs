using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Linq;

namespace LogUtils
{
	public static class Logger
	{
		
		public static event LogEventHandler DebugLog;
		public static event LogEventHandler InformationLog;
		public static event LogEventHandler WarningLog;
		public static event LogEventHandler ErrorLog;
		public static event LogEventHandler FatalLog;

		private static bool logToFile;
		private static string logPath;

		private static StreamWriter writer;
		private static FileStream stream;
		private static DateTime lastFail;

		private static int fileSize;
		private static byte maxFiles;

		static Logger()
		{
		}

		public static void StartLogToFile(string LogPath,byte MaxFiles=10,int FileSize=1000000)
		{
			maxFiles = MaxFiles;
			fileSize = FileSize;		
			if (!Directory.Exists(LogPath))
			{
				try
				{
					Directory.CreateDirectory(LogPath);
				}
				catch
				{
					// cannot create directory
				}
			}
			logPath = LogPath;
			logToFile = true;
		}

		public static void StopLogToFile()
		{
			logPath = null;
			logToFile = false;
		}

		private static void WriteToStream(Log Log)
		{
			if (stream == null) CreateStream();
			if (stream != null)
			{
				writer.WriteLine(Log);
				writer.Flush();
				if (stream.Length >= fileSize) CloseStream();
			}
		}

		private static void CreateStream()
		{
			List<string> files;
			int filesToDelete;

			if (stream == null)
			{
				// in case of failure waits for 10 minutes before retrying
				if (DateTime.Now - lastFail < TimeSpan.FromMinutes(10)) return;
			}

			if (stream != null) CloseStream();

			try
			{
				files=Directory.GetFiles(logPath, "Log_*.log").OrderBy(item=>item).ToList();
				if (files.Count>=maxFiles)
				{
					filesToDelete = files.Count - maxFiles;
					for(int t=0;t<filesToDelete;t++)
					{
						try
						{
							File.Delete(files[t]);
						}
						catch
						{
							// cannot delete file
						}
					}
				}
			}
			catch
			{
				// cannot get files
			}
			try
			{
				stream = new FileStream(Path.Combine(logPath, @"Log_" +DateToFileName() + ".log"), FileMode.Create);
				writer = new StreamWriter(stream);
			}
			catch
			{
				lastFail = DateTime.Now;
				stream = null;
			}
		}
		private static void CloseStream()
		{
			if (stream == null) return;
			writer.Flush();
			writer.Close();
			stream.Close();
			stream = null;
		}
		public static string DateToFileName()
		{
			DateTime now;
			now=DateTime.Now;
			return now.Year.ToString() + "-" + now.Month.ToString("00") + "-" + now.Day.ToString("00") + "_" + now.Hour.ToString("00") + "h" + now.Minute.ToString("00") + "_" + now.Second.ToString("00") + "_" + now.Millisecond.ToString("000");
		}


		public static void WriteLog(LogLevels Level, string Module,int ModuleID, string Message)
		{
			WriteLog(new Log(Level,Module,ModuleID,Message));
		}

		public static void WriteLog(Log Log)
		{
			switch (Log.Level)
			{
				case LogLevels.Debug:
					if (DebugLog != null) DebugLog(null, new LogEventArgs(Log));// DebugLog.BeginInvoke(null,new LogEventArgs(Log),null,null);
					break;
				case LogLevels.Information:
					if (InformationLog != null) InformationLog(null, new LogEventArgs(Log)); //InformationLog.BeginInvoke(null, new LogEventArgs(Log), null, null);
					break;
				case LogLevels.Warning:
					if (WarningLog != null) WarningLog(null, new LogEventArgs(Log));// WarningLog.BeginInvoke(null, new LogEventArgs(Log), null, null);
					break;
				case LogLevels.Error:
					if (ErrorLog != null) ErrorLog(null, new LogEventArgs(Log));// ErrorLog.BeginInvoke(null, new LogEventArgs(Log), null, null);
					break;
				case LogLevels.Fatal:
					if (FatalLog != null) FatalLog(null, new LogEventArgs(Log));// FatalLog.BeginInvoke(null, new LogEventArgs(Log), null, null);
					break;

			}
			if (logToFile) WriteToStream(Log);
			//*/
		}
		

		public static string FormatException(string Message, Exception ex)
		{
			return Message + " (" + ex.Message + ")";
		}


	}
}
