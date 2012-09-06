using System;

namespace Vxml.Exceptions
{
    [Serializable]
    public class CannotModifyElementException : Exception
    {
        public CannotModifyElementException()
            : base("This element cannot be modified")
        {
            
        }
    }
}
