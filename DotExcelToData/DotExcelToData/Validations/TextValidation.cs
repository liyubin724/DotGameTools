using Dot.Tools.ETD.Datas;
using Dot.Tools.ETD.Fields;
using Dot.Tools.ETD.Log;
using ExtractInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Tools.ETD.Validations
{
    public class TextValidation : IValidation
    {
        [EIField(EIFieldUsage.In,false)]
        private LogHandler logHandler;

        [EIField(EIFieldUsage.In, false)]
        public AFieldData field;

        [EIField(EIFieldUsage.In, false)]
        public LineCell cell;

        [EIField(EIFieldUsage.In,false)]
        public Workbook book;

        public void SetRule(string rule)
        {
        }

        public ValidationResultCode Verify()
        {
            
        }
    }
}
