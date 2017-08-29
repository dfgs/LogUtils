using System;
using System.Collections.Generic;
using System.Text;

namespace LogUtils
{
	public class Log
	{
		private DateTime dateTime;
		public DateTime DateTime
		{
			get { return dateTime; }
			set { dateTime = value; }
		}

		private LogLevels level;
		public LogLevels Level
		{
			get { return level; }
			set { level = value; }
		}

		private string module;
		public string Module
		{
			get { return module; }
			set { module = value; }
		}

		private string message;
		public string Message
		{
			get { return message; }
			set { message = value; }
		}

		private int moduleID;
		public int ModuleID
		{
			get { return moduleID; }
			set { moduleID = value; }
		}
		public Log(LogLevels Level, string Module,int ModuleID, string Message)
		{
			this.dateTime = DateTime.Now;
			this.level = Level;
			this.module = Module;
			this.moduleID = ModuleID;
			this.message = Message;
		}


		public override string ToString()
		{
			return dateTime.ToString() + " | " + level.ToString() + " | " + module + " | " + moduleID.ToString() + " | " + message;
		}


	}
}
