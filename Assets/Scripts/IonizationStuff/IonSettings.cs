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

using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using BreakInfinity;
using static BreakInfinity.BigDouble;
using System;
public class IonSettings : MonoBehaviour
{
    public IdleGame game;
    public Text notationTypeText;
    public Text fpsText;
    public Text audioText;
    public GameObject backgroundMusicSource;
    /*
     * Notation Key:
     * 0 = Sci
     * 1 = Eng
     * 2 = Letter
     * 3 = Word
     */

    public void StartSettings()
    {
        UpdateNotationText();
        UpdateFPSText();
        UpdateAudio();
    }

    private void UpdateNotationText()
    {
        var note = game.data.notationType;
        switch (note)
        {
            case 0:
                notationTypeText.text = "Notation:Scientific";
                break;
            case 1:
                notationTypeText.text = "Notation:Engineering";
                break;
            case 2:
                notationTypeText.text = "Notation:Word";
                break;
            case 3:
                notationTypeText.text = "Notation:Letter";
                break;
            case 4:
                notationTypeText.text = "Notation:Cancer";
                break;
        }
    }

    private void UpdateFPSText()
    {
        var note = game.data.frameRateType;
        switch (note)
        {
            case 0:
                fpsText.text = "FPS:60";
                break;
            case 1:
                fpsText.text = "FPS:30";
                break;
            case 2:
                fpsText.text = "FPS:15";
                break;
        }
    }

    private void UpdateAudio()
    {
        var note = game.data.audioType;
        switch (note)
        {
            case 0:
                audioText.text = "Audio:On";
                backgroundMusicSource.gameObject.SetActive(true);
                break;
            case 1:
                audioText.text = "Audio:Off";
                backgroundMusicSource.gameObject.SetActive(false);
                break;
        }
    }

    public void ChangeNotation()
    {
        var note = game.data.notationType;
        if (note == 4) 
            note = -1;
        note++;
        /* switch (note)
        {
            case 0:
                note = 1;
                break;
            case 1:
                note = 2;
                break;
            case 2:
                note = 0;
                break;
        } */
        game.data.notationType = note;
        Methods.NotationSettings = note;
        UpdateNotationText();
    }

    public void ChangeFPS()
    {
        var note = game.data.frameRateType;
        if (note == 2)
            note = -1;
        note++;
        /* switch (note)
        {
            case 0:
                note = 1;
                break;
            case 1:
                note = 2;
                break;
            case 2:
                note = 0;
                break;
        } */
        game.data.frameRateType = note;
        UpdateFPSText();
    }

    public void ChangeAudio()
    {
        var note = game.data.audioType;
        Debug.Log($"Current Note Amount: {game.data.audioType}");
        switch (note)
        {
            case 0:
                note = 1;
                break;
            case 1:
                note = 0;
                break;
        } 
        game.data.audioType = note;
        UpdateAudio();
    }
}
