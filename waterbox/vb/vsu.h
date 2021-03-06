/******************************************************************************/
/* Mednafen Virtual Boy Emulation Module                                      */
/******************************************************************************/
/* vsu.h:
**  Copyright (C) 2010-2016 Mednafen Team
**
** This program is free software; you can redistribute it and/or
** modify it under the terms of the GNU General Public License
** as published by the Free Software Foundation; either version 2
** of the License, or (at your option) any later version.
**
** This program is distributed in the hope that it will be useful,
** but WITHOUT ANY WARRANTY; without even the implied warranty of
** MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
** GNU General Public License for more details.
**
** You should have received a copy of the GNU General Public License
** along with this program; if not, write to the Free Software Foundation, Inc.,
** 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
*/

#pragma once

class VSU
{
  public:
	VSU()
	MDFN_COLD;
	~VSU() MDFN_COLD;

	void SetSoundRate(double rate) MDFN_COLD;

	void Power(void) MDFN_COLD;

	void Write(int32 timestamp, uint32 A, uint8 V);

	int32 EndFrame(int32 timestamp, int16 *SoundBuf, int32 SoundBufMaxSize);

	uint8 PeekWave(const unsigned int which, uint32 Address);
	void PokeWave(const unsigned int which, uint32 Address, uint8 value);

	uint8 PeekModWave(uint32 Address);
	void PokeModWave(uint32 Address, uint8 value);

  private:
	void CalcCurrentOutput(int ch, int &left, int &right);

	void Update(int32 timestamp);

	uint8 IntlControl[6];
	uint8 LeftLevel[6];
	uint8 RightLevel[6];
	uint16 Frequency[6];
	uint16 EnvControl[6]; // Channel 5/6 extra functionality tacked on too.

	uint8 RAMAddress[6];

	uint8 SweepControl;

	uint8 WaveData[5][0x20];

	uint8 ModData[0x20];

	int32 EffFreq[6];
	int32 Envelope[6];

	int32 WavePos[6];
	int32 ModWavePos;

	int32 LatcherClockDivider[6];

	int32 FreqCounter[6];
	int32 IntervalCounter[6];
	int32 EnvelopeCounter[6];
	int32 SweepModCounter;

	int32 EffectsClockDivider[6];
	int32 IntervalClockDivider[6];
	int32 EnvelopeClockDivider[6];
	int32 SweepModClockDivider;

	int32 NoiseLatcherClockDivider;
	uint32 NoiseLatcher;

	uint32 lfsr;

	int32 last_output[6][2];
	int32 last_ts;

	Blip_Buffer sbuf[2];
	Blip_Synth<blip_good_quality, 1024> Synth;
	Blip_Synth<blip_med_quality, 1024> NoiseSynth;
};
