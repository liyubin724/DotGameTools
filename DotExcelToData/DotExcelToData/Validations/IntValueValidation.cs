using Dot.Tools.ETD.Datas;
using Dot.Tools.ETD.Fields;
using ExtractInject;

namespace Dot.Tools.ETD.Validations
{
    public class IntValueValidation : IValidation
    {
        [EIField(EIFieldUsage.In, false)]
        public AField field;
        [EIField(EIFieldUsage.In,false)]
        public CellContent cell;

        private bool isValid = true;
        public bool IsValid => isValid;

        public void SetData(string rule)
        {
        }

        public ResultCode Verify(out string msg)
        {
            msg = null;

            if(field == null || cell == null)
            {
                msg = "Argument is null!";
                return ResultCode.ArgIsNull;
            }

            return ResultCode.Success;
        }
    }
}
