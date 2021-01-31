using UnityEngine.SceneManagement;
using UnityEngine;

public class HouseScript : MonoBehaviour
{

    public int puppiesToFind;
    int puppiesFound;
    void Start()
    {
        puppiesFound = 0;
    }

    void Update()
    {
        
    }

    public void FoundPuppy()
    {
        puppiesFound++;

        if (puppiesFound == puppiesToFind)
            SceneManager.LoadScene("Credits");
    }
}
