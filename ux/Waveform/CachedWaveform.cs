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

using System;
using System.Collections.Generic;

namespace ux.Waveform
{
    abstract class CachedWaveform<T> : StepWaveform
        where T : CacheObject<T>
    {
        protected static readonly LinkedList<T> cache = new LinkedList<T>();

        protected virtual int MaxCacheSize { get { return 32; } }

        protected virtual bool CanResizeData { get { return false; } }

        protected virtual bool GeneratingFloat { get { return false; } }

        protected virtual float[] GenerateFloat(T parameter)
        {
            return new float[1];
        }

        protected virtual byte[] Generate(T parameter)
        {
            return new byte[1];
        }

        protected void Cache(T parameter)
        {
            for (var now = cache.First; now != null; now = now.Next)
            {
                if (now.Value.Equals(parameter))
                {
                    this.value = now.Value.DataValue;
                    this.length = (float)this.value.Length;
                    return;
                }
                else if (this.CanResizeData && now.Value.CanResize(parameter))
                {
                    this.value = new float[parameter.Length];
                    this.length = (float)this.value.Length;

                    Array.Copy(now.Value.DataValue, this.value, parameter.Length);

                    parameter.DataValue = this.value;
                    this.PushCache(parameter);
                    return;
                }
            }

            if (this.GeneratingFloat)
            {
                this.value = this.GenerateFloat(parameter);
                this.length = (float)this.value.Length;
            }
            else
                this.SetStep(this.Generate(parameter));

            parameter.DataValue = this.value;
            this.PushCache(parameter);
        }

        private void PushCache(T parameter)
        {
            cache.AddFirst(parameter);

            if (cache.Count > this.MaxCacheSize)
                cache.RemoveLast();
        }
    }

    interface CacheObject<T> : IEquatable<T>
    {
        float[] DataValue { get; set; }

        int Length { get; }

        bool CanResize(T other);
    }
}
