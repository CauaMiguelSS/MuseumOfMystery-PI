using UnityEngine;

public class ObjectGrabbable : MonoBehaviour
{
    private Rigidbody objectRigidbody;
    private Transform objectGrabPointTransform;

    [Header("Luz do objeto")]
    public Light objectLight;

    private void Awake()
    {
        objectRigidbody = GetComponent<Rigidbody>();

        if (objectLight != null)
        {
            objectLight.enabled = true; // luz inicial ligada
        }
    }

    public void Grab(Transform objectGrabPointTransform)
    {
        this.objectGrabPointTransform = objectGrabPointTransform;
        objectRigidbody.useGravity = false;

        // Luz permanece como filha do objeto, então não precisa mexer
        // Se quiser, pode garantir que continue ligada
        if (objectLight != null)
        {
            objectLight.enabled = true;
        }
    }

    public void Drop()
    {
        this.objectGrabPointTransform = null;
        objectRigidbody.useGravity = true;

        if (objectLight != null)
        {
            objectLight.enabled = true; // permanece ligada
        }
    }

    private void FixedUpdate()
    {
        if (objectGrabPointTransform != null)
        {
            float lerpSpeed = 10f;

            Vector3 newPosition = Vector3.Lerp(
                transform.position,
                objectGrabPointTransform.position,
                Time.deltaTime * lerpSpeed
            );

            objectRigidbody.MovePosition(newPosition);
        }
    }
}