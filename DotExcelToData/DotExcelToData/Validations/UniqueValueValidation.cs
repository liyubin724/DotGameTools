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
    public class UniqueValueValidation : IValidation
    {
        [EIField(EIFieldUsage.In, false)]
        public Sheet sheet;
        [EIField(EIFieldUsage.In, false)]
        public AField field;
        [EIField(EIFieldUsage.In, false)]
        public CellContent cell;

        private bool isValid = true;
        public bool IsValid => isValid;

        public void SetData(string rule)
        {

        }

        public ResultCode Verify(out string msg)
        {
            msg = null;
            if (field == null || cell == null || sheet == null)
            {
                msg = "IntRangeValidation::Verify->Argument is null!";
                return ResultCode.ArgIsNull;
            }

            string content = field.GetContent(cell);

            return ResultCode.Failed;
        }
    }
}
