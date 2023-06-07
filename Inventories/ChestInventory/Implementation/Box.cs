using System;

public class Box : IBox
{
    public Box(EChestType type, ChestContent[] chestContents)
    {
        Id = Guid.NewGuid().ToString();
        Type = type;
        Contents = chestContents;
    }

    public string Id { get; }
    public EChestType Type { get; }
    public ChestContent[] Contents { get; }
}