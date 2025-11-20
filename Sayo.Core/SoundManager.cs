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
        private static readonly SoundEffectInstance[] _eatSounds = new SoundEffectInstance[6];
        private static readonly RandomBag _randomBag = new(6);
        private static GameTime _gameTime;

        public static void Initialize(ContentManager content)
        {
            SEList[SEName.GameOver] = content.Load<SoundEffect>("Sound/gameover").CreateInstance();
            SEList[SEName.Click] = content.Load<SoundEffect>("Sound/click").CreateInstance();
            SEList[SEName.Opening] = content.Load<SoundEffect>("Sound/opening").CreateInstance();
            SEList[SEName.Sayo_itai] = content.Load<SoundEffect>("Sound/Sayo_itai").CreateInstance();
            SEList[SEName.Sayo_hurt] = content.Load<SoundEffect>("Sound/Sayo_hurt").CreateInstance();
            for(int i = 0; i < 6; i++)
            {
                _eatSounds[i] = content.Load<SoundEffect>($"Sound/crunch{i + 1}").CreateInstance();
            }
        }
        public static void Update(GameTime gameTime)
        {
            _gameTime = gameTime;
            foreach (ReadyPlaySound sound in ReadyPlaySounds)
            {
                if(gameTime.TotalGameTime >= sound.BeginTime + sound.DelayTime)
                {
                    sound.Sound.Play();
                    ReadyPlaySounds.Remove(sound);
                }
            }
        }
        public static void PlayInSeconds(SEName soundName,int seconds)
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
        GameOver,
        Click,
        Opening,
        Sayo_itai,
        Sayo_hurt
    }
}
