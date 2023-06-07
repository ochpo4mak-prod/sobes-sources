using System.Net;
using System.Text;
using System.Linq;
using UnityEngine;

public class Translator : MonoBehaviour
{
    public string TranslateRuToEn(string ruText)
    {
        var url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl=auto&tl=en&dt=t&q={ruText}";
        return Translate(url);
    }

    public string TranslateEnToRu(string enText)
    {
        var url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl=auto&tl=ru&dt=t&q={enText}";
        return Translate(url);
    }

    private string Translate(string url)
    {
        var webClient = new WebClient { Encoding = Encoding.UTF8 };
        var result = webClient.DownloadString(url);

        var json = Newtonsoft.Json.JsonConvert.DeserializeObject<object[][][]>(result, new Newtonsoft.Json.JsonSerializerSettings
        {
            Error = (_, e) => { e.ErrorContext.Handled = true; }
        });

        return string.Join(" ", json[0].SelectMany(x => x.Skip(0)?.Take(1)).Cast<string>()).Replace("\\n", "\n").Replace("\n ", "\n");
    }
}
