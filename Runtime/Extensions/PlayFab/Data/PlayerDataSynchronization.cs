#if ENABLE_PLAYFABSERVER_API && !DISABLE_PLAYFAB_STATIC_API

public class PlayerDataSynchronization : PersistentMonoSingleton<PlayerDataSynchronization>
{
    private Dictionary<string, string> _playerDataToSynchronize = new();
    private bool _isSynchronizing = false;
    private const float _SYNCHRONIZE_DELAY = 0.5f;

    private override void Awake()
    {
        base.Awake();
        PlayerDataManager.Instance.Changed += OnChanged;
    }

    private void OnChanged(KeyValuePair<string, string> data)
    {
        if (_playerDataToSynchronize.ContainsKey(data.Key))
            _playerDataToSynchronize[data.Key] = data.Value;
        else
            _playerDataToSynchronize.Add(data.Key, data.Value);

        Changed?.Invoke();

        CancelInvoke(nameof(Synchronize));
        Invoke(nameof(Synchronize), _SYNCHRONIZE_DELAY);
    }

    private void Synchronize()
    {
        if (_isSynchronizing)
            return;

        _isSynchronizing = true;
        ServerPlayFabHandler.UpdateUserData(ClientPlayFabHandler.PlayFabId, _playerDataToSynchronize, (success) =>
        {
            _isSynchronizing = false;
            _playerDataToSynchronize.Clear();
        }, (error) => _isSynchronizing = false);
    }
}
#endif