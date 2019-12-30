using System;
using System.Runtime.Serialization;

namespace Blacksmith.Exceptions
{
    [Serializable]
    public class EmptyCollectionException : ArgumentOutOfRangeException
    {
        public EmptyCollectionException() : base()
        {
        }

        public EmptyCollectionException(string paramName) : base(paramName)
        {
        }

        protected EmptyCollectionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}