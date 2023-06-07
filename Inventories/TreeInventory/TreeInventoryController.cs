using UnityEngine;
using Taxonomy.Model;
using UnityEngine.UIElements;
using System.Collections.Generic;
using Taxonomy.Model.Implementation;

public class TreeInventoryController : MonoBehaviour
{
    private VisualElement m_Container;
    public static List<TreeFolderView> folderViewsList = new List<TreeFolderView>();

    private void Start()
    {
        var m_Root = GetComponentInChildren<UIDocument>().rootVisualElement;
        m_Container = m_Root.Q<VisualElement>("Container");
        m_Container.RegisterCallback<MouseDownEvent>(OnMouseDownEvent);

        var m_registryContainer = new VisualElement();
        m_registryContainer.AddToClassList("folders-container");
        m_Container.Add(m_registryContainer);

        var m_historyContainer = new VisualElement();
        m_historyContainer.AddToClassList("history-container");
        m_registryContainer.Add(m_historyContainer);

        var m_informationContainer = new VisualElement();
        m_informationContainer.AddToClassList("information-container");
        m_Container.Add(m_informationContainer);

        var registry = NodesFactory.CreateTreeNode<ITreeNode>("Registry");
        var treeNodeOfWeapons = NodesFactory.CreateTreeNode<IWeaponNode>("Weapons Tree");
        var treeNodeOfArmors = NodesFactory.CreateTreeNode<IArmorNode>("Armors Tree");

        treeNodeOfArmors.Root.SubFolders.Add("Helmets");
        treeNodeOfArmors.Root.SubFolders.Add("Body armors");
        treeNodeOfWeapons.Root.SubFolders.Add("Shotguns");
        var riflesFolder = treeNodeOfWeapons.Root.SubFolders.Add("Rifles").Result;
        var pistolsFolder = treeNodeOfWeapons.Root.SubFolders.Add("Pistols").Result;
        var firearmsRiflesFolder = riflesFolder.SubFolders.Add("Firearms").Result;
        var traumaticPistolsFolder = pistolsFolder.SubFolders.Add("Traumatic").Result;

        var akNode = new RifleNode("AK-47");
        var deagleNode = new PistolNode("Desert Eagle");
        var makarovNode = new PistolNode("Makarov");

        registry.Root.Nodes.AddNode(treeNodeOfWeapons);
        registry.Root.Nodes.AddNode(treeNodeOfArmors);
        firearmsRiflesFolder.Nodes.AddNode(akNode);
        pistolsFolder.Nodes.AddNode(deagleNode);
        traumaticPistolsFolder.Nodes.AddNode(makarovNode);

        new TreeNodeView(m_registryContainer, registry);
        new HistoryView(m_historyContainer, registry);
    }

    private void OnMouseDownEvent(MouseDownEvent mouseEvent)
    {
        var visualTarget = (VisualElement)mouseEvent.target;
        if (visualTarget.parent is not Toggle && visualTarget.name != "Modal Window" && visualTarget.parent.name != "Modal Window")
        {
            try
            {
                m_Container.Remove(m_Container.ElementAt(2));
            }
            catch { }
        }
    }
}