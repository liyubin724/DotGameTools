using Dot.Tools.ETD.Datas;
using Dot.Tools.ETD.Fields;
using ExtractInject;

namespace Dot.Tools.ETD.Validations
{
    public class LongValueValidation : IValidation
    {
        [EIField(EIFieldUsage.In, false)]
        public AField field;
        [EIField(EIFieldUsage.In, false)]
        public LineCell cell;

        private bool isValid = true;
        public bool IsValid => isValid;

        public string ErrorMsg { get; set; } = null;

        public void SetData(string rule)
        {
            if (field.Type != FieldType.Long)
            {
                ErrorMsg = "FileType is not <FieldType.Long>";
                isValid = false;
            }
        }

        public ValidationResultCode Verify(out string msg)
        {
            msg = null;

            if (field == null || cell == null)
            {
                msg = "LongValueValidation::Verify->Argument is null!";
                return ValidationResultCode.ArgIsNull;
            }

            string content = field.GetContent(cell);
            if (string.IsNullOrEmpty(content))
            {
                msg = $"LongValueValidation::Verify->Cell Content is null. Row = {cell.row},Col = {cell.col}.";
                return ValidationResultCode.ContentIsNull;
            }

            if (!long.TryParse(content, out long value))
            {
                msg = $"LongValueValidation::Verify->Parse content error.Row = {cell.row},Col = {cell.col},Content = {content}";
                return ValidationResultCode.ParseContentFailed;
            }

            return ValidationResultCode.Success;
        }
    }
}
