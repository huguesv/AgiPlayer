// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Resources.Serialization
{
    using System;
    using System.Globalization;
    using System.Runtime.Serialization;

    /// <summary>
    /// The exception that is thrown when the pixel color repeat count is invalid.
    /// </summary>
    [Serializable]
    public class RleCompressionInvalidRepeatCountException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RleCompressionInvalidRepeatCountException"/> class.
        /// </summary>
        public RleCompressionInvalidRepeatCountException()
        {
        }

        public RleCompressionInvalidRepeatCountException(int repeatCount)
            : this(string.Format(CultureInfo.InvariantCulture, "Invalid repeat count: {0}.", repeatCount.ToString(CultureInfo.InvariantCulture)))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RleCompressionInvalidRepeatCountException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public RleCompressionInvalidRepeatCountException(string message)
            : this(message, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RleCompressionInvalidRepeatCountException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public RleCompressionInvalidRepeatCountException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RleCompressionInvalidRepeatCountException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
        protected RleCompressionInvalidRepeatCountException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
