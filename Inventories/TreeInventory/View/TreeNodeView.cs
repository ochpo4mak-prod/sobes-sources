using Taxonomy.Model;
using UnityEngine.UIElements;

public class TreeNodeView
{
    public TreeFolderView treeFolderView;
    
    public TreeNodeView(VisualElement container, ITreeNode treeNode)
    {
        treeFolderView = new TreeFolderView(container, treeNode.Root);

        if (!TreeInventoryController.folderViewsList.Contains(treeFolderView))
            TreeInventoryController.folderViewsList.Add(treeFolderView);

        container.Add(treeFolderView);

        foreach (var folder in treeNode.Root.SubFolders)
        {
            var folderView = new TreeFolderView(container, folder);
            treeFolderView.Add(folderView);

            DrawTree(container, folder, folderView);
        }
        foreach (var node in treeNode.Root.Nodes)
        {
            var nodeView = new NodeView(node, container, treeNode.Root);
            treeFolderView.Add(nodeView);
        }
    }

    private void DrawTree(VisualElement container, ITreeFolder treeFolder, TreeFolderView treeFolderView)
    {
        foreach (var folder in treeFolder.SubFolders)
        {
            var folderView = new TreeFolderView(container, folder);
            treeFolderView.Add(folderView);

            DrawTree(container, folder, folderView);
        }
        foreach (var node in treeFolder.Nodes)
        {
            var nodeView = new NodeView(node, container, treeFolder);
            treeFolderView.Add(nodeView);
        }
    }
}
