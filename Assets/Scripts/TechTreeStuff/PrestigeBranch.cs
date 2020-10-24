﻿/*
Copyright (c) 2020 MrBacon470

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BreakInfinity;
using static BreakInfinity.BigDouble;
public class PrestigeBranch : MonoBehaviour
{
    public TechTreeManager techTree;

    [Header("Object Stuff")]
    public Text[] prestigeBranchText;
    public Image[] prestigeBranchIcons;
    public string[] prestigeBranchDesc;
    [Header("Numbers")]
    public BigDouble[] prestigeBranchLevels;
    public BigDouble[] prestigeBranchMaxLevels;
    public BigDouble[] prestigeBranchBaseCosts;
    public BigDouble[] prestigeBranchCosts;
    public BigDouble[] prestigeBranchCostMults;
    public bool[] isPrestigeBranchModuleLocked;

    public void StartPrestige()
    {
        var data = techTree.game.data;
        prestigeBranchText = new Text[2];
        prestigeBranchIcons = new Image[2];
        prestigeBranchCosts = new BigDouble[2];
        prestigeBranchCostMults = new BigDouble[] { 20, 1 };
        prestigeBranchBaseCosts = new BigDouble[] { 1e3, 1e45 };
        prestigeBranchLevels = new BigDouble[2];
        prestigeBranchMaxLevels = new BigDouble[] { 10, 1 };
        prestigeBranchDesc = new string[] { $"Infusions Boosted by 1.5x Cost:{Methods.NotationMethod(prestigeBranchCosts[0], "F0")} Transformers\nLevel:{Methods.NotationMethod(prestigeBranchLevels[0], "F0")}/{Methods.NotationMethod(prestigeBranchMaxLevels[0], "F0")}"
            ,$"Infusions Become Sacrafices Cost:{Methods.NotationMethod(prestigeBranchCosts[1], "F0")} Transformers\nLevel:{Methods.NotationMethod(prestigeBranchLevels[1], "F0")}/{Methods.NotationMethod(prestigeBranchMaxLevels[1], "F0")}" };
    }

    public void UpdatePrestige()
    {
        var data = techTree.game.data;
        ArrayManager();
        BoolManager();
        ImageManager();

        prestigeBranchCosts[0] = prestigeBranchBaseCosts[0] * Pow(prestigeBranchCostMults[0], data.prestigeBranch1Level);
        prestigeBranchCosts[1] = prestigeBranchBaseCosts[1] * Pow(prestigeBranchCostMults[1], data.prestigeBranch2Level);

        for (int i = 0; i < 2; i++)
        {
            prestigeBranchText[i].text = prestigeBranchLevels[i] >= prestigeBranchMaxLevels[i] ? "MAX" : prestigeBranchDesc[i];
            if (prestigeBranchLevels[i] > prestigeBranchMaxLevels[i])
                prestigeBranchLevels[i] = prestigeBranchMaxLevels[i];
        }
    }

    public void BoolManager()
    {
        var data = techTree.game.data;
        if (data.isTechTreeUnlocked)
            data.isPrestigeBranch1Locked = false;
        else
            data.isPrestigeBranch1Locked = true;
        if (prestigeBranchLevels[0] > 0)
            data.isPrestigeBranch2Locked = false;
        else
            data.isPrestigeBranch2Locked = true;
    }

    public void BuyModule(int index)
    {
        if (isPrestigeBranchModuleLocked[index]) return;
        if (prestigeBranchLevels[index] >= prestigeBranchMaxLevels[index]) return;
        var data = techTree.game.data;
        if (data.superConductors >= prestigeBranchCosts[index])
        {
            prestigeBranchLevels[index]++;
            data.superConductors -= prestigeBranchCosts[index];
        }
        NonArrayManager();
    }

    public void ImageManager()
    {
        var data = techTree.game.data;
        if (data.isMasteryBranch1Locked)
            prestigeBranchIcons[0].sprite = techTree.lockedIcon;
        else
            prestigeBranchIcons[0].sprite = prestigeBranchLevels[0] >= prestigeBranchMaxLevels[0] ? techTree.maxedIcon : techTree.unlockedIcon;
        if (data.isMasteryBranch2Locked)
            prestigeBranchIcons[1].sprite = techTree.lockedIcon;
        else
            prestigeBranchIcons[1].sprite = prestigeBranchLevels[1] >= prestigeBranchMaxLevels[1] ? techTree.maxedIcon : techTree.unlockedIcon;

    }

    public void ArrayManager()
    {
        var data = techTree.game.data;
        prestigeBranchLevels[0] = data.prestigeBranch1Level;
        prestigeBranchLevels[1] = data.prestigeBranch2Level;

        isPrestigeBranchModuleLocked[0] = data.isPrestigeBranch1Locked;
        isPrestigeBranchModuleLocked[1] = data.isPrestigeBranch2Locked;
    }

    private void NonArrayManager()
    {
        var data = techTree.game.data;

        data.masteryBranch1Level = prestigeBranchLevels[0];
        data.masteryBranch2Level = prestigeBranchLevels[1];
    }
}
