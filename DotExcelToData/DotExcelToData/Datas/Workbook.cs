using Dot.Tools.ETD.Verify;
using System.Collections.Generic;
using System.IO;

namespace Dot.Tools.ETD.Datas
{
    public class Workbook : IVerify
    {
        public string bookPath;
        private List<Sheet> sheets = new List<Sheet>();

        public string Name
        {
            get
            {
                return Path.GetFileNameWithoutExtension(bookPath);
            }
        }

        public int SheetCount { get => sheets.Count; }

        public Workbook(string path)
        {
            this.bookPath = path;
        }

        public Sheet GetSheetByName(string sheetName)
        {
            foreach(var sheet in sheets)
            {
                if(sheet.name == sheetName)
                {
                    return sheet;
                }
            }
            return null;
        }

        public Sheet GetSheetByIndex(int index)
        {
            if(index>=0&&index<sheets.Count)
            {
                return sheets[index];
            }
            return null;
        }

        public void AddSheet(Sheet sheet)
        {
            sheets.Add(sheet);
        }

        public bool Verify()
        {
            throw new System.NotImplementedException();
        }
    }
}
