using UnityEngine.InputSystem;
using Unity.XR.CoreUtils;
using UnityEngine;

public class PassthroughActivator : MonoBehaviour
{
    [SerializeField] private bool _enableOnStart;
    [SerializeField, ReadOnly] private bool _enabled;

    [SerializeField] private InputActionProperty _passthroughButton;

    private void OnEnable()
    {
        _passthroughButton.action.performed += TogglePassthrough;
        _passthroughButton.action.Enable();
    }


    private void OnDisable()
    {
        _passthroughButton.action.performed -= TogglePassthrough;
        _passthroughButton.action.Disable();
    }

    private void Start()
    {
        TogglePassthrough(_enableOnStart || XRPassthroughUtility.GetPassthroughEnabled());
    }

    public void TogglePassthrough(InputAction.CallbackContext context)
    {
        if (PlayerManager.Instance.DebugOn)
        {
            var enable = XRPassthroughUtility.GetPassthroughEnabled();
            enable = !enable;
            TogglePassthrough(enable);
        }
    }

    public void TogglePassthrough()
    {
        TogglePassthrough(!_enabled);
    }

    public void TogglePassthrough(bool enable)
    {
        XRPassthroughUtility.SetPassthroughEnabled(enable);
        _enabled = enable;
    } 
}