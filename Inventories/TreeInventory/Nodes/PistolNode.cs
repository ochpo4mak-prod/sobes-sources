using System;

public class PistolNode : IWeaponNode
{
    public PistolNode(string name)
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
    }

    public string Id { get; }
    public string Name { get; set; }
}