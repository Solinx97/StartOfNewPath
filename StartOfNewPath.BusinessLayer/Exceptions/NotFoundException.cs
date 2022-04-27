using System;
using System.Runtime.Serialization;

namespace StartOfNewPath.BusinessLayer.Exceptions
{
    [Serializable]
    public class NotFoundException : Exception
    {
        public NotFoundException()
        {
        }

        public NotFoundException(string message)
            : base(message)
        {
        }

        public NotFoundException(string message, string paramName)
            : base(message)
        {
            if (paramName == null)
            {
                throw new ArgumentNullException(paramName);
            }

            ParamName = paramName;
        }

        public NotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected NotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }

        public string ParamName { get; set; }
    }
}
