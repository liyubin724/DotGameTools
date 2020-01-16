using Dot.Tools.ETD.Fields;
using ExtractInject;

namespace Dot.Tools.ETD.Datas
{
    public class CellContent : IEIContextObject
    {
        public int row;
        public int col;
        public string value;

        public CellContent(int r,int c,string v)
        {
            row = r;
            col = c;
            value = v;
        }

        public string GetContent(AFieldData field)
        {
            if(string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(field.defaultValue))
            {
                return field.defaultValue;
            }
            return value;
        }
    }
}
