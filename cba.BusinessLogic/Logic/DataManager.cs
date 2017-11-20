using System.IO;
using System.Xml.Serialization;
using CBA.BusinessLogic.Models;

namespace CBA.BusinessLogic.Logic
{
    public class DataManager
    {
        public void SaveDataToFile(string fileName, params Series[] series)
        {
            var mapper = new Mapper();

            foreach (var serie in series)
            {
                serie.Items = mapper.DataArrayToItems(serie);
            }

            var serializer = new XmlSerializer(typeof(Series[]));

            using (TextWriter textWriter = new StreamWriter(fileName))
            {
                serializer.Serialize(textWriter, series);
            }
        }

        public Series[] LoadDataFromFile(string fileName)
        {
            Series[] series;

            var deserializer = new XmlSerializer(typeof(Series[]));

            using (TextReader textReader = new StreamReader(fileName))
            {
                series = (Series[])deserializer.Deserialize(textReader);
            }

            var mapper = new Mapper();

            foreach (var serie in series)
            {
                serie.DataArray = mapper.ItemsToDataArray(serie);
            }

            return series;
        }
    }
}