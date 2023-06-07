using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

[Serializable] public class GeneralItem
{
    public ItemDescription itemDescription;
    public ItemVisual itemVisual;
}

public class GameInventory : MonoBehaviour
{
    public static GameInventory sample;
    [SerializeField] private List<GeneralItem> _generalItems = new List<GeneralItem>();

    [SerializeField] private Size _groundSize;
    public static Size slotSize;

    private VisualElement m_Root;
    private VisualElement m_GroundGrid;
    private VisualElement m_Pointer;

    private bool isReady;

    private void Start()
    {
        m_Root = GetComponentInChildren<UIDocument>().rootVisualElement;
        m_GroundGrid = m_Root.Q<VisualElement>("SlotsGround");

        sample = this;
        Configure();

        ItemLoading();
    }

    private async void Configure()
    {
        ConfigurePointer();
        await UniTask.WaitForEndOfFrame();

        CalculationSlotSize(m_GroundGrid);

        isReady = true;
    }

    private void ConfigurePointer()
    {
        m_Pointer = new VisualElement
        {
            name = "Pointer",
        };
        m_Pointer.AddToClassList("pointer-icon");

        m_GroundGrid.Add(m_Pointer);
    }

    private void CalculationSlotSize(VisualElement grid)
    {
        VisualElement firstSlot = grid.Children().First();

        slotSize = new Size
        {
            x = Mathf.RoundToInt(firstSlot.worldBound.width),
            y = Mathf.RoundToInt(firstSlot.worldBound.height)
        };
    }

    private async void ItemLoading()
    {
        await UniTask.WaitUntil(() => isReady);

        foreach (var item in _generalItems)
        {
            ItemVisual groundItemVisual = new ItemVisual(item.itemDescription);

            m_GroundGrid.Add(groundItemVisual);

            bool groundHasSpace = await GetPositionForItem(groundItemVisual);

            if (!groundHasSpace)
            {
                Debug.Log("No space");
                m_GroundGrid.Remove(groundItemVisual);
                continue;
            }

            item.itemVisual = groundItemVisual;
            groundItemVisual.style.visibility = Visibility.Visible;
        }
    }
    
    private async Task<bool> GetPositionForItem(VisualElement newItem)
    {
        for (int y = 0; y < _groundSize.y; y++)
        {
            for (int x = 0; x < _groundSize.x; x++)
            {
                newItem.style.left = slotSize.x * x;
                newItem.style.top = slotSize.y * y;

                await UniTask.WaitForEndOfFrame();

                GeneralItem overlappingItem = _generalItems.FirstOrDefault(s => s.itemVisual != null && s.itemVisual.layout.Overlaps(newItem.layout));

                if (overlappingItem == null)
                    return true;
            }
        }
        return false;
    }

    public bool CheckPlacement(ItemVisual item)
    {
        if (!m_GroundGrid.layout.Contains(new Vector2(item.localBound.xMax, item.localBound.yMax)))
        {
            m_Pointer.style.visibility = Visibility.Hidden;
            return (false);
        }

        var overlapping = _generalItems.Where(x => x.itemVisual != null && x.itemVisual.layout.Overlaps(m_Pointer.layout)).ToArray();

        if (overlapping.Length > 1)
        {
            m_Pointer.style.visibility = Visibility.Hidden;
            return (false);
        }

        return true;
    }

    public Vector2 CalculationPos(ItemVisual item)
    {
        VisualElement resultSlot = m_GroundGrid.Children().Where(x => x.layout.Overlaps(item.layout) && x != item).OrderBy(x => Vector2.Distance(x.worldBound.position, item.worldBound.position)).First();

        m_Pointer.style.width = item.style.width;
        m_Pointer.style.height = item.style.height;

        m_Pointer.style.left = resultSlot.layout.position.x;
        m_Pointer.style.top = resultSlot.layout.position.y;

        m_Pointer.style.visibility = Visibility.Visible;

        return (resultSlot.worldBound.position);
    }
}