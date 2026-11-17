
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [Header("Pilar que desaparecerá")]
    public GameObject pilar;

    public void ResolverPuzzle()
    {
        if (pilar != null)
        {
            pilar.SetActive(false);
        }
    }
}
