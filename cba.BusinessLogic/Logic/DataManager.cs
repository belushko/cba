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
            var seriesModel = mapper.SeriesToModel(series);
            var serializer = new XmlSerializer(typeof (SeriesModel[]));

            using (TextWriter textWriter = new StreamWriter(fileName))
            {
                serializer.Serialize(textWriter, seriesModel);
            }
        }

        public Series[] LoadDataFromFile(string fileName)
        {
            SeriesModel[] seriesModel;

            var deserializer = new XmlSerializer(typeof (SeriesModel[]));

            using (TextReader textReader = new StreamReader(fileName))
            {
                seriesModel = (SeriesModel[]) deserializer.Deserialize(textReader);
            }

            var mapper = new  Mapper();
            var series = mapper.ModelToSeries(seriesModel);

            return series;
        }
    }
}