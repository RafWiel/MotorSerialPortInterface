using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using gTools.Log;
using System.Windows;

namespace gTools.Configuration
{
    [XmlRoot(ElementName = "Configuration", DataType = "string", IsNullable = false)]
    public class gConfiguration
    {        
        [XmlElement(ElementName = "Windows")]
        public WindowsConfiguration Windows { get; set; }

        public gConfiguration()
        {            
            Windows = new WindowsConfiguration();
        }        

        public class WindowsConfiguration
        {
            [XmlElement(ElementName = "Window")]
            public List<WindowConfiguration> Items { get; set; }

            public WindowsConfiguration()
            {
                Items = new List<WindowConfiguration>();
            }
        }

        public class WindowConfiguration
        {
            [XmlElement]
            public string Name { get; set; }

            [XmlElement]
            public int X { get; set; }

            [XmlElement]
            public int Y { get; set; }

            [XmlElement]
            public int Width { get; set; }

            [XmlElement]
            public int Height { get; set; }

            [XmlElement]
            public int State { get; set; }            
        }                     

        #region File management

        private string _filePath;

        [XmlIgnore]
        public string FilePath
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
        
        [XmlIgnore]
        public string FileName
        {
            get
            {
                return Path.GetFileNameWithoutExtension(_filePath);
            }
            set
            {
                _filePath = String.Format("{0}\\{1}", Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), value);
            }
        }

        [XmlIgnore]
        public string Directory
        {
            get
            {
                return Path.GetDirectoryName(_filePath);
            }
        }

        //public static gConfiguration Load(string filePath)
        //{
        //    gConfiguration cfg = new gConfiguration();

        //    try
        //    {
        //        if (File.Exists(filePath) == false)
        //            return cfg;

        //        XmlSerializer xs = new XmlSerializer(typeof(gConfiguration));
        //        StreamReader sr = new StreamReader(filePath);
        //        cfg = (gConfiguration)xs.Deserialize(sr);
        //        sr.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        gLog.Error(ex.ToString());
        //    }
        //    finally
        //    {
        //        cfg.FilePath = filePath;
        //    }

        //    return cfg;
        //}

        public virtual void Save() { }

        //public void Save()
        //{
        //    try
        //    {
        //        XmlSerializer xs = new XmlSerializer(typeof(gConfiguration));

        //        XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
        //        ns.Add(string.Empty, string.Empty);

        //        StreamWriter sw = new StreamWriter(FilePath, false, Encoding.UTF8);
        //        xs.Serialize(sw, this, ns);
        //        sw.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        gLog.Error(ex.ToString());
        //    }
        //}

        #endregion                
    }
}
