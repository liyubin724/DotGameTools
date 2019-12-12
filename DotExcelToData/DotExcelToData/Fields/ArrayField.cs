using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dot.Tools.ETD.Datas;
using Dot.Tools.ETD.Utils;

namespace Dot.Tools.ETD.Fields
{
    public class ArrayField : AField
    {
        private FieldType innerFieldType = FieldType.None;
        private string refName = string.Empty;

        public ArrayField(int c, string n, string d, string t, string p, string dv, string vr) : base(c, n, d, t, p, dv, vr)
        {
            innerFieldType = FieldTypeUtil.GetArrayInnerType(t, out refName);
        }

        public override object GetValue(CellContent cell)
        {
            Type genericType = typeof(List<>);
            Type valueType = FieldTypeUtil.GetType(innerFieldType);
            
            if (valueType == null) return null;

            Type listType = genericType.MakeGenericType(valueType);
            IList list = (IList)Activator.CreateInstance(listType);

            string content = GetContent(cell);
            if(!string.IsNullOrEmpty(content))
            {
                List<string> splitList = new List<string>();

                int startIndex = 0;
                int curIndex = 0;
                while(curIndex>=content.Length)
                {

                }
            }

            return list;
        }
    }
}
