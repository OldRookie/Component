using Component.Infrastructure.Log;
using log4net;
using System;
using System.IO;
using System.Web;

namespace Component.Infrastructure
{
    public class LogFactory
    {
        static LogFactory()
        {
            FileInfo configFile = new FileInfo(FolderHelper.MapPath("/Configs/log4net.config"));
            log4net.Config.XmlConfigurator.Configure(configFile);
        }
        public static Logger GetLogger(Type type)
        {
            return new Logger(LogManager.GetLogger(type));
        }
        public static Logger GetLogger(string str)
        {
            return new Logger(LogManager.GetLogger(str));
        }
    }
}
