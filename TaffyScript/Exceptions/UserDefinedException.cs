using System;

namespace TaffyScript
{
    public class UserDefinedException : Exception
    {
        public UserDefinedException(string message)
            : base(message)
        {
        }
    }
}
