
using UnityEngine;

public class GirarCano : MonoBehaviour
{
    public float velocidadeRotacao = 300f;

    private bool girando = false;
    private Quaternion rotacaoDestino;

    void Start()
    {
        rotacaoDestino = transform.localRotation;

        foreach (Collider col in GetComponentsInChildren<Collider>())
        {
            col.gameObject.AddComponent<BotaoCano>();
            col.GetComponent<BotaoCano>().canoPai = this;
        }
    }

    public void Girar()
    {
        if (girando) return;

        rotacaoDestino *= Quaternion.Euler(0f, 0f, 90f);
        girando = true;
    }

    void Update()
    {
        if (girando)
        {
            transform.localRotation = Quaternion.RotateTowards(
                transform.localRotation,
                rotacaoDestino,
                velocidadeRotacao * Time.deltaTime
            );

            if (Quaternion.Angle(transform.localRotation, rotacaoDestino) < 0.1f)
            {
                transform.localRotation = rotacaoDestino;
                girando = false;
            }
        }
    }
}

public class BotaoCano : MonoBehaviour
{
    public GirarCano canoPai;

    void OnMouseDown()
    {
        if (canoPai != null)
        {
            canoPai.Girar();
        }
    }
}
