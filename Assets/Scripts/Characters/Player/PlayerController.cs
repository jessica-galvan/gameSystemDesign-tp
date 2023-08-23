using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerModel))]
public class PlayerController : BaseCharacterController
{
    [SerializeField, ReadOnly] private Vector2 direction;
    private PlayerModel model;
    private Vector2 prevDir;
    private bool canShoot = false;

    public override void Initialize()
    {
        GameManager.Instance.updateManager.fixCustomUpdater.Remove(this);
    }

    public override void Refresh()
    {
        if (!CanUpdate()) return;

        direction = GameManager.Instance.Input.Gameplay.Movement.ReadValue<Vector2>();

        model.Move(direction);

        if (prevDir != direction)
        {
            prevDir = direction;
            model.LookDirection(direction);
        }
    }

    public override bool CanUpdate()
    {
        return base.CanUpdate() || !model.Alive;
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
