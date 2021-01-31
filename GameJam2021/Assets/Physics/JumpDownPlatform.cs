using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class JumpDownPlatform : MonoBehaviour
{

    private BoxCollider2D dog;
    private BoxCollider2D box;
    
    void Start()
    {
        // effector = GetComponent<PlatformEffector2D>();
        dog = GameObject.Find("Dog").GetComponent<BoxCollider2D>();
        box = GetComponent<BoxCollider2D>();
        box.enabled = dog.bounds.min.y >= box.bounds.max.y;
    }

    void FixedUpdate()
    {
        if (DogControls.CrouchTriggered())
            box.enabled = false;
        else
        {
            float dogFeetY = dog.bounds.min.y;
            float dogHeadY = dog.bounds.max.y;
            float boxBottomY = box.bounds.min.y;
            float boxTopY = box.bounds.max.y;
            if (box.enabled)
                // check if below
                box.enabled = dogHeadY > boxBottomY;
            else
                // check if above
                box.enabled = dogFeetY >= boxTopY;
        }
    }
}

