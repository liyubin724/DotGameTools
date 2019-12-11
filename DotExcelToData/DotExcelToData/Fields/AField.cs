using Dot.Tools.ETD.Datas;
using Dot.Tools.ETD.Factorys;
using Dot.Tools.ETD.Validations;
using System.Collections.Generic;

namespace Dot.Tools.ETD.Fields
{
    public abstract class AField
    {
        protected int col;
        public int Col { get => col; }

        public string Name { get; set; }
        public string Desc { get; set; }
        protected string type;
        public abstract FieldType Type { get;}
        protected string platform;
        public FieldPlatform Platform
        {
            get
            {
                if (platform == "c")
                {
                    return FieldPlatform.Client;
                }
                else if (platform == "s")
                {
                    return FieldPlatform.Server;
                }
                else if (platform == "cs")
                {
                    return FieldPlatform.All;
                }
                return FieldPlatform.None;
            }
            set
            {
                if(value == FieldPlatform.None)
                {
                    value = FieldPlatform.All;
                }

                if(value == FieldPlatform.Client)
                {
                    platform = "c";
                }else if(value == FieldPlatform.Server)
                {
                    platform = "s";
                }else
                {
                    platform = "cs";
                }
            }
        }
        public string DefaultValue { get; set; }
        protected string validationRule;
        public virtual List<IValidation> Validations
        {
            get
            {
                return ValidationFactory.ParseRules(validationRule);
            }
            set
            {
                if(value == null || value.Count == 0)
                {
                    validationRule = "";
                }else
                {
                    List<string> ruleNames = new List<string>();
                    foreach(var rule in value)
                    {
                        ruleNames.Add(rule.GetRule());
                    }
                    validationRule = string.Join("@", ruleNames.ToArray());
                }
            }
        }

        public abstract FieldType InnerValueType { get; }
        public abstract FieldType InnerKeyType { get; }
        public abstract string RefTargetName { get; }

        public AField(int c,string n,string d,string t,string p,string dv,string vr)
        {
            col = c;
            Name = n;
            Desc = d;
            type = t == null ? "" : t.ToLower();
            platform = p == null ? "cs" : p.ToLower();
            DefaultValue = dv;
            validationRule = vr;
        }

        public abstract object GetValue(CellContent cell);

        public FieldPlatform GetPlatform()
        {
            if(platform == "c")
            {
                return FieldPlatform.Client;
            }else if(platform == "s")
            {
                return FieldPlatform.Server;
            }else if(platform == "cs")
            {
                return FieldPlatform.All;
            }
            return FieldPlatform.None;
        }
    }
}
