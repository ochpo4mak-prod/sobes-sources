using Taxonomy.Model;
using UnityEngine.UIElements;

public class TreeFolderView : Foldout
{
    private ITreeFolder _treeFolder;
    private VisualElement _container;

    public TreeFolderView(VisualElement container, ITreeFolder treeFolder)
    {
        _treeFolder = treeFolder;
        _container = container;

        value = false;
        text = treeFolder.Name;
        this.name = treeFolder.Name;
        AddToClassList("foldout");
        AddToClassList("close");

        this.RegisterValueChangedCallback(OnValueChanged);
        RegisterCallback<MouseEnterEvent>(OnMouseEnterEvent);

        var elementForEvent = hierarchy.ElementAt(0).hierarchy.ElementAt(0);
        elementForEvent.RegisterCallback<MouseDownEvent>(OnMouseDownEvent);
    }

    private void OnMouseDownEvent(MouseDownEvent mouseEvent)
    {
        _container.parent.ElementAt(1).Clear();

        if (mouseEvent.button == 1)
        {
            try
            {
                _container.parent.Remove(_container.parent.ElementAt(2));
            }
            catch { }

            new FolderContextMenu(_container.parent, mouseEvent.mousePosition, _treeFolder, this);
        }
        else
        {
            try
            {
                _container.parent.Remove(_container.parent.ElementAt(2));
            }
            catch { }
        }
    }

    private void OnMouseEnterEvent(MouseEnterEvent mouseEvent)
    {
        if (childCount == 0)
            AddToClassList("unavailiable");
        else
            RemoveFromClassList("unavailiable");
    }

    private void OnValueChanged(ChangeEvent<bool> evt)
    {
        if (evt.target == this && childCount != 0)
        {
            if (value)
            {
                AddToClassList("open");
                RemoveFromClassList("close");
            }
            else
            {
                AddToClassList("close");
                RemoveFromClassList("open");
            }
        }
        else if (evt.target == this && childCount == 0)
            value = false;
    }
}
