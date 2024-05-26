using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager instance { get; private set; }

    public Slider staminaBar;
    public Image staminaColor;
    public GameObject pressE;
    public Text paperCount;
    public int papers;
    public GameObject monsterObj;
    public GameObject victoryScreen;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPaper()
    {
        papers++;
        PaperCount();
        monsterObj.SetActive(true);

        if (papers >= 5)
        {
            monsterObj.SetActive(false);
            victoryScreen.SetActive(true);
        }
    }

    void PaperCount()
    {
        paperCount.text = papers.ToString() + "/5";
    }
}
