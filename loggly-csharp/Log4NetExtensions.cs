using System;
using System.Collections.Generic;
using System.Configuration;

using log4net;
using log4net.Core;

using Newtonsoft.Json;

namespace Loggly {
	public static class Log4NetExtensions {
		public const string FATAL = "fatal";
		public const string ERROR = "error";
		public const string WARN = "warning";
		public const string INFO = "info";
		public const string DEBUG = "debug";

		private static readonly Logger _logglyLogger;

		static Log4NetExtensions() {
			var customerToken = ConfigurationManager.AppSettings["LogglyCustomerToken"];
			if (!String.IsNullOrEmpty(customerToken)) {
				_logglyLogger = new Logger(customerToken);
			}
		}

		public static void Publish(this ILog logger, object message, string category, Dictionary<string, object> data, Exception exception) {
			if ((category == DEBUG && logger.IsDebugEnabled)
				|| (category == INFO && logger.IsInfoEnabled)
				|| (category == WARN && logger.IsWarnEnabled)
				|| (category == ERROR && logger.IsErrorEnabled)
				|| (category == FATAL && logger.IsFatalEnabled)) {
				// only publish if enabled in Log4net config
				Publish(message, category, data, exception, logger.Logger.Name);
			}
		}

		private static void Publish(object message, string category, Dictionary<string, object> data, Exception exception, string source) {
			if (_logglyLogger != null) {
				var loggingEvent = new LoggingEvent(new LoggingEventData());
				var dictionary = new Dictionary<string, object>(data ?? new Dictionary<string, object>())
				{
					{"message", message ?? ""},
					{"source", String.IsNullOrEmpty(source) ? "?" : source},
					{"severity", category},
					{"appdomain", loggingEvent.Domain },
					{"thread-identity", loggingEvent.Identity },
					{"thread-name", loggingEvent.ThreadName },
					{"username", loggingEvent.UserName }
				};
				if (exception != null) {
					dictionary.Add("exception", exception.Message);
					dictionary.Add("stacktrace", exception.StackTrace);
					if (exception.InnerException != null) {
						dictionary.Add("innerexception", exception.Message);
						dictionary.Add("innerstack", exception.StackTrace);
					}
				}
				_logglyLogger.LogJson(JsonConvert.SerializeObject(dictionary), source, Environment.MachineName);
			}
		}

		#region Fatal

		public static void PublishFatal(this ILog logger, object message) {
			logger.Fatal(message);
			if (_logglyLogger != null && message != null) {
				Publish(message, FATAL, null, null, logger.Logger.Name);
			}
		}

		public static void PublishFatal(this ILog logger, object message, Exception exception) {
			logger.Fatal(message, exception);
			if (_logglyLogger != null && message != null) {
				Publish(message, FATAL, null, exception, logger.Logger.Name);
			}
		}

		public static void PublishFatalFormat(this ILog logger, string format, object arg0) {
			logger.FatalFormat(format, arg0);
			var message = String.Format(format, arg0);
			if (_logglyLogger != null && !String.IsNullOrEmpty(message)) {
				Publish(message, FATAL, null, null, logger.Logger.Name);
			}
		}

		public static void PublishFatalFormat(this ILog logger, string format, params object[] args) {
			logger.FatalFormat(format, args);
			var message = String.Format(format, args);
			if (_logglyLogger != null && !String.IsNullOrEmpty(message)) {
				Publish(message, FATAL, null, null, logger.Logger.Name);
			}
		}

		public static void PublishFatalFormat(this ILog logger, string format, object arg0, object arg1) {
			logger.FatalFormat(format, arg0, arg1);
			var message = String.Format(format, arg0, arg1);
			if (_logglyLogger != null && !String.IsNullOrEmpty(message)) {
				Publish(message, FATAL, null, null, logger.Logger.Name);
			}
		}

		public static void PublishFatalFormat(this ILog logger, string format, object arg0, object arg1, object arg2) {
			logger.FatalFormat(format, arg0, arg1, arg2);
			var message = String.Format(format, arg0, arg1, arg2);
			if (_logglyLogger != null && !String.IsNullOrEmpty(message)) {
				Publish(message, FATAL, null, null, logger.Logger.Name);
			}
		}

		#endregion

		#region Error

		public static void PublishError(this ILog logger, object message) {
			logger.Error(message);
			if (_logglyLogger != null && message != null) {
				Publish(message, ERROR, null, null, logger.Logger.Name);
			}
		}

		public static void PublishError(this ILog logger, object message, Exception exception) {
			logger.Error(message, exception);
			if (_logglyLogger != null && message != null) {
				Publish(message, ERROR, null, exception, logger.Logger.Name);
			}
		}

		public static void PublishErrorFormat(this ILog logger, string format, object arg0) {
			logger.ErrorFormat(format, arg0);
			var message = String.Format(format, arg0);
			if (_logglyLogger != null && !String.IsNullOrEmpty(message)) {
				Publish(message, ERROR, null, null, logger.Logger.Name);
			}
		}

		public static void PublishErrorFormat(this ILog logger, string format, params object[] args) {
			logger.ErrorFormat(format, args);
			var message = String.Format(format, args);
			if (_logglyLogger != null && !String.IsNullOrEmpty(message)) {
				Publish(message, ERROR, null, null, logger.Logger.Name);
			}
		}

		public static void PublishErrorFormat(this ILog logger, string format, object arg0, object arg1) {
			logger.ErrorFormat(format, arg0, arg1);

			var message = String.Format(format, arg0, arg1);
			if (_logglyLogger != null && !String.IsNullOrEmpty(message)) {
				Publish(message, ERROR, null, null, logger.Logger.Name);
			}
		}

		public static void PublishErrorFormat(this ILog logger, string format, object arg0, object arg1, object arg2) {
			logger.ErrorFormat(format, arg0, arg1, arg2);
			var message = String.Format(format, arg0, arg1, arg2);
			if (_logglyLogger != null && !String.IsNullOrEmpty(message)) {
				Publish(message, ERROR, null, null, logger.Logger.Name);
			}
		}

		#endregion

		#region Warn

		public static void PublishWarn(this ILog logger, object message) {
			logger.Warn(message);
			if (_logglyLogger != null && message != null) {
				Publish(message, WARN, null, null, logger.Logger.Name);
			}
		}

		public static void PublishWarn(this ILog logger, object message, Exception exception) {
			logger.Warn(message, exception);
			if (_logglyLogger != null && message != null) {
				Publish(message, WARN, null, exception, logger.Logger.Name);
			}
		}

		public static void PublishWarnFormat(this ILog logger, string format, object arg0) {
			logger.WarnFormat(format, arg0);

			var message = String.Format(format, arg0);
			if (_logglyLogger != null && !String.IsNullOrEmpty(message)) {
				Publish(message, WARN, null, null, logger.Logger.Name);
			}
		}

		public static void PublishWarnFormat(this ILog logger, string format, params object[] args) {
			logger.WarnFormat(format, args);
			var message = String.Format(format, args);
			if (_logglyLogger != null && !String.IsNullOrEmpty(message)) {
				Publish(message, WARN, null, null, logger.Logger.Name);
			}
		}

		public static void PublishWarnFormat(this ILog logger, string format, object arg0, object arg1) {
			logger.WarnFormat(format, arg0, arg1);
			var message = String.Format(format, arg0, arg1);
			if (_logglyLogger != null && !String.IsNullOrEmpty(message)) {
				Publish(message, WARN, null, null, logger.Logger.Name);
			}
		}

		public static void PublishWarnFormat(this ILog logger, string format, object arg0, object arg1, object arg2) {
			logger.WarnFormat(format, arg0, arg1, arg2);

			var message = String.Format(format, arg0, arg1, arg2);
			if (_logglyLogger != null && !String.IsNullOrEmpty(message)) {
				Publish(message, WARN, null, null, logger.Logger.Name);
			}
		}

		#endregion

		#region Info

		public static void PublishInfo(this ILog logger, object message) {
			logger.Info(message);
			if (_logglyLogger != null && message != null) {
				Publish(message, INFO, null, null, logger.Logger.Name);
			}
		}

		public static void PublishInfo(this ILog logger, object message, Exception exception) {
			logger.Info(message, exception);
			if (_logglyLogger != null && message != null) {
				Publish(message, INFO, null, exception, logger.Logger.Name);
			}
		}

		public static void PublishInfoFormat(this ILog logger, string format, object arg0) {
			logger.InfoFormat(format, arg0);
			var message = String.Format(format, arg0);
			if (_logglyLogger != null && !String.IsNullOrEmpty(message)) {
				Publish(message, INFO, null, null, logger.Logger.Name);
			}
		}

		public static void PublishInfoFormat(this ILog logger, string format, params object[] args) {
			logger.InfoFormat(format, args);
			var message = String.Format(format, args);
			if (_logglyLogger != null && !String.IsNullOrEmpty(message)) {
				Publish(message, INFO, null, null, logger.Logger.Name);
			}
		}

		public static void PublishInfoFormat(this ILog logger, string format, object arg0, object arg1) {
			logger.InfoFormat(format, arg0, arg1);
			var message = String.Format(format, arg0, arg1);
			if (_logglyLogger != null && !String.IsNullOrEmpty(message)) {
				Publish(message, INFO, null, null, logger.Logger.Name);
			}
		}

		public static void PublishInfoFormat(this ILog logger, string format, object arg0, object arg1, object arg2) {
			logger.InfoFormat(format, arg0, arg1, arg2);
			var message = String.Format(format, arg0, arg1, arg2);
			if (_logglyLogger != null && !String.IsNullOrEmpty(message)) {
				Publish(message, INFO, null, null, logger.Logger.Name);
			}
		}

		#endregion

		#region Debug

		public static void PublishDebug(this ILog logger, object message) {
			logger.Debug(message);
			if (_logglyLogger != null && message != null) {
				Publish(message, DEBUG, null, null, logger.Logger.Name);
			}
		}

		public static void PublishDebug(this ILog logger, object message, Exception exception) {
			logger.Debug(message, exception);
			if (_logglyLogger != null && message != null) {
				Publish(message, DEBUG, null, exception, logger.Logger.Name);
			}
		}

		public static void PublishDebugFormat(this ILog logger, string format, object arg0) {
			logger.DebugFormat(format, arg0);
			var message = String.Format(format, arg0);
			if (_logglyLogger != null && !String.IsNullOrEmpty(message)) {
				Publish(message, DEBUG, null, null, logger.Logger.Name);
			}
		}

		public static void PublishDebugFormat(this ILog logger, string format, params object[] args) {
			logger.DebugFormat(format, args);
			var message = String.Format(format, args);
			if (_logglyLogger != null && !String.IsNullOrEmpty(message)) {
				Publish(message, DEBUG, null, null, logger.Logger.Name);
			}
		}

		public static void PublishDebugFormat(this ILog logger, string format, object arg0, object arg1) {
			logger.DebugFormat(format, arg0, arg1);
			var message = String.Format(format, arg0, arg1);
			if (_logglyLogger != null && !String.IsNullOrEmpty(message)) {
				Publish(message, DEBUG, null, null, logger.Logger.Name);
			}
		}

		public static void PublishDebugFormat(this ILog logger, string format, object arg0, object arg1, object arg2) {
			logger.DebugFormat(format, arg0, arg1, arg2);
			var message = String.Format(format, arg0, arg1, arg2);
			if (_logglyLogger != null && !String.IsNullOrEmpty(message)) {
				Publish(message, DEBUG, null, null, logger.Logger.Name);
			}
		}

		#endregion

		public static void WriteToLog(this ILog log, string sMessage, string sUserName, Exception obException = null) {
			MDC.Set("@UserName", sUserName);
			log.PublishDebug(sMessage, obException);
		}
	}
}