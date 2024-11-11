namespace TransparentGames.Essentials.Data
{
    public interface IDataSaveManager
    {
        public IDataProperty<T> GetProperty<T>(string key, T defaultValue);
    }
}