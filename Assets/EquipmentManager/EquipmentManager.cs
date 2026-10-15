using UnityEngine;

/*  primary + Miscellaneous weapons
 *  right click Miscellaneous, left click primary 
 *  
 *  
 *  More notes
 *  Miscellaneous and passives havent been added yet so don't worry about them yet
 *  using another script for the old input System
 */

public class EquipmentManager : MonoBehaviour
{
    //singleton code 
    public static EquipmentManager Instance { get; private set; }

    //weapons code
    [SerializeField] Weapon primaryWeapon; //add these in the inspector for right now

    void Start()
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

        //add weapons, Miscellaneous, and passives here so that there automatically applied when the game starts
        if (primaryWeapon == null)
        {
            Debug.Log("Primary weapon slot is empty!!");
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

    public void shootPrimaryWeapon()
    {
        //shoot weapon from weapon script. Ask if the weapon scriptable object will hold the game object prefab or should I also add the game object
        Debug.Log("shoot Primary");
    }

    public void shootMiscellaneous()
    {
        //shoot Miscellaneous from weapon script. Ask if the weapon scriptable object will hold the game object prefab or should I also add the game object
        Debug.Log("shoot Secondary");
    }

    public void setPrimaryWeapon(Weapon w)
    {
        this.primaryWeapon = w;
    }

    public Weapon getPrimaryWeapon()
    {
        return primaryWeapon;
    }
}
