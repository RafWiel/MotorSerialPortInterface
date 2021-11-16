using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

namespace gTools.Configuration
{
    [XmlRoot(ElementName = "Configuration", DataType = "string", IsNullable = false)]
    public class gConfiguration
    {
        #region Windows configuration

        [XmlElement(ElementName = "Windows")]
        public WindowsConfiguration Windows { get; set; } = new WindowsConfiguration();
        
        public class WindowsConfiguration
        {
            [XmlElement(ElementName = "Window")]
            public List<WindowConfiguration> Items { get; set; } = new List<WindowConfiguration>();
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

        #endregion

        #region File management

        [XmlIgnore]
        public string FilePath { get; set; }
                
        [XmlIgnore]
        public string FileName
        {
            get
            {
                return Path.GetFileNameWithoutExtension(FilePath);
            }
            set
            {
                FilePath = String.Format("{0}\\{1}", Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), value);
            }
        }

        [XmlIgnore]
        public string Directory
        {
            get => Path.GetDirectoryName(FilePath);            
        }
       
        public virtual void Save() { }

        #endregion                
    }
}
