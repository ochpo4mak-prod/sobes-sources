using UnityEngine.UIElements;

namespace Assets.Scripts.Table.UI
{
    public class TableNodeView : TableNodeViewBase
    {
        public TableNodeView(ITableNode tableNode, int pixelSize, Image icon)
            : base(tableNode, pixelSize)
        {
            icon.AddToClassList("icon");
            Icon = icon;
            Add(icon);
        }

        public Image Icon { get; }
    }
}
