namespace TransparentGames.Essentials.Data
{
    public interface IDataSaveManager
    {
        public IDataProperty<int> GetProperty(string key, int defaultValue);
        public IDataProperty<bool> GetProperty(string key, bool defaultValue);
        public IDataProperty<float> GetProperty(string key, float defaultValue);
    }
}