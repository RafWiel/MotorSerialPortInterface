using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using gTools.Log;
using gTools.Configuration;

namespace SerialPortTestGuiApp
{    
    public class Configuration : gConfiguration
    {
        [XmlElement]
        public string Port { get; set; }

        [XmlElement]
        public int BaudRate { get; set; }
        
        #region File management        

        public static Configuration Load(string filePath)
        {
            Configuration cfg = new Configuration();            

            try
            {
                if (File.Exists(filePath) == false)
                    return cfg;

                XmlSerializer xs = new XmlSerializer(typeof(Configuration));
                StreamReader sr = new StreamReader(filePath);
                cfg = (Configuration)xs.Deserialize(sr);
                sr.Close();                
            }
            catch (Exception ex)
            {
                gLog.Error(ex.ToString());
            }
            finally
            {
                cfg.FilePath = filePath;
            }

            return cfg;
        }

        public override void Save()
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(Configuration));

                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add(string.Empty, string.Empty);

                StreamWriter sw = new StreamWriter(FilePath, false, Encoding.UTF8);
                xs.Serialize(sw, this, ns);
                sw.Close();                               
            }
            catch (Exception ex)
            {
                gLog.Error(ex.ToString());
            }
        }

        #endregion 
    }
}
