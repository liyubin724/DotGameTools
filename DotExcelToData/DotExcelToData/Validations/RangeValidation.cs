using Dot.Tools.ETD.Datas;
using Dot.Tools.ETD.Fields;
using Dot.Tools.ETD.Log;
using Dot.Tools.ETD.Fields;
using ExtractInject;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Dot.Tools.ETD.Validations
{
    public class RangeValidation : IValidation
    {
        private const string RANGE_REGEX = @"Range\{(?<min>[-]{0,1}[0-9.]+),(?<max>[-]{0,1}[0-9.]+)\}";

        [EIField(EIFieldUsage.In, false)]
        public AFieldData field;
        [EIField(EIFieldUsage.In, false)]
        public LineCell cell;

        private string rule = string.Empty;
        public void SetRule(string rule)
        {
            this.rule = rule;
        }

        public ValidationResultCode Verify(IEIContext context)
        {
            LogHandler logHandler = context.GetObject<LogHandler>();

            float min = 0.0f;
            float max = 0.0f;
            Match match = new Regex(RANGE_REGEX).Match(rule);
            Group group = match.Groups["min"];
            if (!group.Success || !float.TryParse(group.Value, out min))
            {
                logHandler.Log(LogType.Error, LogConst.LOG_VALIDATION_RANGE_MIN_ERROR, cell.col);
                return ValidationResultCode.ValidationFormatError;
            }
            group = match.Groups["max"];
            if (!group.Success || !float.TryParse(group.Value, out max)) 
            {
                logHandler.Log(LogType.Error, LogConst.LOG_VALIDATION_RANGE_MAX_ERROR, cell.col);
                return ValidationResultCode.ValidationFormatError;
            }

            if (max <= min)
            {
                logHandler.Log(LogType.Error, LogConst.LOG_VALIDATION_RANGE_MAX_LESS_MIN_ERROR, cell.col,min,max);
                return ValidationResultCode.ValidationFormatError;
            }

            if (FieldTypeUtil.IsNumberType(field.Type) ||
                field.Type == FieldType.Array && FieldTypeUtil.IsNumberType(((ArrayFieldData)field).ValueType))
            {
                if (field == null || cell == null)
                {
                    logHandler.Log(LogType.Error, LogConst.LOG_ARG_IS_NULL);

                    return ValidationResultCode.ArgIsNull;
                }

                string content = cell.GetContent(field);
                if (string.IsNullOrEmpty(content))
                {
                    logHandler.Log(LogType.Warning, LogConst.LOG_VALIDATION_NULL, cell.row, cell.col);
                    return ValidationResultCode.ContentIsNull;
                }

                List<string> values = new List<string>();
                if (field.Type != FieldType.Array)
                {
                    values.Add(content);
                }else
                {
                    string[] valueArr = content.Split(new char[] { '{', '}', ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if(valueArr!=null && valueArr.Length>0)
                    {
                        values.AddRange(valueArr);
                    }
                }
                if(values.Count>0)
                {
                    foreach(var v in values)
                    {
                        if(string.IsNullOrEmpty(v))
                        {
                            logHandler.Log(LogType.Error, LogConst.LOG_VALIDATION_NULL, cell.row, cell.col);
                            return ValidationResultCode.ContentIsNull;
                        }else
                        {
                            if(!float.TryParse(v,out float floatValue))
                            {
                                logHandler.Log(LogType.Error, LogConst.LOG_VALIDATION_CONVERT_ERROR, "float", cell.ToString());
                                return ValidationResultCode.ParseContentFailed;
                            }else
                            {
                                if(floatValue<min || floatValue>max)
                                {
                                    logHandler.Log(LogType.Error, LogConst.LOG_VALIDATION_TYPE_FOR_RANGE_ERROR, cell.row, cell.col, field.Type);
                                    return ValidationResultCode.NumberRangeError;
                                }
                            }
                        }
                    }
                }
                return ValidationResultCode.Success;
            }
            else
            {
                logHandler.Log(LogType.Error, LogConst.LOG_VALIDATION_TYPE_FOR_RANGE_ERROR,cell.row,cell.col,field.Type);
                return ValidationResultCode.NumberRangeError;
            }
        }
    }
}
