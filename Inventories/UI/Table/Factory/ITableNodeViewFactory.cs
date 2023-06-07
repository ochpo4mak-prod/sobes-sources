using Assets.Scripts.Table.UI;

namespace Assets.Scripts.Table
{
    public interface ITableNodeViewFactory
    {
        public TableNodeViewBase CreateTableNodeView(ITableNode tableNode, IContent content, SearchView searchView, TableView tableView, PreviewNode previewNode);
        public TableNodeViewBase CreateUndefinedTableNodeView(ITableNode tableNode);
    }
}
