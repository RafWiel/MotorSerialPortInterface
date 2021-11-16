using System;
using System.IO;

namespace gTools.Log
{    
    public class gLog
    {
        #region Initialization

        public static string FilePath { get; set; } = string.Empty;
        public static int MaxSize { get; set; } = 4096000;

        private static StreamWriter _logFile = null;
        
        private static string _fileName = string.Empty;
        public static string FileName
        {
            get => _fileName;
            set
            {
                _fileName = value;
                FilePath = String.Format("{0}\\{1}", Path.GetDirectoryName(System.Reflection.Assembly.GetCallingAssembly().Location), _fileName);                
            }
        }

        #endregion

        #region Methods

        public static void Write(string format, params object[] arg)
        {
            try
            {
                if (_logFile == null || _logFile.BaseStream.Length > MaxSize)
                    CreateLogFile(String.IsNullOrEmpty(FilePath) == false ? FilePath : System.Reflection.Assembly.GetCallingAssembly().Location);

                if (_logFile == null)
                    return;

                string message = arg.Length == 0 ? format : String.Format(format, arg);
                message = String.Format("{0}: {1}", DateTime.Now, message);
                
                Console.WriteLine(message);
                _logFile.WriteLine(message);
                _logFile.WriteLine("--------");
                _logFile.Flush();
            }
            catch
            {
                _logFile = null;                
            }
        }

        public static void Error(string format, params object[] arg)
        {
            Write(format, arg);
        }

        public static void Trace(string format, params object[] arg)
        {
            Write(format, arg);
        }

        public static void Close()
        {
            if (_logFile != null)
            {
                _logFile.Close();
                _logFile.Dispose();
                _logFile = null;
            }
        }

        private static void CreateLogFile(string filePath)
        {
            try
            {
                filePath = Path.Combine(Path.GetDirectoryName(filePath), String.Format("{0}.log", Path.GetFileNameWithoutExtension(filePath)));
                
                bool append = true;
                if (File.Exists(filePath))
                {
                    FileInfo fi = new FileInfo(filePath);
                    append = fi.Length < MaxSize;
                    
                    if (append == false)
                        File.Copy(filePath, String.Format("{0}\\{1}.log1", fi.Directory, fi.Name.Replace(fi.Extension, string.Empty)), true); 
                }

                FileStream fs;
                if (append)
                    fs = new FileStream(filePath, FileMode.OpenOrCreate | FileMode.Append, FileAccess.Write, FileShare.Read);
                else
                    fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Read);

                _logFile = new StreamWriter(fs);   
            }
            catch
            {
                _logFile = null;                
            }
        }

        #endregion
    }
}
