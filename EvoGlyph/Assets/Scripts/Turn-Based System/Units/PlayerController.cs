using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour, IUnitController
{
    PlayerUnit player;

    public InventoryContainer sequencerContainer;
    public GlyphSequencer glyphSequencer;

    public Glyph glyphToActivate;

    public Animator animator;

    private int callCount = 0;
    bool isActionFinished = true;

    void Awake()
    {
        player = GetComponent<PlayerUnit>();
    }
    public void ListenToControllerInput()
    {
        GlyphController.OnCreateGlyph -= ComparePattern;
        GlyphController.OnCreateGlyph += ComparePattern;
    }

    public void StopListenToControllerInput()
    {
        GlyphController.OnCreateGlyph -= ComparePattern;
    }

    public void OnEndTurn()
    {
        //Tutorial has control on end turn
        BattleManager.Instance.Controller.EndPlayerActionPhase();
    }

    public void OnStartTurn()
    {
        isActionFinished = false;
        BattleManager.Instance.ShowActionOptions();
        //adding this for multiple games
        callCount = 0;
    }

    public void ActionPickedCast()
    {
        int manaCount = GameManager.Instance.PlayerData.GetCurrentManaCount();
        if (manaCount <= 0) return;

        BattleManager.Instance.HideActionOptions();
        BattleManager.Instance.glyphBoard.GenerateField();
        BattleManager.Instance.controller.Initialize();

        BattleManager.Instance.controller.CanDrawGlyph(true);
        glyphSequencer.gameObject.SetActive(true);
        glyphSequencer.Initialize();
        ListenToControllerInput();
    }

    public void ActionCastFinished()
    {
        //Clear Glyph Board
        BattleManager.Instance.glyphBoard.ClearField();

        //Clear Glyph Controller
        BattleManager.Instance.controller.GlyphControllerOnEndTurn();
        //reset glyph sequence
        if (!glyphSequencer.SequencerContainer.slots[0].IsEmpty)
        {
            glyphSequencer.EndSequence();
        }
        glyphSequencer.gameObject.SetActive(false);
        StopListenToControllerInput();

        OnActionFinished();
    }

    void OnActionFinished()
    {
        var controller = BattleManager.Instance.Controller;

        if (controller == null) return;
        if (controller.CurrentPhase != BattlePhase.PlayerAction) return;

        player.EndTurn(controller.CurrentPhase);
        isActionFinished = true;
    }
    public void ActionPickedBasicAttack()
    {
        BattleManager.Instance.HideActionOptions();
        player.PerformBasicAttack();
        //Unit target = GetComponent<Unit>().GetTarget();
        //var damageable = target.GetComponent<IDamageable>();

        //int DamageAmount = 5;
        //GameManager.Instance.PlayerData.RefillMana(1); //Gain 1 Mana on attack
        //damageable?.TakeDamage(DamageAmount);
        //Debug.Log("Basic Attack");
        //Debug.Log($"Gain Mana, Total Mana: {GameManager.Instance.PlayerData.GetCurrentManaCount()}");
        OnActionFinished();
    }

    public void ActionPickedDefend()
    {
        //float damageReductionRate = 0.3f;
        BattleManager.Instance.HideActionOptions();
        //HealthComponent health = GetComponent<HealthComponent>();
        //health?.ActivateBarrierAbility(damageReductionRate);

        Unit target = player.GetTarget();
        AIController enemy = target.GetComponent<AIController>();
        if (enemy != null)
        {
            enemy.IsBeingParried = true;
        }
        Debug.Log("Defend");
        OnActionFinished();
    }
    void ComparePattern(bool[] Sequence)
    {
        
        foreach (var glyphs in GameManager.Instance.GlyphDatabase.ExistingGlyphs)
        {
            if (Sequence.SequenceEqual(glyphs.pattern.glyphPattern))
            {
                callCount++;
                Debug.Log($"<color=yellow>Match Found: {glyphs} was formed, #{callCount} this turn</color>");

                ////Has to be unlocked
                //if (GameManager.Instance.PlayerGlyphs.IsUnlocked(glyphs))
                //{
                //    //OnGlyphCast?.Invoke(glyphs);
                //    glyphs.Activate();
                //    return;
                //}

                //No longer locked behind progression
                //glyphs.Activate(this.GetComponent<Unit>());
                //if (!glyphSequencer.gameObject.activeSelf)
                //{
                //    glyphSequencer.gameObject.SetActive(true);
                //}
                if (GameManager.Instance.PlayerData.IsUnlocked(glyphs)) 
                {
                    AddGlyphToSequencer(glyphs);
                }
                return;
            }
        }
    }

    

    public void AddGlyphToSequencer(Glyph glyph)
    {
        if(sequencerContainer == null) return;
        glyphSequencer.AddGlyphToContainer(glyph);
    }

    public void AttackWithGlyph(Glyph glyph)
    {
        glyphToActivate = glyph;
        animator.SetTrigger("OnAttack");
    }
    public void DoAttack()
    {
        Unit caster = this.GetComponent<Unit>();
        if (caster == null) return;
        if (glyphSequencer.gameObject.activeSelf)
        {
            if (glyphToActivate == null)
            {
                glyphSequencer?.EndSequence();
                return;
            }
            glyphToActivate.Activate(caster);
            glyphSequencer?.RegisterSpell(glyphToActivate);
        }
    }   

    private void OnDestroy()
    {
        StopListenToControllerInput();
    }
}
