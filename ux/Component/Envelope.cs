/* ux - Micro Xylph / Software Synthesizer Core Library
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
 */
using ux.Utils;

namespace ux.Component
{
    /// <summary>
    /// 時間によって変化するパラメータを実装するためのエンベロープ (包絡線) クラスです。
    /// </summary>
	class Envelope
	{
		#region Private Members
		private readonly float samplingFreq;
		private int releaseStartTime, t2, t3, t5;
		private float da, dd, dr;
		#endregion

		#region Public Proparties
        /// <summary>
        /// 現在のエンベロープの状態を表す列挙値を取得します。
        /// </summary>
		public EnvelopeState State { get; private set; }

        /// <summary>
        /// アタック時間を取得します。
        /// </summary>
		public int AttackTime { get; private set; }

        /// <summary>
        /// ピーク時間を取得します。
        /// </summary>
		public int PeakTime { get; private set; }

        /// <summary>
        /// ディケイ時間を取得します。
        /// </summary>
		public int DecayTime { get; private set; }

        /// <summary>
        /// サスティンレベルを取得します。
        /// </summary>
		public float SustainLevel { get; private set; }

        /// <summary>
        /// リリース時間を取得します。
        /// </summary>
		public int ReleaseTime { get; private set; }
		#endregion

		#region Constructor
        /// <summary>
        /// サンプリング周波数を指定して新しい Envelope クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="samplingFreq">サンプリング周波数。</param>
		public Envelope(float samplingFreq)
		{
			this.samplingFreq = samplingFreq;
			this.Reset();
		}
		#endregion

		#region Public Methods
        /// <summary>
        /// このインスタンスにおけるすべてのパラメータを既定値に戻します。
        /// </summary>
		public void Reset()
		{
			this.AttackTime = (int)(0.05f * this.samplingFreq);
			this.PeakTime = (int)(0.0f * this.samplingFreq);
			this.DecayTime = (int)(0.0f * this.samplingFreq);
			this.SustainLevel = 1.0f;
			this.ReleaseTime = (int)(0.05f * this.samplingFreq);
			this.State = EnvelopeState.Silence;
		}

        /// <summary>
        /// エンベロープの状態をアタック状態に変更します。
        /// </summary>
		public void Attack()
		{
			this.State = EnvelopeState.Attack;

			//precalc
			this.t2 = this.AttackTime + this.PeakTime;
			this.t3 = t2 + this.DecayTime;
			this.da = 1.0f / this.AttackTime;
			this.dd = (1.0f - this.SustainLevel) / this.DecayTime;
		}

        /// <summary>
        /// エンベロープの状態をリリース状態に変更します。
        /// </summary>
        /// <param name="time">リリースが開始された時間値。</param>
		public void Release(int time)
		{
			if (this.State == EnvelopeState.Attack)
			{
				this.State = EnvelopeState.Release;
				this.releaseStartTime = time;

				//precalc
				this.t5 = time + this.ReleaseTime;
				this.dr = this.SustainLevel / this.ReleaseTime;
			}
		}

        /// <summary>
        /// エンベロープの状態をサイレンス状態に変更します。
        /// </summary>
		public void Silence()
		{
			this.State = EnvelopeState.Silence;
		}

        /// <summary>
        /// 現在のエンベロープの状態に基づき、エンベロープ値を出力します。
        /// </summary>
        /// <param name="time">エンベロープの開始時間値。</param>
        /// <param name="envelopes">出力が格納される実数の配列。</param>
        /// <param name="offset">代入が開始される配列のインデックス。</param>
        /// <param name="count">代入される実数値の数。</param>
		public void Generate(int time, float[] envelopes, int offset, int count)
		{
			float res;
			for (int i = offset; i < count; i++, time++)
			{
				if (this.State == EnvelopeState.Attack)
					res = (time < this.AttackTime) ? time * this.da :
					  (time < this.t2) ? 1.0f :
					  (time < this.t3) ? 1.0f - (time - this.t2) * this.dd : this.SustainLevel;
				else if (this.State == EnvelopeState.Release)
					if (time < this.t5)
						res = this.SustainLevel - (time - this.releaseStartTime) * this.dr;
					else
					{
						res = 0.0f;
						this.State = EnvelopeState.Silence;
					}
				else
					res = 0.0f;

				envelopes[i] = res;
			}
		}

        /// <summary>
        /// パラメータを用いてこのエンベロープの設定値を変更します。
        /// </summary>
        /// <param name="parameter">パラメータを表す PValue。</param>
		public void SetParameter(PValue parameter)
		{
			switch (parameter.Name)
			{
				case "a":
				case "attack":
					this.AttackTime = (int)(parameter.Value.Clamp(float.MaxValue, 0.0f) * this.samplingFreq);
					break;

				case "p":
				case "peak":
					this.PeakTime = (int)(parameter.Value.Clamp(float.MaxValue, 0.0f) * this.samplingFreq);
					break;

				case "d":
				case "decay":
					this.DecayTime = (int)(parameter.Value.Clamp(float.MaxValue, 0.0f) * this.samplingFreq);
					break;

				case "s":
				case "sustain":
					this.SustainLevel = parameter.Value.Clamp(1.0f, 0.0f);
					break;

				case "r":
				case "release":
					this.ReleaseTime = (int)(parameter.Value.Clamp(float.MaxValue, 0.0f) * this.samplingFreq);
					break;

				default:
					break;
			}
		}
		#endregion
	}
}
