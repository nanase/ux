/* ux - Micro Xylph / Software Synthesizer Core Library

LICENSE - The MIT License (MIT)

Copyright (c) 2013-2014 Tomona Nanase

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

// refer to:
// https://github.com/mono/mono/blob/mono-3.2.8/mcs/class/corlib/System/Random.cs

//
// System.Random.cs
//
// Authors:
// Bob Smith (bob@thestuff.net)
// Ben Maurer (bmaurer@users.sourceforge.net)
// Sebastien Pouliot <sebastien@xamarin.com>
//
// (C) 2001 Bob Smith. http://www.thestuff.net
// (C) 2003 Ben Maurer
// Copyright (C) 2004 Novell, Inc (http://www.novell.com)
// Copyright 2013 Xamarin Inc. (http://www.xamarin.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;

namespace ux.Waveform
{
    class JKissRandom
    {
        #region -- Private Fields --
        private uint x, y, z, c;
        #endregion

        #region -- Constructors --
        public JKissRandom()
            : this(Environment.TickCount)
        {
        }

        public JKissRandom(int Seed)
        {
            x = (uint)Seed;
            y = 987654321U;
            z = 43219876U;
            c = 6543217U;
        }
        #endregion

        #region -- Public Methods --
        public int Next(int minValue, int maxValue)
        {
            if (minValue > maxValue)
                throw new ArgumentOutOfRangeException("Maximum value is less than minimal value.");

            // special case: a difference of one (or less) will always return the minimum
            // e.g. -1,-1 or -1,0 will always return -1
            uint diff = (uint)(maxValue - minValue);

            if (diff <= 1)
                return minValue;

            return minValue + (int)(this.JKiss() % diff);
        }

        public int Next(int maxValue)
        {
            if (maxValue < 0)
                throw new ArgumentOutOfRangeException("Maximum value is less than minimal value.");

            return maxValue > 0 ? (int)(this.JKiss() % maxValue) : 0;
        }

        public int Next()
        {
            // returns a non-negative, [0 - Int32.MacValue], random number
            // but we want to avoid calls to Math.Abs (call cost and branching cost it requires)
            // and the fact it would throw for Int32.MinValue (so roughly 1 time out of 2^32)
            int random = (int)this.JKiss();

            while (random == Int32.MinValue)
                random = (int)this.JKiss();

            int mask = random >> 31;
            random ^= mask;
            return random + (mask & 1);
        }

        public void NextBytes(byte[] buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer");

            // each random `int` can fill 4 bytes
            int p = 0;
            uint random;

            for (int i = 0; i < (buffer.Length >> 2); i++)
            {
                random = this.JKiss();
                buffer[p++] = (byte)(random >> 24);
                buffer[p++] = (byte)(random >> 16);
                buffer[p++] = (byte)(random >> 8);
                buffer[p++] = (byte)random;
            }

            if (p == buffer.Length)
                return;

            // complete the array
            random = this.JKiss();

            while (p < buffer.Length)
            {
                buffer[p++] = (byte)random;
                random >>= 8;
            }
        }

        public double NextDouble()
        {
            // a single 32 bits random value is not enough to create a random double value
            uint a = this.JKiss() >> 6;	// Upper 26 bits
            uint b = this.JKiss() >> 5;	// Upper 27 bits
            return (a * 134217728.0 + b) / 9007199254740992.0;
        }
        #endregion

        #region -- Private Methods --
        private uint JKiss()
        {
            this.x = 314527869U * this.x + 1234567U;
            this.y ^= this.y << 5;
            this.y ^= this.y >> 7;
            this.y ^= this.y << 22;

            ulong t = (4294584393UL * this.z + this.c);
            this.c = (uint)(t >> 32);
            this.z = (uint)t;

            return (this.x + this.y + this.z);
        }
        #endregion
    }
}
