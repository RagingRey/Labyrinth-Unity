using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

namespace Assets.Scripts
{
    public class Health : MonoBehaviour
    {
        public static double PlayerHealth; 
        public Image HealthBar;
        public GameObject Gun;
        public GameObject Manager;

        void Start()
        {
            PlayerHealth = Mathf.Clamp(1,0,1);
        }

        void Update()
        {
            HealthBar.fillAmount = (float)PlayerHealth;
            if (PlayerHealth <= 0)
            {
                Manager.SetActive(false);
                GetComponent<FirstPersonController>().enabled = false;
                Gun.SetActive(false);
            }
        }
    }
}
