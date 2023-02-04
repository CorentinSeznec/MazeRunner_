using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWallCooldown : WallCooldown
{
    [SerializeField] GameObject skillIcon;
    [SerializeField] Color activeSkill;
    [SerializeField] Color onCooldownSkill;

    void Start()
    {
        lastActivation = Time.fixedTime;
        UIWallReady();
    }

    override public void SetOnCooldown(float coolDown)
    {
        base.SetOnCooldown(coolDown);
        skillIcon.GetComponent<Image>().color = onCooldownSkill;
        Invoke("UIWallReady", coolDown);
    }

    private void UIWallReady()
    {
        skillIcon.GetComponent<Image>().color = activeSkill;
    }
}
