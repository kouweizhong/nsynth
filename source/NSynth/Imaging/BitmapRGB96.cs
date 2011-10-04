﻿/******************************************************************************
 * NSynth - A Managed Multimedia API - http://nsynth.gearedstudios.com/       *
 * Copyright © 2009-2011 William 'cathode' Shelley. All Rights Reserved.      *
 * This software is released under the terms and conditions of the MIT/X11    *
 * license; see the included 'license.txt' file for the full text.            *
 *****************************************************************************/
using System;
using System.Diagnostics.Contracts;

namespace NSynth.Imaging
{
    /// <summary>
    /// Represents a two-dimensional grid of 96-bit RGB color values.
    /// </summary>
    public sealed class BitmapRGB96 : Bitmap<ColorRGB96>
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapRGB96"/> class.
        /// </summary>
        /// <param name="size">The dimensions (in pixels) of the new bitmap.</param>
        public BitmapRGB96(Size size)
            : base(size)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapRGB96"/> class.
        /// </summary>
        /// <param name="size">The dimensions (in pixels) of the new bitmap.</param>
        /// <param name="pixels">The pixel data to initialize the new bitmap with.</param>
        public BitmapRGB96(Size size, ColorRGB96[] pixels)
            : base(size, pixels)
        {
            Contract.Requires(pixels != null);
            Contract.Requires(pixels.Length == size.Elements);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapRGB96"/> class.
        /// </summary>
        /// <param name="width">The width (in pixels) of the new bitmap.</param>
        /// <param name="height">The height (in pixels) of the new bitmap.</param>
        public BitmapRGB96(int width, int height)
            : base(width, height)
        {
            Contract.Requires(width >= 0);
            Contract.Requires(height >= 0);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapRGB96"/> class.
        /// </summary>
        /// <param name="width">The width (in pixels) of the new bitmap.</param>
        /// <param name="height">The height (in pixels) of the new bitmap.</param>
        /// <param name="pixels">The pixel data to initialize the new bitmap with.</param>
        public BitmapRGB96(int width, int height, ColorRGB96[] pixels)
            : base(width, height, pixels)
        {
            Contract.Requires(width >= 0);
            Contract.Requires(height >= 0);
            Contract.Requires(pixels != null);
            Contract.Requires(pixels.Length == width * height);
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets a <see cref="ColorFormat"/> that describes how color information is stored in the bitmap.
        /// </summary>
        public override ColorFormat Format
        {
            get
            {
                return ColorFormat.RGB96;
            }
        }
        #endregion
        #region Methods
        protected override ColorRGB96 GetTColor(IColor color)
        {
            return new ColorRGB96(color.Red, color.Green, color.Blue);
        }
        #endregion
    }
}
