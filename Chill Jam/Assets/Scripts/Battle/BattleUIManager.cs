using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BattleUIManager : MonoBehaviour {
    [SerializeField] private BattleManager battleManager;

    // store character information
    private PlayerBattle player;
    private List<EnemyBattle> enemies = new();
    private CharacterAction selectedAttack;
    private EnemyBattle characterToAttack;

    private InputActions actions;

    private void Awake() {
        actions = new InputActions();
    }

    private void OnEnable() {
        actions.Gameplay.Enable();

        actions.Gameplay.Click.performed += _ => HandleMouseClick();
    }

    private void OnDisable() {
        actions.Gameplay.Click.performed -= _ => HandleMouseClick();

        actions.Gameplay.Disable();
    }

    private void HandleMouseClick()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

        if (hit.collider != null)
        {
            EnemyBattle enemy = hit.collider.GetComponent<EnemyBattle>();
            if (enemy != null)
            {
                characterToAttack = enemy;
                selectedAttack = player.GetAction(0);
                SendAttackMessage();
            }
        }
    }

    private void SendAttackMessage()
    {
        battleManager.SetPlayerAction(characterToAttack, selectedAttack);
    }

    public void SetForBattle(PlayerBattle player, List<EnemyBattle> enemies)
    {
        // ensure everything is reset
        ClearUI();

        // Initialize characters in the fight
        this.player = player;
        this.enemies = enemies;
    }

    public void ActivateForPlayerTurn()
    {
        // reset used variables
        characterToAttack = null;
    }

    public void DefeatedEnemy(EnemyBattle enemy)
    {
        // get the enemy's index in enemy list
        int enemyIndex = enemies.IndexOf(enemy);

        // remove from list
        enemies.Remove(enemy);
    }

    public void ClearUI()
    {
        // reset used variables
        characterToAttack = null;
    }
    
}