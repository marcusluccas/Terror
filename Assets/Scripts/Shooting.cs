using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    public GameObject gunObj;
    public GameObject pointedObj;

    Vector3 initialPosition;
    Quaternion initialRotation;
    Vector3 pointedPosition;
    Quaternion pointedRotation;

    public GameObject flashlight;
    public GameObject particleShoot;

    public int currentBullet;
    public int maxBullet;
    public int invBullet;

    public bool isReloading;
    public float reloadTime = 3;
    public float reloadCooldown;

    public Animator pistolAnimator;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = gunObj.transform.localPosition;
        initialRotation = gunObj.transform.localRotation;
        pointedPosition = pointedObj.transform.localPosition;
        pointedRotation = pointedObj.transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        Aim();
        FlashlightState();

        if (Input.GetKeyDown(KeyCode.R) && currentBullet != maxBullet)
        {
            reloadCooldown = reloadTime;
            isReloading = true;
        }
        ReloadGun();
    }

    void Aim()
    {
        if (Input.GetMouseButton(1) && isReloading != true)
        {
            if (gunObj.transform.localPosition != pointedPosition)
            {
                gunObj.transform.localPosition = Vector3.Lerp(gunObj.transform.localPosition, pointedPosition, Time.deltaTime * 15);
                gunObj.transform.localRotation = Quaternion.Lerp(gunObj.transform.localRotation, pointedRotation, Time.deltaTime * 15);
            }
            else
            {
                Shoot();
            }
        }
        else
        {
            if (gunObj.transform.localPosition != initialPosition)
            {
                gunObj.transform.localPosition = Vector3.Lerp(gunObj.transform.localPosition, initialPosition, Time.deltaTime * 15);
                gunObj.transform.localRotation = Quaternion.Lerp(gunObj.transform.localRotation, initialRotation, Time.deltaTime * 15);
            }
        }
    }

    void FlashlightState()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            flashlight.SetActive(!flashlight.activeSelf);
        }
    }

    void Shoot()
    {
        if (Input.GetMouseButtonDown(0) && currentBullet > 0 && isReloading == false)
        {
            RaycastHit bulletHit;
            Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out bulletHit);

            currentBullet--;

            HUDManager.instance.BulletCount();

            GameObject particle = Instantiate(particleShoot, gunObj.transform);
            particle.transform.localPosition = new Vector3(0f, 0.055f, 0.2f);
            Destroy(particle, 1f);

            if (bulletHit.transform.tag == "Enemy")
            {
                Destroy(bulletHit.transform.gameObject);
            }

            gunObj.transform.position += new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.25f), 0f);
        }
    }

    void ReloadGun()
    {
        if (isReloading == true)
        {
            pistolAnimator.enabled = true;
            pistolAnimator.Play("PistolReload");
            Debug.Log(reloadCooldown);

            reloadCooldown -= Time.deltaTime;
            if (reloadCooldown < 0)
            {
                invBullet = maxBullet - currentBullet;
                currentBullet = maxBullet;
                HUDManager.instance.BulletCount();
                pistolAnimator.enabled = false;
                pistolAnimator.Play("PistolIdle");
                isReloading = false;
            }
        }
    }
}
