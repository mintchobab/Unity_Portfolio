using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Audio;


namespace lsy
{
    public class SoundManager : IManager
    {
        private Dictionary<SoundType, AudioSource> audioSourceDic = new Dictionary<SoundType, AudioSource>();

        private AudioMixer audioMixer;

        private readonly string mixerBGM = "BGMVolume";
        private readonly string mixerSFX = "SFXVolume";


        public void Init()
        {
            GameObject manager = GameObject.Find("SoundManager");

            audioMixer = Managers.Instance.ResourceManager.Load<AudioMixer>(ResourcePath.AudioMixer);

            if (manager == null)
            {
                manager = new GameObject("SoundManager");

                foreach (SoundType value in System.Enum.GetValues(typeof(SoundType)))
                {
                    GameObject obj = new GameObject(value.ToString());
                    obj.AddComponent<AudioSource>();
                    obj.transform.parent = manager.transform;

                    AudioSource audioSource = obj.GetComponent<AudioSource>();
                    audioSourceDic.Add(value, audioSource);

                    if (value.Equals(SoundType.BGM))
                    {
                        audioSource.loop = true;
                        audioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("BGM")[0];
                    }
                    else
                    {
                        audioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("SFX")[0];
                    }
                }
            }
        }


        public void Play(string name, SoundType type, float volume = 1f, float pitch = 1f)
        {
            AudioClip clip;

            if (type == SoundType.BGM)
            {
                clip = Managers.Instance.ResourceManager.Load<AudioClip>($"{ResourcePath.BGM}/{name}");
                audioSourceDic[type].clip = clip;
                audioSourceDic[type].pitch = pitch;
                audioSourceDic[type].volume = volume;
                audioSourceDic[type].Play();
            }
            else
            {
                clip = Managers.Instance.ResourceManager.Load<AudioClip>($"{ResourcePath.SFX}/{name}");
                audioSourceDic[type].pitch = pitch;
                audioSourceDic[type].volume = volume;

                if (type == SoundType.SFX)
                {
                    audioSourceDic[type].PlayOneShot(clip);
                }
                else
                {
                    audioSourceDic[type].clip = clip;
                    audioSourceDic[type].Play();
                }
            }              
        }


        public void Stop(SoundType type)
        {
            audioSourceDic[type].Stop();
        }



        public void ChangeVolumeBGM(float value)
        {
            value = Mathf.Log10(value) * 20;
            audioMixer.SetFloat(mixerBGM, value);
        }


        public void ChangeVolumeSFX(float value)
        {
            value = Mathf.Log10(value) * 20;
            audioMixer.SetFloat(mixerSFX, value);
        }
    }
}
