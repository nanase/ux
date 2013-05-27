/* ux - Micro Xylph / Software Synthesizer Core Library
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
 */
using ux.Utils;

namespace ux.Component
{
	class Envelope
	{
		#region Private Members
		private readonly float samplingFreq;
		private int releaseStartTime, t2, t3, t5;
		private float da, dd, dr;
		#endregion

		#region Public Proparties
		public EnvelopeState State { get; private set; }

		public int AttackTime { get; private set; }

		public int PeakTime { get; private set; }

		public int DecayTime { get; private set; }

		public float SustainLevel { get; private set; }

		public int ReleaseTime { get; private set; }
		#endregion

		#region Constructor
		public Envelope(float samplingFreq)
		{
			this.samplingFreq = samplingFreq;
			this.Reset();
		}
		#endregion

		#region Public Methods
		public void Reset()
		{
			this.AttackTime = (int)(0.05f * this.samplingFreq);
			this.PeakTime = (int)(0.0f * this.samplingFreq);
			this.DecayTime = (int)(0.0f * this.samplingFreq);
			this.SustainLevel = 1.0f;
			this.ReleaseTime = (int)(0.05f * this.samplingFreq);
			this.State = EnvelopeState.Silence;
		}

		public void Attack()
		{
			this.State = EnvelopeState.Attack;

			//precalc
			this.t2 = this.AttackTime + this.PeakTime;
			this.t3 = t2 + this.DecayTime;
			this.da = 1.0f / this.AttackTime;
			this.dd = (1.0f - this.SustainLevel) / this.DecayTime;
		}

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

		public void Silence()
		{
			this.State = EnvelopeState.Silence;
		}

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

		public void SetParameter(PValue parameter)
		{
			switch (parameter.Name)
			{
				case "a":
				case "attack":
					this.AttackTime = (int)(parameter.Value.Normalize(float.MaxValue, 0.0f) * this.samplingFreq);
					break;

				case "p":
				case "peak":
					this.PeakTime = (int)(parameter.Value.Normalize(float.MaxValue, 0.0f) * this.samplingFreq);
					break;

				case "d":
				case "decay":
					this.DecayTime = (int)(parameter.Value.Normalize(float.MaxValue, 0.0f) * this.samplingFreq);
					break;

				case "s":
				case "sustain":
					this.SustainLevel = parameter.Value.Normalize(1.0f, 0.0f);
					break;

				case "r":
				case "release":
					this.ReleaseTime = (int)(parameter.Value.Normalize(float.MaxValue, 0.0f) * this.samplingFreq);
					break;

				default:
					break;
			}
		}

		#endregion
	}
}
