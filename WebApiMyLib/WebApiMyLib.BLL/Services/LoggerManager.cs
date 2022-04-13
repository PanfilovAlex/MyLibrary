using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiMyLib.BLL.Interfaces;

using NLog;

namespace WebApiMyLib.BLL.Services
{
    public class LoggerManager : ILoggerManager
    {
        private static ILogger _logger = LogManager.GetCurrentClassLogger();
        public void Debug(string message)
        {
            _logger.Debug(message);
        }

        public void Error(string message)
        {
            _logger.Error(message);
        }

        public void Info(string message)
        {
            _logger.Info(message);
        }

        public void Warn(string message)
        {
            _logger.Warn(message);
        }
    }
}
