using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerModel))]
public class PlayerController : BaseCharacterController<PlayerModel>
{
    private Vector2 prevDir;
    private Vector2 currentDirection;

    public override void Initialize()
    {
        base.Initialize();
        AddToUpdate();
    }

    public override void Refresh()
    {
        if (!CanUpdate()) return;

        currentDirection = GameManager.Instance.Input.Gameplay.Movement.ReadValue<Vector2>();

        if (prevDir != Model.Direction)
        {
            prevDir = Model.Direction;
            Model.LookDirection(prevDir);
        }

        if (GameManager.Instance.Input.Gameplay.Attack.IsPressed())
            Model.Shoot(GameManager.Instance.cameraController.MouseWorldPos());

        Model.ShootingCooldown();
    }

    public override void FixedRefresh()
    {
        Model.Move(currentDirection);
    }

    public override bool CanUpdate()
    {
        return base.CanUpdate() || !Model.Alive;
    }

    protected override void AddToUpdate()
    {
        GameManager.Instance.updateManager.uncappedCustomUpdate.Add(this);
        GameManager.Instance.updateManager.AddFixedUpdate(this);
    }

    protected override void RemoveFromUpdate()
    {
        if (GameManager.HasInstance)
        {
            GameManager.Instance.updateManager.uncappedCustomUpdate.Remove(this);
            GameManager.Instance.updateManager.RemoveFixedUpdate(this);
        }
    }

    private void OnDestroy()
    {
        RemoveFromUpdate();
    }
}
