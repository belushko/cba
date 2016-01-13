using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Xml.Serialization;
using System.IO; 

namespace cba
{
    public class Access
    {
        public Access()
        { }

        public void Save(string fileName, params Series[] series)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Series[]));

            using (TextWriter textWriter = new StreamWriter(fileName))
            {
                serializer.Serialize(textWriter, series);
            }
        }

        public Series[] Load(string fileName)
        {
            Series[] series;
            XmlSerializer deserializer = new XmlSerializer(typeof(Series[]));

            using (TextReader textReader = new StreamReader(fileName))
            {
                series = (Series[])deserializer.Deserialize(textReader);
            }

            return series;
        }
    }
}
