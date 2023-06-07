using UnityEngine.UIElements;
using Assets.Scripts.Table.UI;
using System.Collections.Generic;

namespace Assets.Scripts.Table
{
    public class TableNodeViewFactory : ITableNodeViewFactory
    {
        private TableView _tableView;
        private Dictionary<ENodeContent, Image> _contentName2Icon = new Dictionary<ENodeContent, Image>();

        public TableNodeViewFactory(TableView tableView, IEnumerable<ContentToIconPair> contents)
        {
            _tableView = tableView;
            
            foreach (var content in contents)
            {
                var icon = new Image { sprite = content.Sprite };
                _contentName2Icon.Add(content.Content, icon);
            }
        }

        public TableNodeViewBase CreateTableNodeView(ITableNode tableNode, IContent content, SearchView searchView, TableView tableView, PreviewNode previewNode)
        {
            if (!_contentName2Icon.TryGetValue(content.Content, out var icon))
                icon = _contentName2Icon[ENodeContent.Default];

            icon = new Image { sprite = icon.sprite };
            searchView.SearchActivity.AddElement(icon, content.Content.ToString(), tableView, previewNode);

            var nodeView = new TableNodeView(tableNode, _tableView.pixelSize, icon);

            return nodeView;
        }

        public TableNodeViewBase CreateUndefinedTableNodeView(ITableNode tableNode)
        {
            var nodeView = new UndefinedTableNodeView(tableNode, _tableView.pixelSize);
            return nodeView;
        }
    }
}
