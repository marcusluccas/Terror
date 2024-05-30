using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    public GameObject[] gunObj;
    public GameObject[] pointedObj;

    public int selectedGun;

    public List<Vector3> initialPosition;
    public List<Quaternion> initialRotation;
    public List<Vector3> pointedPosition;
    public List<Quaternion> pointedRotation;

    public GameObject flashlight;
    public GameObject particleShoot;

    public int[] currentBullet;
    public int[] maxBullet;
    public int[] invBullet;

    public bool isReloading = false;
    public float reloadTime = 3;
    public float reloadCooldown;
    public bool isAuto = false;
    public float shootSpeed = 15;

    public List<Animator> gunAnimator;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < gunObj.Length; i++)
        {
            initialPosition.Add(gunObj[i].transform.localPosition);
            initialRotation.Add(gunObj[i].transform.localRotation);
            pointedPosition.Add(pointedObj[i].transform.localPosition);
            pointedRotation.Add(pointedObj[i].transform.localRotation);
            gunAnimator.Add(gunObj[i].GetComponent<Animator>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        Aim();
        FlashlightState();
        ChangeGun();

        Debug.Log(currentBullet[selectedGun] + "/" + maxBullet[selectedGun]);
        if (Input.GetKeyDown(KeyCode.R) && currentBullet[selectedGun] != maxBullet[selectedGun])
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
            if (gunObj[selectedGun].transform.localPosition != pointedPosition[selectedGun])
            {
                gunObj[selectedGun].transform.localPosition = Vector3.Lerp(gunObj[selectedGun].transform.localPosition, pointedPosition[selectedGun], Time.deltaTime * shootSpeed);
                gunObj[selectedGun].transform.localRotation = Quaternion.Lerp(gunObj[selectedGun].transform.localRotation, pointedRotation[selectedGun], Time.deltaTime * shootSpeed);
            }
            else
            {
                Shoot();
            }
        }
        else
        {
            if (gunObj[selectedGun].transform.localPosition != initialPosition[selectedGun])
            {
                gunObj[selectedGun].transform.localPosition = Vector3.Lerp(gunObj[selectedGun].transform.localPosition, initialPosition[selectedGun], Time.deltaTime * shootSpeed);
                gunObj[selectedGun].transform.localRotation = Quaternion.Lerp(gunObj[selectedGun].transform.localRotation, initialRotation[selectedGun], Time.deltaTime * shootSpeed);
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
        if (((Input.GetMouseButtonDown(0) && isAuto == false) || (Input.GetMouseButton(0) && isAuto == true)) && currentBullet[selectedGun] > 0 && isReloading == false)
        {
            RaycastHit bulletHit;
            Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out bulletHit);

            currentBullet[selectedGun]--;

            HUDManager.instance.BulletCount();

            GameObject particle = Instantiate(particleShoot, gunObj[selectedGun].transform);
            particle.transform.localPosition = new Vector3(0f, 0.025f, 1.5f);
            Destroy(particle, 1f);

            if (bulletHit.transform.tag == "Enemy")
            {
                Destroy(bulletHit.transform.gameObject);
            }

            gunObj[selectedGun].transform.position += new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.25f), 0f);
        }
    }

    void ReloadGun()
    {
        if (isReloading == true)
        {
            gunAnimator[selectedGun].enabled = true;
            if (selectedGun == 0)
            {
                gunAnimator[selectedGun].Play("PistolReload");
            }
            if (selectedGun == 1)
            {
                gunAnimator[selectedGun].Play("RifleReload");
            }

            reloadCooldown -= Time.deltaTime;
            if (reloadCooldown <= 0)
            {
                invBullet[selectedGun] = maxBullet[selectedGun] - currentBullet[selectedGun];
                currentBullet[selectedGun] = maxBullet[selectedGun];
                HUDManager.instance.BulletCount();
                gunAnimator[selectedGun].enabled = false;
                if (selectedGun == 0)
                {
                    gunAnimator[selectedGun].Play("PistolIdle");
                }
                if (selectedGun == 1)
                {
                    gunAnimator[selectedGun].Play("RifleIdle");
                }
                isReloading = false;
            }
        }
    }

    void ChangeGun()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && reloadCooldown <= 0)
        {
            selectedGun = 0;
            isAuto = false;
            shootSpeed = 15;
            HUDManager.instance.BulletCount();
            gunObj[0].SetActive(true);
            gunObj[1].SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && reloadCooldown <= 0)
        {
            selectedGun = 1;
            isAuto = true;
            shootSpeed = 60;
            HUDManager.instance.BulletCount();
            gunObj[1].SetActive(true);
            gunObj[0].SetActive(false);
        }
    }
}
