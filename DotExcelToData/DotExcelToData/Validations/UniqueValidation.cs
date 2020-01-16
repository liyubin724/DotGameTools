﻿using Dot.Tools.ETD.Datas;
using Dot.Tools.ETD.Fields;
using ExtractInject;

namespace Dot.Tools.ETD.Validations
{
    public class UniqueValidation : IValidation
    {
        [EIField(EIFieldUsage.In, false)]
        public Sheet sheet;
        [EIField(EIFieldUsage.In, false)]
        public AField field;
        [EIField(EIFieldUsage.In, false)]
        public CellContent cell;

        private bool isValid = true;
        public bool IsValid => isValid;

        public string ErrorMsg { get; set; }

        public void SetData(string rule)
        {
        }

        public ValidationResultCode Verify(out string msg)
        {
            msg = null;
            if (field == null || cell == null || sheet == null)
            {
                msg = "UniqueValueValidation::Verify->Argument is null!";
                return ValidationResultCode.ArgIsNull;
            }

            string content = field.GetContent(cell);
            if(string.IsNullOrEmpty(content))
            {
                msg = "UniqueValueValidation::Verify->Content is null";
                return ValidationResultCode.ContentIsNull;
            }

            int index = sheet.Field.fields.IndexOf(field);

            foreach(var line in sheet.Line.lines)
            {
                CellContent cc = line.cells[index];
                if(cc.Row == cell.Row)
                {
                    continue;
                }
                string c = field.GetContent(cc);
                if(c == content)
                {
                    msg = $"UniqueValueValidation::Verify->Content is Repeat.content = {content},Row={cell.Row}--{cc.Row},Col = {cell.Col}";
                    return ValidationResultCode.ContentRepeatError;
                }
            }

            return ValidationResultCode.Success;
        }
    }
}
