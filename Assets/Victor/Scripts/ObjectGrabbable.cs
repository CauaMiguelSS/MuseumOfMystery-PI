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
            objectLight.enabled = true;
        }
    }

    public void Grab(Transform objectGrabPointTransform)
    {
        this.objectGrabPointTransform = objectGrabPointTransform;
        objectRigidbody.useGravity = false;


        if (objectLight != null)
        {
            objectLight.enabled = false;
        }
    }

    public void Drop()
    {
        this.objectGrabPointTransform = null;
        objectRigidbody.useGravity = true;

        if (objectLight != null)
        {
            objectLight.enabled = true;
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
