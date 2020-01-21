using Dot.Tools.ETD.Fields;

namespace Dot.Tools.ETD.IO
{
    public static class WriterUtil
    {
        internal static FieldPlatform GetPlatform(ETDWriterTarget target)
        {
            FieldPlatform platform = FieldPlatform.None;
            if (target == ETDWriterTarget.Client)
            {
                platform = FieldPlatform.Client;
            }
            else if (target == ETDWriterTarget.Server)
            {
                platform = FieldPlatform.Server;
            }
            return platform;
        }

        internal static string GetIndent(int indent)
        {
            string indentStr = "";
            for (int i = 0; i < indent; ++i)
            {
                indentStr += "    ";
            }
            return indentStr;
        }
    }
}
