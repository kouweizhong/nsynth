﻿/******************************************************************************
 * NSynth - A Managed Multimedia API - http://nsynth.gearedstudios.com/       *
 * Copyright © 2009-2013 William 'cathode' Shelley. All Rights Reserved.      *
 * This software is released under the terms and conditions of the MIT/X11    *
 * license; see the included 'license.txt' file for the full text.            *
 *****************************************************************************/

namespace NSynth.Imaging.TIFF
{
    /// <summary>
    /// Represents the orientation of a TIFF image with respect to rows and columns of pixels.
    /// </summary>
    public enum TIFFOrientation
    {
        /// <summary>
        /// The 0th row represents the visual top of the image, and the 0th column represents the visual left-hand side.
        /// </summary>
        TopLeft = 1,

        /// <summary>
        /// The 0th row represents the visual top of the image, and the 0th column represents the visual right-hand side.
        /// </summary>
        TopRight = 2,

        /// <summary>
        /// The 0th row represents the visual bottom of the image, and the 0th column represents the visual right-hand side.
        /// </summary>
        BottomRight = 3,

        /// <summary>
        /// The 0th row represents the visual bottom of the image, and the 0th column represents the visual left-hand side.
        /// </summary>
        BottomLeft = 4,

        /// <summary>
        /// The 0th row represents the visual left-hand side of the image, and the 0th column represents the visual top.
        /// </summary>
        LeftTop = 5,

        /// <summary>
        /// The 0th row represents the visual right-hand side of the image, and the 0th column represents the visual top.
        /// </summary>
        RightTop = 6,

        /// <summary>
        /// The 0th row represents the visual right-hand side of the image, and the 0th column represents the visual bottom.
        /// </summary>
        RightBottom = 7,

        /// <summary>
        /// The 0th row represents the visual left-hand side of the image, and the 0th column represents the visual bottom.
        /// </summary>
        LeftBottom = 8
    }
}
