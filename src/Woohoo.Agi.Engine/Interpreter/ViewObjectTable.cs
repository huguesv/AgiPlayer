// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

#nullable enable

namespace Woohoo.Agi.Engine.Interpreter;

/// <summary>
/// Table of view objects.
/// </summary>
public class ViewObjectTable
{
    /// <summary>
    /// Index of the ego view object.
    /// </summary>
    public const int EgoIndex = 0;

    private readonly ViewObject[] objects;

    /// <summary>
    /// Initializes a new instance of the <see cref="ViewObjectTable"/> class.
    /// </summary>
    /// <param name="size">Size of the object table.</param>
    public ViewObjectTable(int size)
    {
        this.objects = new ViewObject[size];
        for (int i = 0; i < this.objects.Length; i++)
        {
            this.objects[i] = new ViewObject
            {
                Number = (byte)i,
            };
        }
    }

    /// <summary>
    /// Gets size of the object table.
    /// </summary>
    public int Length => this.objects.Length;

    /// <summary>
    /// Retrieve the index of the specified view object.
    /// </summary>
    /// <param name="view">View object to find.</param>
    /// <returns>Index of object, or -1 if not found.</returns>
    public int IndexOf(ViewObject view)
    {
        return Array.IndexOf(this.objects, view);
    }

    /// <summary>
    /// Get the view object at the specified index.
    /// </summary>
    /// <param name="index">Index.</param>
    /// <returns>View object.</returns>
    public ViewObject GetAt(int index)
    {
        return this.objects[index];
    }
}
