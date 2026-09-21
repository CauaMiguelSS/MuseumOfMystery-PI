using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DadosSalvos
{
    public List<string> cadeadosAbertos = new List<string>();

    public float playerX;
    public float playerY;
    public float playerZ;

    public float playerRotX;
    public float playerRotY;
    public float playerRotZ;

    public bool possuiPosicaoPlayer = false;
}
