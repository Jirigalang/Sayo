using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;

namespace Sayo.Core
{
    public static class SoundManager
    {
        public static readonly Dictionary<SEName, SoundEffectInstance> SEList = [];
        public static readonly List<ReadyPlaySound> ReadyPlaySounds = [];
        private static readonly SoundEffectInstance[] _eatSounds = new SoundEffectInstance[7];
        private static readonly RandomBag _randomBag = new(7);
        private static GameTime _gameTime;

        public static void Initialize(ContentManager content)
        {
            SEList[SEName.Click] = content.Load<SoundEffect>(@"Sound\click").CreateInstance();
            SEList[SEName.Opening] = content.Load<SoundEffect>(@"Sound\opening").CreateInstance();
            SEList[SEName.Sayo_gua] = content.Load<SoundEffect>(@"Sound\Sayo_gua").CreateInstance();
            SEList[SEName.Sayo_hurt] = content.Load<SoundEffect>(@"Sound\Sayo_hurt").CreateInstance();
            for(int i = 0; i < 7; i++)
            {
                _eatSounds[i] = content.Load<SoundEffect>($@"Sound\crunch{i + 1}").CreateInstance();
            }
        }
        public static void Update(GameTime gameTime)
        {
            _gameTime = gameTime;
            for (int i = ReadyPlaySounds.Count - 1; i >= 0; i--)
            {
                var sound = ReadyPlaySounds[i];

                if (gameTime.TotalGameTime >= sound.BeginTime + sound.DelayTime)
                {
                    if (sound.Sound.State != SoundState.Playing)
                        sound.Sound.Play();

                    ReadyPlaySounds.RemoveAt(i);
                }
            }
        }
        public static void PlayInSeconds(SEName soundName,double seconds)
        {
            ReadyPlaySounds.Add(new ReadyPlaySound(SEList[soundName], _gameTime.TotalGameTime, TimeSpan.FromSeconds(seconds)));
        }
        public static void PlayEatSounds()
        {
            _eatSounds[_randomBag.Current].Stop();
            _eatSounds[_randomBag.Next].Play();
        }
        public static void MuteAll()
        {
            foreach(var sound in SEList.Values)
            {
                sound.Stop();
            }
            foreach(var sound in _eatSounds)
            {
                sound.Stop();
            }
        }

    }
    public class ReadyPlaySound(SoundEffectInstance sound, TimeSpan beginTime, TimeSpan delayTime)
    {
        public SoundEffectInstance Sound = sound;
        public TimeSpan BeginTime = beginTime;
        public TimeSpan DelayTime = delayTime;
    }
    public enum SEName
    {
        Click,
        Opening,
        Sayo_gua,
        Sayo_hurt
    }
}
