using System.IO;
using System.Xml.Serialization;

namespace cba.Logic
{
    public class Access
    {
        public void Save(string fileName, params Series[] series)
        {
            var serializer = new XmlSerializer(typeof (Series[]));

            using (TextWriter textWriter = new StreamWriter(fileName))
            {
                serializer.Serialize(textWriter, series);
            }
        }

        public Series[] Load(string fileName)
        {
            Series[] series;
            var deserializer = new XmlSerializer(typeof (Series[]));

            using (TextReader textReader = new StreamReader(fileName))
            {
                series = (Series[]) deserializer.Deserialize(textReader);
            }

            return series;
        }
    }
}