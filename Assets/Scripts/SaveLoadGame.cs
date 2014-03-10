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
		private static string dataFile = "data.xml";

        public static void SaveStats(Statistics stats)
        {
			var filePath = Path.Combine( Application.dataPath, dataFile );
            FileStream fs = new FileStream( filePath, FileMode.OpenOrCreate );
            
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
			var filePath = Path.Combine( Application.dataPath, dataFile );

            XmlSerializer xs = new XmlSerializer(typeof(Statistics));
            using (var reader = new XmlTextReader( filePath ))
            {
                Statistics statsFromXml = (Statistics)xs.Deserialize(reader);
                return statsFromXml;
            }
        }
    }
}
