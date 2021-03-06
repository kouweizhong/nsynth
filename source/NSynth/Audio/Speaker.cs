﻿/******************************************************************************
 * NSynth - A Managed Multimedia API - http://nsynth.gearedstudios.com/       *
 * Copyright © 2009-2013 William 'cathode' Shelley. All Rights Reserved.      *
 * This software is released under the terms and conditions of the MIT/X11    *
 * license; see the included 'license.txt' file for the full text.            *
 *****************************************************************************/
using System;

namespace NSynth.Audio
{
    /// <summary>
    /// Represents supported speaker positions.
    /// </summary>
    [Flags]
    public enum Speaker
    {
        /// <summary>
        /// Indicates no speaker position specified.
        /// </summary>
        None = 0,

        /// <summary>
        /// Indicates a speaker located in the center.
        /// </summary>
        Center = 1,

        /// <summary>
        /// Indicates a speaker located to the left of the listener.
        /// </summary>
        Left = 2,

        /// <summary>
        /// Indicates a speaker located to the right of the listener.
        /// </summary>
        Right = 4,

        /// <summary>
        /// Indicates a speaker located in front of the listener.
        /// </summary>
        Front = 8,

        /// <summary>
        /// Indicates a speaker located behind the listener.
        /// </summary>
        Rear = 16,

        /// <summary>
        /// Indicates a speaker located to the side of the listener.
        /// </summary>
        Side = 32,

        /// <summary>
        /// Indicates a speaker located farther away from the listener.
        /// </summary>
        Far = 64,

        /// <summary>
        /// Indicates a speaker located closer to the listener.
        /// </summary>
        Near = 128,

        /// <summary>
        /// Indicates a speaker located above the listener.
        /// </summary>
        Upper = 256,

        /// <summary>
        /// Indicates a speaker located below the listener.
        /// </summary>
        Lower = 512,

        /// <summary>
        /// Indicates a low-frequency effects speaker (subwoofer)
        /// </summary>
        LowFrequency = 1024,

        /// <summary>
        /// Indicates all speakers.
        /// </summary>
        All = 2047,
    }
}
