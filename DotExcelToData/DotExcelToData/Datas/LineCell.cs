using Dot.Tools.ETD.Fields;
using ExtractInject;
using System;
using System.Collections.Generic;

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
            if(!string.IsNullOrEmpty(content) && field.Type == FieldType.Dic)
            {
                string[] splitStr = content.Split(new char[] { '{', '}', ';' }, StringSplitOptions.RemoveEmptyEntries);
                if(splitStr!=null && splitStr.Length>0)
                {
                    List<string> result = new List<string>(splitStr);
                    result.Sort();
                    content = $"{{{string.Join(";", result.ToArray())}}}";
                }
            }
            return content;
        }

        public override string ToString()
        {
            return $"<row:{row},col:{col},value:{(string.IsNullOrEmpty(value) ? "" : value)}>";
        }
    }
}
