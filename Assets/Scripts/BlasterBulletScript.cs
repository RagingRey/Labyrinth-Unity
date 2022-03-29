using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class BlasterBulletScript : MonoBehaviour, IAllBulletsBehaviour
    {
        private Ray ray;
        RaycastHit hit;
        public GameObject impactAnimation;

        Vector3 cameraToRootObj;
        Vector3 velocity;

        private Camera _mainCamera;

        readonly List<Collider> ignoredCollider = new List<Collider>();
        readonly List<Collider> _damagable = new List<Collider>();

        Manager _obj;
        HoverBotScript _hoverBot;
        BlasterScript m_BlasterScript;

        public LayerMask Layer;

        void Start()
        {
            m_BlasterScript = FindObjectOfType<BlasterScript>();
            _mainCamera = Camera.main;
            _obj = FindObjectOfType<Manager>();
            _hoverBot = FindObjectOfType<HoverBotScript>();
            ray = new Ray(_mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0)), _mainCamera.transform.forward);
        }

        void FixedUpdate()
        {
            OnShoot();
            BulletCollision();
        }

        public void OnShoot(float speed = 300f)
        {
            velocity = ray.direction * speed;
            transform.position += velocity * Time.deltaTime;
            DestroyBullet(1f);
        }
        
        public void BulletCollision()
        {
            cameraToRootObj = transform.position - m_BlasterScript.rootPosition.position;
            Collider[] ownerCollider = _obj.owner.GetComponentsInChildren<Collider>();
            ignoredCollider.AddRange(ownerCollider);
            if (HoverBotHealth.EnemyHealth != 0)
            {
                _damagable.Add(_hoverBot.thisGameObject.GetComponentInChildren<Collider>());
            }

            if (Physics.Raycast(m_BlasterScript.rootPosition.position, cameraToRootObj.normalized, out hit, cameraToRootObj.magnitude))
            {
                if (ignoredCollider.Contains(hit.collider))
                {
                    //nothing should happen
                }
                else if (_damagable.Contains(hit.collider))
                {
                    GameObject impactAnimation = Instantiate(this.impactAnimation, hit.point + (hit.normal * 0.1f), Quaternion.LookRotation(hit.normal));
                    Destroy(gameObject); 
                    Destroy(impactAnimation, 5f);
                    _hoverBot.State = RobotState.Follow;
                    HoverBotHealth.EnemyHealth -= 0.125;
                }
                else 
                {
                    GameObject impactAnimation = Instantiate(this.impactAnimation, hit.point + (hit.normal * 0.1f), Quaternion.LookRotation(hit.normal));
                    Destroy(gameObject);
                    Destroy(impactAnimation, 5f);
                }    
            }
        }

        public void DestroyBullet(float lifeTime)
        {
            Destroy(gameObject, lifeTime);
        }
    }
}