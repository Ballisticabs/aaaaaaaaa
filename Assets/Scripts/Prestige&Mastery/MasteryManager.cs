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

public class MasteryManager : MonoBehaviour
{
    public IdleGame game;
    public UpgradesManager upgrades;
    public ResearchManager research;
    public Text masteryText;
    public GameObject masteryMenu;
    public GameObject tempVictoryPopUp;
    public HyperResearchManager hyper;

    public BigDouble superConductorsToGet => 150 * Sqrt(game.data.power / 1e154) + (150 * Sqrt(game.data.power / 1e154) * (.25 * game.data.sacraficeULevel2));

    

    public void Run()
    {
        var data = game.data;

        if (data.power >= 1e154)
            masteryMenu.gameObject.SetActive(true);
        else
            masteryMenu.gameObject.SetActive(false);

        if(data.power >= 1.79e308 && !data.isScreenClosed)
            tempVictoryPopUp.gameObject.SetActive(true);

        masteryText.text = $"Mastery +{Methods.NotationMethod(superConductorsToGet, "F2")} Super Conductors";
    }

    public void Mastery()
    {
        var data = game.data;
        if (data.power < 1.79e308) return;

        data.hasMastered = true;

        data.superConductors += superConductorsToGet;

        data.power = 10;
        data.transformers = 0;
        data.productionUpgrade1Level = data.productionUpgrade2Level = data.productionUpgrade3Level = data.productionUpgrade4Level = data.productionUpgrade5Level = data.productionUpgrade6Level
            = data.productionUpgrade7Level = data.productionUpgrade8Level = 0;

        data.isCompleted0 = true;
        data.isCompleted1 = false;
        data.isCompleted2 = false;
        data.isCompleted3 = false;
        data.isCompleted4 = false;
        data.isCompleted5 = false;
        data.isCompleted6 = false;
        data.isCompleted7 = false;

        data.isHyperCompleted0 = true;
        data.isHyperCompleted1 = false;
        data.isHyperCompleted2 = false;
        data.isHyperCompleted3 = false;
        data.isHyperCompleted4 = false;
        data.isHyperCompleted5 = false;
        data.isHyperCompleted6 = false;
        data.isHyperCompleted7 = false;
        data.isHyperCompleted8 = false;
        data.isHyperCompleted9 = false;

        data.researchIndex = 0;
        data.hyperIndex = 0;

        data.currentPollution = 0;

        upgrades.Deactivate();
        research.Activate();
        hyper.ActivateHyper();
    }

    public BigDouble ConductorBoost()
    {
        var data = game.data;
        BigDouble temp = data.superConductors * 0.01;

        return temp + 1;
    }

    public void Close()
    {
        var data = game.data;
        data.isScreenClosed = true;
        tempVictoryPopUp.gameObject.SetActive(false);
    }
}
