using UnityEngine;

public class DoorSimple : MonoBehaviour
{
    public GameObject keyObject;

    private bool playerNear = false;
    private bool unlocked = false;

    void Update()
    {
        if (playerNear && unlocked && Input.GetKeyDown(KeyCode.E))
        {
            Destroy(gameObject);
            Destroy(keyObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Player perto
        if (other.CompareTag("Player"))
        {
            playerNear = true;
        }

        // Chave encostou
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
