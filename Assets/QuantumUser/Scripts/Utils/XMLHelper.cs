using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

namespace CustomUtils
{
    public class XMLHelper : MonoBehaviour
    {
        public static T DeserealizeFromXML<T>(TextAsset data, params Type[] types) where T : class
        {
            if (data == null || string.IsNullOrEmpty(data.text))
            {
                return null;
            }

            try
            {
                var doc = new XmlDocument();
                var stream = new MemoryStream();

                doc.LoadXml(data.text);
                doc.Save(stream);

                stream.Flush();
                stream.Position = 0;

                var serializer = new XmlSerializer(typeof(T), types);

                var reader = XmlReader.Create(stream);

                return serializer.Deserialize(reader) as T;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            return null;
        }

        public static T DeserealizeFromXML<T>(string path, params Type[] types) where T : class
        {
            var xml = Resources.Load(path);
            var data = (TextAsset)xml;

            if (data != null)
            {
                return DeserealizeFromXML<T>(data, types);
            }

            Debug.LogErrorFormat("Can't load resource file {0}", path);

            return null;
        }

        public static object DeserealizeFromXML<T>(string shipDefaultLevelPath, object serializable)
        {
            throw new NotImplementedException();
        }

        public static void Destroy(GameObject obj)
        {
            if (Application.isPlaying)
                UnityEngine.Object.Destroy(obj);
            else
                UnityEngine.Object.DestroyImmediate(obj);

        }
    }
}
