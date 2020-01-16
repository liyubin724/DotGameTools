using Dot.Tools.ETD.Datas;

namespace Dot.Tools.ETD.Fields
{
    public class StringtField : AField
    {
        public StringtField(int c, string n, string d, string t, string p, string dv, string vr) : base(c, n, d, t, p, dv, vr)
        {
        }

        public override object GetValue(LineCell cell)
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
