using System;
using System.IO;
using System.Text;

namespace Dot.Tool.Proto.Writer
{
    public static class CSharpMessageRecognizer
    {
        private static string PROTO_HEAD_CONTENT = 
            "/*"+
            "The file was created by tool.\r\n" +
            "-----------------------------------------------\r\n"+
            "Please don't change it manually!!!\r\n" +
            "Please don't change it manually!!!\r\n" +
            "Please don't change it manually!!!\r\n"+
            "-----------------------------------------------\r\n"+
            "*/\r\n" ;

        public static void CreateRecognizerScript(string rootDir,ProtoConfig config)
        {
            string outputDir = GetOutputDir(config.SpaceName, rootDir);
            ProtoGroup[] protoGroups = new ProtoGroup[] { config.S2CGroup, config.C2SGroup };
            foreach (var protoGroup in protoGroups)
            {
                string outputPath = $"{outputDir}/{protoGroup.Name}.cs";
                StreamWriter fileWriter = new StreamWriter(outputPath, false, Encoding.UTF8);

                WriteHeadContent(fileWriter);

                int indent = 0;
                BeginWriteProto(ref indent, fileWriter, config);
                {
                    BeginWriteProtoGroup(ref indent, fileWriter, protoGroup);
                    {
                        foreach (var messageGroup in protoGroup.MessageGroups)
                        {
                            BeginWriteMessageGroup(ref indent, fileWriter, messageGroup);
                            {
                                foreach (var message in messageGroup.Messages)
                                {
                                    WriteMessage(ref indent, fileWriter, message);
                                }
                            }
                            EndWriteMessageGroup(ref indent, fileWriter, messageGroup);
                        }
                    }
                    EndWriteProtoGroup(ref indent, fileWriter);
                }
                EndWriteProto(ref indent, fileWriter, config);

                fileWriter.Flush();
                fileWriter.Close();
            }
        }

        private static void WriteHeadContent(TextWriter writer)
        {
            writer.WriteLine(PROTO_HEAD_CONTENT);
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

        private static void WriteComment(ref int indent, TextWriter writer, string comment)
        {
            if (!string.IsNullOrEmpty(comment))
            {
                string[] lines = comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (lines != null && lines.Length > 0)
                {
                    foreach (var line in lines)
                    {
                        writer.WriteLine($"{GetIndent(indent)}//{line}");
                    }
                }
            }
        }

        private static void BeginWriteProto(ref int indent, TextWriter writer, ProtoConfig protoConfig)
        {
            if(!string.IsNullOrEmpty(protoConfig.SpaceName))
            {
                writer.WriteLine($"namespace {protoConfig.SpaceName}");
                writer.WriteLine("{");

                ++indent;
            }
        }

        private static void EndWriteProto(ref int indent, TextWriter writer, ProtoConfig protoConfig)
        {
            if (!string.IsNullOrEmpty(protoConfig.SpaceName))
            {
                --indent;
                writer.WriteLine("}");
            }
        }

        private static void BeginWriteProtoGroup(ref int indent, TextWriter writer, ProtoGroup protoGroup)
        {
            writer.WriteLine($"{GetIndent(indent)}public static class {protoGroup.Name}");
            writer.WriteLine($"{GetIndent(indent)}{{");
            ++indent;
        }

        private static void EndWriteProtoGroup(ref int indent, TextWriter writer)
        {
            --indent;
            writer.WriteLine($"{GetIndent(indent)}}}");
        }

        private static void BeginWriteMessageGroup(ref int indent, TextWriter writer, ProtoMessageGroup messageGroup)
        {
            writer.WriteLine($"#region Start --{messageGroup.Name}--");
        }

        private static void EndWriteMessageGroup(ref int indent, TextWriter writer, ProtoMessageGroup messageGroup)
        {
            writer.WriteLine($"#endregion End --{messageGroup.Name}--");
            writer.WriteLine();
        }

        private static void WriteMessage(ref int indent, TextWriter writer, ProtoMessage message)
        {
            if(!string.IsNullOrEmpty(message.Comment))
            {
                WriteComment(ref indent, writer, message.Comment);
            }
            writer.WriteLine($"{GetIndent(indent)}public static readonly int {message.Name} = {message.Value};");
        }

        private static string GetIndent(int indent)
        {
            StringBuilder sb = new StringBuilder();
            for(int i =0;i<indent;++i)
            {
                sb.Append("    ");
            }
            return sb.ToString();
        }
    }
}
