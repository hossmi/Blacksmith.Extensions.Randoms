using System;
using System.Runtime.Serialization;

namespace Blacksmith.Exceptions
{
    [Serializable]
    public class TooSmallShuffleBufferSize : Exception
    {
        public TooSmallShuffleBufferSize(int bufferSize) : base("Buffer of size less than 2 has no sense for shuffle.")
        {
            this.BufferSize = bufferSize;
        }

        protected TooSmallShuffleBufferSize(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public int BufferSize { get; }
    }
}