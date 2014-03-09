using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

namespace Assets.Scripts
{
    public class SaveLoadGame  
    {
        private readonly string _FileLocation = Application.dataPath;
        private readonly string _FileName = "data.xml";
        public string _data;


        public string UTF8ByteArrayToString(byte[] characters)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            string constructedString = encoding.GetString(characters);
            return (constructedString);
        }

        public byte[] StringToUTF8ByteArray(string pXmlString)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] byteArray = encoding.GetBytes(pXmlString);
            return byteArray;
        }


        public string SerializeObject(Statistics pObject)
        {
            string XmlizedString = null;
            MemoryStream memoryStream = new MemoryStream();
            XmlSerializer xs = new XmlSerializer(typeof(Statistics));
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
            xs.Serialize(xmlTextWriter, pObject);
            memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
            XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());
            return XmlizedString;
        }

        public Statistics DeserializeObject(string pXmlizedString)
        {
            XmlSerializer xs = new XmlSerializer(typeof (Statistics));
            MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString));
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
            return (Statistics)xs.Deserialize(memoryStream);
        }

       public void CreateXML()
        {
            StreamWriter writer;
            FileInfo t = new FileInfo(_FileLocation + "\\" + _FileName);
            if (!t.Exists)
            {
                writer = t.CreateText();
            }
            else
            {
                t.Delete();
                writer = t.CreateText();
            }
            writer.Write(_data);
            writer.Close();
            Debug.Log("File written.");
        }

        public void LoadXML()
        {
            FileInfo fi = new FileInfo(_FileLocation + "\\" + _FileName);
            if (fi.Exists)
            {
                StreamReader r = File.OpenText(_FileLocation + "\\" + _FileName);
                string _info = r.ReadToEnd();
                r.Close();
                _data = _info;
                Debug.Log("File Read");
            }          
        }            
    }
   
}
