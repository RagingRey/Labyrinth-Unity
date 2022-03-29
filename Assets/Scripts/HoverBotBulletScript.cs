using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class HoverBotBulletScript : MonoBehaviour, IAllBulletsBehaviour
    {
        RaycastHit hit;
        public GameObject impactAnimation;

        Vector3 cameraToRootObj;
        Vector3 velocity;

        readonly List<Collider> ignoredCollider = new List<Collider>();
        readonly List<Collider> _damagable = new List<Collider>();

        Manager _obj;
        HoverBotScript _hoverBot;

        public LayerMask Layer;

        void Start()
        {
            _obj = FindObjectOfType<Manager>();
            _hoverBot = FindObjectOfType<HoverBotScript>();
        }

        void Update()
        {
            OnShoot();
            BulletCollision();
        }

        public void OnShoot(float speed = 150f)
        {
            velocity = transform.forward * speed;
            transform.position += velocity * Time.deltaTime;
            DestroyBullet(1f);
        }

        public void DestroyBullet(float lifeTime)
        {
            Destroy(gameObject, lifeTime);
        }

        public void BulletCollision()
        {
            cameraToRootObj = transform.position - _hoverBot.WeaponRoot.position;
            Collider[] ownerCollider = _hoverBot.thisGameObject.GetComponentsInChildren<Collider>();
            ignoredCollider.AddRange(ownerCollider);
            _damagable.Add(_obj.owner.GetComponentInChildren<Collider>());


            if (Physics.Raycast(_hoverBot.WeaponRoot.position, cameraToRootObj.normalized, out hit, cameraToRootObj.magnitude, Layer, QueryTriggerInteraction.Collide))
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
                    Health.PlayerHealth -= 0.05;
                }
                else
                {
                    GameObject impactAnimation = Instantiate(this.impactAnimation, hit.point + (hit.normal * 0.1f), Quaternion.LookRotation(hit.normal));
                    Destroy(gameObject);
                    Destroy(impactAnimation, 5f);
                }
            }
        }
    }
}
