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
    [SerializeField] private Weapon primaryWeapon; //add these in the inspector for right now
    [SerializeField] private Equipment miscellaneous;
    [SerializeField] private Equipment passive;

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

    bool hasPrimaryWeaponGun()
    {
        if (primaryWeapon != null)
        {
            return true;
        }
        //return if has primary
        return false;
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
        if (miscellaneous is IUsableEquipment usable)
        {
            usable.Use();
        }
    }
    public void UseEquipment(Equipment equipment)
    {
        if (equipment is IUsableEquipment usable)
        {
            usable.Use();
        }
    }

    public void setPrimaryWeapon(Weapon w)
    {
        this.primaryWeapon = w;
    }

    public Weapon getPrimaryWeapon()
    {
        return primaryWeapon;
    }

    public void SetMiscellaneous(Equipment equipment)
    {
        miscellaneous = equipment;
    }

    public Equipment GetMiscellaneous()
    {
        return miscellaneous;
    }

    public void SetPassive(Equipment equipment)
    {
        passive = equipment;
    }

    public Equipment GetPassive()
    {
        return passive;
    }
}
