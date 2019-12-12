using Dot.Tools.ETD.Datas;
using Dot.Tools.ETD.Utils;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Dot.Tools.ETD.Fields
{
    public class DicField : AField
    {
        private FieldType keyFieldType = FieldType.None;
        private FieldType valueFieldType = FieldType.None;
        private string keyRefName = string.Empty;
        private string valueRefName = string.Empty;

        public FieldType KeyFieldType { get => keyFieldType; }
        public FieldType ValueFieldType { get => valueFieldType; }
        public string KeyRefName { get => keyRefName; }
        public string ValueRefName { get => valueRefName; }

        public DicField(int c, string n, string d, string t, string p, string dv, string vr) : base(c, n, d, t, p, dv, vr)
        {
            FieldTypeUtil.GetDicInnerType(t, out keyFieldType, out keyRefName, out valueFieldType, out valueRefName);
        }

        public override object GetValue(CellContent cell)
        {
            Type genericType = typeof(Dictionary<,>);
            Type keyType = FieldTypeUtil.GetType(keyFieldType);
            Type valueType = FieldTypeUtil.GetType(valueFieldType);

            if(keyType == null || valueType == null)
            {
                return null;
            }

            Type dicType = genericType.MakeGenericType(keyType, valueType);
            IDictionary dic = (IDictionary)Activator.CreateInstance(dicType);

            string content = GetContent(cell);
            if(!string.IsNullOrEmpty(content) && content.Length>2)
            {
                if(content[0] =='{' && content[content.Length-1]=='}')
                {
                    content = content.Substring(1, content.Length - 2);
                    string[] contents = ContentUtil.SplitContent(content, new char[] { ',', ';' });
                    if (contents != null && contents.Length > 0)
                    {
                        for (int i = 0; i < contents.Length; i += 2)
                        {
                            dic.Add(ContentUtil.GetValue(contents[i], keyFieldType), ContentUtil.GetValue(contents[i + 1], valueFieldType));
                        }
                    }
                }
                
            }
            return dic;
        }
    }
}
