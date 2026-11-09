using PlayerSystem;
using UnityEngine;

public class ToolUser : MonoBehaviour
{
    [SerializeField] Tool SecondaryTool;

    private PlayerInputHandler input;

    void Awake()
    {
        input = GetComponentInParent<PlayerInputHandler>();
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
}
