using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSelectionManager : MonoBehaviour
{
    public static ShipSelectionManager instance;

    public SelectedShipData selectedShipData;

    private void Awake()
    {
        instance = this;
    }

    public void SelectShip(Sprite selectedShipSprite)
    {
        selectedShipData.shipSprite = selectedShipSprite;

    }

    public void SelectControl(Sprite selectedControlSprite)
    {
        selectedShipData.controlSprite = selectedControlSprite;
    }
}
