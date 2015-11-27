using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Timesheet.Helpers
{
    public static class LogManager
    {
        public static void Publish(string Adicional = "")
        {
            Publish(null, Adicional);
        }

        public static void Publish(Exception ex, string Adicional = "")
        {
            var _log = new LogParameters()
            {
                Date = DateTime.Now,
                Ex = ex,
                Adicional = Adicional
            };

            new System.Threading.Thread((object argument) =>
            {
                var parameters = argument as LogParameters;

                LogFile(parameters);

            }).Start(_log);
        }

        private static bool Disabled()
        {
            var logApp = ConfigurationManager.AppSettings["DisableLog"];

            if (string.IsNullOrEmpty(logApp))
                return false;

            var result = false;

            bool.TryParse(logApp, out result);

            return result && Convert.ToBoolean(logApp);
        }

        private static void LogFile(LogParameters parameters)
        {
            try
            {
                if (Disabled())
                    return;

                var path = ConfigurationManager.AppSettings["Log"];

                if (string.IsNullOrEmpty(path))
                    path = AppDomain.CurrentDomain.BaseDirectory + "Log";

                if (!System.IO.Directory.Exists(path))
                    System.IO.Directory.CreateDirectory(path);

                var logName = path + "\\log_" + parameters.Date.ToString("yyyyMMdd_HHmmssffff") + ".log";

                using (var writer = new System.IO.FileStream(logName, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite))
                {
                    byte[] line = null;

                    if (parameters.Ex == null)
                    {
                        line = Encoding.UTF8.GetBytes(parameters.Adicional);
                    }
                    else
                    {
                        line = Encoding.UTF8.GetBytes(
                            string.Format("Exception::::\r\n{0}\r\n\r\nAdicional::::\r\n{1}",
                                parameters.Ex.ToString(),
                                parameters.Adicional));
                    }

                    writer.Write(line, 0, line.Length);
                }
            }
            catch(Exception ex)
            {
                LogManager.Publish(ex);
            }
        }
    }

    public class LogParameters
    {
        private string _adicional;

        public string Adicional
        {
            get
            {
                if (string.IsNullOrEmpty(_adicional)) return "";

                return _adicional;
            }
            set { _adicional = value; }
        }

        public Exception Ex { get; set; }

        public DateTime Date { get; set; }
    }
}
