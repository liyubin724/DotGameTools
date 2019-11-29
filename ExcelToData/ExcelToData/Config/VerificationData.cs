using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Dot.Core.Config
{
    public class VerificationCompose
    {
        private AVerification[] verifications = null;
        public VerificationCompose(ConfigFieldType fieldType, string verificatonStr)
        {
            verifications = VerificatoinFactroy.GetVerifications(fieldType, verificatonStr);
        }

        public bool Verify(ConfigSheetData sheetData, ConfigContentData contentData,out string error)
        {
            error = null;
            if (verifications == null || verifications.Length == 0)
                return true;
            StringBuilder sb = new StringBuilder();
            bool result = true;
            foreach(var ver in verifications)
            {
                string outErr = "";
                if(!ver.Verify(sheetData,contentData,out outErr))
                {
                    result = false;
                    sb.AppendLine(outErr);
                }
            }
            error = sb.ToString();
            return result;
        }
    }

    public static class VerificatoinFactroy
    {
        public static AVerification[] GetVerifications(ConfigFieldType fieldType,string verificationStr)
        {
            if(string.IsNullOrEmpty(verificationStr))
            {
                return null;
            }
            List<AVerification> verifications = new List<AVerification>();
            string typeName = $"Dot.Core.Config.Is{Enum.GetName(typeof(ConfigFieldType), fieldType)}";
            Type t = Type.GetType(typeName);
            if(t!=null)
            {
                var ver = t.Assembly.CreateInstance(typeName);
                verifications.Add((AVerification)ver);
            }
            string[] vStrs = verificationStr.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            foreach(var s in vStrs)
            {
                string[] strs = s.Split(new char[] { '(', ')', ',' }, StringSplitOptions.RemoveEmptyEntries);
                if(strs!=null && strs.Length>0)
                {
                    string clsName = $"Dot.Core.Config.{strs[0]}";
                    Type clsType = Type.GetType(clsName);
                    if(clsType!=null)
                    {
                        var ver = clsType.Assembly.CreateInstance(clsName);
                        if(strs.Length>1)
                        {
                            string[] datas = new string[strs.Length - 1];
                            Array.Copy(strs, 1, datas, 0, datas.Length);
                            string error = "";
                            if(((AVerification)ver).SetData(datas,out error))
                            {
                                verifications.Add((AVerification)ver);
                            }else
                            {
                                DebugLogger.Error(error);
                            }
                        }else
                        {
                            verifications.Add((AVerification)ver);
                        }
                    }
                }
            }
            return verifications.ToArray();
        }
    }

    public abstract class AVerification
    {
        public virtual bool SetData(string[] datas, out string error)
        {
            error = null;
            return true;
        }

        public abstract bool Verify(ConfigSheetData sheetData, ConfigContentData contentData, out string error);
    }

    public class IsInt : AVerification
    {
        public override bool Verify(ConfigSheetData sheetData, ConfigContentData contentData, out string error)
        {
            error = null;
            ConfigFieldData fieldData = sheetData.GetFieldData(contentData.col);
            if(fieldData.DefineType != ConfigFieldType.Int)
            {
                error = $"IsInt较验规则只适用于Int类型字段!";
                return false;
            }
            string content = contentData.GetContent(fieldData);
            if (string.IsNullOrEmpty(content))
            {
                error = $"数据表{sheetData.name}第{contentData.row}行第{contentData.col}列数据不能为空！";
                return false;
            }else
            {
                if(!int.TryParse(content,out int result))
                {
                    error = $"数据表{sheetData.name}第{contentData.row}行第{contentData.col}列数据值({content})无法转换为Int类型！";
                }
            }

            return true;
        }
    }

    public class IsFloat : AVerification
    {
        public override bool Verify(ConfigSheetData sheetData, ConfigContentData contentData, out string error)
        {
            error = null;
            ConfigFieldData fieldData = sheetData.GetFieldData(contentData.col);
            if (fieldData.DefineType != ConfigFieldType.Float)
            {
                error = $"IsFloat较验规则只适用于Float类型字段!";
                return false;
            }
            string content = contentData.GetContent(fieldData);
            if (string.IsNullOrEmpty(content))
            {
                error = $"数据表{sheetData.name}第{contentData.row}行第{contentData.col}列数据不能为空！";
                return false;
            }
            else
            {
                if (!float.TryParse(content, out float result))
                {
                    error = $"数据表{sheetData.name}第{contentData.row}行第{contentData.col}列数据值({content})无法转换为Float类型！";
                }
            }

            return true;
        }
    }

    public class IsString : AVerification
    {
        public override bool Verify(ConfigSheetData sheetData, ConfigContentData contentData, out string error)
        {
            error = null;
            ConfigFieldData fieldData = sheetData.GetFieldData(contentData.col);
            ConfigFieldType fieldType = fieldData.DefineType;
            if (fieldType != ConfigFieldType.String || fieldType != ConfigFieldType.Stringt)
            {
                error = "IsString较验规则只适用于String或者Stringt类型字段!";
                return false;
            }
            string content = contentData.GetContent(fieldData);
            if (content==null)
            {
                error = $"数据表{sheetData.name}第{contentData.row}行第{contentData.col}列数据不能为空！";
                return false;
            }

            return true;
        }
    }

    public class IsArray : AVerification
    {
        public override bool Verify(ConfigSheetData sheetData, ConfigContentData contentData, out string error)
        {
            error = null;
            ConfigFieldData fieldData = sheetData.GetFieldData(contentData.col);
            ConfigFieldType fieldType = fieldData.DefineType;
            if (fieldType != ConfigFieldType.Array)
            {
                error = "IsArray较验规则只适用于Array类型字段!";
                return false;
            }
            string content = contentData.GetContent(fieldData);
            if (string.IsNullOrEmpty(content))
            {
                error = $"数据表{sheetData.name}第{contentData.row}行第{contentData.col}列数据不能为空！";
                return false;
            }
            if(!Regex.IsMatch(content,@"^\[\w*\]$"))
            {
                error = $"数据表{sheetData.name}第{contentData.row}行第{contentData.col}列数据值({content})格式不正确，格式类似于[a1##a2##a3]！";
                return false;
            }
            ConfigFieldType innerFieldType = fieldData.InnerFieldType;
            if(innerFieldType == ConfigFieldType.Int || innerFieldType == ConfigFieldType.Float || innerFieldType == ConfigFieldType.String
                || innerFieldType == ConfigFieldType.Stringt)
            {
                if(innerFieldType == ConfigFieldType.String || innerFieldType == ConfigFieldType.Stringt)
                {
                    return true;
                }
                string[] values = content.Substring(1, content.Length - 1).Split(new string[] { "##" },StringSplitOptions.RemoveEmptyEntries);
                if(values.Length>0)
                {
                    bool isValid = true;
                    foreach(var v in values)
                    {
                        if(innerFieldType == ConfigFieldType.Int)
                        {
                            if(!int.TryParse(v,out int result))
                            {
                                isValid = false;
                                break;
                            }
                        }else if(innerFieldType == ConfigFieldType.Float)
                        {
                            if (!float.TryParse(v, out float result))
                            {
                                isValid = false;
                                break;
                            }
                        }
                    }
                    return isValid;
                }
                return true;
            }else
            {
                error = $"数据表表头类型指定不正确，只能是Int,Float,String或StringT";
                return false;
            }
        }
    }

    public class IsDic : AVerification
    {
        public override bool Verify(ConfigSheetData sheetData, ConfigContentData contentData, out string error)
        {
            error = null;
            ConfigFieldData fieldData = sheetData.GetFieldData(contentData.col);
            ConfigFieldType fieldType = fieldData.DefineType;
            if (fieldType != ConfigFieldType.Dic)
            {
                error = "IsDic较验规则只适用于Dic类型字段!";
                return false;
            }
            string content = contentData.GetContent(fieldData);
            if (string.IsNullOrEmpty(content))
            {
                error = $"数据表{sheetData.name}第{contentData.row}行第{contentData.col}列数据不能为空！";
                return false;
            }
            if (!Regex.IsMatch(content, @"^\{\w*\}$"))
            {
                error = $"数据表{sheetData.name}第{contentData.row}行第{contentData.col}列数据值({content})格式不正确，格式类似于{{k1=v1##k2=v2##k3=v3}}！";
                return false;
            }
            if(fieldData.InnerNames == null || fieldData.InnerTypes == null || fieldData.InnerTypes.Length!=fieldData.InnerNames.Length)
            {
                error = "字段定义类型不正确";
                return false;
            }
            string[] contents = content.Substring(1,content.Length-1).Split(new string[] { "##"}, StringSplitOptions.RemoveEmptyEntries);
            if(contents.Length!=fieldData.InnerNames.Length)
            {
                error = $"数据表{sheetData.name}第{contentData.row}行第{contentData.col}列数据值({content})长度与字段数量不符";
                return false;
            }
            return true;
        }
    }

    public class NotNull : AVerification
    {
        public override bool Verify(ConfigSheetData sheetData, ConfigContentData contentData, out string error)
        {
            error = null;
            ConfigFieldData fieldData = sheetData.GetFieldData(contentData.col);
            string content = contentData.GetContent(fieldData);
            if (content == null)
            {
                error = $"数据表{sheetData.name}第{contentData.row}行第{contentData.col}列数据不能为空！";
                return false;
            }

            return true;
        }
    }

    public class NotRepeat : AVerification
    {
        public override bool Verify(ConfigSheetData sheetData, ConfigContentData contentData, out string error)
        {
            error = null;
            List<ConfigContentData> contentList = new List<ConfigContentData>();
            for(int i =0;i<sheetData.fields.Length;i++)
            {
                if(sheetData.fields[i].col == contentData.col)
                {
                    foreach(var line in sheetData.lines)
                    {
                        if(line.row == contentData.row)
                        {
                            continue;
                        }
                        contentList.Add(line.contents[i]);
                    }

                    break;
                }
            }

            if(contentList.Count>0)
            {
                ConfigFieldData fieldData = sheetData.GetFieldData(contentData.col);
                string content = contentData.GetContent(fieldData);
                foreach(var c in contentList)
                {
                    string cContent = c.GetContent(fieldData);
                    if(content == cContent && cContent!=null)
                    {
                        error = $"[{c.row},{c.col}]与{contentData.row},{contentData.col}的数据重复";
                        return false;
                    }
                }
            }

            return true;
        }
    }

    public class IntRange : AVerification
    {
        private int min;
        private int max;
        public override bool SetData(string[] datas, out string error)
        {
            error = null;
            if(datas == null || datas.Length<2)
            {
                error = "参数长度不足，至少需要两个参数，一个表示最小值，另一个表示最大值";
                return false;
            }
            if(int.TryParse(datas[0],out min) && int.TryParse(datas[1],out max))
            {
                if(max>=min)
                {
                    return true;
                }
            }
            error = "参数无法转换为Int值，或者指定的最大值小于了最小值";
            return false;
        }

        public override bool Verify(ConfigSheetData sheetData, ConfigContentData contentData, out string error)
        {
            error = null;
            string content = contentData.GetContent(sheetData.GetFieldData(contentData.col));
            if(content!=null)
            {
                if(!int.TryParse(content,out int result))
                {
                    error = "数值无法转换为Int值";
                    return false;
                }else
                {
                    if(result>=min&&result<=max)
                    {
                        return true;
                    }
                    error = "数值取值范围不在合理范围";
                    return false;
                }
            }
            return true;
        }
    }

    public class FloatRange : AVerification
    {
        public override bool SetData(string[] datas, out string error)
        {
            throw new System.NotImplementedException();
        }

        public override bool Verify(ConfigSheetData sheetData, ConfigContentData contentData, out string error)
        {
            throw new System.NotImplementedException();
        }
    }

    public class StringLength : AVerification
    {
        public override bool SetData(string[] datas, out string error)
        {
            throw new System.NotImplementedException();
        }

        public override bool Verify(ConfigSheetData sheetData, ConfigContentData contentData, out string error)
        {
            throw new System.NotImplementedException();
        }
    }

    public class UnityAsset : AVerification
    {
        public override bool Verify(ConfigSheetData sheetData, ConfigContentData contentData, out string error)
        {
            throw new NotImplementedException();
        }
    }
}
