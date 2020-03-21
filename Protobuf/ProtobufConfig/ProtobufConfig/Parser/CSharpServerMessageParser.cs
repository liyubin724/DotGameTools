using System.IO;
using System.Text;

namespace Dot.Tool.Proto
{
    public class CSharpServerMessageParser
    {
        private static string PROTO_HEAD_CONTENT =
            "/*" +
            "The file was created by tool.\r\n" +
            "-----------------------------------------------\r\n" +
            "Please don't change it manually!!!\r\n" +
            "Please don't change it manually!!!\r\n" +
            "Please don't change it manually!!!\r\n" +
            "-----------------------------------------------\r\n" +
            "*/\r\n";

        public static void CreateParserScript(string rootDir, ProtoConfig protoConfig)
        {
            ProtoGroup protoGroup = protoConfig.C2SGroup;
            string outputDir = GetOutputDir(protoConfig.SpaceName, rootDir);
            string scriptPath = $"{outputDir}/{protoGroup.Name}_Parser.cs";
            StreamWriter writer = new StreamWriter(scriptPath, false, Encoding.UTF8);

            WriteHeadContent(writer);
            writer.WriteLine();

            int indent = 0;
            BeginWriteNamespace(ref indent, writer, protoConfig.SpaceName);
            {
                BeginWriteClass(ref indent, writer, protoGroup.Name);
                {
                    BeginWriteRegister(ref indent, writer);
                    {
                        foreach (var messageGroup in protoGroup.MessageGroups)
                        {
                            foreach (var message in messageGroup.Messages)
                            {
                                WriterRegisteMessage(ref indent, writer, protoGroup, message);
                            }
                        }
                    }
                    EndWriteRegister(ref indent, writer);

                    writer.WriteLine();

                    foreach (var messageGroup in protoGroup.MessageGroups)
                    {
                        foreach (var message in messageGroup.Messages)
                        {
                            BeginWriteMessageParser(ref indent, writer, message);
                            {
                                WriteMessageParser(ref indent, writer, message);
                            }
                            EndWriteMessageParser(ref indent, writer);
                        }
                    }
                }
                EndWriteClass(ref indent, writer);
            }
            EndWriteNamespace(ref indent, writer, protoConfig.SpaceName);
            writer.Flush();
            writer.Close();
        }

        private static void WriteMessageParser(ref int indent, TextWriter writer, ProtoMessage message)
        {
            writer.WriteLine($"{GetIndent(indent)}return {message.ClassName}.Parser.ParseFrom(msgBytes);");
        }

        private static void BeginWriteMessageParser(ref int indent, TextWriter writer, ProtoMessage message)
        {
            writer.WriteLine($"{GetIndent(indent)}private static object Parse_{message.ClassName}(int messageID,byte[] msgBytes)");
            writer.WriteLine($"{GetIndent(indent)}{{");
            ++indent;
        }

        private static void EndWriteMessageParser(ref int indent, TextWriter writer)
        {
            --indent;
            writer.WriteLine($"{GetIndent(indent)}}}");
        }

        private static void WriterRegisteMessage(ref int indent, TextWriter writer, ProtoGroup protoGroup, ProtoMessage message)
        {
            writer.WriteLine($"{GetIndent(indent)}serverNetListener.RegisterMessageParser({protoGroup.Name}.{message.Name},Parse_{message.ClassName});");
        }

        private static void BeginWriteRegister(ref int indent, TextWriter writer)
        {
            writer.WriteLine($"{GetIndent(indent)}public static void RegisterParser(Dot.Net.Server.ServerNetListener serverNetListener)");
            writer.WriteLine($"{GetIndent(indent)}{{");
            ++indent;
        }

        private static void EndWriteRegister(ref int indent, TextWriter writer)
        {
            --indent;
            writer.WriteLine($"{GetIndent(indent)}}}");
        }

        private static void BeginWriteClass(ref int indent, TextWriter writer, string protoGroupName)
        {
            writer.WriteLine($"{GetIndent(indent)}public static class {protoGroupName}_Parser");
            writer.WriteLine($"{GetIndent(indent)}{{");
            ++indent;
        }

        private static void EndWriteClass(ref int indent, TextWriter writer)
        {
            --indent;
            writer.WriteLine($"{GetIndent(indent)}}}");
        }

        private static void BeginWriteNamespace(ref int indent, TextWriter writer, string spaceName)
        {
            if (!string.IsNullOrEmpty(spaceName))
            {
                writer.WriteLine($"namespace {spaceName}");
                writer.WriteLine("{");

                ++indent;
            }
        }

        private static void EndWriteNamespace(ref int indent, TextWriter writer, string spaceName)
        {
            if (!string.IsNullOrEmpty(spaceName))
            {
                --indent;
                writer.WriteLine("}");
            }
        }

        private static string GetOutputDir(string spaceName, string rootDirPath)
        {
            string outputDir = rootDirPath;
            //if (!string.IsNullOrEmpty(spaceName))
            //{
            //    outputDir = $"{rootDirPath}/{spaceName.Replace(".", "/")}";
            //}
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }
            return outputDir;
        }

        private static void WriteHeadContent(TextWriter writer)
        {
            writer.WriteLine(PROTO_HEAD_CONTENT);
        }

        private static string GetIndent(int indent)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < indent; ++i)
            {
                sb.Append("    ");
            }
            return sb.ToString();
        }
    }
}
