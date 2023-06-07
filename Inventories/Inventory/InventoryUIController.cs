// using System;
// using System.Collections.Generic;
// using System.Linq;
// using UnityEngine.UIElements;
// using UnityEngine;

// public class InventoryUIController : MonoBehaviour
// {
//     public List<InventorySlot> InventoryItems = new List<InventorySlot>();

//     private VisualElement m_Root;
//     private VisualElement m_SlotContainer;
//     private static VisualElement m_GhostIcon;

//     private static bool m_IsDragging;
//     private static InventorySlot m_OriginalSlot;

//     private void OnEnable()
//     {
//         m_Root = GetComponent<UIDocument>().rootVisualElement;
//         m_GhostIcon = m_Root.Query<VisualElement>("GhostIcon");
//         m_SlotContainer = m_Root.Q<VisualElement>("SlotContainer");

//         for (int i = 0; i < 24; i++)
//         {
//             InventorySlot item = new InventorySlot();
//             InventoryItems.Add(item);
//             m_SlotContainer.Add(item);
//         }

//         if (!PauseMenu.firstOpenInv)
//         {
//             GameController.OnInventoryChanged += GameController_OnInventoryChanged;
//         }

//         m_GhostIcon.RegisterCallback<PointerMoveEvent>(OnPointerMove);
//         m_GhostIcon.RegisterCallback<PointerUpEvent>(OnPointerUp);

//         GameController.RenderInv();
//     }

//     private void OnDisable()
//     {
//         InventoryItems.Clear();
//         m_SlotContainer.Clear();
//     }

//     private void GameController_OnInventoryChanged(string[] itemGuid, InventoryChangeType change)
//     {
//         foreach (string item in itemGuid)
//         {
//             if (change == InventoryChangeType.Pickup)
//             {
//                 var emptySlot = InventoryItems.FirstOrDefault(x => x.ItemGuid.Equals(""));

//                 if (emptySlot != null)
//                 {
//                     emptySlot.HoldItem(GameController.GetItemByGuid(item));
//                 }
//             }
//         }
//     }

//     public static void StartDrag(Vector2 position, InventorySlot originalSlot)
//     {
//         m_IsDragging = true;
//         m_OriginalSlot = originalSlot;

//         m_GhostIcon.style.top = position.y - m_GhostIcon.layout.height / 2;
//         m_GhostIcon.style.left = position.x - m_GhostIcon.layout.width / 2;

//         m_GhostIcon.style.backgroundImage = GameController.GetItemByGuid(originalSlot.ItemGuid).Icon.texture;

//         m_GhostIcon.style.visibility = Visibility.Visible;
//     }

//     private void OnPointerMove(PointerMoveEvent evt)
//     {
//         if (!m_IsDragging)
//         {
//             return;
//         }

//         m_GhostIcon.style.top = evt.position.y - m_GhostIcon.layout.height / 2;
//         m_GhostIcon.style.left = evt.position.x - m_GhostIcon.layout.width / 2;
//     }
//     private void OnPointerUp(PointerUpEvent evt)
//     {
//         if (!m_IsDragging)
//         {
//             return;
//         }

//         IEnumerable<InventorySlot> slots = InventoryItems.Where(x =>
//                x.worldBound.Overlaps(m_GhostIcon.worldBound));

//         if (slots.Count() != 0)
//         {
//             InventorySlot closestSlot = slots.OrderBy(x => Vector2.Distance
//                (x.worldBound.position, m_GhostIcon.worldBound.position)).First();


//             closestSlot.HoldItem(GameController.GetItemByGuid(m_OriginalSlot.ItemGuid));

//             m_OriginalSlot.DropItem();
//         }

//         else
//         {
//             m_OriginalSlot.Icon.image =
//                   GameController.GetItemByGuid(m_OriginalSlot.ItemGuid).Icon.texture;
//         }

//         m_IsDragging = false;
//         m_OriginalSlot = null;
//         m_GhostIcon.style.visibility = Visibility.Hidden;
//     }
// }