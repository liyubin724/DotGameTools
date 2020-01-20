using Dot.Tools.ETD.Datas;
using Dot.Tools.ETD.Fields;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Tools.ETD.IO
{
    public class OptimizeLuaWriter
    {
        public static void WriteBook(Workbook book, string outputDir, ETDWriterTarget target)
        {
            if (book == null)
            {
                return;
            }
            FieldPlatform platform = GetPlatform(target);

            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            for (int i = 0; i < book.SheetCount; ++i)
            {
                Sheet sheet = book.GetSheetByIndex(i);

                string filePath = $"{outputDir}/{book.Name}_{sheet.name}{IOConst.LUA_EXTERSION}";

                SummarySheetData summarySheet = OptimizeLuaAnalyzer.OptimizeSheet(sheet, 3, platform);

            }
        }

        private static FieldPlatform GetPlatform(ETDWriterTarget target)
        {
            FieldPlatform platform = FieldPlatform.None;
            if (target == ETDWriterTarget.Client)
            {
                platform = FieldPlatform.Client;
            }
            else if (target == ETDWriterTarget.Server)
            {
                platform = FieldPlatform.Server;
            }
            return platform;
        }

        private static string GetIndent(int indent)
        {
            string indentStr = "";
            for (int i = 0; i < indent; ++i)
            {
                indentStr += "    ";
            }
            return indentStr;
        }
    }
}
