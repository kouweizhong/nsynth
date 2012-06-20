﻿/******************************************************************************
 * NSynth - A Managed Multimedia API - http://nsynth.gearedstudios.com/       *
 * Copyright © 2009-2012 William 'cathode' Shelley. All Rights Reserved.      *
 * This software is released under the terms and conditions of the MIT/X11    *
 * license; see the included 'license.txt' file for the full text.            *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace NSynth
{
    /// <summary>
    /// Represents an input slot of a filter.
    /// </summary>
    public sealed class FilterInputSlot
    {
        #region Fields
        /// <summary>
        /// Holds the default input slot name.
        /// </summary>
        public const string DefaultName = "default";
        /// <summary>
        /// Backing field for the <see cref="FilterInputSlot.Name"/> property.
        /// </summary>
        private string name;

        /// <summary>
        /// Backing field for the <see cref="FilterInputSlot.Source"/> property.
        /// </summary>
        private Filter source;
        #endregion
        internal FilterInputSlot(string name)
        {
            Contract.Requires(name != null);

            this.name = name;
        }
        #region Properties
        /// <summary>
        /// Gets the name of the input slot.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Filter"/> that is assigned to the
        /// current input slot.
        /// </summary>
        public Filter Source
        {
            get
            {
                return this.source;
            }
            set
            {
                this.source = value;
            }
        }
        #endregion
        #region Methods
        public bool Bind(Filter source)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Determines if a specified <see cref="Filter"/> is already bound as part of the subtree of the current input slot.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public bool IsInSubtree(Filter filter)
        {
            if (this.source == null || filter == null)
                return false; // Nothing to look in / look for.

            if (this.source == filter)
                return true; // Found!
            else
                foreach (var input in this.source.Inputs)
                    if (input.IsInSubtree(filter))
                        return true; // Found in a subtree
            // Not found
            return false;
        }
        #endregion
    }
}
