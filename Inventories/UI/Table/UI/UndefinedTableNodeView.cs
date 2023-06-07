namespace Assets.Scripts.Table.UI
{
    public class UndefinedTableNodeView : TableNodeViewBase
    {
        public UndefinedTableNodeView(ITableNode tableNode, int pixelSize)
            : base(tableNode, pixelSize)
        {
            AddToClassList("undefined");
        }
    }
}
