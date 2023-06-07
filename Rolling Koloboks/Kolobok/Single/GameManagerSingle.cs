using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManagerSingle : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoints;
    [SerializeField] private GameObject _botPrefab;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private TMP_Text _counterText;

    private List<string> _nicknames = new()
    {
        "Giggle Queen",
        "Chees3l0v3r",
        "Cabbage Head",
        "Bubble_Blower",
        "PotatoKing",
        "Rock_Star",
        "SockThief",
        "Sp00n_Swall0wer",
        "DancingBearrr",
        "Pillow_Fighter",
        "xXBorschtinatorXx",
        "TicklishTeapot",
        "Laughing_Lizard",
        "PajamaBandit",
        "Battle_State",
        "БабаЯгаBoss",
        "РжаваяРусалка",
        "ХохотунЧерт",
        "Волчий Хохол",
        "Богатырь Балалайка",
        "Пельмень с глазами",
        "ШалунМедведь",
        "Шапокляк_Хакер",
        "Водитель_Трамвая",
        "ВесёлыйВолк",
        "Ёжик_В_Сметане",
        "Валид_Олл",
        "СыктывкарАндерграунд",
        "Сельдь_В_Шубе",
        "Очпочмак"
    };

    private void Start()
    {
        SpawnKoloboks();
        StartCoroutine(GameStartCounter());
    }

    private void SpawnKoloboks()
    {
        List<int> indexesForSpawn = GetRandomSpawnPointIndexes(_spawnPoints.childCount);

        var player = Instantiate(_playerPrefab, transform);
        player.transform.position = _spawnPoints.GetChild(indexesForSpawn[0]).transform.position;

        List<int> indexesForNicknames = GetRandomSpawnPointIndexes(_nicknames.Count);
        for (int i = 1; i < indexesForSpawn.Count; i++)
        {
            var bot = Instantiate(_botPrefab, transform);
            bot.transform.position = _spawnPoints.GetChild(indexesForSpawn[i]).transform.position;
            bot.GetComponentInChildren<TMP_Text>().text = _nicknames[indexesForNicknames[i]];
        }
    }

    private List<int> GetRandomSpawnPointIndexes(int maxExclusive)
    {
        List<int> indexes = new();

        for (int i = 0; i < CreateGameUI.Instanse.BotsCount + 1; i++)
        {
            int index = Random.Range(0, maxExclusive);

            if (indexes.Contains(index))
                i--;
            else
                indexes.Add(index);
        }

        return indexes;
    }

    private IEnumerator GameStartCounter()
    {
        SoundManager.Instance.PlayBeginCounterSound();

        int seconds = 3;
        _counterText.text = seconds.ToString();

        while (seconds != 0)
        {
            yield return new WaitForSeconds(1);
            seconds--;

            if (seconds != 0)
                _counterText.text = seconds.ToString();
            else
                EnableRolling();
        }

        FinishSingle.Instance.SetStartTime();
        _counterText.text = Translation.Instance.Translate("Game.Roll");
        yield return new WaitForSeconds(1);
        _counterText.text = string.Empty;
    }

    private void EnableRolling()
    {
        GetComponentInChildren<KolobokSinglePlayer>().IsMoving = true;

        foreach (var npc in GetComponentsInChildren<KolobokNPC>())
            npc.IsMoving = true;

        MusicManager.Instance.PlayGameMusic();
        SinglePause.Instance.PauseButtonEnable();
    }
}
