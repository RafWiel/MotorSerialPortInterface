using System;
//using System.Collections.Generic;
//using System.Text;
using System.IO;
using System.Diagnostics;

namespace gTools.Log
{    
    public class gLog
    {
        private static StreamWriter _logFile = null;
        private static string _fileName = string.Empty;
        private static string _filePath = string.Empty;
        private static int _maxSize = 4096000;

        public static string FileName
        {
            get
            {
                return _fileName;
            }
            set
            {
                _fileName = value;
                _filePath = String.Format("{0}\\{1}", Path.GetDirectoryName(System.Reflection.Assembly.GetCallingAssembly().Location), _fileName);                
            }
        }

        public static string FilePath
        {
            get
            {
                return _filePath;
            }
            set
            {
                _filePath = value;                
            }
        }

        public static int MaxSize
        {
            get
            {
                return _maxSize;
            }
            set
            {
                _maxSize = value;
            }
        }       

        public static void Write(string format, params object[] arg)
        {
            try
            {
                if (_logFile == null || _logFile.BaseStream.Length > _maxSize)
                    CreateLogFile(_filePath.Length > 0 ? _filePath : System.Reflection.Assembly.GetCallingAssembly().Location);

                if (_logFile == null)
                    return;

                string message = arg.Length == 0 ? format : String.Format(format, arg);
                message = String.Format("{0}: {1}", DateTime.Now, message);

                //if (message.Length > 32)
                //    message = String.Format("{0}: {1}", DateTime.Now, message);
                //else
                //{
                //    StackFrame sf = new StackFrame(2, true);
                //    string fn = Path.GetFileName(sf.GetFileName());
                //    string m = sf.GetMethod().Name;
                //    int l = sf.GetFileLineNumber();

                //    message = String.Format("{0}: {1}, file: {2}, method: {3}, line: {4}", DateTime.Now, message.Replace(Environment.NewLine, string.Empty), fn, m, l);
                //}

                Console.WriteLine(message);
                _logFile.WriteLine(message);
                _logFile.WriteLine("--------");
                _logFile.Flush();
            }
            catch //(Exception ex)
            {
                _logFile = null;
                //Console.WriteLine(ex.ToString());
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
                    append = fi.Length < _maxSize;
                    
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
    }
}
