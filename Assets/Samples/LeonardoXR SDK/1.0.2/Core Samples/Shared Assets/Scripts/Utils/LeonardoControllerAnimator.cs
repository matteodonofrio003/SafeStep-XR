using UnityEngine;
using UnityEngine.InputSystem;

public class LeonardoControllerAnimator : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer m_primaryButton_SMR;
    [SerializeField] private SkinnedMeshRenderer m_secondaryButton_SMR;
    [SerializeField] private SkinnedMeshRenderer m_optionButton_SMR;
    [SerializeField] private SkinnedMeshRenderer m_thumbstick_SMR;
    [SerializeField] private SkinnedMeshRenderer m_trigger_SMR;
    [SerializeField] private SkinnedMeshRenderer m_grip_SMR;

    [SerializeField] private InputActionProperty m_primaryButton;
    [SerializeField] private InputActionProperty m_secondaryButton;
    [SerializeField] private InputActionProperty m_optionButton;
    [SerializeField] private InputActionProperty m_thumbstick;
    [SerializeField] private InputActionProperty m_thumbstickButton;
    [SerializeField] private InputActionProperty m_trigger;
    [SerializeField] private InputActionProperty m_grip;

    private int m_thumbstickX = 0;
    private int m_thumbstickY = 1;
    private int m_thumbstickPress = 2;

    private void OnEnable()
    {
        m_primaryButton.action.performed += OnPrimaryButtonPressed;
        m_primaryButton.action.canceled += OnPrimaryButtonPressed;
        m_secondaryButton.action.performed += OnSecondaryButtonPressed;
        m_secondaryButton.action.canceled += OnSecondaryButtonPressed;
        m_optionButton.action.performed += OnMenuButtonPressed;
        m_optionButton.action.canceled += OnMenuButtonPressed;
        m_thumbstick.action.performed += OnThumbstickMoved;
        m_thumbstick.action.canceled += OnThumbstickMoved;
        m_thumbstickButton.action.performed += OnThumbstickPressed;
        m_thumbstickButton.action.canceled += OnThumbstickPressed;
        m_trigger.action.performed += OnTriggerPressed;
        m_trigger.action.canceled += OnTriggerPressed;
        m_grip.action.performed += OnGripPressed;
        m_grip.action.canceled += OnGripPressed;

        m_primaryButton.action.Enable();
        m_secondaryButton.action.Enable();
        m_optionButton.action.Enable();
        m_thumbstick.action.Enable();
        m_trigger.action.Enable();
        m_grip.action.Enable();
    }

    private void OnDisable()
    {
        m_primaryButton.action.performed -= OnPrimaryButtonPressed;
        m_primaryButton.action.canceled -= OnPrimaryButtonPressed;
        m_secondaryButton.action.performed -= OnSecondaryButtonPressed;
        m_secondaryButton.action.canceled -= OnSecondaryButtonPressed;
        m_optionButton.action.performed -= OnMenuButtonPressed;
        m_optionButton.action.canceled -= OnMenuButtonPressed;
        m_thumbstick.action.performed -= OnThumbstickMoved;
        m_thumbstick.action.canceled -= OnThumbstickMoved;
        m_thumbstickButton.action.performed += OnThumbstickPressed;
        m_thumbstickButton.action.canceled += OnThumbstickPressed;
        m_trigger.action.performed -= OnTriggerPressed;
        m_trigger.action.canceled -= OnTriggerPressed;
        m_grip.action.performed -= OnGripPressed;
        
        m_grip.action.canceled -= OnGripPressed;
        m_primaryButton.action.Disable();
        m_secondaryButton.action.Disable();
        m_optionButton.action.Disable();
        m_thumbstick.action.Disable();
        m_trigger.action.Disable();
        m_grip.action.Disable();
    }

    private void OnPrimaryButtonPressed(InputAction.CallbackContext value)
    {
        m_primaryButton_SMR.SetBlendShapeWeight(0, value.ReadValue<float>() * 100);
    }

    private void OnSecondaryButtonPressed(InputAction.CallbackContext value)
    {
        m_secondaryButton_SMR.SetBlendShapeWeight(0, value.ReadValue<float>() * 100);
    }

    private void OnMenuButtonPressed(InputAction.CallbackContext value)
    {
        m_optionButton_SMR.SetBlendShapeWeight(0, value.ReadValue<float>() * 100);
    }

    private void OnThumbstickMoved(InputAction.CallbackContext value)
    {
        m_thumbstick_SMR.SetBlendShapeWeight(m_thumbstickX, value.ReadValue<Vector2>().x * 100);
        m_thumbstick_SMR.SetBlendShapeWeight(m_thumbstickY, value.ReadValue<Vector2>().y * 100);
    }

    private void OnThumbstickPressed(InputAction.CallbackContext value)
    {
        m_trigger_SMR.SetBlendShapeWeight(m_thumbstickPress, value.ReadValue<float>() * 100);
    }

    private void OnTriggerPressed(InputAction.CallbackContext value)
    {
        m_trigger_SMR.SetBlendShapeWeight(0, value.ReadValue<float>() * 100);
    }

    private void OnGripPressed(InputAction.CallbackContext value)
    {
        m_grip_SMR.SetBlendShapeWeight(0, value.ReadValue<float>() * 100);
    }
}