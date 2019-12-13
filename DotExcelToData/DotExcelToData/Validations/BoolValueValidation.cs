using Dot.Tools.ETD.Datas;
using Dot.Tools.ETD.Fields;
using ExtractInject;

namespace Dot.Tools.ETD.Validations
{
    public class BoolValueValidation : IValidation
    {
        [EIField(EIFieldUsage.In, false)]
        public AField field;
        [EIField(EIFieldUsage.In, false)]
        public CellContent cell;

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

        public ResultCode Verify(out string msg)
        {
            msg = null;

            if (field == null || cell == null)
            {
                msg = "BoolValidation::Verify->Argument is null!";
                return ResultCode.ArgIsNull;
            }

            string content = field.GetContent(cell);
            if (string.IsNullOrEmpty(content))
            {
                msg = $"BoolValidation::Verify->Cell Content is null. Row = {cell.Row},Col = {cell.Col}.";
                return ResultCode.ContentIsNull;
            }

            if (!bool.TryParse(content, out bool value))
            {
                msg = $"BoolValidation::Verify->Parse content error.Row = {cell.Row},Col = {cell.Col},Content = {content}";
                return ResultCode.ParseContentFailed;
            }

            return ResultCode.Success;
        }
    }
}
