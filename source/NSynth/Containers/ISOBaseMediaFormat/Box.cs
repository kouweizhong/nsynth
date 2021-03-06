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
using System.Threading.Tasks;

namespace NSynth.Containers.ISOBaseMediaFormat
{
    /// <summary>
    /// Represents an "object-oriented building block defined by a unique type identifier and length".
    /// </summary>
    public class Box
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Box"/> class.
        /// </summary>
        /// <param name="type"></param>
        public Box(uint type)
        {
            this.Type = type;
        }

        public Box(uint type, Guid userType)
        {
            this.Type = type;
            this.UserType = userType;
        }
        public uint Size
        {
            get;
            set;
        }


        public uint Type
        {
            get;
            set;
        }

        public ulong LargeSize
        {
            get;
            set;
        }
        public Guid UserType { get; set; }
    }
}