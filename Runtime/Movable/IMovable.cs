using UnityEngine;

public interface IMovable
{
    public float MovementSpeed { get; }
    public Vector2 MoveDirection { get; }
    public Transform Transform { get; }

    public abstract void AddForce(Vector3 force);
    public abstract void MovePosition(Vector2 position);
    public abstract void MoveRotation(Quaternion rotation);
    public abstract void Stop();
}