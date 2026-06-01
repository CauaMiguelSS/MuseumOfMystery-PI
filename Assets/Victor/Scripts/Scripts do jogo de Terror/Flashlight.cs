using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ActiveState
{
    OFF, ON
}

public class Flashlight : MonoBehaviour
{
    private ActiveState _activeState = ActiveState.ON;
    private Light _light;
    private float _originalIntensity;
    [SerializeField] private float _intensityDecreaserRate = 0.5f;
    [SerializeField] private float _batteryDuration = 10;
    private bool _lostingPower;//Booleana que habilita a perda de intensidade da lanterna ao estar com bateria fraca
    private bool _isFullBattery = true;
    private float _batteryTimer;//Timer para controlar o tempo de duração de bateria
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _light = GetComponent<Light>();
        _originalIntensity = _light.intensity;
        GameController.Instance.OnUseBattery.AddListener(Recharge);
        GameController.Instance.OnUseFlashlight.AddListener(TurnFlashlight);
        _batteryTimer = _batteryDuration;
    }

    private void Recharge()
    {
        _light.intensity = _originalIntensity;
        _batteryTimer = _batteryDuration;
        _lostingPower = false;
    }

    // Update is called once per frame
    void Update()
    {
        /*
         Esse switch é para simular algo como o OnCollisionStay, pois executa
        lógica a cada frame dependendo do estado atual da lanterna
         */
        switch (_activeState)
        {
            case ActiveState.OFF:
                //Se a lanterna estiver desligada, não precisa executar nada
                //os sistemas relacionados a bateria ficam "suspensos"
                break;
            case ActiveState.ON:
                if (_lostingPower)//Aqui é caso a lanterna esteja perdendo a luz já
                {
                    if (_light.intensity <= 0)//Nullcheck para evitar que a intensidade fique negativa
                        return;
                    _light.intensity -= Time.deltaTime * _intensityDecreaserRate;
                }
                else//Aqui é caso a lanterna esteja com a bateria ainda boa
                {
                    _batteryTimer -= Time.deltaTime;//Diminui o timer da bateria a cada frame
                    if (_batteryTimer <= 0)//Se o timer da bateria acabar
                    {
                        _lostingPower = true;//Começa a perder intensidade
                    }
                }
                break;
            default:
                break;
        }
    }

    public void TurnFlashlight()
    {
        /*
         Com esse procedimento, sempre que apertamos para ligar ou desligar a lanterna,
        iniciaremos um processo parecido com o do OnCollisionEnter, onde podemos ativar
        alguma consequência dependendo do estado atual da lanterna, que não precisa ser
        executado a todo momento
         */
        if (_activeState.Equals(ActiveState.ON))
        {
            SetState(ActiveState.OFF);
        }
        else
        {
            SetState(ActiveState.ON);
        }
    }

    public void SetState(ActiveState newState)
    {
        switch (newState)//Variável de controle
        {
            case ActiveState.OFF://if(newState.Equals(ActiveState.OFF))
                //Aqui é onde executa
                _light.enabled = false;//Desativa a luz
                break;//Encerra a execução do switch
            case ActiveState.ON://if(newState.Equals(ActiveState.ON))
                //Aqui é onde executa
                _light.enabled = true;//Ativa a luz
                break;//Encerra a execução do switch
            default://Caso nenhum dos anteriores seja verdadeiro, executa isso
                break;//Encerra a execução do switch
        }
        _activeState = newState;//Aqui atualizo o valor da máquina com o novo valor
    }
}