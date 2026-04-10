using UnityEngine;

public class DoorSimple : MonoBehaviour
{
    public GameObject keyObject;
    public Animator animator;

    private bool playerNear = false;
    private bool unlocked = false;
    private bool opened = false;

    void Update()
    {
        if (playerNear && unlocked && !opened && Input.GetKeyDown(KeyCode.E))
        {
            opened = true;

            // toca animação
            animator.SetTrigger("Open");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;
        }

        if (other.gameObject == keyObject)
        {
            unlocked = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;
        }
    }
}
