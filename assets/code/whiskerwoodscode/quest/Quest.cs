using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public string title;
    public string description;
    public bool isCompleted;
    public int item1Index;
    public int numItem1Collected;
    public int numItem1Used;
    public int item2Index;
    public int numItem2Collected;
    public int numItem2Used;
    public int item3Index;
    public int numItem3Collected;
    public int numItem3Used;

    public Quest(string title, string description, int index1, int collected1, int used1, int index2, int collected2, int used2, int index3, int collected3, int used3)
    {
        this.title = title;
        this.description = description;
        this.isCompleted = false;
        this.item1Index = index1;
        this.numItem1Collected = collected1;
        this.numItem1Used = used1;
        this.item2Index = index2;
        this.numItem2Collected = collected2;
        this.numItem2Used = used2;
        this.item3Index = index3;
        this.numItem3Collected = collected3;
        this.numItem3Used = used3;
    }
}
