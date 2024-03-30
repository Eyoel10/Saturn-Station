using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    class Spawnable
    {
        public GameObject gameObject;
        public bool isWaitingToSpawn = false;
    }

    [SerializeField] GameObject prefab;
    [SerializeField] int instanceCount;
    [SerializeField] float minTimeToSpawn, maxTimeToSpawn;
    Spawnable[] instances;
    Vector2 screenSizeWorld;

    static Bounds bounds = new(new Vector3(), new Vector3(40.0f, 40.0f));

    protected virtual void AfterSpawn(GameObject instance)
    {

    }

    IEnumerator Spawn(Spawnable instance)
    {
        instance.gameObject.SetActive(false);
        instance.isWaitingToSpawn = true;

        yield return new WaitForSeconds(Random.Range(minTimeToSpawn, maxTimeToSpawn));

        instance.gameObject.SetActive(true);
        instance.isWaitingToSpawn = false;

        float x = screenSizeWorld.x + 4.0f;
        float y = Random.Range(-4.0f * screenSizeWorld.y, 4.0f * screenSizeWorld.y);
        instance.gameObject.transform.position = new Vector2(x, y);

        AfterSpawn(instance.gameObject);
    }

    void Start()
    {
        screenSizeWorld = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
        instances = new Spawnable[instanceCount];
        for (int i = 0; i < instanceCount; ++i)
        {
            Spawnable instance = new() { gameObject = Instantiate(prefab) };
            instances[i] = instance;
            StartCoroutine(Spawn(instance));
        }
    }

    void Update()
    {
        foreach (Spawnable instance in instances)
        {
            if (instance.gameObject.activeInHierarchy)
            {
                if (!bounds.Contains(instance.gameObject.transform.position))
                    instance.gameObject.SetActive(false);
            }
            else
            {
                if (!instance.isWaitingToSpawn)
                    StartCoroutine(Spawn(instance));
            }
        }
    }
}
