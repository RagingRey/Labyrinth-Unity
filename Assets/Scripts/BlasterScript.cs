using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class BlasterScript : MonoBehaviour
    {
        public Transform rootPosition;
        public Transform rootRotation;

        public AudioClip bulletSound;
        AudioSource _source;

        int _maxAmmo = 16;
        public int _currentAmmo;
        int _weaponMax = 80;
        public float currentAmmoRatio;
        int _ammoAdded;

        public bool canShoot = true;
        public bool hasReloaded = true;
        public bool thereIsStillAmmo = true;

        public Text ammoStatus;
        Animation m_BlasterAnimations;

        public GameObject bullet;

        WeaponSlotScript _behaviour;
        private Manager m_Manager;

        void Awake()
        {
            m_BlasterAnimations = GetComponent<Animation>();
            _source = GetComponent<AudioSource>();
            _currentAmmo = _maxAmmo;
            _behaviour = GetComponent<WeaponSlotScript>();
            _ammoAdded = _maxAmmo;
            m_Manager = FindObjectOfType<Manager>();
        }

        void Update()
        {
            Shoot();
            WeaponState();
            ManualReload();
            

            if (_maxAmmo == Mathf.Infinity)
            {
                currentAmmoRatio = 1f;
            }
            else
            {
                currentAmmoRatio = (float)_currentAmmo / _ammoAdded;
            }
        }

        void Shoot()
        {
            if (Input.GetMouseButtonDown(0) && _currentAmmo > 0 && canShoot && _weaponMax > -1 && !m_Manager.isAiming && !PauseMenu.m_IsPaused)
            {
                Instantiate(bullet, rootPosition.position, rootRotation.rotation);
                _source.PlayOneShot(bulletSound);
                _currentAmmo--;
                m_BlasterAnimations.Play("DefaultAimShootAnim");
            }
            if (Input.GetMouseButtonDown(0) && _currentAmmo > 0 && canShoot && _weaponMax > -1 && m_Manager.isAiming && !PauseMenu.m_IsPaused)
            {
                Instantiate(bullet, rootPosition.position, rootRotation.rotation);
                _source.PlayOneShot(bulletSound);
                _currentAmmo--;
                m_BlasterAnimations.Play("AimShootAnim");
            }
            else if (_currentAmmo < 1)
            {
                StartCoroutine(Reload());
            }
        }

        IEnumerator Reload()
        {
            canShoot = false;
            hasReloaded = false;

            if (_weaponMax > 0)
            {
                int _ammoToAdd = _maxAmmo - _currentAmmo;

                if (_weaponMax > _ammoToAdd)
                { 
                    _weaponMax -= _ammoToAdd;
                    _currentAmmo += _ammoToAdd;
                    _ammoAdded = _currentAmmo;
                }
                else
                {
                    _currentAmmo += _weaponMax;
                    _weaponMax -= _weaponMax;
                    _ammoAdded = _currentAmmo;
                }

                _currentAmmo = Mathf.Clamp(_currentAmmo, 0, _maxAmmo);
            }

            yield return new WaitForSeconds(3f);

            canShoot = true;
            hasReloaded = true;
        }

        void WeaponState()
        {
            if (canShoot)
            {
                ammoStatus.GetComponent<Animator>().enabled = false;
                ammoStatus.color = Color.black;
                ammoStatus.text = _weaponMax + " : " + _currentAmmo;
            }
            else if (_weaponMax == 0 && _currentAmmo == 0)
            {
                ammoStatus.text = "Out of Ammo!";
                thereIsStillAmmo = false;
            }
            else
            {
                ammoStatus.text = "RELOADING!";
                ammoStatus.GetComponent<Animator>().enabled = true;

                foreach (var t in _behaviour.BulletSlots)
                {
                    t.transform.localPosition = Vector3.Lerp(t.transform.localPosition, new Vector3(0, 0, 0), 3f * Time.deltaTime);
                }
            }
        }

        void ManualReload()
        {
            if (Input.GetKeyDown(KeyCode.R) && _currentAmmo != _maxAmmo && _weaponMax > 0)
            {
                StartCoroutine(Reload());
            }
        }
    }
}
