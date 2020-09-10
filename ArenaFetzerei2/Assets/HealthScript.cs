using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{


    public static int health = 100;

    Text healthText;
    // Start is called before the first frame update
    void Start()
    {
        healthText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = "Health: " + health;
    }
}
