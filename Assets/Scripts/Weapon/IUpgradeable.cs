public interface IUpgradeable
{
    void UpgradeStat(string statName, float value);
    float GetStat(string statName);
}
