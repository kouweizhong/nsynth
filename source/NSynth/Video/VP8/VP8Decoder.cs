﻿/******************************************************************************
 * NSynth - A Managed Multimedia API - http://nsynth.gearedstudios.com/       *
 * Copyright © 2009-2013 William 'cathode' Shelley. All Rights Reserved.      *
 * This software is released under the terms and conditions of the MIT/X11    *
 * license; see the included 'license.txt' file for the full text.            *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace NSynth.Video.VP8
{
    public sealed class VP8Decoder : VideoDecoder
    {
        public override bool Initialize()
        {
            throw new NotImplementedException();
        }


        public override Codec Codec
        {
            get
            {
                return Codecs.VP8;
            }
        }
    }
}
