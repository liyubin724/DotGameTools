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
            string content = value;
            if(string.IsNullOrEmpty(content) && !string.IsNullOrEmpty(field.defaultValue))
            {
                content = field.defaultValue;
            }
            if(string.IsNullOrEmpty(content))
            {
                content = field.GetOriginalDefault();
            }
            return content;
        }

        public override string ToString()
        {
            return $"<row:{row},col:{col},value:{(string.IsNullOrEmpty(value) ? "" : value)}>";
        }
    }
}
