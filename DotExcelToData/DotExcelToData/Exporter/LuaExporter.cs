using Dot.Tools.ETD.Datas;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Tools.ETD.Exporter
{
    public static class LuaExporter
    {
        public static void Export(string outputDirPath, Sheet sheet)
        {
            if (!Directory.Exists(outputDirPath))
            {
                if (!Directory.CreateDirectory(outputDirPath).Exists)
                {
                    return;
                }
            }

            foreach (var line in sheet.Line.lines)
            {

            }

        }
    }
}
