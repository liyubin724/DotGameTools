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

        public bool IsValid => throw new NotImplementedException();

        public bool SetData(string rule)
        {
            Match match = new Regex(RANGE_REGEX).Match(rule);
            Group group = match.Groups["min"];
            if(group.Success)
            {
                if(!int.TryParse(group.Value,out min))
                {
                    return false;
                }
            }
            group = match.Groups["max"];
            if(group.Success)
            {
                if(!int.TryParse(group.Value,out max))
                {
                    return false;
                }
            }
            return true;
        }

        public ResultCode Verify(out string msg)
        {
            throw new NotImplementedException();
        }

        void IValidation.SetData(string rule)
        {
            throw new NotImplementedException();
        }
    }
}
