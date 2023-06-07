using System;

public class RifleNode : IWeaponNode
{
    public RifleNode(string name)
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
    }

    public string Id { get; }
    public string Name { get; set; }
}
