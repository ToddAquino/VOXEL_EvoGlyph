using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusEffectComponent : MonoBehaviour
{
    public List<StatusEffectData> ActiveStatuses = new List<StatusEffectData>();

    [Header("UI")]
    public Transform statusIconsContainer;
    public GameObject statusIconPrefab;
    public List<GameObject> activeStatusIcons = new List<GameObject>();

    public void ApplyStatusElement(StatusEffectData status)
    {
        if (ActiveStatuses.Contains(status))
        {
            Debug.Log("No Status Effect Added");
            return;
        }
        ApplyStatus(status);
        HandleStatusSynergies(status);
    }

    //======== Status UI
    public void AddStatusUI(StatusEffectData status)
    {
        GameObject icon = Instantiate(statusIconPrefab, statusIconsContainer);
        icon.GetComponent<Image>().sprite = status.Icon;
        activeStatusIcons.Add(icon);
    }

    public void RemoveStatusUI(StatusEffectData status)
    {
        for (int i = 0; i < activeStatusIcons.Count; i++)
        {
            var icon = activeStatusIcons[i];

            if (icon.GetComponent<Image>().sprite == status.Icon)
            {
                Destroy(icon);
                activeStatusIcons.RemoveAt(i);
                return;
            }
        }
    }
    //=======================

    //======= Handle Synergies
    public void HandleStatusSynergies(StatusEffectData appliedStatus)
    {
        var synergy = GameManager.Instance.ElementalSynergyDatabase.GetMatchingSynergy(ActiveStatuses, appliedStatus);
        if (synergy == null) return;
        RemoveStatus(synergy.A);
        RemoveStatus(synergy.B);
        ApplyStatus(synergy.Result);
    }

    //======= Add and Remove Status Effects
    public void ApplyStatus(StatusEffectData status)
    {
        ActiveStatuses.Add(status);
        AddStatusUI(status);
    }
    public void RemoveStatus(StatusEffectData status)
    {
        ActiveStatuses.Remove(status);
        RemoveStatusUI(status);
    }
    //=========================

    //Derived Status Element Effects
    //public void ApplyDerivedStatusEffect(BaseStatus type)
    //{
    //    ActiveBaseStatusEffects.Add(type);
    //}
    //public void RemoveDerivedStatusEffecct(BaseStatus type)
    //{
    //    ActiveBaseStatusEffects.Remove(type);
    //}

    ////Handle Combinations of Base Status Elements to form Derived Status Effect
    //private void HandleStatusSynergies(BaseStatus appliedElement)
    //{
    //    DerivedStatus newStatus = GameManager.Instance.ElementalSynergyDatabase.TryGetSynergyResult(appliedElement, ActiveBaseStatusEffects);
    //    if (newStatus != DerivedStatus.None)
    //    {
    //        ActiveDerivedStatusEffects.Add(newStatus);
    //        RemoveBaseStatusesForSynergy(appliedElement, newStatus);
    //    }
    //}

    //private void RemoveBaseStatusesForSynergy(BaseStatus appliedElement, DerivedStatus derived)
    //{
    //    ElementalSynergy synergy = GameManager.Instance.ElementalSynergyDatabase.Synergies.Find(s => s.Result == derived &&(s.A == appliedElement || s.B == appliedElement));

    //    if (synergy == null) return;

    //    ActiveBaseStatusEffects.Remove(synergy.A);
    //    ActiveBaseStatusEffects.Remove(synergy.B);
    //}
}

