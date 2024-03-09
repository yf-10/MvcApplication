using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Text;

namespace MvcApplication.Controllers
{
    class Logger
    {
        // 0:Error 1:Warn 2:Info 3:Debug
        private enum LogLevel
        {
            ERROR,
            WARN,
            INFO,
            DEBUG
        }

        // Field
        private static Logger? singleton = null;
        private readonly object lockObj = new object();
        private StreamWriter? stream = null;

        // Configuration
        private static bool isLogFile = true;
        private static int logLevel = 2; // default 2:Info
        private static string logDirPath = "";
        private static string logFileName = "";
        private static string logFilePath = "";
        private static int logFileMaxSize = 10485760; // default 10MB
        private static int logFilePeriod = 30; // default 30days

        // Get Singleton Instance
        public static Logger GetInstance(IConfiguration conf)
        {
            if (singleton is null)
            {
                // Load Configuration
                const string section = "LoggingSettings";
                isLogFile = conf.GetValue<bool>(section + ":IsLogFile");
                logLevel = conf.GetValue<int>(section + ":LogLevel");
                logDirPath = conf.GetValue<string>(section + ":LogDirPath");
                logFileName = conf.GetValue<string>(section + ":LogFileName");
                logFileMaxSize = conf.GetValue<int>(section + ":LogFileMaxSize");
                logFilePeriod = conf.GetValue<int>(section + ":LogFilePeriod");
                logFilePath = logDirPath + logFileName + ".log";
                // Create Logger
                singleton = new Logger();
            }
            return singleton;
        }

        // Constructor
        private Logger()
        {
            CreateLogfile(new FileInfo(logFilePath));
        }

        // ERROR (String)
        public void Error(string msg)
        {
            if ((int)LogLevel.ERROR <= logLevel)
            {
                Out(LogLevel.ERROR, msg);
            }
        }

        // ERROR (Exception)
        public void Error(Exception ex)
        {
            if ((int)LogLevel.ERROR <= logLevel)
            {
                Out(LogLevel.ERROR, ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        // WARN (String)
        public void Warn(string msg)
        {
            if ((int)LogLevel.WARN <= logLevel)
            {
                Out(LogLevel.WARN, msg);
            }
        }

        // INFO (String)
        public void Info(string msg)
        {
            if ((int)LogLevel.INFO <= logLevel)
            {
                Out(LogLevel.INFO, msg);
            }
        }

        // DEBUG (String)
        public void Debug(string msg)
        {
            if ((int)LogLevel.DEBUG <= logLevel)
            {
                Out(LogLevel.DEBUG, msg);
            }
        }

        // Write Log
        private void Out(LogLevel level, string msg)
        {
            if (isLogFile)
            {
                int tid = System.Threading.Thread.CurrentThread.ManagedThreadId;
                string fullMsg = string.Format("[{0}][{1}][{2}] {3}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), tid, level.ToString(), msg);

                lock (this.lockObj)
                {
                    this.stream?.WriteLine(fullMsg);

                    FileInfo logFile = new FileInfo(logFilePath);
                    if (logFileMaxSize < logFile.Length)
                    {
                        // Compress log files over max file size
                        CompressLogFile();

                        // Delete old log files over period
                        DeleteOldLogFile();
                    }
                }
            }
        }

        // Create Log File
        private void CreateLogfile(FileInfo logFile)
        {
            if (!Directory.Exists(logFile.DirectoryName) && logFile.DirectoryName is not null)
            {
                Directory.CreateDirectory(logFile.DirectoryName);
            }

            this.stream = new StreamWriter(logFile.FullName, true, Encoding.UTF8)
            {
                AutoFlush = true
            };
        }

        // Compress Log File
        private void CompressLogFile()
        {
            this.stream?.Close();
            string oldFilePath = logDirPath + logFileName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            File.Move(logFilePath, oldFilePath + ".log");

            FileStream inStream = new FileStream(oldFilePath + ".log", FileMode.Open, FileAccess.Read);
            FileStream outStream = new FileStream(oldFilePath + ".gz", FileMode.Create, FileAccess.Write);
            GZipStream gzStream = new GZipStream(outStream, CompressionMode.Compress);

            int size = 0;
            byte[] buffer = new byte[logFileMaxSize + 1000];
            while (0 < (size = inStream.Read(buffer, 0, buffer.Length)))
            {
                gzStream.Write(buffer, 0, size);
            }

            inStream.Close();
            gzStream.Close();
            outStream.Close();

            File.Delete(oldFilePath + ".log");
            CreateLogfile(new FileInfo(logFilePath));
        }

        // Delete Old Log File
        private void DeleteOldLogFile()
        {
            Regex regex = new Regex(logFileName + @"_(\d{14}).*\.gz");
            DateTime retentionDate = DateTime.Today.AddDays(-logFilePeriod);
            string[] filePathList = Directory.GetFiles(logDirPath, logFileName + "_*.gz", SearchOption.TopDirectoryOnly);
            foreach (string filePath in filePathList)
            {
                Match match = regex.Match(filePath);
                if (match.Success)
                {
                    DateTime logCreatedDate = DateTime.ParseExact(match.Groups[1].Value.ToString(), "yyyyMMddHHmmss", null);
                    if (logCreatedDate < retentionDate)
                    {
                        File.Delete(filePath);
                    }
                }
            }
        }
    }
}
