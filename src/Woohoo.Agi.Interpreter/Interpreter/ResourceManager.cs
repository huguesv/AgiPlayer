// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Interpreter
{
    using Woohoo.Agi.Resources;

    /// <summary>
    /// Manages the currently loaded game resources.
    /// </summary>
    public class ResourceManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceManager"/> class.
        /// </summary>
        public ResourceManager()
        {
            this.LogicResources = new Collection<LogicResource>();
            this.ViewResources = new Collection<ViewResource>();
            this.PictureResources = new Collection<PictureResource>();
            this.SoundResources = new Collection<SoundResource>();
        }

        /// <summary>
        /// Gets logic resources.
        /// </summary>
        public Collection<LogicResource> LogicResources { get; }

        /// <summary>
        /// Gets view resources.
        /// </summary>
        public Collection<ViewResource> ViewResources { get; }

        /// <summary>
        /// Gets picture resources.
        /// </summary>
        public Collection<PictureResource> PictureResources { get; }

        /// <summary>
        /// Gets sound resources.
        /// </summary>
        public Collection<SoundResource> SoundResources { get; }

        /// <summary>
        /// Gets or sets vocabulary resource.
        /// </summary>
        public VocabularyResource VocabularyResource { get; set; }

        /// <summary>
        /// Gets or sets inventory resource.
        /// </summary>
        public InventoryResource InventoryResource { get; set; }

        /// <summary>
        /// Find the logic resource with the specified resource index.
        /// </summary>
        /// <param name="logicResourceIndex">Resource index to find.</param>
        /// <returns>Logic resource, or null if not found.</returns>
        public LogicResource FindLogic(int logicResourceIndex)
        {
            foreach (LogicResource resource in this.LogicResources)
            {
                if (resource.ResourceIndex == logicResourceIndex)
                {
                    return resource;
                }
            }

            return null;
        }

        /// <summary>
        /// Find the view resource with the specified resource index.
        /// </summary>
        /// <param name="viewResourceIndex">Resource index to find.</param>
        /// <returns>View resource, or null if not found.</returns>
        public ViewResource FindView(int viewResourceIndex)
        {
            foreach (ViewResource resource in this.ViewResources)
            {
                if (resource.ResourceIndex == viewResourceIndex)
                {
                    return resource;
                }
            }

            return null;
        }

        /// <summary>
        /// Find the sound resource with the specified resource index.
        /// </summary>
        /// <param name="soundResourceIndex">Resource index to find.</param>
        /// <returns>Sound resource, or null if not found.</returns>
        public SoundResource FindSound(int soundResourceIndex)
        {
            foreach (SoundResource resource in this.SoundResources)
            {
                if (resource.ResourceIndex == soundResourceIndex)
                {
                    return resource;
                }
            }

            return null;
        }

        /// <summary>
        /// Find the picture resource with the specified resource index.
        /// </summary>
        /// <param name="pictureResourceIndex">Resource index to find.</param>
        /// <returns>Picture resource, or null if not found.</returns>
        public PictureResource FindPicture(int pictureResourceIndex)
        {
            foreach (PictureResource resource in this.PictureResources)
            {
                if (resource.ResourceIndex == pictureResourceIndex)
                {
                    return resource;
                }
            }

            return null;
        }
    }
}
