﻿/******************************************************************************
 * NSynth - A Managed Multimedia API - http://nsynth.gearedstudios.com/       *
 * Copyright © 2009-2013 William 'cathode' Shelley. All Rights Reserved.      *
 * This software is released under the terms and conditions of the MIT/X11    *
 * license; see the included 'license.txt' file for the full text.            *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSynth
{
    /// <summary>
    /// Represents a bitstream index that maps frame data starting offsets in the stream.
    /// </summary>
    public class BitstreamIndex
    {
        #region Fields
        private readonly List<IndexEntry> entries = new List<IndexEntry>();
        #endregion
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BitstreamIndex"/> class.
        /// </summary>
        public BitstreamIndex()
        {

        }
        #endregion
        #region Indexers

        #endregion
        #region Types
        public struct IndexEntry
        {
            public FrameCoding Coding;
            public long Offset;
        }
        #endregion
    }
}
