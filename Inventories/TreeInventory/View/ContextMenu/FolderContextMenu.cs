using UnityEngine;
using Taxonomy.Model;
using UnityEngine.UIElements;

public class FolderContextMenu : ContextMenuBase
{
    private ITreeFolder _treeFolder;
    private Foldout _treeFolderView;

    public FolderContextMenu(VisualElement container, Vector2 mousePosition, ITreeFolder treeFolder, Foldout treeFolderView)
        : base(container, mousePosition)
    {
        _treeFolder = treeFolder;
        _treeFolderView = treeFolderView;

        var folderButton = new Button() { text = "Create new folder" };
        var nodeButton = new Button() { text = "Create new node" };
        var renameButton = new Button() { text = "Rename Folder" };
        var deleteButton = new Button() { text = "Delete Folder" };
        folderButton.AddToClassList("context-button");
        nodeButton.AddToClassList("context-button");
        renameButton.AddToClassList("context-button");
        deleteButton.AddToClassList("context-button");
        folderButton.clicked += ClickOnCreateFolderButton;
        nodeButton.clicked += ClickOnNodeButton;
        renameButton.clicked += ClickOnRenameFolderButton;
        deleteButton.clicked += ClickOnDeleteButton;

        Add(folderButton);
        Add(nodeButton);
        Add(renameButton);
        Add(deleteButton);
    }

    private void ClickOnDeleteButton()
    {
        if (!TreeInventoryController.folderViewsList.Contains((TreeFolderView)_treeFolderView))
        {
            _treeFolderView.parent.Remove(_treeFolderView);
            _treeFolder.Destroy();            
        }
        else
            Debug.Log("You can't delete the root folder");

        Container.Remove(Container.ElementAt(2));
    }

    private void ClickOnCreateFolderButton()
    {
        Container.Remove(Container.ElementAt(2));

        new CreateFolderModalWindow(Container, _treeFolder, _treeFolderView);
    }

    private void ClickOnRenameFolderButton()
    {
        Container.Remove(Container.ElementAt(2));

        new RenameFolderModalWindow(Container, _treeFolder, _treeFolderView);
    }

    private void ClickOnNodeButton()
    {
        Container.Remove(Container.ElementAt(2));

        new NodeModalWindow(Container, _treeFolder, _treeFolderView);
    }
}