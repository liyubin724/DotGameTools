using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Dot.Core.Config
{
    public enum ConfigFieldType
    {
        None = 'n',
        Int = 'i',
        Float = 'f',
        String = 's',
        Stringt = 't',
        Array = 'a',
        Dic = 'd',
        Lua = 'x',
    }

    public class ConfigFieldData
    {
        public int col;
        public string name;
        public string desc;
        public string platform;
        public string define;
        public string defaultValue;
        public string verification;

        private string defineName = null;
        private ConfigFieldType fieldType = ConfigFieldType.None;

        private ConfigFieldType innerFieldType = ConfigFieldType.None;
        public ConfigFieldType InnerFieldType
        {
            get
            {
                if(innerFieldType == ConfigFieldType.None)
                {
                    SplitDefine();
                }
                return innerFieldType;
            }
        }
        
        private string[] innerNames = null;
        public string[] InnerNames
        {
            get
            {
                if(innerNames == null)
                {
                    SplitDefine();
                }
                return innerNames;
            }
        }
        private ConfigFieldType[] innerTypes = null;
        public ConfigFieldType[] InnerTypes
        {
            get
            {
                if(innerTypes!=null)
                {
                    SplitDefine();
                }
                return innerTypes;
            }
        }
        public void SplitDefine()
        {
            if (string.IsNullOrEmpty(define))
            {
                return;
            }
            Match match = Regex.Match(define, @"^\w*\((float|dic|int|string|stringt|array|lua)");
            if (match.Success)
            {
                string[] nameAndType = match.Value.Split(new char[] { '(' });

                defineName = nameAndType[0];
                fieldType = (ConfigFieldType)Enum.Parse(typeof(ConfigFieldType), nameAndType[1], true);

                if (fieldType == ConfigFieldType.Array)
                {
                    match = Regex.Match(define, @"\((float|dic|int|string|stringt|array|lua)\)");
                    if (match.Success)
                    {
                        string[] strs = match.Value.Split(new char[] { '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
                        innerFieldType = (ConfigFieldType)Enum.Parse(typeof(ConfigFieldType), strs[0], true);
                    }
                }
                else if (fieldType == ConfigFieldType.Dic)
                {
                    MatchCollection matches = Regex.Matches(define, @"\w*\((float|dic|int|string|stringt|array|lua)\)");
                    List<string> names = new List<string>();
                    List<ConfigFieldType> types = new List<ConfigFieldType>();
                    if(matches.Count>0)
                    {
                        foreach(Match m in matches)
                        {
                            string[] strs = m.Value.Split(new char[] { '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
                            names.Add(strs[0]);
                            types.Add((ConfigFieldType)Enum.Parse(typeof(ConfigFieldType), strs[1], true));
                        }
                    }
                    innerNames = names.ToArray();
                    innerTypes = types.ToArray();
                }

            }
        }
        public string DefineName
        {
            get
            {
                if (defineName == null)
                {
                    SplitDefine();
                }
                return defineName;
            }
        }
        public ConfigFieldType DefineType
        {
            get
            {
                if (fieldType == ConfigFieldType.None)
                {
                    SplitDefine();
                }
                return fieldType;
            }
        }

        private VerificationCompose verifyCompose = null;
        public VerificationCompose VerifyCompose
        {
            get
            {
                if(verifyCompose == null)
                {
                    verifyCompose = new VerificationCompose(DefineType, verification);
                }
                return verifyCompose;
            }
        }
    }

    public class ConfigContentData
    {
        public int row;
        public int col;
        public string content;

        public string GetContent(ConfigFieldData fieldData)
        {
            if (string.IsNullOrEmpty(content))
            {
                if (string.IsNullOrEmpty(fieldData.defaultValue))
                {
                    return null;
                }
                else
                {
                    return fieldData.defaultValue;
                }
            }
            else
            {
                return content;
            }
        }
    }

    public class ConfigSheetLineData
    {
        public int row;
        public ConfigContentData[] contents = new ConfigContentData[0];
    }

    public class ConfigSheetData
    {
        public string name;
        public ConfigFieldData[] fields = new ConfigFieldData[0];
        public ConfigSheetLineData[] lines = new ConfigSheetLineData[0];

        public ConfigFieldData GetFieldData(int col)
        {
            foreach (var fieldData in fields)
            {
                if (fieldData.col == col)
                {
                    return fieldData;
                }
            }

            return null;
        }

        public int GetColNum()
        {
            if (fields == null || fields.Length == 0)
            {
                return 0;
            }
            return fields.Length;
        }

        public bool IsValid()
        {
            if (fields == null || fields.Length == 0)
            {
                return false;
            }
            return true;
        }
    }

    public class ConfigWorkbookData
    {
        public string name;
        public ConfigSheetData[] sheets = new ConfigSheetData[0];
    }
}
