using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionScript : MonoBehaviour
{
    GameObject camera_external, camera_kitchen, camera_bedroom;

    // Start is called before the first frame update
    void Start()
    {
        camera_external = GameObject.Find("Main Camera");
        camera_kitchen = GameObject.Find("Kitchen Camera");
        camera_bedroom = GameObject.Find("Bedroom Camera");
    }

    void OnTriggerEnter2D(Collider2D other) {
        float dog_position_x = other.gameObject.transform.position.x;
        float dog_position_y = other.gameObject.transform.position.y;
        bool isDog = other.tag == "Player";
        // Transition External -> Kitchen
        if (dog_position_x > -10)
        {
            other.gameObject.transform.position = new Vector3(-27.5f, dog_position_y, 0);
            if (isDog)
            {
                camera_external.GetComponent<Camera>().enabled = false;
                camera_kitchen.GetComponent<Camera>().enabled = true;
            }
        }
        // Transition Kitchen -> External
        else if (dog_position_x < -10 && dog_position_x > -40)
        {
            other.gameObject.transform.position = new Vector3(-6.5f, dog_position_y, 0);
            if (isDog)
            {
                camera_external.GetComponent<Camera>().enabled = true;
                camera_kitchen.GetComponent<Camera>().enabled = false;
            }
        }
        // Transition Kitchen -> Bedroom
        else if (dog_position_x < -40 && dog_position_x > -55)
        {
            other.gameObject.transform.position = new Vector3(-61.5f, dog_position_y, 0);
            if (isDog)
            {
                camera_kitchen.GetComponent<Camera>().enabled = false;
                camera_bedroom.GetComponent<Camera>().enabled = true;
            }
        }
        // Transition Bedroom -> Kitchen
        else if (dog_position_x < -55 && dog_position_x > -65)
        {
            other.gameObject.transform.position = new Vector3(-44f, dog_position_y, 0);
            if (isDog)
            {
                camera_kitchen.GetComponent<Camera>().enabled = true;
                camera_bedroom.GetComponent<Camera>().enabled = false;
            }
        }
    }
}
