using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PassiveHolder : MonoBehaviour
{
    public List<Passive> listOfPassive = new List<Passive>();
    public Passive currentPassive;
    void Start()
    {
        if (currentPassive == null)
        {
            Debug.Log("No Passive added");
        }

        if (listOfPassive.Count == 0)
        {
            Debug.Log("No Passive in Passive List");
        }
    }

    public void setPassive(Passive passive)
    {
        currentPassive = passive;
    }

    public Passive getPassive()
    {
        return currentPassive;
    }
}
