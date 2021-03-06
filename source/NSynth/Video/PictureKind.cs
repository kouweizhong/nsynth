﻿/******************************************************************************
 * NSynth - A Managed Multimedia API - http://nsynth.gearedstudios.com/       *
 * Copyright © 2009-2013 William 'cathode' Shelley. All Rights Reserved.      *
 * This software is released under the terms and conditions of the MIT/X11    *
 * license; see the included 'license.txt' file for the full text.            *
 *****************************************************************************/

namespace NSynth.Video
{
    /// <summary>
    /// Enumerates kinds of frame pictures within a GOP (Group-of-Pictures).
    /// </summary>
    /// <remarks>
    /// This enumeration provides a generalized view of picture coding methods.
    /// Specific video codecs may use different designations.
    /// </remarks>
    public enum PictureKind
    {
        /// <summary>
        /// Indicates an intra-coded picture, which can be decoded independantly of any previous frames.
        /// </summary>
        IFrame,

        /// <summary>
        /// Indicates a predictive-coded picture, which requires preceeding frames to decode.
        /// </summary>
        PFrame,

        /// <summary>
        /// Indicates a bidirectionally-predictive-coded picture, which requires preceeding and following frames to decode.
        /// </summary>
        BFrame,

        /// <summary>
        /// Indicates a direct-coded picture. Only used by MPEG-1.
        /// </summary>
        DFrame,
    }
}
