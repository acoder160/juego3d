using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider slider;
    public float maxHealth = 100f;
    public float health;

    // cooldown
    //Tiempo de espera
    private float lastActionTime;
    private float cooldownDuration = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        lastActionTime = Time.time;
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (slider.value != health)
        {
            slider.value = health;
        }

    }

    public void takeDamage()
    {
        if (Time.time > lastActionTime + cooldownDuration) {
            if (health > 0) {
                health -= 10;
                lastActionTime = Time.time;
            }
               
        }
        
       
    }
}
