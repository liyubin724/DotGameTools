using System;
using System.Collections;

namespace Dot.Tools.ETD.Fields
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false,Inherited = false)]
    public class FieldRealyType : Attribute
    {
        public Type RealyType { get; set; }
        public FieldRealyType(Type type)
        {
            RealyType = type;
        }
    }

    public enum FieldType
    {
        None = 0,

        [FieldRealyType(typeof(int))]
        ID,
        [FieldRealyType(typeof(int))]
        Int,
        [FieldRealyType(typeof(int))]
        Ref,
        [FieldRealyType(typeof(int))]
        Text,

        [FieldRealyType(typeof(long))]
        Long,

        [FieldRealyType(typeof(float))]
        Float,

        [FieldRealyType(typeof(bool))]
        Bool,

        [FieldRealyType(typeof(string))]
        String,
        [FieldRealyType(typeof(string))]
        Stringt,
        [FieldRealyType(typeof(string))]
        Res,
        [FieldRealyType(typeof(string))]
        Lua,

        [FieldRealyType(typeof(IList))]
        Array,
        [FieldRealyType(typeof(IDictionary))]
        Dic,

        Max,
    }
}
