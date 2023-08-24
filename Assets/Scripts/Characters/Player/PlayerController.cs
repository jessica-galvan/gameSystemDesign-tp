using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerModel))]
public class PlayerController : BaseCharacterController
{
    [SerializeField, ReadOnly] private Vector2 direction;
    private Vector2 prevDir;
    private bool canShoot = false;
    public PlayerModel Model { get; private set; }

    public override void Initialize()
    {
        base.Initialize();
        Model = GetComponent<PlayerModel>();
    }

    public override void Refresh()
    {
        if (!CanUpdate()) return;

        direction = GameManager.Instance.Input.Gameplay.Movement.ReadValue<Vector2>();

        Model.Move(direction);

        if (prevDir != direction)
        {
            prevDir = direction;
            Model.LookDirection(direction);
        }
    }

    public override bool CanUpdate()
    {
        return base.CanUpdate() || !Model.Alive;
    }

    protected override void AddToUpdate()
    {
        GameManager.Instance.updateManager.fixCustomUpdater.Remove(this);
    }

    protected override void RemoveFromUpdate()
    {
        if (GameManager.HasInstance)
            GameManager.Instance.updateManager.fixCustomUpdater.Remove(this);
    }
}
