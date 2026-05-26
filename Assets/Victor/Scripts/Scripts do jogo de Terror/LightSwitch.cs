using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Outline))]
public class LightSwitch : MonoBehaviour, IInteractable
{
    [SerializeField] private bool _isOn;
    [SerializeField] private UnityEvent OnTurnOn;
    [SerializeField] private UnityEvent OnTurnOff;
    private Outline _outline;

    private void Start()
    {
        _outline = GetComponentInChildren<Outline>();
        _outline.enabled = false;
    }

    public void HideOutline()
    {
        if (_outline != null)
        {
            _outline.enabled = false;
        }
    }

    public void Interact()
    {
        if (_isOn)
        {
            OnTurnOff.Invoke();
        }
        else
        {
            OnTurnOn.Invoke();
        }
        _isOn = !_isOn;
        //Animação do interruptor mudando o botão
    }

    public void ShowOutline()
    {
        if (_outline != null)
        {
            _outline.enabled = true;
        }
    }
}