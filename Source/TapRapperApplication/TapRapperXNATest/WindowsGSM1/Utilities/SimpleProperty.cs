using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities
{
    public class SimpleProperty
    {
        public SimpleProperty()
        {
        }

        public SimpleProperty(string pName,object pValue)
        {
            Name = pName;
            Value = pValue;
        }

        public string Name { get; set; }
        public object Value { get; set; }
    }
}
