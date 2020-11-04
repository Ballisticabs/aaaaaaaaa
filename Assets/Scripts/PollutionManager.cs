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
using BreakInfinity;
using static BreakInfinity.BigDouble;
using UnityEngine.UI;
using System;

public class PollutionManager : MonoBehaviour
{
    public IdleGame game;

    public Text pollutionText;

    public BigDouble[] pollutionAmount;
    public BigDouble totalPollution => game.data.isChallenge2Active ? 409.8e6 : 409.8e6 + (409.8e6 * (.25 * game.data.infusionULevel2));

    public BigDouble pollutionBoost => game.data.currentPollution / totalPollution;
    
    public float tickTimer;

    private void Start()
    {
        pollutionAmount = new BigDouble[] { 1e3, 5e3, 1e4, 1e5, 5e5 };
    }

    public void Run()
    {
        var data = game.data;

        if (data.currentPollution >= totalPollution)
            data.currentPollution = totalPollution;

        pollutionText.text = $"Total Pollution: {Methods.NotationMethod((data.currentPollution / totalPollution) * 100, "F2")}%\n+{Methods.NotationMethod(pollutionPerSec(),"F0")} Pollution/s";
        if(tickTimer >= 2)
            data.currentPollution += pollutionPerSec();


        if (tickTimer < 2)
            tickTimer += Time.deltaTime;
        else
            tickTimer = 0;
    }

    public BigDouble pollutionPerSec()
    {
        var data = game.data;
        BigDouble temp = 0;
        if (!data.isGen2Broken)
            temp += pollutionAmount[0] * data.productionUpgrade2Level;
        if (!data.isGen3Broken)
            temp += pollutionAmount[1] * data.productionUpgrade3Level;
        if (!data.isGen4Broken)
            temp += pollutionAmount[2] * data.productionUpgrade4Level;
        if (!data.isGen5Broken)
            temp += pollutionAmount[3] * data.productionUpgrade5Level;
        if (!data.isGen7Broken)
            temp += pollutionAmount[4] * data.productionUpgrade7Level;
        return temp;
    }
}
