// using UnityEngine.UIElements;

// public class InventorySlot : VisualElement
// {
//     public Image Icon;
//     public string ItemGuid = "";

//     public InventorySlot()
//     {
//         Icon = new Image();
//         Add(Icon);

//         Icon.AddToClassList("slotIcon");
//         AddToClassList("slotContainer");

//         RegisterCallback<PointerDownEvent>(OnPointerDown);
//     }

//     public void HoldItem(ItemDetails item)
//     {
//         Icon.image = item.Icon.texture;
//         ItemGuid = item.GUID;
//     }

//     public void DropItem()
//     {
//         ItemGuid = "";
//         Icon.image = null;
//     }

//     private void OnPointerDown(PointerDownEvent evt)
//     {
//         if (evt.button != 0 || ItemGuid.Equals(""))
//         {
//             return;
//         }

//         Icon.image = null;

//         InventoryUIController.StartDrag(evt.position, this);
//     }
// }