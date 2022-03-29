using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class OverHeatingScript : MonoBehaviour
    {
        List<Renderer> _renderer;
        public Material weaponMaterial;
        [GradientUsage(true)] public Gradient overUsed;
        Color reload;

        MaterialPropertyBlock _material;
        ParticleSystem.EmissionModule overHeatRate;
        public ParticleSystem overHeatParticle;

        BlasterScript _ammo;

        void Awake()
        {
            _renderer = new List<Renderer>();
            _material = new MaterialPropertyBlock();
            _ammo = GetComponent<BlasterScript>();
            overHeatRate = overHeatParticle.emission;

            foreach (var rendererInWeapon in GetComponentsInChildren<Renderer>())
            {
                if (rendererInWeapon.sharedMaterial == weaponMaterial)
                {
                    _renderer.Add(rendererInWeapon);
                }
            }
        }

        void Update()
        {
            if (_ammo.hasReloaded)
            {
                _material.SetColor("_EmissionColor", overUsed.Evaluate(1f - _ammo.currentAmmoRatio));

                foreach (var item in _renderer)
                {
                    item.SetPropertyBlock(_material);
                }
            }

            if(_ammo.thereIsStillAmmo)
            {
                //overHeatRate.rateOverTimeMultiplier = 16f * (1f - _ammo.currentAmmoRatio);
            }

        }
    }
}
