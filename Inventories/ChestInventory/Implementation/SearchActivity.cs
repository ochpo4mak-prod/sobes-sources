using System.Linq;
using UnityEngine.UIElements;
using Assets.Scripts.Table.UI;
using System.Collections.Generic;

public struct ViewInfo
{
    public TableView TableView;
    public PreviewNode PreviewNode;
}

public class SearchActivity
{
    private Dictionary<VisualElement, string> _element2Name = new Dictionary<VisualElement, string>();
    private Dictionary<VisualElement, ViewInfo> _element2ViewInfo = new Dictionary<VisualElement, ViewInfo>();

    public SearchActivity(Button searchButton, List<ContentToIconPair> contentsNode, SearchView searchView)
    {
        _searchView = searchView;
        _contentsNode = contentsNode;

        searchButton.clicked += SearchItems;
    }

    private readonly SearchView _searchView;
    private readonly List<ContentToIconPair> _contentsNode;

    public VisualElement[] Elements => _element2Name.Keys.ToArray();

    private void SearchItems()
    {
        var objName = "";
        var occurCount = 0;
        var tableViews = new List<TableView>();
        var previewNode = new List<PreviewNode>();
        var enteredText = _searchView.EnteredText.ToLower();

        foreach (var element in _contentsNode)
            if (element.Content.ToString().ToLower().Contains(enteredText))
                ++occurCount;

        foreach (var element in _element2Name)
        {
            _element2ViewInfo.TryGetValue(element.Key, out var viewInfo);
            viewInfo.PreviewNode.SearchText.text = "";
            var isValid = element.Value.ToLower().Contains(enteredText) && occurCount < 2;

            if (isValid)
            {
                tableViews.Add(viewInfo.TableView);
                previewNode.Add(viewInfo.PreviewNode);
                objName = element.Value.ToString();
                
                if (element.Key.ClassListContains("searched-item"))
                    continue;
                else
                    element.Key.AddToClassList("searched-item");
            }
            else
                if (element.Key.ClassListContains("searched-item"))
                    element.Key.RemoveFromClassList("searched-item");
        }

        var tableViewObjCount = tableViews.GroupBy(t => t).Select(t => new
        {
            Table = t.Key,
            Object = objName,
            ObjectsCount = t.Count()
        });
        
        foreach (var table in tableViewObjCount)
        {
            var text = table.Object + ": " + table.ObjectsCount;
            previewNode.Where(i => i == table.Table.PreviewNode).First().SearchText.text = text;
        }
    }

    public void AddElement(VisualElement element, string name, TableView tableView, PreviewNode previewNode)
    {
        _element2Name.Add(element, name);
        _element2ViewInfo.Add(element, new ViewInfo
        {
            TableView = tableView,
            PreviewNode = previewNode
        });
    }
}
