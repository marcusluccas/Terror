using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject gunObj;
    public GameObject pointedObj;

    Vector3 initialPosition;
    Quaternion initialRotation;
    Vector3 pointedPosition;
    Quaternion pointedRotation;

    bool canShoot;

    public GameObject flashlight;
    public GameObject particleShoot;

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
    }

    void Aim()
    {
        if (Input.GetMouseButton(1))
        {
            if (gunObj.transform.localPosition != pointedPosition)
            {
                gunObj.transform.localPosition = Vector3.Lerp(gunObj.transform.localPosition, pointedPosition, Time.deltaTime * 5);
                gunObj.transform.localRotation = Quaternion.Lerp(gunObj.transform.localRotation, pointedRotation, Time.deltaTime * 5);
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
                gunObj.transform.localPosition = Vector3.Lerp(gunObj.transform.localPosition, initialPosition, Time.deltaTime * 5);
                gunObj.transform.localRotation = Quaternion.Lerp(gunObj.transform.localRotation, initialRotation, Time.deltaTime * 5);
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
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit bulletHit;
            Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out bulletHit);

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
}
