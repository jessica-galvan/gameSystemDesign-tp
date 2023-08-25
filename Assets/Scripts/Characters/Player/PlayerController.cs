using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerModel))]
public class PlayerController : BaseCharacterController
{
    private Vector2 direction;
    private Vector2 prevDir;
    private Vector2 mousePos;
    private Camera mainCam;

    public PlayerModel Model { get; private set; }

    public override void Initialize()
    {
        base.Initialize();
        Model = GetComponent<PlayerModel>();
        mainCam = Camera.main;
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

        if (GameManager.Instance.Input.Gameplay.Attack.IsPressed())
        {
            mousePos = Mouse.current.position.ReadValue();
            Model.Shoot(mainCam.ScreenToWorldPoint(mousePos));
        }

        Model.ShootingCooldown();
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
