using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectShip : MonoBehaviour
{
    public GameObject unselected;
    public GameObject selected;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeSelection()
    {
        unselected.SetActive(!unselected.activeSelf);
        selected.SetActive(!selected.activeSelf);
    }

    public void Select()
    {
        selected.SetActive(true);
        unselected.SetActive(false);
    }
    public void Deselect()
    {
        unselected.SetActive(true);
        selected.SetActive(false);
    }
}
