using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dot.Tools.ETD.Datas;

namespace Dot.Tools.ETD.Fields
{
    public class StringField : AField
    {
        public StringField(int c, string n, string d, string t, string p, string dv, string vr) : base(c, n, d, t, p, dv, vr)
        {
        }

        public override object GetValue(CellContent cell)
        {
            string content = GetContent(cell);
            if (string.IsNullOrEmpty(content))
            {
                return string.Empty;
            }
            return content;
        }
    }
}
