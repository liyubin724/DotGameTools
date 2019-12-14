using Dot.Tools.ETD.Datas;
using Dot.Tools.ETD.Fields;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.IO;
using System;

namespace Dot.Tools.ETD.Exporter
{
    public static class JsonExporter
    {
        public static JToken WriteAsList(IList list)
        {
            JArray jArray = new JArray();
            foreach (var d in list)
            {
                jArray.Add(JToken.FromObject(d));
            }
            
            return jArray;
        }

        public static JToken WriteAsDic(IDictionary dic)
        {
            JObject jObject = new JObject();
            var enumerator = dic.GetEnumerator();
            while (enumerator.MoveNext())
            {
                jObject[enumerator.Key.ToString()] = JToken.FromObject(enumerator.Value);
            }
            return jObject;
        }

        public static void Export(string outputDirPath,Sheet sheet)
        {
            if(!Directory.Exists(outputDirPath))
            {
                if(!Directory.CreateDirectory(outputDirPath).Exists)
                {
                    return;
                }
            }

            JObject sheetData = new JObject();
            foreach(var line in sheet.Line.lines)
            {
                JObject jsonData = new JObject();
                string key = string.Empty;
                for (int i = 0; i < sheet.Field.fields.Count; i++)
                {
                    AField field = sheet.Field.fields[i];
                    CellContent cellContent = line.cells[i];

                    var value = field.GetValue(cellContent);

                    if(i == 0)
                    {
                        key = value.ToString();
                    }
                    if(value == null)
                    {
                        continue;
                    }

                    if (value.GetType().IsSubclassOf(typeof(IList)))
                    {
                        jsonData[field.Name] = WriteAsList((IList)value);
                    }else if(value.GetType().IsSubclassOf(typeof(IDictionary)))
                    {
                        jsonData[field.Name] = WriteAsDic((IDictionary)value);
                    }
                    else
                    {
                        jsonData[field.Name] = JToken.FromObject(value);
                    }
                }
                sheetData[key] = jsonData;
            }

            string json = sheetData.ToString();
            File.WriteAllText(outputDirPath + "/" + sheet.Name + ".json", json);
        }
    }
}
