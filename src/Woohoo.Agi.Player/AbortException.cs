// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Player
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    internal class AbortException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AbortException"/> class.
        /// Initializes a new instance of the class.
        /// </summary>
        public AbortException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbortException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public AbortException(string message)
            : this(message, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbortException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public AbortException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbortException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
        protected AbortException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
