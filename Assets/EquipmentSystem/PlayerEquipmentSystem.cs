using FMOD;
using JetBrains.Annotations;
using System;
using UnityEngine;

public class PlayerEquipmentSystem : MonoBehaviour
{
    /*
     * Goal: create a funciton that takes in a object and switches the item the player is holding
     * 
     */

    public int totalEquipments = 0;
    [SerializeField] int currentEquipmentIndex = 0;
    [SerializeField] GameObject[] equipment;
    public GameObject equipmentHolster;
    public GameObject currentEquipment;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        try
        {
            totalEquipments = equipmentHolster.transform.childCount;
            equipment = new GameObject[totalEquipments];

            for (int i = 0; i < totalEquipments; i++)
            {
                equipment[i] = equipmentHolster.transform.GetChild(i).gameObject;
            }

            currentEquipment = equipment[currentEquipmentIndex];
            currentEquipment.SetActive(true);
        } catch (Exception e)
        {
            UnityEngine.Debug.LogError("ERROR in equipment system set up" + e);
        }

    }

    //returns a boolean stating that weapon has or hasnt been switched
    public Boolean setEquipment(int  equipmentIndex)
    {
        if(equipmentIndex < totalEquipments && equipmentIndex >= 0)
        {
            currentEquipmentIndex = equipmentIndex;
            currentEquipment.SetActive(false);
            currentEquipment = equipment[equipmentIndex];
            currentEquipment.SetActive(true);
            return true;
        }

        return false;
    }
}
