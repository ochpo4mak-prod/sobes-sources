using OpenAI_API;
using OpenAI_API.Chat;

using System;
using System.Collections;
using System.Threading.Tasks;

using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ChatManager : MonoBehaviour
{
    private const string API_KEY = "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";
    private const string TEXT_SETTING = "You are a text-based RPG game where you give me options (A, B, C, and D) as my choices. The setting is FOR_REPLACE. I start out with 100 health and I could die. You can use a maximum of 400 characters for the answer. You don't have to explain the outcomes";

    [SerializeField] private Image _image;
    [SerializeField] private TMP_InputField _chatField;
    [SerializeField] private TMP_InputField _inputField;

    private Conversation _chat;
    private Translator _translator;
    private ImageGenerator _imageGenerator;

    private bool IsSetSetting { get; set; } = true;
    private Conversation Chat => _chat;
    private Translator Translator => _translator;
    private ImageGenerator ImageGenerator => _imageGenerator;

    private void Awake()
    {
        _translator = FindObjectOfType<Translator>();
        _imageGenerator = FindObjectOfType<ImageGenerator>();

        var _openAI = new OpenAIAPI(API_KEY);
        _chat = _openAI.Chat.CreateConversation();
        Chat.RequestParameters.MaxTokens = 500;
    }

    private void Start()
    {
        WriteToChat(Role.Game, "Напишите сеттинг для игры");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            OnSendButtonClick();
    }

    public async void OnSendButtonClick()
    {
        string answer;
        string imageUrl;

        var input = _inputField.text.Trim();
        _inputField.text = string.Empty;
        WriteToChat(Role.User, input);

        if (IsSetSetting)
        {
            IsSetSetting = false;

            var fullTextSetting = await Task.Run(() =>
            {
                return $"{TEXT_SETTING.Replace("FOR_REPLACE", Translator.TranslateRuToEn(input))}";
            });
            answer = await Task.Run(() => SendToBot(fullTextSetting));
        }
        else
        {
            answer = await Task.Run(() => SendToBot(Translator.TranslateRuToEn(input)));
        }

        WriteToChat(Role.Bot, Translator.TranslateEnToRu(answer));

        if (answer.IndexOf("A)") > 0)
        {
            imageUrl = await Task.Run(() =>
            {
                return ImageGenerator.GetImageAboutText(answer.Split("A)", StringSplitOptions.None)[0]);
            });
        }
        else
        {
            EndGame();
            imageUrl = await Task.Run(() => ImageGenerator.GetImageAboutText(answer));
        }

        StartCoroutine(SetImageFromUrl(imageUrl));
    }

    private async Task<string> SendToBot(string question)
    {
        Chat.AppendUserInput(question);
        var result = await Chat.GetResponseFromChatbot();

        return result;
    }

    private void EndGame()
    {
        WriteToChat(Role.Game, "Игра окончена");
    }

    private IEnumerator SetImageFromUrl(string url)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
            Debug.LogError(www.error);
        else
        {
            for (float i = 1; i > 0; i -= 0.01f)
            {
                _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, i);
                yield return new WaitForSeconds(0.01f);
            }

            var texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            _image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

            for (float i = 0; i < 1; i += 0.01f)
            {
                _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, i);
                yield return new WaitForSeconds(0.01f);
            }
        }
    }

    private void WriteToChat(Role role, string text)
    {
        switch (role)
        {
            case Role.Bot:
                StartCoroutine(PrintingText($"Бот: {text}\n"));
                break;
            case Role.User:
                _chatField.text += $"Вы: {text}\n";
                break;
            case Role.Game:
                StartCoroutine(PrintingText($"{text}\n"));
                break;
        }
    }
    
    private IEnumerator PrintingText(string text)
    {
        for (int i = 0; i < text.Length; i++)
        {
            _chatField.text += text[i];
            yield return new WaitForSeconds(0.05f);
        }
    }
}

public enum Role
{
    Bot,
    User,
    Game
}

//private async Task GenerateAnswer(string question)
//{
//    var http = new HttpClient();
//    http.DefaultRequestHeaders.Add("Authorization", $"Bearer {API_KEY}");

//    var jsonContent = new
//    {
//        prompt = question,
//        model = "gpt-3.5-turbo",
//        max_tokens = 60
//    };

//    var responseContent = await http.PostAsync("https://api.openai.com/v1/chat/completions",
//                                                new StringContent(JsonConvert.SerializeObject(jsonContent),
//                                                Encoding.UTF8,
//                                                "application/json"));

//    var resContext = await responseContent.Content.ReadAsStringAsync();
//    var data = JsonConvert.DeserializeObject<dynamic>(resContext);
//    string answer = data.choices[0].text;

//    WriteToChat($"Bot: {answer.Trim()}\n");
//}