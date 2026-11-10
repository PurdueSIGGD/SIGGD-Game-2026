using PlayerSystem;
using UnityEngine;

public class ToolUser : MonoBehaviour
{
    [SerializeField] private Transform armRotatePoint;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject SecondaryToolObject;

    private Tool SecondaryTool;

    private PlayerInputHandler input;

    void Awake()
    {
        input = GetComponentInParent<PlayerInputHandler>();

        EquipTool(SecondaryToolObject);
    }

    void OnEnable()
    {
        input.UseToolPressed += HandleUse;
        input.UseToolAltPressed += HandleUseAlt;
    }

    void OnDisable()
    {
        input.UseToolPressed -= HandleUse;
        input.UseToolAltPressed -= HandleUseAlt;
    }

    public void SetTool(Tool tool)
    {
        SecondaryTool = tool;
    }

    void HandleUse()
    {
        if (SecondaryTool != null) {
            SecondaryTool.Use();
        }
    }

    void HandleUseAlt() {
        if (SecondaryTool != null) {
            SecondaryTool.UseAlt();
        }
    }

    public void EquipTool(GameObject newTool)
    {
        if (newTool == null) return;

        SecondaryTool = newTool.GetComponent<Tool>();

        // might not actually be needed depends on if we make a "shooter for the misc"
        WeaponPrefabTransformHolder holder = GetComponent<WeaponPrefabTransformHolder>();

        if (SecondaryTool is ProjectileTool projectileTool)
        {
            projectileTool.Equip(firePoint, armRotatePoint);
        }
        // else if (SecondaryTool is MeleeTool) // will need to be added eventually
        

        // maybe do the actual equip stuff here?
    }
}
