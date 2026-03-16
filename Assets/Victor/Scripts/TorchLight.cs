using UnityEngine;
using UnityEngine.InputSystem;

public class TorchLight : MonoBehaviour
{
    [SerializeField] private InputActionReference toggleAction;
    [SerializeField] private Light torchLight;
    [SerializeField] public GameObject Lanterna;

    private void Awake()
    {
        Lanterna.SetActive(false);
    }

    private void OnEnable()
    {
        toggleAction.action.performed += OnToggle;
    }

    private void OnDisable()
    {
        toggleAction.action.performed -= OnToggle;
    }

    private void OnToggle(InputAction.CallbackContext ctx)
    {
        Lanterna.SetActive(!Lanterna.activeSelf);
    }
}