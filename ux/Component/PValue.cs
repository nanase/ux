/* ux - Micro Xylph / Software Synthesizer Core Library
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
 */
using System;

namespace ux.Component
{
    public struct PValue
    {
        #region Public Properties
        public string Name { get; private set; }

        public float Value { get; private set; }
        #endregion

        #region Constructors
        public PValue(string name, float value)
            : this()
        {
            if (name != null)
                this.Name = name.ToLower();
            this.Value = value;
        }
        #endregion

        #region Public Override Methods
        public override string ToString()
        {
            return String.Format("{0}={1}", this.Name, this.Value);
        }
        #endregion
    }
}
