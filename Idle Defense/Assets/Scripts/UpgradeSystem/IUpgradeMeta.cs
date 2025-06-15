namespace Assets.Scripts.UpgradeSystem
{
    public interface IUpgradeMeta
    {
        string Type { get; }
        string DisplayName { get; }
        string Description { get; }
    }
}