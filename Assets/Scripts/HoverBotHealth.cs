using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class HoverBotHealth : MonoBehaviour
    {
        public static double EnemyHealth; 
        public Image HealthBar;

        void Start()
        {
            EnemyHealth = Mathf.Clamp(1,0,1);
        }

        void Update()
        {
            HealthBar.fillAmount = (float)EnemyHealth;
        }
    }
}
