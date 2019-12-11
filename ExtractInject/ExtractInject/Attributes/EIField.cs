using System;

namespace ExtractInject
{
    /// <summary>
    /// 设定字段使用方式
    /// </summary>
    public enum EIFieldUsage
    {
        InOut,//字段即作为注入也做为提取
        In,//字段作为注入
        Out,//字段作为提取
    }

    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Field,AllowMultiple =false)]
    public class EIField : Attribute
    {
        public EIFieldUsage Usage { get; set; }
        public bool IsOptional { get; set; }

        public EIField(EIFieldUsage usage = EIFieldUsage.InOut,bool optional = false)
        {
            Usage = usage;
            IsOptional = optional;
        }
    }
}
