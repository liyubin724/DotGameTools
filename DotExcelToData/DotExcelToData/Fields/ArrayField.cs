using Dot.Tools.ETD.Datas;
using Dot.Tools.ETD.Utils;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Dot.Tools.ETD.Fields
{
    public class ArrayField : AField
    {
        private FieldType valueFieldType = FieldType.None;
        public FieldType ValueFieldType { get => valueFieldType; }

        private string refName = string.Empty;
        public string RefName { get => refName; }

        public ArrayField(int c, string n, string d, string t, string p, string dv, string vr) : base(c, n, d, t, p, dv, vr)
        {
            valueFieldType = FieldTypeUtil.GetArrayInnerType(t, out refName);
        }

        public override object GetValue(CellContent cell)
        {
            Type genericType = typeof(List<>);
            Type valueType = FieldTypeUtil.GetType(valueFieldType);
            
            if (valueType == null) return null;

            Type listType = genericType.MakeGenericType(valueType);
            IList list = (IList)Activator.CreateInstance(listType);

            string content = GetContent(cell);
            if(!string.IsNullOrEmpty(content))
            {
                string[] contents = ContentUtil.SplitContent(content, new char[] { ',' });
                if(contents!=null && contents.Length>0)
                {
                    foreach(var c in contents)
                    {
                        list.Add(ContentUtil.GetValue(c, valueFieldType));
                    }
                }
            }

            return list;
        }
    }
}
