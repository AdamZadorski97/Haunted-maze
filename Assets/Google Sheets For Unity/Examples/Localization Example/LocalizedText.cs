using UnityEngine;
using UnityEngine.UI;

namespace GoogleSheetsForUnity
{
    public class LocalizedText : MonoBehaviour
    {
        public string key;

        
        public Localization localization;

        private void OnEnable()
        {
            LocalizationManager.OnLanguageSet += LocalizeText;
        }

        private void OnDisable()
        {
            LocalizationManager.OnLanguageSet -= LocalizeText;
        }

        private void Start()
        {
            LocalizeText();
        }

        private void LocalizeText()
        {
            Text text = GetComponent<Text>();
            //text.text = LocalizationManager.Instance.GetLocalizedValue(key);
        }

    }
}
