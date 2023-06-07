// using System;
// using System.Linq;
// using UnityEngine;
// using System.Collections.Generic;

// [Serializable] public class ItemDetails
// {
//     public string Name;
//     public string GUID;
//     public Sprite Icon;
//     public bool CanDrop;
// }

// public enum InventoryChangeType
// {
//     Pickup,
//     Drop
// }

// public delegate void OnInventoryChangedDelegate(string[] itemGuid, InventoryChangeType change);
// public class GameController : MonoBehaviour
// {
//     [SerializeField] private List<Sprite> IconSprites;
//     public static List<ItemDetails> m_PlayerInventory = new List<ItemDetails>();
//     public static event OnInventoryChangedDelegate OnInventoryChanged = delegate { };
//     public static Dictionary<string, ItemDetails> m_ItemDatabase = new Dictionary<string, ItemDetails>();

//     private void Awake()
//     {
//         PopulateDatabase();
//     }

//     public static void RenderInv()
//     {
//         if (!PauseMenu.firstOpenInv)
//         {
//             PauseMenu.firstOpenInv = !PauseMenu.firstOpenInv;
//             m_PlayerInventory.AddRange(m_ItemDatabase.Values);
//         }
//         OnInventoryChanged.Invoke(m_PlayerInventory.Select(x => x.GUID).ToArray(), InventoryChangeType.Pickup);
//     }

//     public void PopulateDatabase()
//     {
//         m_ItemDatabase.Add("1B9C6CAA-2541-412D-91BF-37F22C9A0E7B", new ItemDetails()
//         {
//             Name = "Health",
//             GUID = "1B9C6CAA-2541-412D-91BF-37F22C9A0E7B",
//             Icon = IconSprites.FirstOrDefault(x => x.name.Contains("Health")),
//             CanDrop = true
//         });

//         m_ItemDatabase.Add("8B0EF21A-F2D9-4E6F-8B79-031CA9E202BA", new ItemDetails()
//         {
//             Name = "Grenade",
//             GUID = "8B0EF21A-F2D9-4E6F-8B79-031CA9E202BA",
//             Icon = IconSprites.FirstOrDefault(x => x.name.Contains("Grenade")),
//             CanDrop = false
//         });

//         m_ItemDatabase.Add("992D3386-B743-4CD3-9BB7-0234A057C265", new ItemDetails()
//         {
//             Name = "Pistol",
//             GUID = "992D3386-B743-4CD3-9BB7-0234A057C265",
//             Icon = IconSprites.FirstOrDefault(x => x.name.Contains("Pistol")),
//             CanDrop = true
//         });

//         m_ItemDatabase.Add("1B9C6CAA-754E-156345-91BF-37F22C9A0E7B", new ItemDetails()
//         {
//             Name = "Rifle",
//             GUID = "1B9C6CAA-754E-156345-91BF-37F22C9A0E7B",
//             Icon = IconSprites.FirstOrDefault(x => x.name.Contains("Rifle")),
//             CanDrop = true
//         });
        
//         m_ItemDatabase.Add("1B9C6CAA-754E-412D-1835893-37F22C9A0E7B", new ItemDetails()
//         {
//             Name = "Rocket Launcher",
//             GUID = "1B9C6CAA-754E-412D-1835893-37F22C9A0E7B",
//             Icon = IconSprites.FirstOrDefault(x => x.name.Contains("RocketLauncher")),
//             CanDrop = true
//         });
//     }

//     public static ItemDetails GetItemByGuid(string guid)
//     {
//         if (m_ItemDatabase.ContainsKey(guid))
//         {
//             return m_ItemDatabase[guid];
//         }

//         return null;
//     }
// }