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
        stats.Initialize();
        AddToUpdate();
    }

    public override void Refresh(float deltaTime)
    {
        if (!CanUpdate()) return;

        currentDirection = GameManager.Instance.Input.Gameplay.Movement.ReadValue<Vector2>();

        if (prevDir != Model.Direction)
        {
            prevDir = Model.Direction;
            Model.LookDirection(prevDir);
        }

        if (GameManager.Instance.Input.Gameplay.AbilityOne.WasPressedThisFrame() && Model.CanUseAbility(0))
            Model.UseAbility(0);

        if (GameManager.Instance.Input.Gameplay.AbilityTwo.WasPressedThisFrame() && Model.CanUseAbility(1))
            Model.UseAbility(1);

        if (GameManager.Instance.Input.Gameplay.AbilityThree.WasPressedThisFrame() && Model.CanUseAbility(2))
            Model.UseAbility(2);

        if (GameManager.Instance.Input.Gameplay.Attack.IsPressed())
            Model.Shoot(GameManager.Instance.cameraController.MouseWorldPos());

        Model.RefreshAbilities();
        Model.ShootingCooldown();
        Model.Refresh(deltaTime);

        HandelCheatsInput();
    }

    private void HandelCheatsInput()
    {
        if (!GameManager.Instance.playerData.canCheat || !Model.LifeController.Alive) return;

        if (GameManager.Instance.Input.Cheats.AddMana.WasPerformedThisFrame())
            GameManager.Instance.manaSystem.AddMana(GameManager.Instance.manaSystem.maxMana);

        if (GameManager.Instance.Input.Cheats.PlayerInvinsible.WasPerformedThisFrame())
            Model.LifeController.SetInivincibility(!Model.LifeController.Invincible);

        if (GameManager.Instance.Input.Cheats.LevelUp.WasPerformedThisFrame())
            GameManager.Instance.experienceSystem.AddExperience(GameManager.Instance.experienceSystem.GetExpNeedForNextLeve(), false);
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
