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
public class MasteryBranch : MonoBehaviour
{
    public TechTreeManager techTree;

    [Header("Object Stuff")]
    public Text[] masteryBranchText;
    public Image[] masteryBranchIcons;
    public string[] masteryBranchDesc;
    [Header("Numbers")]
    public BigDouble[] masteryBranchLevels;
    public BigDouble[] masteryBranchMaxLevels;
    public BigDouble[] masteryBranchBaseCosts;
    public BigDouble[] masteryBranchCosts;
    public BigDouble[] masteryBranchCostMults;
    public bool[] isMasteryBranchModuleLocked;

    public void StartMastery()
    {
        var data = techTree.game.data;
        masteryBranchText = new Text[2];
        masteryBranchIcons = new Image[2];
        masteryBranchCosts = new BigDouble[2];
        masteryBranchCostMults = new BigDouble[] { 20, 1 };
        masteryBranchBaseCosts = new BigDouble[] { 1e6, 1e75 };
        masteryBranchLevels = new BigDouble[2];
        masteryBranchMaxLevels = new BigDouble[] { 5, 1 };
        masteryBranchDesc = new string[] { $"2x Dyson Sphere Production Cost:{Methods.NotationMethod(masteryBranchCosts[0], "F0")} Super Conductors\nLevel:{Methods.NotationMethod(masteryBranchLevels[0], "F0")}/{Methods.NotationMethod(masteryBranchMaxLevels[0], "F0")}"
            ,$"Convert Sun into Neutron Star 50x Dyson Sphere Production Cost:{Methods.NotationMethod(masteryBranchCosts[1], "F0")} Super Conductors\nLevel:{Methods.NotationMethod(masteryBranchLevels[1], "F0")}/{Methods.NotationMethod(masteryBranchMaxLevels[1], "F0")}" };
    }

    public void UpdateMastery()
    {
        var data = techTree.game.data;
        ArrayManager();
        BoolManager();

        masteryBranchCosts[0] = masteryBranchBaseCosts[0] * Pow(masteryBranchCostMults[0], data.consoleBranch1Level);

        for (int i = 0; i < 2; i++)
        {
            masteryBranchText[i].text = masteryBranchLevels[i] >= masteryBranchMaxLevels[i] ? "MAX" : masteryBranchDesc[i];
            if (masteryBranchLevels[i] > masteryBranchMaxLevels[i])
                masteryBranchLevels[i] = masteryBranchMaxLevels[i];
        }
    }

    public void BoolManager()
    {
        var data = techTree.game.data;
        if (data.isTechTreeUnlocked)
            data.isMasteryBranch1Locked = false;
        else
            data.isMasteryBranch1Locked = true;
        if (masteryBranchLevels[0] > 0)
            data.isMasteryBranch2Locked = false;
        else
            data.isMasteryBranch2Locked = true;
    }

    public void BuyModule(int index)
    {
        if (isMasteryBranchModuleLocked[index]) return;
        if (masteryBranchLevels[index] >= masteryBranchMaxLevels[index]) return;
        var data = techTree.game.data;
        if (data.superConductors >= masteryBranchCosts[index])
        {
            masteryBranchLevels[index]++;
            data.superConductors -= masteryBranchCosts[index];
        }
        NonArrayManager();
    }

    public void ArrayManager()
    {
        var data = techTree.game.data;
        masteryBranchLevels[0] = data.masteryBranch1Level;
        masteryBranchLevels[1] = data.masteryBranch2Level;

        isMasteryBranchModuleLocked[0] = data.isMasteryBranch1Locked;
        isMasteryBranchModuleLocked[1] = data.isMasteryBranch2Locked;
    }

    private void NonArrayManager()
    {
        var data = techTree.game.data;

        data.masteryBranch1Level = masteryBranchLevels[0];
        data.masteryBranch2Level = masteryBranchLevels[1];
    }
}
