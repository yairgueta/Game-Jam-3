using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Object = UnityEngine.Object;

public class UpgradeUI : MonoBehaviour
{
    public WallLevel wallLevel;

    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private Button upgradeBtn;
    public TMP_Text woodAmount;
    public TMP_Text rockAmount;
    public TMP_Text description;
    public Image image;
    private Upgrader upgrader;
    
    
    
    public void WallSelection(object args)
    {
        upgradePanel.SetActive(true);
        GameObject wall = (GameObject) args;
        upgrader = wall.GetComponent<Upgrader>();    // todo: change
        SetLevel(upgrader.GetCurWallLeve());
        if (!upgrader.CheckIfCanUpgrade())
        {
            upgradeBtn.GetComponent<Image>().color = Color.grey;
            upgradeBtn.interactable = false;
        }

    }

    private void ResetPanel()
    {
        upgradeBtn.GetComponent<Image>().color = Color.white;
        upgradeBtn.interactable = true;
        woodAmount.color = Color.white;
        rockAmount.color = Color.white;

    }
    
    private void SetLevel(WallLevel level)
    {
        wallLevel = level;
        SetUp();
    }

    private void SetUp()
    {
        woodAmount.text = wallLevel.requiredWoods.ToString();
        rockAmount.text = wallLevel.requiredRocks.ToString();
        description.text = wallLevel.description;
        image.sprite = wallLevel.sprite;
        ResetPanel();
    }

    public void WoodLack()
    {
        woodAmount.color = Color.red;
    }
    
    public void RockLack()
    {
        rockAmount.color = Color.red;
    }

    public void Upgrade()
    {
        upgrader.UpgradeWall();
    }

    public void CloseUpdatePanel()
    {
        upgradePanel.SetActive(false);
    }
}
