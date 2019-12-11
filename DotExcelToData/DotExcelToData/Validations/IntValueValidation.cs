using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dot.Tools.ETD.Datas;
using Dot.Tools.ETD.Fields;
using ExtractInject;

namespace Dot.Tools.ETD.Validations
{
    public class IntValueValidation : IValidation
    {
        [EIField(EIFieldUsage.In, false)]
        public AField field;
        [EIField]
        public CellContent cell;

        public string GetRule()
        {
            return "IntValue";
        }

        public void SetRule(string rule)
        {
            
        }

        public ResultCode Valid(out string msg)
        {
            msg = null;
            return ResultCode.Success;
        }
    }
}
