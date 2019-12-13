using Dot.Tools.ETD.Datas;
using Dot.Tools.ETD.Fields;
using Dot.Tools.ETD.Utils;
using ExtractInject;
using System.Collections.Generic;

namespace Dot.Tools.ETD.Validations
{
    public class DicKeyValidation : IValidation
    {
        [EIField(EIFieldUsage.In, false)]
        public AField field;
        [EIField(EIFieldUsage.In, false)]
        public CellContent cell;

        public string ErrorMsg { get; set; }

        private bool isValid = true;
        public bool IsValid =>isValid;

        public void SetData(string rule)
        {
            if (field.Type != FieldType.Dic)
            {
                ErrorMsg = "FileType is not <FieldType.Int/FieldType.Ref>";
                isValid = false;
            }
        }

        public ResultCode Verify(out string msg)
        {
            msg = null;

            if (field == null || cell == null)
            {
                msg = "DicKeyValidation::Verify->Argument is null!";
                return ResultCode.ArgIsNull;
            }

            string content = field.GetContent(cell);
            if (string.IsNullOrEmpty(content))
            {
                msg = $"DicKeyValidation::Verify->Cell Content is null. Row = {cell.Row},Col = {cell.Col}.";
                return ResultCode.ContentIsNull;
            }

            if(content[0]!='{' || content[content.Length-1]!='}')
            {
                msg = $"DicKeyValidation::Verify->Cell Content should start with {{ and end with }} . Row = {cell.Row},Col = {cell.Col}.";
                return ResultCode.ContentDicFormatError;
            }

            string[] values = ContentUtil.SplitContent(content, new char[] { ',', ';' });
            if(values!=null && values.Length%2!=0)
            {
                msg = $"DicKeyValidation::Verify->The length should be Divisible by 2 . Row = {cell.Row},Col = {cell.Col}.";
                return ResultCode.ContentDicKeyValueCountError;
            }
            List<object> keys = new List<object>();
            for(int  i=0;i<values.Length;i+=2)
            {
                var key = ContentUtil.GetValue(values[i], ((DicField)field).KeyFieldType);
                if(keys.Contains(key))
                {
                    msg = $"DicKeyValidation::Verify->Key repeat . Row = {cell.Row},Col = {cell.Col}.key ={key}";
                    return ResultCode.ContentDicKeyRepeatError;
                }
            }
            return ResultCode.Success;
        }
    }
}
