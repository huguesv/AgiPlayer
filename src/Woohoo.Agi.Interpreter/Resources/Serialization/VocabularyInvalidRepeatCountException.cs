// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Resources.Serialization;

/// <summary>
/// The exception that is thrown when the character repeat count is invalid.
/// </summary>
[Serializable]
public class VocabularyInvalidRepeatCountException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VocabularyInvalidRepeatCountException"/> class.
    /// </summary>
    public VocabularyInvalidRepeatCountException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VocabularyInvalidRepeatCountException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public VocabularyInvalidRepeatCountException(string message)
        : this(message, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VocabularyInvalidRepeatCountException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
    public VocabularyInvalidRepeatCountException(string message, Exception? innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VocabularyInvalidRepeatCountException"/> class with serialized data.
    /// </summary>
    /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
    protected VocabularyInvalidRepeatCountException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
