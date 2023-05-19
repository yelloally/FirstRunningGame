using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LocaleSelector : MonoBehaviour
{
    private bool active = false;

    //method which change locale
    public void ChangeLocal (int localeID)
    {
        //if a local change is already in progress
        if (active == true)
            return;
        //set the new local
        StartCoroutine(SetLocale(localeID));
    }

    IEnumerator SetLocale (int _localeID)
    {
        //mark the locale
        active = true;
        //wait for init 
        yield return LocalizationSettings.InitializationOperation;
        //set selected local to the locale ID
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_localeID];
        //not active 
        active = false;
    }
}
