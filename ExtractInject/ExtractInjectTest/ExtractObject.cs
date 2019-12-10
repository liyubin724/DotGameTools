using ExtractInject;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExtractInjectTest
{
    public class ExtractObject : ExtractInjectTarget
    {
        [ExtractInjectField(ExtractInjectUsage.Out,false)]
        public EIObject eiObj;

        public ExtractObject()
        {
            eiObj = new EIObject();
        }

    }
}
