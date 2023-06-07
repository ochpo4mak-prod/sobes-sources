using UnityEngine;
using Taxonomy.Model;
using UnityEngine.UIElements;
using System.Collections.Generic;
using Taxonomy.Model.Implementation;

public class NodeModalWindow : ModalWindowBase
{
    private TextField _textField;
    private Foldout _treeFolderView;
    private ITreeFolder _treeFolder;
    private DropdownField _dropdown;
    private List<string> _weaponCoises = new List<string>() {"Pistol Node", "Rifle Node"};
    private List<string> _rootChoises = new List<string>() {"Weapons Tree", "Armors Tree"};

    public NodeModalWindow(VisualElement container, ITreeFolder treeFolder, Foldout treeFolderView)
        : base(container)
    {
        var textField = new TextField();
        var dropdown = new DropdownField();
        var buttonsContainer = new VisualElement();
        var createBtn = new Button() { text = "Create" };
        var cancelBtn = new Button() { text = "Cancel" };

        textField.AddToClassList("modal-element");
        dropdown.AddToClassList("modal-element");
        buttonsContainer.AddToClassList("modal-container-for-buttons");
        createBtn.AddToClassList("modal-button");
        cancelBtn.AddToClassList("modal-button");

        Title.text = "Create new node";

        textField.RegisterCallback<KeyDownEvent>(e => 
        { 
            if (e.keyCode == KeyCode.Return)
                CreateNode();
            else if (e.keyCode == KeyCode.Escape)
                CloseModalWindow();
        });
        textField.maxLength = 20;

        createBtn.clicked += CreateNode;
        cancelBtn.clicked += CloseModalWindow;

        if (treeFolder is ITreeFolder<ITreeNode> treeFolderTree)
            dropdown.choices = _rootChoises;
        else if (treeFolder is ITreeFolder<IWeaponNode> treeFolderWeapon)
            dropdown.choices = _weaponCoises;

        Add(textField);
        Add(dropdown);
        Add(buttonsContainer);
        buttonsContainer.Add(createBtn);
        buttonsContainer.Add(cancelBtn);

        textField.Q(TextInputBaseField<string>.textInputUssName).Focus();

        _dropdown = dropdown;
        _textField = textField;
        _treeFolder = treeFolder;
        _treeFolderView = treeFolderView;
    }

    private void CreateNode()
    {
        if (_textField.text.Trim() == "")
        {
            CloseModalWindow();
            return;
        }

        IGraphNode node;

        if (_dropdown.value == "Weapons Tree")
        {
            var newTreeNode = NodesFactory.CreateTreeNode<IWeaponNode>(_textField.text.Trim());
            ((ITreeFolder<ITreeNode>)_treeFolder).Nodes.AddNode(newTreeNode);
            node = newTreeNode;
        }
        else if (_dropdown.value == "Armors Tree")
        {
            var newTreeNode = NodesFactory.CreateTreeNode<IArmorNode>(_textField.text.Trim());
            ((ITreeFolder<ITreeNode>)_treeFolder).Nodes.AddNode(newTreeNode);
            node = newTreeNode;
        }
        else if (_dropdown.value == "Pistol Node")
        {
            var newTreeNode = new PistolNode(_textField.text.Trim());
            ((ITreeFolder<IWeaponNode>)_treeFolder).Nodes.AddNode(newTreeNode);
            node = newTreeNode;
        }
        else if (_dropdown.value == "Rifle Node")
        {
            var newTreeNode = new RifleNode(_textField.text.Trim());
            ((ITreeFolder<IWeaponNode>)_treeFolder).Nodes.AddNode(newTreeNode);
            node = newTreeNode;
        }
        else
        {
            Debug.Log("null object");
            return;
        }

        var treeNodeView = new NodeView(node, Container.ElementAt(0), _treeFolder);
        _treeFolderView.Add(treeNodeView);

        CloseModalWindow();
    }

    private void CloseModalWindow()
    {
        Container.Remove(Container.ElementAt(2));
    }
}