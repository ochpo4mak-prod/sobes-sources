using UnityEngine;
using Taxonomy.Model;
using UnityEngine.UIElements;

public class RenameFolderModalWindow : ModalWindowBase
{
    private TextField _textField;
    private Foldout _treeFolderView;
    private ITreeFolder _treeFolder;

    public RenameFolderModalWindow(VisualElement container, ITreeFolder treeFolder, Foldout treeFolderView)
        : base(container)
    {
        var textField = new TextField();
        var buttonsContainer = new VisualElement();
        var okBtn = new Button() { text = "OK" };
        var cancelBtn = new Button() { text = "Cancel" };

        _textField = textField;
        _treeFolder = treeFolder;
        _treeFolderView = treeFolderView;

        textField.AddToClassList("modal-element");
        buttonsContainer.AddToClassList("modal-container-for-buttons");
        okBtn.AddToClassList("modal-button");
        cancelBtn.AddToClassList("modal-button");

        Title.text = "Rename folder";

        textField.RegisterCallback<KeyDownEvent>(e => 
        { 
            if (e.keyCode == KeyCode.Return)
                RenameFolder();
            else if (e.keyCode == KeyCode.Escape)
                CloseModalWindow();
        });
        textField.maxLength = 20;
        textField.value = treeFolder.Name;

        cancelBtn.clicked += CloseModalWindow;
        okBtn.clicked += RenameFolder;

        Add(textField);
        Add(buttonsContainer);
        buttonsContainer.Add(okBtn);
        buttonsContainer.Add(cancelBtn);

        textField.Q(TextInputBaseField<string>.textInputUssName).Focus();
    }

    private void RenameFolder()
    {
        if (_textField.text.Trim() == "")
        {
            CloseModalWindow();
            return;
        }

        _treeFolder.ChangeName(_textField.value);
        _treeFolderView.text = _textField.value;

        CloseModalWindow();
    }

    private void CloseModalWindow()
    {
        Container.Remove(Container.ElementAt(2));
    }
}