using System;
using System.Collections.Generic;
using System.Text;

namespace LogUtils
{
	public class LogEventArgs:EventArgs
	{
		private Log log;
		public Log Log
		{
			get { return log; }
		}

		public LogEventArgs(Log Log)
		{
			this.log = Log;
		}

	}

	public delegate void LogEventHandler(object sender,LogEventArgs e);




}
