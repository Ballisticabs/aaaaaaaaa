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

public class ChallengeManager : MonoBehaviour
{
    public IdleGame game;
    public UpgradesManager upgrades;
    public ResearchManager research;
    public ChallengeBranch challenge;

    private BigDouble reward1 => 1e6 * Pow(1.5, game.data.challengeLevel1);
    private BigDouble reward2 => 1e9 * Pow(1.75, game.data.challengeLevel2);
    private BigDouble reward3 => 1e12 * Pow(5, game.data.challengeLevel3);

    public BigDouble challengeGoal1 => 1e38 * Pow(1.5, game.data.challengeLevel1);
    public BigDouble challengeGoal2 => 1e38 * Pow(1.75, game.data.challengeLevel2);
    public BigDouble challengeGoal3 => 1e38 * Pow(2.5, game.data.challengeLevel3);


    public Text[] challengeText = new Text[3];
    public GameObject[] challengePopUp = new GameObject[3];
    public Text[] challengePopUpText = new Text[3];

    public BigDouble[] challengeReward;
    public BigDouble[] challengeLevels;


    public void StartChallenges()
    {
        challengeReward = new BigDouble[3];
        challengeLevels = new BigDouble[3];
    }

    public void Run()
    {
        var data = game.data;

        if (data.power >= 1e18 && data.hasPrestiged && data.isConsoleUnlocked)
            data.isChallengesUnlocked = true;

        UI();
        ArrayManager();
        Conditions();

        void UI()
        {
            if(game.challengeCanvas.gameObject.activeSelf)
            {
                if (challenge.challengeBranchLevels[0] > 0)
                    challengeText[0].text = data.isChallenge2Active || data.isChallenge3Active || data.isChallenge1Active || data.isChallenge5Active ? "OTHER CHALLENGE ACTIVE" : $"Challenge: Sterlitzia\nAll Numbers Are replaced with Sterliztia\nGet To{Methods.NotationMethod(challengeGoal1, "F2")} Power\nReward: {Methods.NotationMethod(reward1, "F2")}";
                else
                    challengeText[0].text = data.isChallenge2Active || data.isChallenge3Active || data.isChallenge4Active || data.isChallenge1Active ? "OTHER CHALLENGE ACTIVE" : $"Challenge: Clean Energy\nUse only Manual Generators, Steam Turbines and Fusion Reactors to get to {Methods.NotationMethod(challengeGoal1, "F2")} Power\nReward: {Methods.NotationMethod(challengeReward[0], "F0")} Amps\nCompletions: {Methods.NotationMethod(challengeLevels[0], "F0")}";
                if (challenge.challengeBranchLevels[1] > 0)
                    challengeText[1].text = data.isChallenge1Active || data.isChallenge3Active || data.isChallenge4Active || data.isChallenge5Active ? "OTHER CHALLENGE ACTIVE" : $"Challenge: Red Dwarf\nYour DysonSphere no longer produces power\nGet to{Methods.NotationMethod(challengeGoal2, "F2")} Power\nReward: {Methods.NotationMethod(reward2, "F2")}";
                else
                    challengeText[1].text = data.isChallenge1Active || data.isChallenge3Active || data.isChallenge4Active || data.isChallenge5Active ? "OTHER CHALLENGE ACTIVE" : $"Challenge: No Console\nGet to {Methods.NotationMethod(challengeGoal2, "F2")} Power with no Bytes and No Boost\nReward: {Methods.NotationMethod(challengeReward[1], "F0")} Amps\nCompletions: {Methods.NotationMethod(challengeLevels[1], "F0")}";
                challengeText[2].text = data.isChallenge1Active || data.isChallenge2Active || data.isChallenge4Active || data.isChallenge5Active ? "OTHER CHALLENGE ACTIVE" : $"Challenge: Impossible Mode\nGet to {Methods.NotationMethod(challengeGoal3, "F2")} Power with side effects of Clean Energy and No Console plus No Prestige and Mastery Upgrades\nReward: {Methods.NotationMethod(challengeReward[2], "F0")} Amps\nCompletions: {Methods.NotationMethod(challengeLevels[2], "F0")}";
            }
        }
    }

    public void ChooseChallenge(int id)
    {
        var data = game.data;
        if (data.isChallenge1Active) return;
        if (data.isChallenge2Active) return;
        if (data.isChallenge3Active) return;
        if (data.isChallenge4Active) return;
        if (data.isChallenge5Active) return;
        switch (id)
        {
            case 0:
                if (challenge.challengeBranchLevels[0] <= 0)
                    data.isChallenge1Active = true;
                else
                    data.isChallenge4Active = true;
                StartChallenge();
                break;

            case 1:
                if (challenge.challengeBranchLevels[1] <= 0)
                    data.isChallenge2Active = true;
                else
                    data.isChallenge5Active = true;
                StartChallenge();
                break;

            case 2:
                data.isChallenge3Active = true;
                StartChallenge();
                break;
        }


    }

    public void ExitChallenge()
    {
        var data = game.data;
        data.isChallenge1Active = false;
        data.isChallenge2Active = false;
        data.isChallenge3Active = false;
        data.isChallenge4Active = false;
        data.isChallenge5Active = false;
    }

    public void StartChallenge()
    {
        var data = game.data;
        data.power = 10;
        data.powerCollected = 10;

        data.power = 10;
        data.productionUpgrade1Level = 0;
        data.productionUpgrade2Level = 0;
        data.productionUpgrade3Level = 0;
        data.productionUpgrade4Level = 0;
        data.productionUpgrade5Level = 0;
        data.productionUpgrade6Level = 0;
        data.productionUpgrade7Level = 0;
        data.productionUpgrade8Level = 0;

        data.isCompleted0 = true;
        data.isCompleted1 = false;
        data.isCompleted2 = false;
        data.isCompleted3 = false;
        data.isCompleted4 = false;
        data.isCompleted5 = false;
        data.isCompleted6 = false;
        data.isCompleted7 = false;

        data.researchIndex = 0;

        data.currentPollution = 0;

        upgrades.Deactivate();
        research.Activate();
    }

    public void CompleteChallenge(int index)
    {
        var data = game.data;
        switch(index)
        {
            case 0:
                data.amps += reward1;
                data.isChallenge1Active = false;
                data.challengeLevel1++;
                challengePopUp[0].gameObject.SetActive(true);
                challengePopUpText[0].text = $"CHALLENGE 1 COMPLETED\nREWARD:{Methods.NotationMethod(reward1, "F0")} AMPS";
                break;
            case 1:
                data.amps += reward2;
                data.isChallenge2Active = false;
                data.challengeLevel2++;
                challengePopUp[1].gameObject.SetActive(true);
                challengePopUpText[1].text = $"CHALLENGE 2 COMPLETED\nREWARD:{Methods.NotationMethod(reward2, "F0")} AMPS";
                break;
            case 2:
                data.amps += reward3;
                data.isChallenge3Active = false;
                data.challengeLevel3++;
                challengePopUp[2].gameObject.SetActive(true);
                challengePopUpText[2].text = $"CHALLENGE 3 COMPLETED\nREWARD:{Methods.NotationMethod(reward3, "F0")} AMPS";
                break;
        }
    }

    public void ArrayManager()
    {
        challengeReward[0] = reward1;
        challengeReward[1] = reward2;
        challengeReward[2] = reward3;

        challengeLevels[0] = game.data.challengeLevel1;
        challengeLevels[1] = game.data.challengeLevel2;
        challengeLevels[2] = game.data.challengeLevel3;
    }
    public void Conditions()
    {
        var data = game.data;
        if(data.isChallenge1Active)
            if(data.power >= challengeGoal1)
            {
                CompleteChallenge(0);
            }
        if (data.isChallenge2Active)
            if (data.power >= challengeGoal2)
            {
                CompleteChallenge(1);
            }
        if (data.isChallenge3Active)
            if (data.power >= challengeGoal3)
            {
                CompleteChallenge(2);
            }
    }

    public void ClosePopup()
    {
        challengePopUp[0].gameObject.SetActive(false);
        challengePopUp[1].gameObject.SetActive(false);
        challengePopUp[2].gameObject.SetActive(false);
    }
    public BigDouble QuarkBoost()
    {
        var temp = game.data.amps * 0.1;
        return temp + 1;
    }
}

