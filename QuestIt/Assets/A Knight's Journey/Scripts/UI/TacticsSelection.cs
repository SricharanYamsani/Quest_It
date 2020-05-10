using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class TacticsSelection : MonoBehaviour
{
    public List<TacticsSelectionUI> tacticSelections = new List<TacticsSelectionUI>();

    public void OpenTacticsSelection()
    {
        LoadTeamTacticsPlayer();
    }

    private void LoadTeamTacticsPlayer()
    {
        List<BattlePlayer> team = BattleManager.Instance.GetTeamPlayers();

        for (int i = 0; i < tacticSelections.Count; i++)
        {
            if (i < team.Count)
            {
                tacticSelections[i].Setup(team[i]);
                tacticSelections[i].gameObject.SetActive(true);
            }
            else
            {
                tacticSelections[i].gameObject.SetActive(false);
            }
        }
    }
}