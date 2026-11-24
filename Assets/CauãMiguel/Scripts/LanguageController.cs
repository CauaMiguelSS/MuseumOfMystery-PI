using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;
public class LanguageController : MonoBehaviour
{
    [SerializeField] private LocalizationSettings _playerWeapon; 
    public void SetLanguage(string languageCode)
    {
        var locale = LocalizationSettings.AvailableLocales.GetLocale(languageCode);
        LocalizationSettings.SelectedLocale = locale;
    }
}
