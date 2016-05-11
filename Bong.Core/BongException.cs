using System;
using System.Runtime.Serialization;

namespace Bong.Core
{
    [Serializable]
    public class BongException : Exception
    {
        public BongException()
        {
        }

        public BongException(string message) : base(message)
        {
        }

        public BongException(string messageFormat, params object[] args) : base(string.Format(messageFormat, args))
        {
        }

        protected BongException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public BongException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
