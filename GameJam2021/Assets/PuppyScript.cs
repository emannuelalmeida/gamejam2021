using UnityEngine.SceneManagement;
using UnityEngine;

public class PuppyScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        GameObject obj = col.collider.gameObject;
        Debug.Log("Colided with " + obj.name);
        if (obj.tag == "house")
        {
            Debug.Log("With house");

            obj.SendMessage("FoundPuppy");
            Destroy(gameObject);
        }
    }
}
