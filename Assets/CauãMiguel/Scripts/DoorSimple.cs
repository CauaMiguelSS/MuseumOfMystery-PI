using UnityEngine;

public class DoorSimple : MonoBehaviour
{
    public GameObject keyObject;
    public GameObject cadeado;
    public Animator animator; // arrasta o Animator da porta aqui

    private bool playerNear = false;
    private bool unlocked = false;
    private bool opened = false; // evita repetir

    void Update()
    {
        if (playerNear && unlocked && !opened && Input.GetKeyDown(KeyCode.E))
        {
            opened = true;

            animator.SetTrigger("Open"); // ativa animação

            Destroy(keyObject); // opcional: remove a chave
            Destroy(cadeado);
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