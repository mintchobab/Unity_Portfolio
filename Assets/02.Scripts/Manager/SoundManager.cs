using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace lsy
{
    public class SoundManager : IManager
    {
        //private AudioSource[] audioSources = new AudioSource[System.Enum.GetValues(typeof(SoundType)).Length];
        private Dictionary<SoundType, AudioSource> audioSourceDic = new Dictionary<SoundType, AudioSource>();
        private List<AudioClip> audioClips = new List<AudioClip>();


        private readonly string soundPath = "Assets/Resources/Load/Sound";


        public void Init()
        {
            GameObject manager = GameObject.Find("SoundManager");

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

                audioSourceDic[type].PlayOneShot(clip);
            }              
        }


        public void Stop(SoundType type)
        {
            audioSourceDic[type].Stop();
        }
    }
}
