using Dot.Tools.ETD.Datas;

namespace Dot.Tools.ETD.Fields
{
    public class LongField : AField
    {
        public LongField(int c, string n, string d, string t, string p, string dv, string vr) : base(c, n, d, t, p, dv, vr)
        {
        }

        public override object GetValue(CellContent cell)
        {
            string content = GetContent(cell);
            if (string.IsNullOrEmpty(content))
            {
                return 0L;
            }
            if (long.TryParse(content, out long result))
            {
                return result;
            }
            return 0L;
        }
    }
}
