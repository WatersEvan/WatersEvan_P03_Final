using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class ShootingSystem : MonoBehaviour
{

    MovementInput input;

    [SerializeField] ParticleSystem inkParticle;
    [SerializeField] Transform parentController;
    [SerializeField] Transform splatGunNozzle;
    [SerializeField] Transform splatGunNozzle2;
    [SerializeField] CinemachineFreeLook freeLookCamera;
    CinemachineImpulseSource impulseSource;

    public AudioSource shootSound;

    public int maxAmmo = 500;
    private int currentAmmo;
    public float reloadTime = 1f;

    private bool isReloading;

    public InkBar inkBar;

    void Start()
    {
        input = GetComponent<MovementInput>();
        impulseSource = freeLookCamera.GetComponent<CinemachineImpulseSource>();
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        Vector3 angle = parentController.localEulerAngles;
        input.blockRotationPlayer = Input.GetMouseButton(0);
        bool pressing = Input.GetMouseButton(0);

        if (Input.GetMouseButton(0))
        {
            VisualPolish();
            input.RotateToCamera(transform);
        }

        if (isReloading)
        {
            return;
        }

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            inkParticle.Stop();
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            inkParticle.Play();
            shootSound.Play();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            inkParticle.Stop();
        }

        parentController.localEulerAngles
            = new Vector3(Mathf.LerpAngle(parentController.localEulerAngles.x, pressing ? RemapCamera(freeLookCamera.m_YAxis.Value, 0, 1, -25, 25) : 0, .3f), angle.y, angle.z);
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading");

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        inkBar.SetInk(currentAmmo);
        isReloading = false;
    }

    void VisualPolish()
    {
        currentAmmo--;
        inkBar.SetInk(currentAmmo);

        if (!DOTween.IsTweening(parentController))
        {
            parentController.DOComplete();
            Vector3 forward = -parentController.forward;
            Vector3 localPos = parentController.localPosition;
            parentController.DOLocalMove(localPos - new Vector3(0, 0, .2f), .03f)
                .OnComplete(() => parentController.DOLocalMove(localPos, .1f).SetEase(Ease.OutSine));

            impulseSource.GenerateImpulse();
        }

        if (!DOTween.IsTweening(splatGunNozzle))
        {
            splatGunNozzle.DOComplete();
            splatGunNozzle.DOPunchScale(new Vector3(0, 1, 1) / 1.5f, .15f, 10, 1);

            splatGunNozzle2.DOComplete();
            splatGunNozzle2.DOPunchScale(new Vector3(0, 1, 1) / 1.5f, .15f, 10, 1);
        }
    }

    float RemapCamera(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
