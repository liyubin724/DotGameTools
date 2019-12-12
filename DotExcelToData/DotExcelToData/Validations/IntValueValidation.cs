using Dot.Tools.ETD.Datas;
using Dot.Tools.ETD.Fields;
using ExtractInject;

namespace Dot.Tools.ETD.Validations
{
    public class IntValueValidation : IValidation
    {
        [EIField(EIFieldUsage.In, false)]
        public AField field;
        [EIField(EIFieldUsage.In, false)]
        public CellContent cell;

        private bool isValid = true;
        public bool IsValid => isValid;

        public void SetData(string rule)
        {
            if(field.Type != FieldType.Int || field.Type != FieldType.Ref)
            {
                isValid = false;
            }
        }

        public ResultCode Verify(out string msg)
        {
            msg = null;

            if (field == null || cell == null)
            {
                msg = "IntValueValidation::Verify->Argument is null!";
                return ResultCode.ArgIsNull;
            }

            string content = field.GetContent(cell);
            if (string.IsNullOrEmpty(content))
            {
                msg = $"IntValueValidation::Verify->Cell Content is null. Row = {cell.Row},Col = {cell.Col}.";
                return ResultCode.ContentIsNull;
            }

            if (!int.TryParse(content, out int value))
            {
                msg = $"IntValueValidation::Verify->Parse content error.Row = {cell.Row},Col = {cell.Col},Content = {content}";
                return ResultCode.ParseContentFailed;
            }

            return ResultCode.Success;
        }
    }
}
