using System;

namespace CertificatesProblem.Model.Exceptions
{
    public class CanNotBeSolvedException : Exception
    {
        public CanNotBeSolvedException()
        {
        }

        public CanNotBeSolvedException(string message)
            : base(message)
        {
        }

        public CanNotBeSolvedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}