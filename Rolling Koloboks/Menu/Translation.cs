using System;
using System.IO;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;

public class Translation : MonoBehaviour
{
    public static Translation Instance { get; private set; }

    public const string KEY_LANGUAGE = "LANGUAGE";
    private const string PATH = "/Translation.json";

    private List<Translations> LocalizationList { get; set; }

    private void Awake()
    {
        Instance = this;

        if (!PlayerPrefs.HasKey(KEY_LANGUAGE))
        {
            SystemLanguage lang = Application.systemLanguage;
            string langCode;

            switch (lang)
            {
                case SystemLanguage.Russian:
                    langCode = "RU";
                    break;
                case SystemLanguage.English:
                    langCode = "EN";
                    break;
                default:
                    langCode = "EN";
                    break;
            }

            PlayerPrefs.SetString(KEY_LANGUAGE, langCode);
        }

        string fileContents = "";

        if (Application.platform == RuntimePlatform.Android)
        {
            WWW reader = new WWW(Application.streamingAssetsPath + PATH);
            while (!reader.isDone) { }

            fileContents = reader.text;
        }
        else
        {
            fileContents = File.ReadAllText(Application.streamingAssetsPath + PATH);
        }

        LocalizationList = JsonConvert.DeserializeObject<Root>(fileContents).translations;
    }

    public string Translate(string localizationKey)
    {
        if (PlayerPrefs.GetString(KEY_LANGUAGE) == "RU")
            return LocalizationList.FirstOrDefault(x => x.key == localizationKey).RU;
        else
            return LocalizationList.FirstOrDefault(x => x.key == localizationKey).EN;
    }
}

[Serializable]
public class Root
{
    public List<Translations> translations { get; set; }
}

public class Translations
{
    public string key { get; set; }
    public string RU { get; set; }
    public string EN { get; set; }
}
