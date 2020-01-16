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
    public class NotNullValidation : IValidation
    {
        [EIField(EIFieldUsage.In, false)]
        public AField field;
        [EIField(EIFieldUsage.In, false)]
        public LineCell cell;

        public void SetRule(string rule)
        {
            throw new System.NotImplementedException();
        }

        public ValidationResultCode Verify(EIContext context)
        {
            throw new System.NotImplementedException();
        }


        //--------------------------------

        private bool isValid = true;
        public bool IsValid => isValid;

        public string ErrorMsg { get; set; }

        public void SetData(string rule)
        {
        }

        public ValidationResultCode Verify(out string msg)
        {
            msg = null;

            if (field == null || cell == null)
            {
                msg = "NotNullValidation::Verify->Argument is null!";
                return ValidationResultCode.ArgIsNull;
            }

            string content = field.GetContent(cell);
            if (string.IsNullOrEmpty(content))
            {
                msg = $"NotNullValidation::Verify->Cell Content is null. Row = {cell.row},Col = {cell.col}.";
                return ValidationResultCode.ContentIsNull;
            }

            return ValidationResultCode.Success;
        }
    }
}
