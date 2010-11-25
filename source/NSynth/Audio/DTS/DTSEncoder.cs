﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace NSynth.Audio.DTS
{
    /// <summary>
    /// Provides DTS Coherent Acoustics bitstream encoding of audio frames.
    /// </summary>
    public sealed class DTSEncoder : AudioEncoder
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DTSEncoder"/> class.
        /// </summary>
        /// <param name="bitstream"></param>
        public DTSEncoder(Stream bitstream)
            : base(bitstream)
        {
        }
        #endregion
        #region Properties
        public override bool CanSuspend
        {
            get
            {
                return false;
            }
        }
        public override Codec Codec
        {
            get
            {
                return new DTSCodec();
            }
        }
        #endregion
        #region Methods
        public override void EncodeFrame(Frame frame)
        {
            throw new NotImplementedException();
        }

        public override bool Open()
        {
            throw new NotImplementedException();
        }

        public override bool Close()
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
