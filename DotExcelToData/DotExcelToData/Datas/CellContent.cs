using ExtractInject;

namespace Dot.Tools.ETD.Datas
{
    public class CellContent : IEIContextObject
    {
        public int Row { get; set; }
        public int Col { get; set; }

        public string Content { get; set; }
    }
}
