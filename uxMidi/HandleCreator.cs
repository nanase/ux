/* uxMidi / Software Synthesizer Library
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
 */

using System;
using ux.Component;

namespace uxMidi
{
    /// <summary>
    /// 文字列から新しいハンドルを生成するメソッドを提供します。
    /// </summary>
    static class HandleCreator
    {
        #region -- Public Methods --
        /// <summary>
        /// ハンドル名、タイプ、値から新しいハンドルを生成します。
        /// </summary>
        /// <param name="name">ハンドル名。</param>
        /// <param name="type">タイプ。</param>
        /// <param name="value">値。</param>
        /// <returns></returns>
        public static Handle Create(string name, string type, string value)
        {
            HandleType handleType;
            int data1;
            float data2;

            if (!Enum.TryParse(name, true, out handleType))
                throw new ArgumentException();

            data1 = (string.IsNullOrWhiteSpace(type)) ? 0 : HandleCreator.ParseOperators(handleType, type);

            if (string.IsNullOrWhiteSpace(value))
                data2 = 0.0f;
            else if (!float.TryParse(value, out data2))
                throw new ArgumentException();

            return new Handle(0, handleType, data1, data2);
        }
        #endregion

        #region -- Private Methods --
        private static int ParseOperators(HandleType type, string operators)
        {
            int result = 0;
            int tmp;

            foreach (var op in operators.Split(','))
                result |= (int.TryParse(op, out tmp)) ? tmp : HandleCreator.ParseOperator(type, op);

            return result;
        }

        private static int ParseOperator(HandleType type, string @operator)
        {
            switch (type)
            {
                case HandleType.Volume:
                    return (int)Enum.Parse(typeof(VolumeOperate), @operator, true);

                case HandleType.Vibrate:
                    return (int)Enum.Parse(typeof(VibrateOperate), @operator, true);

                case HandleType.Waveform:
                    return (int)Enum.Parse(typeof(WaveformType), @operator, true);

                case HandleType.EditWaveform:
                    FMOperate fmo;
                    BasicWaveformOperate bwo;
                    StepWaveformOperate swo;
                    RandomNoiseOperate rno;

                    if (Enum.TryParse(@operator, true, out fmo))
                        return (int)fmo;
                    else if (Enum.TryParse(@operator, true, out bwo))
                        return (int)bwo;
                    else if (Enum.TryParse(@operator, true, out swo))
                        return (int)swo;
                    else if (Enum.TryParse(@operator, true, out rno))
                        return (int)rno;
                    else
                        goto case HandleType.Envelope;

                case HandleType.Envelope:
                    return (int)Enum.Parse(typeof(EnvelopeOperate), @operator, true);

                case HandleType.Portament:
                    return (int)Enum.Parse(typeof(PortamentOperate), @operator, true);

                default:
                    throw new ArgumentException();
            }
        }
        #endregion
    }
}
