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
using System.IO;

namespace NSynth.Imaging.TGA
{
    /// <summary>
    /// Provides a decoder for image data stored in the Truevision TARGA format.
    /// </summary>
    public class TGADecoder : ImageDecoder
    {
        #region Fields
        /// <summary>
        /// Holds the <see cref="TGADecodeContext"/> instance used for the decoding process.
        /// </summary>
        private TGADecodeContext context;
        #endregion
        #region Constructors
        public TGADecoder()
        {
        }
        public TGADecoder(Stream bitstream)
        {
            this.Bitstream = bitstream;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Overridden. Gets the <see cref="Codec"/> of the current <see cref="MediaDecoder"/>.
        /// </summary>
        public override Codec Codec
        {
            get
            {
                return Codecs.TGA;
            }
        }
        #endregion
        #region Methods
        /// <summary>
        /// Overridden. Decodes the next frame from the bitstream.
        /// </summary>
        /// <returns></returns>
        public override void Decode(Frame output)
        {
            this.DecodeBitmap();

            output.Video[0] = this.context.DecodedBitmap;
        }

        /// <summary>
        /// Overridden. Initializes the current <see cref="TGADecoder"/>.
        /// </summary>
        /// <returns></returns>
        public override bool Initialize()
        {
            if (!base.Initialize())
                throw new NotImplementedException();

            this.InitContext();
            this.DecodeHeader();
            return true;
        }

        /// <summary>
        /// Initializes the decoding context.
        /// </summary>
        internal void InitContext()
        {
            this.context = new TGADecodeContext(this.Bitstream);
            this.context.Stage = TGADecodeStage.Initialized;
        }

        /// <summary>
        /// Decodes the header information from the TGA bitstream.
        /// </summary>
        internal TGADecodeContext DecodeHeader()
        {
            if (this.context.Stage < TGADecodeStage.Initialized)
                this.InitContext();

            var bytes = new byte[18];
            this.context.Bitstream.Seek(0, SeekOrigin.Begin);
            this.context.Bitstream.Read(bytes, 0, bytes.Length);
            DataBuffer buffer = new DataBuffer(bytes, ByteOrder.LittleEndian);

            TGABitstreamHeader header;
            header.IdLength = buffer.ReadByte();
            header.ColorMapType = (TGAColorMapType)buffer.ReadByte();
            header.ImageType = (TGAImageType)buffer.ReadByte();
            header.ColorMapOrigin = buffer.ReadUInt16();
            header.ColorMapLength = buffer.ReadUInt16();
            header.ColorMapDepth = buffer.ReadByte();
            header.XOrigin = buffer.ReadUInt16();
            header.YOrigin = buffer.ReadUInt16();
            header.Width = buffer.ReadUInt16();
            header.Height = buffer.ReadUInt16();
            header.BitsPerPixel = buffer.ReadByte();
            byte descriptor = buffer.ReadByte();
            byte attributeBits = (byte)(descriptor & 0x0F);
            byte imgdesc = (byte)(descriptor & 0x30);
            header.ImageDescriptor = (TGAImageDescriptor)imgdesc;
            header.AttributeBits = attributeBits;
            if (header.Validate())
                this.context.Header = header;
            else
                throw new NotImplementedException();

            this.context.PixelDataLength = this.context.Bitstream.Length - 18;
            this.context.PixelDataOffset = 18 + header.IdLength + header.ColorMapLength;
            this.context.PixelOrder = (TGAPixelOrder)header.ImageDescriptor;
            this.context.Stage = TGADecodeStage.HeaderDecoded;

            return this.context;
        }

        private void DecodeFooter(TGADecodeContext dc)
        {
            if (dc.Stage < TGADecodeStage.HeaderDecoded)
                this.DecodeHeader();

            dc.Stage = TGADecodeStage.FooterDecoded;
        }

        private void DecodeBitmap()
        {
            var dc = this.context;
            if (dc.Stage < TGADecodeStage.FooterDecoded)
                this.DecodeFooter(dc);

            var header = dc.Header;

            // Syntax sugar
            int w = dc.Header.Width;
            int h = dc.Header.Height;

            if (w < 1 || h < 1)
                throw new NotImplementedException();

            if (header.BitsPerPixel % 8 != 0)
                throw new NotImplementedException("BitsPerPixel not multiple of 8");
            // Number of bytes per scanline
            int stride = header.Width * (header.BitsPerPixel / 8);

            // Read all the bytes that contain pixel data
            var data = new byte[dc.PixelDataLength];
            this.Bitstream.Seek(dc.PixelDataOffset, SeekOrigin.Begin);
            this.Bitstream.Read(data, 0, data.Length);

            if (header.ColorMapType == TGAColorMapType.ColorMapped) // Read color map data
                throw new NotImplementedException();
            else if (header.ImageType == TGAImageType.UncompressedTrueColor)
            {
                if (header.BitsPerPixel == 16)
                    throw new NotImplementedException(); // TODO: Implement support for 16-bit TGA decoding.
                else if (header.BitsPerPixel == 24)
                {
                    // Create an array of all the pixels in the bitmap (width x height).
                    var pixels = new ColorRGB24[w * h];
                    // ...and assign it to the Bitmap that will eventually be returned.
                    dc.DecodedBitmap = new BitmapRGB24(w, h, pixels);

                    var n = -1;

                    if (dc.PixelOrder == TGAPixelOrder.BottomLeft)
                    {
                        var line = new ColorRGB24[w];
                        for (int y = h; y > 0; --y)
                        {
                            for (int x = 0; x < line.Length; ++x)
                            {
                                line[x].Blue = data[++n];
                                line[x].Green = data[++n];
                                line[x].Red = data[++n];
                            }
                            line.CopyTo(pixels, (y - 1) * line.Length);
                        }

                    }
                    else if (dc.PixelOrder == TGAPixelOrder.BottomRight)
                    {
                        var line = new ColorRGB24[w];

                        // BTT + Right-to-Left (RTL) pixel order.
                        for (int y = h; y > 0; --y)
                        {
                            // Iterate over the pixels in the scanline from right-to-left.
                            for (int x = line.Length - 1; x >= 0; --x)
                            {
                                line[x].Blue = data[++n];
                                line[x].Green = data[++n];
                                line[x].Red = data[++n];
                            }
                            line.CopyTo(pixels, (y - 1) * line.Length);
                        }
                    }

                    else if (dc.PixelOrder == TGAPixelOrder.TopLeft)
                    {
                        // Top-to-bottom (TTB) + LTR pixel order.
                        for (int i = 0; i < pixels.Length; i++)
                        {
                            // All pixels can be decoded in one pass; TTB + LTR is the way bitmaps are laid out in memory.
                            pixels[i].Blue = data[++n];
                            pixels[i].Green = data[++n];
                            pixels[i].Red = data[++n];
                        }
                    }
                    else if (dc.PixelOrder == TGAPixelOrder.TopRight)
                    {
                        var line = new ColorRGB24[w];

                        for (int y = 0; y < h; ++y)
                        {
                            for (int x = line.Length - 1; x >= 0; --x)
                            {
                                line[x].Blue = data[++n];
                                line[x].Green = data[++n];
                                line[x].Red = data[++n];
                            }
                            line.CopyTo(pixels, y * line.Length);
                        }
                    }
                    else
                        throw new NotImplementedException();
                }
                else if (header.BitsPerPixel == 32) // Same as above, plus alpha channel.
                {
                    // Create an array of all the pixels in the bitmap (width x height).
                    var pixels = new ColorRGB32[w * h];
                    // ...and assign it to the Bitmap that will eventually be returned.
                    dc.DecodedBitmap = new BitmapRGB32(w, h, pixels);

                    int n = -1;
                    var line = new ColorRGB32[w];
                    // Most common scanline direction first; TGA stores scanlines bottom-to-top by default.
                    if (dc.PixelOrder == TGAPixelOrder.BottomLeft)
                    {
                        // Bottom-to-top (BTT) + Left-to-right (LTR) pixel order.
                        for (int y = h; y > 0; --y)
                        {
                            for (int x = 0; x < line.Length; ++x)
                            {
                                line[x].Blue = data[++n];
                                line[x].Green = data[++n];
                                line[x].Red = data[++n];
                                line[x].Alpha = data[++n];
                            }
                            line.CopyTo(pixels, (y - 1) * line.Length);
                        }
                    }
                    else if (dc.PixelOrder == TGAPixelOrder.BottomRight)
                    {
                        for (int y = h; y > 0; --y)
                        {
                            // Iterate over the pixels in the scanline from right-to-left.
                            for (int x = line.Length - 1; x >= 0; --x)
                            {
                                line[x].Blue = data[++n];
                                line[x].Green = data[++n];
                                line[x].Red = data[++n];
                                line[x].Alpha = data[++n];
                            }
                            line.CopyTo(pixels, (y - 1) * line.Length);
                        }
                    }
                    else if (dc.PixelOrder == TGAPixelOrder.TopLeft)
                    {
                        for (int i = 0; i < pixels.Length; ++i)
                        {
                            // All pixels can be decoded in one pass; TTB + LTR is the way bitmaps are laid out in memory.
                            pixels[i].Blue = data[++n];
                            pixels[i].Green = data[++n];
                            pixels[i].Red = data[++n];
                            pixels[i].Alpha = data[++n];
                        }
                    }
                    else if (dc.PixelOrder == TGAPixelOrder.TopRight)
                    {
                        for (int y = 0; y < h; ++y)
                        {
                            for (int x = line.Length - 1; x >= 0; --x)
                            {
                                line[x].Blue = data[++n];
                                line[x].Green = data[++n];
                                line[x].Red = data[++n];
                                line[x].Alpha = data[++n];
                            }
                            line.CopyTo(pixels, y * line.Length);
                        }
                    }
                }
            }
            else if (header.ImageType == TGAImageType.RunLengthEncodedTrueColor)
            {
                int n = -1;
                bool raw = false;
                byte p;

                if (header.BitsPerPixel == 16)
                {
                    throw new NotImplementedException();
                }
                if (header.BitsPerPixel == 24)
                {
                    // Create an array of all the pixels in the bitmap (width x height).
                    var pixels = new ColorRGB24[w * h];
                    // ...and assign it to the Bitmap that will eventually be returned.
                    dc.DecodedBitmap = new BitmapRGB24(w, h, pixels);
                    ColorRGB24 px = new ColorRGB24();

                    // Operate on a line-by-line basis; RLE packets will never wrap from one scanline to another.
                    var line = new ColorRGB24[w];

                    // Most common scanline direction first; TGA stores scanlines bottom-to-top by default.
                    if (dc.PixelOrder == TGAPixelOrder.BottomLeft)
                    {
                        for (int y = h; y > 0; --y)
                        {
                            for (int x = 0, e = 0; x < line.Length; ++x, --e)
                            {
                                if (e == 0)
                                {
                                    // Read the "packet header"
                                    p = data[++n];
                                    // Highest bit of packet header indicates RLE (1) or RAW (0).
                                    raw = (p & 0x80) != 0x80;
                                    // Remaining 7 bits indicate RLE packet length.
                                    e = 1 + (p & 0x7F);

                                    // Read color
                                    px.Blue = data[++n];
                                    px.Green = data[++n];
                                    px.Red = data[++n];
                                }
                                else if (raw)
                                {
                                    px.Blue = data[++n];
                                    px.Green = data[++n];
                                    px.Red = data[++n];
                                }

                                line[x] = px;
                            }
                            line.CopyTo(pixels, (y - 1) * w);
                        }
                    }
                    else if (dc.PixelOrder == TGAPixelOrder.BottomRight)
                    {
                        for (int y = h; y > 0; --y)
                        {
                            for (int x = w, e = 0; x >= 0; --x, --e)
                            {
                                if (e == 0)
                                {
                                    // Read the "packet header"
                                    p = data[++n];
                                    // Highest bit of packet header indicates RLE (1) or RAW (0).
                                    raw = (p & 0x80) != 0x80;
                                    // Remaining 7 bits indicate RLE packet length.
                                    e = 1 + (p & 0x7F);

                                    // Read color
                                    px.Blue = data[++n];
                                    px.Green = data[++n];
                                    px.Red = data[++n];
                                }
                                else if (raw)
                                {
                                    px.Blue = data[++n];
                                    px.Green = data[++n];
                                    px.Red = data[++n];
                                }

                                line[x] = px;
                            }
                            line.CopyTo(pixels, (y - 1) * w);
                        }
                    }
                    else if (dc.PixelOrder == TGAPixelOrder.TopLeft)
                    {
                        for (int i = 0, e = 0; i < pixels.Length; ++i)
                        {
                            if (e == 0)
                            {
                                // Read the "packet header"
                                p = data[++n];
                                // Highest bit of packet header indicates RLE (1) or RAW (0).
                                raw = (p & 0x80) != 0x80;
                                // Remaining 7 bits indicate RLE packet length.
                                e = 1 + (p & 0x7F);

                                // Read color
                                px.Blue = data[++n];
                                px.Green = data[++n];
                                px.Red = data[++n];
                            }
                            else if (raw)
                            {
                                px.Blue = data[++n];
                                px.Green = data[++n];
                                px.Red = data[++n];
                            }

                            pixels[i] = px;
                        }
                    }
                    else if (dc.PixelOrder == TGAPixelOrder.TopRight)
                    {
                        throw new NotImplementedException();
                        /*
                        for (int y = 0; y < h; y++)
                        {
                            for (int x = w - 1; x >= 0; x--)
                            {
                                if (e == 0)
                                {
                                    // Read the "packet header"
                                    p = data[++n];
                                    // Highest bit of packet header indicates RLE (1) or RAW (0).
                                    raw = (p & 0x80) != 0x80;
                                    // Remaining 7 bits indicate RLE packet length.
                                    e = 1 + (p & 0x7F);

                                    // Read color
                                    px.Blue = data[++n];
                                    px.Green = data[++n];
                                    px.Red = data[++n];
                                }
                                else if (raw)
                                {
                                    px.Blue = data[++n];
                                    px.Green = data[++n];
                                    px.Red = data[++n];
                                }

                                line[x] = px;
                            }
                        }*/
                    }
                }
                else if (header.BitsPerPixel == 32) // Just like above, but with an alpha channel.
                {
                    // Create an array of all the pixels in the bitmap (width x height).
                    var pixels = new ColorRGB32[w * h];
                    // ...and assign it to the Bitmap that will eventually be returned.
                    dc.DecodedBitmap = new BitmapRGB32(w, h, pixels);
                    ColorRGB32 px = new ColorRGB32();
                    // Operate on a line-by-line basis; RLE packets will never wrap from one scanline to another.
                    var line = new ColorRGB32[w];
                    var len = line.Length;
                    if (dc.PixelOrder == TGAPixelOrder.BottomLeft)
                    {
                        for (int y = h; y > 0; --y)
                        {
                            for (int x = 0, e = 0; x < len; ++x, --e)
                            {
                                if (e == 0)
                                {
                                    // Read the "packet header"
                                    p = data[++n];
                                    // Highest bit of packet header indicates RLE (1) or RAW (0).
                                    raw = (p & 0x80) != 0x80;
                                    // Remaining 7 bits indicate RLE packet length.
                                    e = 1 + (p & 0x7F);

                                    // Read color
                                    px.Blue = data[++n];
                                    px.Green = data[++n];
                                    px.Red = data[++n];
                                    px.Alpha = data[++n];
                                }
                                else if (raw)
                                {
                                    px.Blue = data[++n];
                                    px.Green = data[++n];
                                    px.Red = data[++n];
                                    px.Alpha = data[++n];
                                }

                                line[x] = px;
                            }
                            line.CopyTo(pixels, (y - 1) * w);
                        }
                    }
                    else if (dc.PixelOrder == TGAPixelOrder.BottomRight)
                    {
                        throw new NotImplementedException();
                    }
                    else if (dc.PixelOrder == TGAPixelOrder.TopLeft)
                    {
                        throw new NotImplementedException();
                    }
                    else if (dc.PixelOrder == TGAPixelOrder.TopRight)
                    {
                        throw new NotImplementedException();
                    }
                }
                else
                    throw new NotImplementedException();
            }

            dc.Stage = TGADecodeStage.BitmapDecoded;
        }
        #endregion
    }
}
