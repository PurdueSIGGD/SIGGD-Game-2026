using UnityEngine;

public class ToolUser : MonoBehaviour
{
    [SerializeField] Tool SecondaryTool;

    void Start()
    {
        InputManager.Instance.OnPlayerRightClick.AddListener(HandleUse);
        InputManager.Instance.OnUseMiscItem.AddListener(HandleUseAlt);
    }

    void OnDestroy()
    {
        if (InputManager.Instance == null) return;
        InputManager.Instance.OnPlayerRightClick.RemoveListener(HandleUse);
        InputManager.Instance.OnUseMiscItem.RemoveListener(HandleUseAlt);
    }

    public void SetTool(Tool tool)
    {
        SecondaryTool = tool;
    }

    void HandleUse(Vector2 _)
    {
        if (SecondaryTool != null) {
            SecondaryTool.Use();
        }
    }

    void HandleUseAlt(Vector2 _) {
        if (SecondaryTool != null) {
            SecondaryTool.UseAlt();
        }
    }
}
