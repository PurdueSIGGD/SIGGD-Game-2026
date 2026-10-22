using UnityEngine;

/*  primary + Miscellaneous weapons
 *  right click Miscellaneous, left click primary 
 *  
 *  
 *  More notes
 *  Miscellaneous and passives havent been added yet so don't worry about them yet
 *  using another script for the old input System
 *  
 *  Questions
 *  shoot weapon from weapon script. Ask if the weapon scriptable object will hold the game object prefab or should I also add the game object
 *  will weapons get changed with equipment
 *  will we add anything to equipment??
 */

public class EquipmentManager : MonoBehaviour
{
    //singleton code 
    public static EquipmentManager Instance { get; private set; }

    //weapons code
    [SerializeField] private IEquipment primaryWeapon; //add these in the inspector for right now
    [SerializeField] private IEquipment miscellaneous;
    [SerializeField] private IEquipment passive;

    void Awake()
    {
        //more singleton code
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

    }

    private void Start()
    {
        if (primaryWeapon == null)
        {
            Debug.Log("Primary slot is empty!!");
        }

        if (miscellaneous == null)
        {
            Debug.Log("Miscellaneous slot is empty!!");
        }

        if (passive == null)
        {
            Debug.Log("Passuve weapon slot is empty!!");
        }
    }

    void Update()
    {
        //no use yet
    }

    bool hasMisc()
    {
        //return Miscellaneous weapon
        return false;
    }

    //change to UseEquipment when weapon gets fixed!!!
    public void shootPrimaryWeapon()
    {
        Debug.Log("shoot Primary");
    }

    //change to UseEquipment when weapon gets fixed!!!
    public void shootMiscellaneous()
    {
        if (miscellaneous is IEquipment usable)
        {
            usable.Use();
        }
    }
    public void UseEquipment(IEquipment equipment)
    {
        if (equipment is IEquipment usable)
        {
            usable.Use();
        }
    }

    public void setPrimaryWeapon(PrimaryWeapon w)
    {
        this.primaryWeapon = w;
    }

    public IEquipment getPrimaryWeapon()
    {
        return primaryWeapon;
    }

    public void SetMiscellaneous(IEquipment equipment)
    {
        miscellaneous = equipment;
    }

    public IEquipment GetMiscellaneous()
    {
        return miscellaneous;
    }

    public void SetPassive(IEquipment equipment)
    {
        passive = equipment;
    }

    public IEquipment GetPassive()
    {
        return passive;
    }
}
