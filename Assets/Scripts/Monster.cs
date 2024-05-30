using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    GameObject player;
    public GameObject gameOverScreen;
    public GameObject gameOverAnimation;

    public int hp = 3;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<NavMeshAgent>().destination = player.transform.position;
    }

    public void RemoveHP(int demage)
    {
        hp -= demage;

        if (hp <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") 
        {
            gameOverScreen.SetActive(true);
            gameOverAnimation.SetActive(true);
            other.gameObject.GetComponent<CharacterController>().enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Destroy(this.gameObject);
        }
    }
}
