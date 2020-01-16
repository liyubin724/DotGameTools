using Dot.Tools.ETD.Fields;
using ExtractInject;

namespace Dot.Tools.ETD.Datas
{
    public class LineCell : IEIContextObject
    {
        public int row;
        public int col;
        public string value;

        public LineCell(int r,int c,string v)
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
