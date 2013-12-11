/* uxMidi / Software Synthesizer Library
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
 */

using System;
using ux.Component;

namespace ux.Utils
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
                throw new ArgumentException("ハンドル名の変換に失敗しました。", "name");

            data1 = (string.IsNullOrWhiteSpace(type)) ? 0 : HandleCreator.ParseOperators(handleType, type);

            if (string.IsNullOrWhiteSpace(value))
                data2 = 0.0f;
            else if (!float.TryParse(value, out data2))
                throw new ArgumentException("データ値の変換に失敗しました。", "value");

            return new Handle(0, handleType, data1, data2);
        }

        /// <summary>
        /// ハンドル名、タイプ、値から新しいハンドルを生成し、結果を返却します。
        /// </summary>
        /// <param name="part">ハンドルのパート。</param>
        /// <param name="name">ハンドル名。</param>
        /// <param name="type">ハンドルのタイプ。</param>
        /// <param name="value">ハンドル値。</param>
        /// <param name="handle">生成されたハンドル。</param>
        /// <returns>変換の結果を表す真偽値。成功したとき true、失敗したとき false。</returns>
        public static bool TryCreate(int part, string name, string type, string value, out Handle handle)
        {
            HandleType handleType;
            int data1;
            float data2;

            handle = null;

            if (!Enum.TryParse(name, true, out handleType))
                return false;

            data1 = (string.IsNullOrWhiteSpace(type)) ? 0 : HandleCreator.ParseOperators(handleType, type);

            if (string.IsNullOrWhiteSpace(value))
                data2 = 0.0f;
            else if (!float.TryParse(value, out data2))
                return false;

            handle = new Handle(part, handleType, data1, data2);
            return true;
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
