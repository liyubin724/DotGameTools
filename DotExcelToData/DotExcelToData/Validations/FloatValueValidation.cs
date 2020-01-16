using Dot.Tools.ETD.Datas;
using Dot.Tools.ETD.Fields;
using ExtractInject;

namespace Dot.Tools.ETD.Validations
{
    public class FloatValueValidation : IValidation
    {
        [EIField(EIFieldUsage.In, false)]
        public AField field;
        [EIField(EIFieldUsage.In, false)]
        public CellContent cell;

        private bool isValid = true;
        public bool IsValid => isValid;

        public string ErrorMsg { get; set; } = null;

        public void SetData(string rule)
        {
            if (field.Type != FieldType.Float)
            {
                ErrorMsg = "FileType is not <FieldType.Float>";
                isValid = false;
            }
        }

        public ValidationResultCode Verify(out string msg)
        {
            msg = null;

            if (field == null || cell == null)
            {
                msg = "FloatValueValidation::Verify->Argument is null!";
                return ValidationResultCode.ArgIsNull;
            }

            string content = field.GetContent(cell);
            if (string.IsNullOrEmpty(content))
            {
                msg = $"FloatValueValidation::Verify->Cell Content is null. Row = {cell.Row},Col = {cell.Col}.";
                return ValidationResultCode.ContentIsNull;
            }

            if (!float.TryParse(content, out float value))
            {
                msg = $"FloatValueValidation::Verify->Parse content error.Row = {cell.Row},Col = {cell.Col},Content = {content}";
                return ValidationResultCode.ParseContentFailed;
            }

            return ValidationResultCode.Success;
        }
    }
}
