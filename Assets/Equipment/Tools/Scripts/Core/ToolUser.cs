using PlayerSystem;
using UnityEngine;

public class ToolUser : MonoBehaviour
{
    [SerializeField] private Transform armRotatePoint;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject SecondaryToolObject; // holds the reference for what tool should be used on runtime, in the future a equipment manager will hold all of these secondary objects and just call equip passing in a new object but thats for shop team

    private GameObject activeTool;
    private Tool secondaryTool;

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
        secondaryTool = tool;
    }

    void HandleUse()
    {
        if (secondaryTool != null) {
            Debug.Log("secondary tool use called");
            secondaryTool.Use();
        }
    }

    void HandleUseAlt() {
        if (secondaryTool != null) {
            secondaryTool.UseAlt();
        }
    }

    public void EquipTool(GameObject newTool)
    {
        if (newTool == null) return;
        if (activeTool != null) Destroy(activeTool);

        activeTool = Instantiate(newTool, transform);

        secondaryTool = activeTool.GetComponent<Tool>();

        // might not actually be needed depends on if we make a "shooter for the misc"
        WeaponPrefabTransformHolder holder = GetComponent<WeaponPrefabTransformHolder>();

        if (secondaryTool is ProjectileTool projectileTool)
        {
            projectileTool.Equip(firePoint, armRotatePoint);
        }
        // else if (SecondaryTool is MeleeTool) // will need to be added eventually
        
    }
}
