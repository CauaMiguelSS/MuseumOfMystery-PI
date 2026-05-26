using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Outline))]
public class Battery : MonoBehaviour, ICollectable
{
    private Outline _outline;
    public void Collect()
    {
        Destroy(gameObject);//Destroi o objeto quando coletado
    }

    public void HideOutline()
    {
        if (_outline != null)
        {
            _outline.enabled = false;//Desativa o outline quando não for mais mirado
        }
    }

    public void ShowOutline()
    {
        if (_outline != null)
        {
            _outline.enabled = true;//Ativa o outline quando for mirado
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _outline = GetComponentInChildren<Outline>();//Pega o outline
        _outline.enabled = false;//Desabilita para não aparecer o outline no início
    }
}