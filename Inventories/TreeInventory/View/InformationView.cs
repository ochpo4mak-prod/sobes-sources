using Taxonomy.Model;
using UnityEngine.UIElements;

public class InformationView
{
    private int _foldersCount = 0;
    private int _elementsCount = 0;

    public InformationView(VisualElement container, IGraphNode _graphNode)
    {
        var nameLabel = new Label() { text = _graphNode.Name };
        nameLabel.AddToClassList("information-text");
        nameLabel.AddToClassList("title");
        container.Add(nameLabel);

        var icon = new Image();
        icon.AddToClassList("information-icon");
        container.Add(icon);

        var infoLabel = new Label();
        infoLabel.AddToClassList("information-text");

        if (_graphNode is ITreeNode treeNode)
        {
            icon.AddToClassList("tree");

            foreach (var folder in treeNode.Root.SubFolders)
            {
                _foldersCount++;
                CountNumber(folder);
            }
            _elementsCount += treeNode.Root.Nodes.Count;

            infoLabel.text = $"Number of folders: {_foldersCount}\nNumber of elements: {_elementsCount}\nID: {treeNode.Id}";
        }

        if (_graphNode is RifleNode)
        {
            icon.AddToClassList("rifle");

            infoLabel.text = $"ID: {_graphNode.Id}";
        }

        if (_graphNode is PistolNode)
        {
            icon.AddToClassList("pistol");

            infoLabel.text = $"ID: {_graphNode.Id}";
        }

        container.Add(infoLabel);
    }

    private void CountNumber(ITreeFolder folder)
    {
        foreach (var subfolder in folder.SubFolders)
        {
            _foldersCount++;
            CountNumber(subfolder);
        }
        _elementsCount += folder.Nodes.Count;
    }
}