using Dot.Tools.ETD.Datas;

namespace Dot.Tools.ETD.Fields
{
    public class BoolField : AField
    {
        public BoolField(int c, string n, string d, string t, string p, string dv, string vr) : base(c, n, d, t, p, dv, vr)
        {
        }

        public override object GetValue(CellContent cell)
        {
            string content = GetContent(cell);
            if (string.IsNullOrEmpty(content))
            {
                return false;
            }
            if (bool.TryParse(content.ToLower(), out bool result))
            {
                return result;
            }
            return false;
        }
    }
}
