// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Resources.Serialization;

/// <summary>
/// The exception that is thrown when the volume files are not found in the game folder.
/// </summary>
[Serializable]
public class VolumeFilesNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VolumeFilesNotFoundException"/> class.
    /// </summary>
    public VolumeFilesNotFoundException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VolumeFilesNotFoundException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public VolumeFilesNotFoundException(string message)
        : this(message, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VolumeFilesNotFoundException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
    public VolumeFilesNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VolumeFilesNotFoundException"/> class with serialized data.
    /// </summary>
    /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
    protected VolumeFilesNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
