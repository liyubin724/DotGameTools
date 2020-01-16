using Dot.Tools.ETD.Datas;
using Dot.Tools.ETD.Fields;
using ExtractInject;
using System.Collections.Generic;
using SystemObject = System.Object;

namespace Dot.Tools.ETD.Validations
{
    public class BoolValueValidation : IValidation
    {
        [EIField(EIFieldUsage.In, false)]
        public AField field;
        [EIField(EIFieldUsage.In, false)]
        public LineCell cell;

        public void SetRule(string rule)
        {
        }

        public ValidationResultMsg Verify(EIContext context)
        {
            ValidationResultCode resultCode = ValidationResultCode.Success;
            List<SystemObject> values = new List<SystemObject>();

            EIUtil.Inject(context, this);

            if (field == null || cell == null)
            {
                resultCode = ValidationResultCode.ArgIsNull;
            }else
            {

            }

            EIUtil.Extract(context, this);

            return new ValidationResultMsg(resultCode, values.ToArray());
        }







        public string ErrorMsg { get; set; }

        private bool isValid = true;
        public bool IsValid => isValid;

        public void SetData(string rule)
        {
            if(field.Type != FieldType.Bool)
            {
                ErrorMsg = "FieldType is not bool";
                isValid = false;
            }
        }

        public ValidationResultCode Verify(out string msg)
        {
            msg = null;

            if (field == null || cell == null)
            {
                msg = "BoolValidation::Verify->Argument is null!";
                return ValidationResultCode.ArgIsNull;
            }

            string content = field.GetContent(cell);
            if (string.IsNullOrEmpty(content))
            {
                msg = $"BoolValidation::Verify->Cell Content is null. Row = {cell.row},Col = {cell.col}.";
                return ValidationResultCode.ContentIsNull;
            }

            if (!bool.TryParse(content, out bool value))
            {
                msg = $"BoolValidation::Verify->Parse content error.Row = {cell.row},Col = {cell.col},Content = {content}";
                return ValidationResultCode.ParseContentFailed;
            }

            return ValidationResultCode.Success;
        }

        
    }
}
