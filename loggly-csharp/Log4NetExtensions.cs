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
				publish(message, category, data, exception, logger.Logger.Name);
			}
		}

		private static void publish(object message, string category, Dictionary<string, object> data, Exception exception, string source) {
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
			if (logger.IsFatalEnabled) {
				logger.Fatal(message);
				if (_logglyLogger != null && message != null) {
					publish(message, FATAL, null, null, logger.Logger.Name);
				}
			}
		}

		public static void PublishFatal(this ILog logger, object message, Exception exception) {
			if (logger.IsFatalEnabled) {
				logger.Fatal(message, exception);
				if (_logglyLogger != null && message != null) {
					publish(message, FATAL, null, exception, logger.Logger.Name);
				}
			}
		}

		public static void PublishFatalFormat(this ILog logger, string format, params object[] args) {
			if (logger.IsFatalEnabled) {
				logger.FatalFormat(format, args);
				var message = String.Format(format, args);
				if (_logglyLogger != null && !String.IsNullOrEmpty(message)) {
					publish(message, FATAL, null, null, logger.Logger.Name);
				}
			}
		}

		#endregion

		#region Error

		public static void PublishError(this ILog logger, object message) {
			if (logger.IsErrorEnabled) {
				logger.Error(message);
				if (_logglyLogger != null && message != null) {
					publish(message, ERROR, null, null, logger.Logger.Name);
				}
			}
		}

		public static void PublishError(this ILog logger, object message, Exception exception) {
			if (logger.IsErrorEnabled) {
				logger.Error(message, exception);
				if (_logglyLogger != null && message != null) {
					publish(message, ERROR, null, exception, logger.Logger.Name);
				}
			}
		}

		public static void PublishErrorFormat(this ILog logger, string format, params object[] args) {
			if (logger.IsErrorEnabled) {
				logger.ErrorFormat(format, args);
				var message = String.Format(format, args);
				if (_logglyLogger != null && !String.IsNullOrEmpty(message)) {
					publish(message, ERROR, null, null, logger.Logger.Name);
				}
			}
		}

		#endregion

		#region Warn

		public static void PublishWarn(this ILog logger, object message) {
			if (logger.IsWarnEnabled) {
				logger.Warn(message);
				if (_logglyLogger != null && message != null) {
					publish(message, WARN, null, null, logger.Logger.Name);
				}
			}
		}

		public static void PublishWarn(this ILog logger, object message, Exception exception) {
			if (logger.IsWarnEnabled) {
				logger.Warn(message, exception);
				if (_logglyLogger != null && message != null) {
					publish(message, WARN, null, exception, logger.Logger.Name);
				}
			}
		}

		public static void PublishWarnFormat(this ILog logger, string format, params object[] args) {
			if (logger.IsWarnEnabled) {
				logger.WarnFormat(format, args);
				var message = String.Format(format, args);
				if (_logglyLogger != null && !String.IsNullOrEmpty(message)) {
					publish(message, WARN, null, null, logger.Logger.Name);
				}
			}
		}

		#endregion

		#region Info

		public static void PublishInfo(this ILog logger, object message) {
			if (logger.IsInfoEnabled) {
				logger.Info(message);
				if (_logglyLogger != null && message != null) {
					publish(message, INFO, null, null, logger.Logger.Name);
				}
			}
		}

		public static void PublishInfo(this ILog logger, object message, Exception exception) {
			if (logger.IsInfoEnabled) {
				logger.Info(message, exception);
				if (_logglyLogger != null && message != null) {
					publish(message, INFO, null, exception, logger.Logger.Name);
				}
			}
		}

		public static void PublishInfoFormat(this ILog logger, string format, params object[] args) {
			if (logger.IsInfoEnabled) {
				logger.InfoFormat(format, args);
				var message = String.Format(format, args);
				if (_logglyLogger != null && !String.IsNullOrEmpty(message)) {
					publish(message, INFO, null, null, logger.Logger.Name);
				}
			}
		}

		#endregion

		#region Debug

		public static void PublishDebug(this ILog logger, object message) {
			if (logger.IsDebugEnabled) {
				logger.Debug(message);
				if (_logglyLogger != null && message != null) {
					publish(message, DEBUG, null, null, logger.Logger.Name);
				}
			}
		}

		public static void PublishDebug(this ILog logger, object message, Exception exception) {
			if (logger.IsDebugEnabled) {
				logger.Debug(message, exception);
				if (_logglyLogger != null && message != null) {
					publish(message, DEBUG, null, exception, logger.Logger.Name);
				}
			}
		}

		public static void PublishDebugFormat(this ILog logger, string format, params object[] args) {
			if (logger.IsDebugEnabled) {
				logger.DebugFormat(format, args);
				var message = String.Format(format, args);
				if (_logglyLogger != null && !String.IsNullOrEmpty(message)) {
					publish(message, DEBUG, null, null, logger.Logger.Name);
				}
			}
		}

		#endregion

		public static void WriteToLog(this ILog log, string sMessage, string sUserName, Exception obException = null) {
			MDC.Set("@UserName", sUserName);
			log.PublishDebug(sMessage, obException);
		}
	}
}