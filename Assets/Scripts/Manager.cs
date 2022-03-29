using UnityEngine;

namespace Assets.Scripts
{
    public class Manager : MonoBehaviour
    {
        [HideInInspector] public GameObject owner;
        public GameObject BlasterCurrentPosition;

        public GameObject defaultPosition;
        public GameObject aimingPosition;

        GameObject _aimCamera;
        Camera _mainCamera;

        float _aimingAnimationSpeed = 5f;
        float _defaultCameraFOV = 60f;
        float _aimingCameraFOV = 30f;

        public bool isAiming = false;
        public int amountOfEnemyKilled = 0;

        void Start()
        {
            owner = GameObject.FindGameObjectWithTag("Player");
            _mainCamera = Camera.main;
            _aimCamera = GameObject.FindGameObjectWithTag("AimCamera");
        }

        void Update()
        {
            Aim();   
        }

        void Aim()
        {
            if (Input.GetMouseButton(1))
            {
                BlasterCurrentPosition.transform.position = Vector3.Lerp(BlasterCurrentPosition.transform.position, aimingPosition.transform.position, _aimingAnimationSpeed * Time.deltaTime);
                _mainCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(_mainCamera.fieldOfView, _aimingCameraFOV, _aimingAnimationSpeed * Time.deltaTime);
                _aimCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(_aimCamera.GetComponent<Camera>().fieldOfView, _aimingCameraFOV, _aimingAnimationSpeed * Time.deltaTime);
                isAiming = true;
            }
            else
            {
                BlasterCurrentPosition.transform.position = Vector3.Lerp(BlasterCurrentPosition.transform.position, defaultPosition.transform.position, _aimingAnimationSpeed * Time.deltaTime);
                _mainCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(_mainCamera.fieldOfView, _defaultCameraFOV, _aimingAnimationSpeed * Time.deltaTime);
                _aimCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(_aimCamera.GetComponent<Camera>().fieldOfView, _defaultCameraFOV, _aimingAnimationSpeed * Time.deltaTime);
                isAiming = false;
            }
        }
    }
}
