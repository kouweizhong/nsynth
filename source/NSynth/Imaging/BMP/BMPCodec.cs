﻿/* NSynth - A Managed Multimedia API. http://nsynth.codeplex.com/
 * Copyright © 2009-2010 Will 'cathode' Shelley. All Rights Reserved. */
using System;
using System.IO;

namespace NSynth.Imaging.BMP
{
    public class BMPCodec : ImageCodec
    {
        public override Size MaximumSize
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool CanDecode
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool CanEncode
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override Version Version
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool SupportsFrameAccurateSeeking
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        public override bool SupportsNonLinearAccess
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        public override bool SupportsLayers
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override MediaEncoder CreateEncoder(Stream output)
        {
            throw new NotImplementedException();
        }

        public override MediaDecoder CreateDecoder(Stream input)
        {
            throw new NotImplementedException();
        }

        public override Image CreateImage(int width, int height)
        {
            throw new NotImplementedException();
        }

        public override int MaxThreads
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
