﻿using Dot.Tools.ETD.Datas;
using Dot.Tools.ETD.Factorys;
using Dot.Tools.ETD.Utils;
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
        public FieldType Type
        {
            get
            {
                return FieldTypeUtil.GetFieldType(type);
            }
        }
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
        }
        public string DefaultValue { get; set; }
        protected string validationRule;
        public virtual List<IValidation> Validations
        {
            get
            {
                return ValidationFactory.GetValidations(validationRule);
            }
        }

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
    }
}
