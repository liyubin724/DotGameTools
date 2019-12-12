using Dot.Tools.ETD.Datas;
using Dot.Tools.ETD.Fields;
using ExtractInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Dot.Tools.ETD.Validations
{
    public class IntRangeValidation : IValidation
    {
        private const string RANGE_REGEX = @"IntRange\{(?<min>[-]{0,1}[0-9]+),(?<max>[-]{0,1}[0-9]+)\}";

        [EIField(EIFieldUsage.In, false)]
        public AField field;
        [EIField(EIFieldUsage.In, false)]
        public CellContent cell;

        public int min;
        public int max;

        private bool isValid = true;
        public bool IsValid => isValid;

        public void SetData(string rule)
        {
            if(field.Type != FieldType.Int || field.Type != FieldType.Ref)
            {
                isValid = false;
                return;
            }

            Match match = new Regex(RANGE_REGEX).Match(rule);
            Group group = match.Groups["min"];
            if (group.Success)
            {
                if (!int.TryParse(group.Value, out min))
                {
                    isValid = false;
                }
            }
            group = match.Groups["max"];
            if (group.Success)
            {
                if (!int.TryParse(group.Value, out max))
                {
                    isValid = false;
                }
            }
        }

        public ResultCode Verify(out string msg)
        {
            msg = null;

            if (field == null || cell == null)
            {
                msg = "IntRangeValidation::Verify->Argument is null!";
                return ResultCode.ArgIsNull;
            }

            string content = field.GetContent(cell);
            if (string.IsNullOrEmpty(content))
            {
                msg = $"IntRangeValidation::Verify->Cell Content is null. Row = {cell.Row},Col = {cell.Col}.";
                return ResultCode.ContentIsNull;
            }

            if (!int.TryParse(content, out int value))
            {
                msg = $"IntRangeValidation::Verify->Parse content error.Row = {cell.Row},Col = {cell.Col},Content = {content}";
                return ResultCode.ParseContentFailed;
            }
            else
            {
                if (value >= min && value <= max)
                {
                    return ResultCode.Success;
                }
                else
                {
                    msg = $"IntRangeValidation::Verify->Compare error.Row = {cell.Row},Col = {cell.Col},Compare={min}--{value}--{max}";
                    return ResultCode.NumberRangeError;
                }
            }
        }
    }
}
