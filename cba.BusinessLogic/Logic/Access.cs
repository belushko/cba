using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using CBA.BusinessLogic.Models;

namespace CBA.BusinessLogic.Logic
{
    public class Access
    {
        public void Save(string fileName, params Series[] series)
        {
            var seriesModel = new SeriesModel[3];

            seriesModel = SeriesToModel(series);

            var serializer = new XmlSerializer(typeof (SeriesModel[]));

            using (TextWriter textWriter = new StreamWriter(fileName))
            {
                serializer.Serialize(textWriter, seriesModel);
            }
        }

        //TODO: refactor
        public Series[] Load(string fileName)
        {
            SeriesModel[] seriesModel;

            var deserializer = new XmlSerializer(typeof (SeriesModel[]));

            using (TextReader textReader = new StreamReader(fileName))
            {
                seriesModel = (SeriesModel[]) deserializer.Deserialize(textReader);
            }

            var series = ModelToSeries(seriesModel);

            return series;
        }

        public SeriesModel[] SeriesToModel(Series[] series)
        {
            var seriesModel = new SeriesModel[3];

            for (var k = 0; k < series[0].SizeOfSeries; k++)
            {
                seriesModel[k] = new SeriesModel();
                seriesModel[k].items = new List<double[]>();

                seriesModel[k].SizeOfSeries = series[k].SizeOfSeries;
                seriesModel[k].Size = series[k].Size;
                seriesModel[k].MOValue = series[k].MOValue;

                for (var i = 0; i < series[k].Size; i++)
                {
                    var mas = new double[12];

                    for (var j = 0; j < 12; j++)
                    {
                        mas[j] = series[k].a[i][j];
                    }

                    seriesModel[k].items.Add(mas);
                    seriesModel[k].Name = series[k].Name;
                    seriesModel[k].activity = series[k].activity;
                    seriesModel[k].ratio = series[k].ratio;
                    seriesModel[k].breeding = series[k].breeding;
                }
            }

            return seriesModel;
        }

        public Series[] ModelToSeries(SeriesModel[] seriesModel)
        {
            var series = new Series[3];


            for (var k = 0; k < seriesModel[0].SizeOfSeries; k++)
            {
                series[k] = new Series();
                series[k].a = new DataArray(seriesModel[0].Size);

                series[k].SizeOfSeries = seriesModel[k].SizeOfSeries;
                series[k].Size = seriesModel[k].Size;
                series[k].MOValue = seriesModel[k].MOValue;

                for (var i = 0; i < seriesModel[0].Size; i++)
                {
                    var mas = seriesModel[k].items[i];

                    for (var j = 0; j < 12; j++)
                    {
                        series[k].a[i][j] = mas[j];
                    }

                    series[k].Name = seriesModel[k].Name;
                    series[k].activity = seriesModel[k].activity;
                    series[k].ratio = seriesModel[k].ratio;
                    series[k].breeding = seriesModel[k].breeding;
                }
            }

            return series;
        }     
    }
}