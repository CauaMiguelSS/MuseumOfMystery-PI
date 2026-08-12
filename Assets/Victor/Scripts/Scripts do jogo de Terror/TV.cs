using UnityEngine;

public class TV : MonoBehaviour, IInteractable
{
    [SerializeField] private TVInteraction tvInteraction;

    private Outline outline;

    private void Awake()
    {
        outline = GetComponentInChildren<Outline>(true);

        if (outline != null)
            outline.enabled = false;
    }

    public void Interact()
    {
        tvInteraction.EnterTV();
    }

    public void ShowOutline()
    {
        if (outline != null)
            outline.enabled = true;
    }

    public void HideOutline()
    {
        if (outline != null)
            outline.enabled = false;
    }
}