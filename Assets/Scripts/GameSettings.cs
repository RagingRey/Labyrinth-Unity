using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

namespace Assets.Scripts
{
    public class GameSettings : MonoBehaviour
    {
        public TMP_Dropdown ResolutionSetting;
        private Resolution[] m_DisplayResolutions;
        readonly List<string> m_ReadableResolutionList = new List<string>();
        private int resolutionIndex;
        public AudioMixer audio;

        void Start()
        {
            m_DisplayResolutions = Screen.resolutions;

            for (var i = 0; i < m_DisplayResolutions.Length; i++)
            {
                var height = m_DisplayResolutions[i].height;
                var width = m_DisplayResolutions[i].width;
                var refreshRate = m_DisplayResolutions[i].refreshRate;

                m_ReadableResolutionList.Add(width + " x " + height +"@" +refreshRate +"Hz");

                if (height == Screen.currentResolution.height && width == Screen.currentResolution.width)
                {
                    resolutionIndex = i;
                }
            }

            ResolutionSetting.ClearOptions();
            ResolutionSetting.AddOptions(m_ReadableResolutionList);
            ResolutionSetting.value = resolutionIndex;
            ResolutionSetting.RefreshShownValue();
        }

        public void ChangeResolution(int index)
        {
            Screen.SetResolution(m_DisplayResolutions[index].width, m_DisplayResolutions[index].height, FullScreenMode.Windowed, m_DisplayResolutions[index].refreshRate);
        }

        public void ChangeGraphics()
        {

        }

        public void ChangeVolume(float volume)
        {
            audio.SetFloat("GameVolume", volume);
        }
    }
}