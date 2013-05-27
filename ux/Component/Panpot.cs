/* ux - Micro Xylph / Software Synthesizer Core Library
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
 */
using System;

namespace ux.Component
{
	struct Panpot
	{
		public float L { get; set; }

		public float R { get; set; }

        public Panpot(float lChannel, float rChannel)
            : this()
        {
            this.L = lChannel;
            this.R = rChannel;
        }

        public Panpot(float value)
            : this()
        {
            this.L = value >= 0.0f ? (float)Math.Sin((value + 1f) * Math.PI / 2.0) : 1.0f;
            this.R = value <= 0.0f ? (float)Math.Sin((-value + 1f) * Math.PI / 2.0) : 1.0f;
        }
	}
}
