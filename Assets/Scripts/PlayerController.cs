using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helper;
using Cinemachine;

[RequireComponent(typeof(Camera))]
public class PlayerController : MonoBehaviour
{

    [SerializeField] private EventManager _EventManager;
    [SerializeField] private Animator HandsAndGun;
    [SerializeField, Range(.01f, 1000f)] private float RotationSpeed = 360;
    [SerializeField, Range(.01f, 100f)] private float FoV = 60;
    [SerializeField] private Transform Target;
    [SerializeField] private GameObject[] MuzzleFlashes;
    [SerializeField] private Transform Muzzle;
    [SerializeField] private GameObject BulletHole;
    [SerializeField] private CinemachineVirtualCamera vCam;
    private Camera _camera;

    private Vector2 _screenSize = new Vector2(Screen.width, Screen.height);

    private void Start()
    {
        _EventManager = FindObjectOfType<EventManager>();
        _camera = GetComponent<Camera>();
        _camera.fieldOfView = FoV;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
        float targetPosX = Input.mousePosition.x.Remap(0, _screenSize.x, -100, 100);
        float targetPosY = Input.mousePosition.y.Remap(0, _screenSize.y, -20, 45);
        Target.position = new Vector3(targetPosX, targetPosY, 50);
        if (Input.GetKey(KeyCode.Escape)) Cursor.visible = true;

        if (Input.GetMouseButtonDown(0))
        {
            _EventManager.InvokeFireEvent(Target.position);
        }
    }

    public void ShowMuzzleFlash()
    {
        System.Random flashType = new System.Random();
        StartCoroutine(ShowingFlash(MuzzleFlashes[flashType.Next(4)]));
    }

    private IEnumerator ShowingFlash(GameObject muzzleFlash)
    {
        float size = 0;
        float initialSize = .03f;
        muzzleFlash.transform.localScale = Vector3.zero;
        muzzleFlash.SetActive(true);
        while (size <= initialSize)
        {
            size += Time.deltaTime;
            muzzleFlash.transform.localScale = size * Vector3.one;
            yield return null;
        }
        muzzleFlash.SetActive(false);
    }

    public void CreateBulletHole(Vector3 target)
    {
        Ray r = new Ray(Muzzle.position, target - Muzzle.position);
        if (Physics.Raycast(r, out RaycastHit hit))
        {
            StartCoroutine(ShowingBulletHole(hit.point, hit.normal));
        }
    }

    private IEnumerator ShowingBulletHole(Vector3 hitPoint, Vector3 hitNormal)
    {
        GameObject bulletHole = GameObject.Instantiate(BulletHole, hitPoint + hitNormal / 10, Quaternion.identity);
        Quaternion normalRot = Quaternion.LookRotation(hitNormal);
        bulletHole.transform.rotation = normalRot;
        
        yield return new WaitForSecondsRealtime(2);
        GameObject.Destroy(bulletHole);
    }
    public void RelockCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }
    public void ShakeTheCamera()
    {
        StartCoroutine(CameraShaking());
    }

    private IEnumerator CameraShaking()
    {
        vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 2;
        yield return new WaitForSecondsRealtime(.1f);
        vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
    }
}
