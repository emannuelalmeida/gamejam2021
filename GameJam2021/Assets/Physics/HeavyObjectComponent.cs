using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyObjectComponent : MonoBehaviour
{

    private Rigidbody2D body;
    private DogControls dog;

    private bool hasStrengthPowerUp = false;
    
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        dog = GameObject.Find("Dog").GetComponent<DogControls>();
        dog.OnPowerUpPicked += CheckPowerUp;
    }

    void OnDisable()
    {
        dog.OnPowerUpPicked -= CheckPowerUp;
    }

    void CheckPowerUp(PowerUp powerUp)
    {
        hasStrengthPowerUp = powerUp == PowerUp.Strength;
        if (hasStrengthPowerUp)
            body.bodyType = RigidbodyType2D.Dynamic;
    }

    void FixedUpdate()
    {
        if (hasStrengthPowerUp)
            return;

        if (body.IsSleeping())
        {
            body.bodyType = RigidbodyType2D.Static;
        }
    }
}
