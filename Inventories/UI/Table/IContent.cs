namespace Assets.Scripts.Table
{
    public interface IContent
    {
        ENodeContent Content { get; }
    }

    public enum ENodeContent
    {
        Default,
        Health,
        Grenade,
        Pistol,
        Rifle,
        RocketLauncher
    }
}
