using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

namespace Assets.Scripts
{
    public class PauseMenu : MonoBehaviour
    {
        public GameObject MenuPanel;
        public GameObject PlayerState;
        public GameObject SettingPage;
        public static bool m_IsPaused;

        void Start()
        {
            MenuPanel.SetActive(false);
            SettingPage.SetActive(false);
        }

        void Update()
        {
            if (Input.GetButtonDown("Cancel"))
            {
                Pause();
            }
        }

        public void Pause()
        {
            Time.timeScale = 0; 
            FindObjectOfType<FirstPersonController>().enabled = false; 
            PlayerState.SetActive(false); 
            MenuPanel.SetActive(true); 
            Cursor.lockState = CursorLockMode.None; 
            Cursor.visible = true;
            m_IsPaused = true;
        }

        public void Resume()
        {
            Time.timeScale = 1;
            FindObjectOfType<FirstPersonController>().enabled = true;
            PlayerState.SetActive(true);
            MenuPanel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            m_IsPaused = false;
        }

        public void Settings()
        {
            SettingPage.SetActive(true);
        }

        public void MainMenu()
        {

        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}