public interface IBox
{
    string Id { get; }
    EChestType Type { get; }
    ChestContent[] Contents { get; }
}