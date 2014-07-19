using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System.Text;

namespace MMORPGCopierClient
{
    public class AudioSystem
    {
        // Audio objects
        private AudioEngine audioEngine;
        private SoundBank soundBank;
        private WaveBank waveBank;
        private AudioListener audioListener;

        public void Initialize()
        {
            // Initialize audio objects.
            audioEngine = new AudioEngine("Content\\Sounds.xgs");
            soundBank = new SoundBank(audioEngine, "Content\\Sounds.xsb");
            waveBank = new WaveBank(audioEngine, "Content\\Waves.xwb");
            audioListener = new AudioListener();
        }

        public void update()
        {
            audioEngine.Update();
        }

        public void setListenerPosition(Vector3 pos)
        {
            audioListener.Position = pos;
        }

        public Vector3 getListenerPosition()
        {
            return audioListener.Position;
        }

        public AudioEngine getAudioEngine()
        {
            return audioEngine;
        }
        public SoundBank getSoundBank()
        {
            return soundBank;
        }
        public WaveBank getWaveBank()
        {
            return waveBank;
        }
        public AudioListener getAudioListener()
        {
            return audioListener;
        }
    }
}
