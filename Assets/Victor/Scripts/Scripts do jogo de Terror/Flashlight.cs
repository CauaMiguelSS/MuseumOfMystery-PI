using System.Collections;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    private Light _light;
    private float _originalIntensity;
    [SerializeField] private float _intensityDecreaserRate = 0.5f;
    [SerializeField] private float _batteryDuration = 10;
    private bool _lostingPower;//Booleana que habilita a perda de intensidade da lanterna ao estar com bateria fraca
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _light = GetComponent<Light>();
        _originalIntensity = _light.intensity;
        GameController.Instance.OnUseBattery.AddListener(Recharge);
    }

    private void Recharge()
    {
        _light.intensity = _originalIntensity;
        _lostingPower = false;
        StopAllCoroutines();//Se o player usar uma pilha nova antes da antiga acabar, a contagem de tempo é resetada
        StartCoroutine(FullBattery());//Inicia a contagem de tempo para a bateria acabar
    }

    IEnumerator FullBattery()
    {
        yield return new WaitForSeconds(_batteryDuration);
        _lostingPower = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_lostingPower)//Se não estiver perdendo energia, não faça nada
            return;
        if (_light.intensity <= 0)//Nullcheck para evitar que a intensidade fique negativa
            return;

        _light.intensity -= Time.deltaTime * _intensityDecreaserRate;
    }
}