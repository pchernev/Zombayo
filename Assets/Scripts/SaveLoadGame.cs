using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;
using System.Threading;

namespace Assets.Scripts
{
    public static class SaveLoadGame
    {
        public static void SaveStats(Statistics stats)
        {
            FileStream fs = new FileStream(Application.dataPath + "\\data.xml", FileMode.OpenOrCreate);
            
            using (fs)
            {              
                XmlTextWriter xmlTextWriter = new XmlTextWriter(fs, Encoding.UTF8);
                using (xmlTextWriter)
                {
                    XmlSerializer xs = new XmlSerializer(typeof(Statistics));
                    xs.Serialize(xmlTextWriter, stats);
                    xmlTextWriter.Flush();                
                }
            }
            Thread.Sleep(1700);
        }

        public static Statistics LoadStats()
        {
            XmlSerializer xs = new XmlSerializer(typeof(Statistics));
            using (var reader = new XmlTextReader(Application.dataPath + "\\data.xml"))
            {
                Statistics statsFromXml = (Statistics)xs.Deserialize(reader);
                return statsFromXml;
            }
        }
    }
}
