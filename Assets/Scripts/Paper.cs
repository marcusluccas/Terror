using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paper : MonoBehaviour
{
    public GameObject playerObj;
    public float distanceToPlayer;

    private void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, playerObj.transform.position);
    }

    private void OnMouseOver()
    {
        if (distanceToPlayer < 5)
        {
            HUDManager.instance.pressE.SetActive(true);
            CollectPaper();
        }
        else
        {
            HUDManager.instance.pressE.SetActive(false);
        }
    }
    private void OnMouseExit()
    {
        HUDManager.instance.pressE.SetActive(false);
    }

    void CollectPaper()
    {
        if (distanceToPlayer < 5 && Input.GetKeyDown(KeyCode.E))
        {
            HUDManager.instance.pressE.SetActive(false);
            HUDManager.instance.AddPaper();
            Destroy(this.gameObject);
        }
    }
}
