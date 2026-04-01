using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.EventSystems.EventTrigger;

public enum ActionPicked
{
    None,
    Cast,
    Attack,
    Defend
}
public class PlayerController : MonoBehaviour, IUnitController
{
    public event Action<ActionPicked> OnActionPicked;
    public event Action OnActionEnded;
    PlayerUnit player;

    [Header("Basic Attack QTE")]
    public BasicAttackQTE qteHandler;
    public GameObject qteObj;

    public InventoryContainer sequencerContainer;
    public GlyphSequencer glyphSequencer;

    public Glyph glyphToActivate;
    private int callCount = 0;
    bool isActionFinished = true;
    public bool isInTutorial = false;

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
        if (isInTutorial)
        {
            OnActionPicked?.Invoke(ActionPicked.Cast);
        }

        int manaCount = GameManager.Instance.PlayerData.GetCurrentManaCount();
        if (manaCount <= 0) return;
        BattleManager.Instance.HideActionOptions();
        BattleManager.Instance.glyphBoard.GenerateField();
        BattleManager.Instance.controller.Initialize();
        if (!isInTutorial)
        {
            BattleManager.Instance.controller.CanDrawGlyph(true);
        }
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
        if (!isInTutorial)
        {
            var controller = BattleManager.Instance.Controller;

            if (controller == null) return;
            if (controller.CurrentPhase != BattlePhase.PlayerAction) return;

            player.EndTurn(controller.CurrentPhase);
        }
        else
        {
            OnActionEnded?.Invoke();
        }
        isActionFinished = true;
    }
    public void ActionPickedBasicAttack()
    {
        if (isInTutorial)
        {
            OnActionPicked?.Invoke(ActionPicked.Attack);
        }
        BattleManager.Instance.HideActionOptions();

        qteObj.SetActive(true);
        qteHandler.StartQuickTimeEvent(isInTutorial);
        qteHandler.OnQTEFinished += HandleQTEResult;
        //Unit target = GetComponent<Unit>().GetTarget();
        //var damageable = target.GetComponent<IDamageable>();

        //int DamageAmount = 5;
        //GameManager.Instance.PlayerData.RefillMana(1); //Gain 1 Mana on attack
        //damageable?.TakeDamage(DamageAmount);
        //Debug.Log("Basic Attack");
        //Debug.Log($"Gain Mana, Total Mana: {GameManager.Instance.PlayerData.GetCurrentManaCount()}");
    }
    void HandleQTEResult(QuickTimeEventResult result)
    {
        qteHandler.OnQTEFinished -= HandleQTEResult;
        player.animator.SetTrigger("OnAttack");
        player.PerformBasicAttack();
        switch (result)
        {
            case QuickTimeEventResult.Success:
                player.GainMana(1);
                break;

            default:
                break;
        }
        qteObj.SetActive(false);

        if (!isInTutorial)
            OnActionFinished();
    }

    public void ActionPickedDefend()
    {
        if (isInTutorial)
        {
            OnActionPicked?.Invoke(ActionPicked.Defend);
        }
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
        if(!isInTutorial)
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
        player.animator.SetTrigger("OnCast");
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
