using UnityEngine;

public class OutlineDetector : MonoBehaviour
{
    [SerializeField] private float distance = 3f;

    private Outline currentOutline;
    private RaycastHit currentHit;
    private bool hasHit;

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        hasHit = Physics.Raycast(ray, out currentHit, distance);

        if (hasHit)
        {
            Outline outline = currentHit.collider.GetComponentInParent<Outline>();

            if (outline != currentOutline)
            {
                if (currentOutline != null)
                    currentOutline.enabled = false;

                currentOutline = outline;

                if (currentOutline != null)
                    currentOutline.enabled = true;
            }
        }
        else
        {
            if (currentOutline != null)
            {
                currentOutline.enabled = false;
                currentOutline = null;
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && hasHit)
        {
            TVInteraction tv =
                currentHit.collider.GetComponentInParent<TVInteraction>();

            if (tv != null)
            {
                tv.EnterTV();
            }
        }
    }
}