using UnityEngine;

namespace Assets.Scripts
{
    public class WeaponSlotScript : MonoBehaviour
    {
        public GameObject[] BulletSlots;
        [HideInInspector] public Vector3 UsedSlot;
        [HideInInspector] public Vector3 UnUsedSlot;
        BlasterScript m_Ammo;

        void Start()
        {
            UsedSlot = new Vector3(0, 0.1f, 0);
            UnUsedSlot = new Vector3(0, 0, 0);
            m_Ammo = GetComponent<BlasterScript>();
        }

        void Update()
        {
            WeaponSlotBehaviour();
        }

        void WeaponSlotBehaviour()
        {
            for (int i = 0; i < BulletSlots.Length; i++)
            {
                float length = BulletSlots.Length;
                float lim1 = i / length;
                float lim2 = (i + 1) / length;

                if (m_Ammo.canShoot)
                {
                    float value = Mathf.InverseLerp(lim1, lim2, m_Ammo.currentAmmoRatio);
                    value = Mathf.Clamp01(value);
                    BulletSlots[i].transform.localPosition = Vector3.Lerp(UsedSlot, UnUsedSlot, value);
                }
            }
        }
    }
}
