// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Engine.Interpreter;

[Serializable]
public class ExecutionException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExecutionException"/> class.
    /// </summary>
    public ExecutionException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExecutionException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public ExecutionException(string message)
        : this(message, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExecutionException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
    public ExecutionException(string message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
