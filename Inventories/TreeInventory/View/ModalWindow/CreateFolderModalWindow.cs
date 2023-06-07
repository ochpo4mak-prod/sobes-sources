using UnityEngine;
using Taxonomy.Model;
using UnityEngine.UIElements;

public class CreateFolderModalWindow : ModalWindowBase
{
    private TextField _textField;
    private Foldout _treeFolderView;
    private ITreeFolder _treeFolder;

    public CreateFolderModalWindow(VisualElement container, ITreeFolder treeFolder, Foldout treeFolderView)
        : base(container)
    {
        var textField = new TextField();
        var buttonsContainer = new VisualElement();
        var createBtn = new Button() { text = "Create" };
        var cancelBtn = new Button() { text = "Cancel" };

        _textField = textField;
        _treeFolder = treeFolder;
        _treeFolderView = treeFolderView;

        textField.AddToClassList("modal-element");
        buttonsContainer.AddToClassList("modal-container-for-buttons");
        createBtn.AddToClassList("modal-button");
        cancelBtn.AddToClassList("modal-button");

        Title.text = "Create new folder";

        textField.RegisterCallback<KeyDownEvent>(e => 
        { 
            if (e.keyCode == KeyCode.Return)
                CreateFolder();
            else if (e.keyCode == KeyCode.Escape)
                CloseModalWindow();
        });
        textField.maxLength = 20;

        cancelBtn.clicked += CloseModalWindow;
        createBtn.clicked += CreateFolder;

        Add(textField);
        Add(buttonsContainer);
        buttonsContainer.Add(createBtn);
        buttonsContainer.Add(cancelBtn);

        textField.Q(TextInputBaseField<string>.textInputUssName).Focus();
    }

    private void CreateFolder()
    {
        if (_textField.text.Trim() == "")
        {
            CloseModalWindow();
            return;
        }

        ITreeFolder folder;

        folder = _treeFolder.SubFolders.Add(_textField.text.Trim()).Result;

        var treeFolderView = new TreeFolderView(Container.ElementAt(0), folder);
        _treeFolderView.Add(treeFolderView);

        CloseModalWindow();
    }

    private void CloseModalWindow()
    {
        Container.Remove(Container.ElementAt(2));
    }
}