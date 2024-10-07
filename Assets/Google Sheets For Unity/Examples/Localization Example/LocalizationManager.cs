using UnityEngine;
using UnityEngine.Events;

namespace GoogleSheetsForUnity
{
    /*
        Localization Example. Simple implementation of a localization system using
        Spreadsheets as support for game translation data. The advantage of having
        the localization data available directly in a Google Drive Spreadsheet are
        big: data entry, review, distribution, collaboration, etc.
    */
    public class LocalizationManager : MonoBehaviour
    {
        public LocalizationDataSO localizationDataSO;

        private Languages _currentLanguage = Languages.english; // Default Language.
        public Languages CurrentLanguage { get { return _currentLanguage; } }
        
        // Callback for when the language is changed.
        public static UnityAction OnLanguageSet;

        #region Simple Singleton
        private static LocalizationManager _instance;
        public static LocalizationManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject("LocalizationManager");
                    _instance = go.AddComponent<LocalizationManager>();
                }

                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
                Destroy(gameObject);

            _instance = this;
        }
        #endregion

        public void SetCurrentLanguage(Languages language)
        {
            _currentLanguage = language;

            if (OnLanguageSet != null)
                OnLanguageSet();
        }

        public void SetCurrentLanguage(string languageCode)
        {
            if (languageCode == "EN")
                SetCurrentLanguage(Languages.english);

            if (languageCode == "ES")
                SetCurrentLanguage(Languages.spanish);
        }

        //public string GetLocalizedValue(string key)
        //{
        //    Localization localization = localizationDataSO.localizationData.Find((loc) => loc.key == key);

        //    if (localization == null)
        //        return localizationDataSO.missingKey;

        //    switch (_currentLanguage)
        //    {
        //        case Languages.english:
        //            return localization.english;
        //        case Languages.spanish:
        //            return localization.spanish;
        //        default:
        //            return localizationDataSO.missingTranslation;
        //    }
        //}
    }
}
