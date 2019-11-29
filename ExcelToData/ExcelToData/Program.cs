using Dot.Core.Config;
using System;
using System.IO;
using System.Text;

namespace Game.Core.ExcelToData
{
    class Program
    {
        static void ExportConfig(string excelPath)
        {
            ConfigWorkbookData configWorkbook = ExcelWorkbook.ParseWorkbook(excelPath);

            if (configWorkbook != null && configWorkbook.sheets != null)
            {
                foreach (ConfigSheetData sheet in configWorkbook.sheets)
                {
                    StringBuilder errorMsg = new StringBuilder();
                    foreach (var line in sheet.lines)
                    {
                        for (int i = 0; i < line.contents.Length; i++)
                        {
                            ConfigFieldData fieldData = sheet.fields[i];
                            string error = "";
                            if (fieldData.VerifyCompose != null && !fieldData.VerifyCompose.Verify(sheet, line.contents[i], out error))
                            {
                                errorMsg.AppendLine(error);
                            }
                        }
                    }
                    if (errorMsg.Length > 0)
                    {
                        errorMsg.Insert(0, "---------" + sheet.name);
                        DebugLogger.Error(errorMsg.ToString());
                    }
                    else
                    {
                        SimpleLuaExporter exporter = new SimpleLuaExporter();
                        string value = exporter.Export(sheet);
                        if(!Directory.Exists("./exporter_lua"))
                        {
                            Directory.CreateDirectory("./exporter_lua");
                        }

                        File.WriteAllText(@"./exporter_lua/" + sheet.name + ".txt", value, new UTF8Encoding(false));
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            string[] xlsFiles = Directory.GetFiles("./", "*.xls", SearchOption.AllDirectories);
            string[] xlsxFiles = Directory.GetFiles("./", "*.xlsx", SearchOption.AllDirectories);

            foreach(var f in xlsFiles)
            {
                ExportConfig(f);
            }
            foreach(var f in xlsxFiles)
            {
                ExportConfig(f);
            }

            
            Console.WriteLine("Press Any Key To Continue");
            Console.ReadKey();
        }
    }
}
