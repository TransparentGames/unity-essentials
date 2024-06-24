using TransparentGames.Essentials.UpdateManagement;
using UnityEngine;

public class WorldSpaceFollow : MonoBehaviour
{
    [SerializeField] private Vector3 offset;

    private Transform _followTarget;
    private IUpdateEntity _updateEntity;

    private void OnLateUpdate()
    {
        if (_followTarget == null) return;
        if (gameObject.activeSelf == false) return;

        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position,
            _followTarget.position + offset, 0.5f);
        gameObject.transform.forward = Camera.main.transform.forward;
    }

    private void OnDisable()
    {
        UpdateManager.Stop(_updateEntity);
    }

    public void SetTarget(Transform target)
    {
        _updateEntity = UpdateManager.StartUpdate(OnLateUpdate, UpdateType.LateUpdate);

        _followTarget = target;
        gameObject.transform.position = _followTarget.position + offset;
    }
}