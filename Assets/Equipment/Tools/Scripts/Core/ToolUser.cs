using PlayerSystem;
using UnityEngine;

public class ToolUser : MonoBehaviour
{
    [SerializeField] private Transform armRotatePoint;
    [SerializeField] private Tool SecondaryTool;
    [SerializeField] private GameObject SecondaryToolObject;

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

        WeaponPrefabTransformHolder holder = GetComponent<WeaponPrefabTransformHolder>();

        // i hate object oriented programming so much
        if (SecondaryTool is ProjectileTool projectileTool)
        {
            projectileTool.Equip(transform, armRotatePoint);
        }
        // else if (SecondaryTool is MeleeTool) // will need to be added eventually
        

        // maybe do the actual equip stuff here?
    }
}
