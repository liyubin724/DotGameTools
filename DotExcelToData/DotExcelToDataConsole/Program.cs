using Dot.Tools.ETD.Datas;
using Dot.Tools.ETD.Exporter;
using Dot.Tools.ETD.Factorys;
using Dot.Tools.ETD.Fields;
using Dot.Tools.ETD.Utils;
using Dot.Tools.ETD.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DotExcelToDataConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string excelPath = @"D:\WorkSpace\DotGameProject\DotGameTools\cofing.xlsx";
            Workbook book = new Workbook();
            bool result = book.LoadExcel(excelPath, out string msg);

            LuaOptimizeExporter.Export("D:/", book.sheets[0], FieldPlatform.Server);
            //string[] targets = new string[]
            //{
            //    "array[ref<tablename>]",
            //    "array[string]",
            //    "int",
            //    "res",
            //    "dic{int,float}",
            //    "ref<refname>",
            //    "dic{ref<tablename>,ref<tablename>}",
            //};
            //string typeNameRegex = @"^(?<typename>[A-Za-z]+)(?<other>\S*)$";
            //string refNameRegex = @"^<(?<refname>[A-Za-z]+)>$";
            //string arrayValueRegex = @"^\[(?<typename>[A-Za-z]+)[<]{0,1}(?<refname>[A-Za-z]*)[>]{0,1}\]$";

            //string tt = @"^(?<typename>[A-Za-z]+)[(<(?<refname>[A-Za-z0-9]+)>)|(\[(?<valuetypename>[A-Za-z]+)[(<(?<refname>[A-Za-z0-9]+)>)]{0,1}\])]{0,1}";

            //foreach(var target in targets)
            //{
            //    Match match = new Regex(tt).Match(target);

            //    if (match.Groups["typename"].Success)
            //    {
            //        Console.Write(match.Groups["typename"].Value);
            //    }
            //    if(match.Groups["refname"].Success)
            //    {
            //        Console.Write("----"+match.Groups["refname"].Value);
            //    }
            //    if(match.Groups["valuetypename"].Success)
            //    {
            //        Console.Write("----"+match.Groups["valuetypename"].Value);
            //    }
            //    Console.WriteLine();
            //}

            //foreach (var target in targets)
            //{
            //    Match match = new Regex(typeNameRegex).Match(target);
            //    if(match.Groups["typename"].Success)
            //    {
            //        FieldType fieldType = FieldTypeUtil.GetFieldType(match.Groups["typename"].Value);
            //        Console.Write(fieldType);
            //        if(match.Groups["other"].Success)
            //        {
            //            string other = match.Groups["other"].Value;
            //            if(fieldType == FieldType.Ref)
            //            {
            //                match = new Regex(refNameRegex).Match(other);
            //                if(match.Success)
            //                {
            //                    Console.Write("----" + match.Groups["refname"].Value);
            //                }
            //            }else if(fieldType == FieldType.Array)
            //            {
            //                match = new Regex(arrayValueRegex).Match(other);
            //                if(match.Groups["typename"].Success)
            //                {
            //                    Console.Write("----" + match.Groups["typename"].Value);
            //                }
            //                if(match.Groups["refname"].Success)
            //                {
            //                    Console.Write("----" + match.Groups["refname"].Value);
            //                }
            //            }
            //        }
            //    }

            //    Console.WriteLine();
            //}

            //string[] targets = new string[]
            //{
            //    "array[ref<tablename>]",
            //    "array[string]",
            //};
            //foreach(var target in targets)
            //{
            //    string regexStr = @"^array\[(?<typename>[A-Za-z]+)";
            //    Match match = new Regex(regexStr).Match(target);
            //    Console.WriteLine(match.Groups["typename"].Value);

            //    FieldType fieldType = FieldTypeUtil.GetFieldType(target);
            //    Console.WriteLine(fieldType.ToString());
            //}


            //string validationValue = "Range{100.0,100}";
            //Match match1 = new Regex(@"(?<name>[A-Za-z]+)").Match(validationValue);//@"\w*{(?<min>.[0-9]*[.]?[0-9]*?),(?<max>.[0-9]*[.]?[0-9]*?)}").Match(validationValue);
            //Console.WriteLine(match1.Groups["name"].Value);
            //Console.WriteLine(match1.Groups["max"].Value);

            //string temp = "ErrorCode:-1,Message:{\"UserId\" : \"1000\",\"userName\" : \"ZhangSan\"}";
            //Regex reg = new Regex("ErrorCode:(?<key1>.*?),Message:{(?<key2>.*?)}");
            //Match match = reg.Match(temp);
            //string tempStr = match.Groups["key1"].Value + "--" + match.Groups["key2"].Value;

            //Console.WriteLine(tempStr);

            Console.ReadKey();
        }
    }
}
