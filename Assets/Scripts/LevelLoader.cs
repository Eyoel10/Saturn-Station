using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    // GameObjects to preserve across scene changes
    //public GameObject[] objectsToNotPreserve;
    //public GameObject[] objectsToPreserve;

    public Animator transition;
    // Start is called before the first frame update
    void Start()
    {
        //objectsToPreserve = FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void LoadPreviousLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex - 1));
    }

    IEnumerator LoadLevel(int index)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(index);
    }

    IEnumerator LoadLevel(string name)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(name);
    }
    /*
    public void LoadNextLevelWithPreserve()
    {
        LoadLevelWithPreserve(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    // Load the different scene
    public void LoadLevelWithPreserve(string name)
    {
        
        if (objectsToNotPreserve != null)
        {
            foreach (GameObject obj in objectsToNotPreserve)
            {
                if (objectsToPreserv)
            }
        }
        
        // Preserve specified GameObjects across scene changes
        foreach (GameObject obj in objectsToPreserve)
        {
            DontDestroyOnLoad(obj);
        }

        // Load the different scene
        StartCoroutine(LoadLevel(name));

    }

    // Load the different scene
    public void LoadLevelWithPreserve(int index)
    {
        if (objectsToPreserve == null)
        {
            objectsToPreserve = FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        }
        // Preserve specified GameObjects across scene changes
        foreach (GameObject obj in objectsToPreserve)
        {
            DontDestroyOnLoad(obj);
        }

        // Load the different scene
        StartCoroutine(LoadLevel(index));
    }
    */
    public void LoadaScene(string name)
    {
        StartCoroutine(LoadLevel(name));
    }

    public void Quit()
    {
        Application.Quit();
    }
}
