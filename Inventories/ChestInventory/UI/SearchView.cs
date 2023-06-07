using UnityEngine.UIElements;
using System.Collections.Generic;

public class SearchView : VisualElement
{
    private readonly TextField _searchField;

    public SearchView() { }

    public SearchView(List<ContentToIconPair> contentsNode)
    {
        name = "SearchPanel";

        _searchField = new TextField("Search: ");
        var _searchButton = new Button();

        AddToClassList("search-panel");
        _searchField.AddToClassList("search-field");
        _searchButton.AddToClassList("search-button");

        Add(_searchField);
        Add(_searchButton);

        SearchActivity = new SearchActivity(_searchButton, contentsNode, this);
    }

    public SearchActivity SearchActivity { get; }
    public string EnteredText => _searchField.text;
}
