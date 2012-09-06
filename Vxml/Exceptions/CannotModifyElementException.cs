using System;

namespace Vxml.Exceptions
{
    public class CannotModifyElementException : Exception
    {
        public CannotModifyElementException()
            : base("This element cannot be modified")
        {
            
        }
    }
}
