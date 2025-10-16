using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutroCutsceneController : MonoBehaviour
{
    public bool isTimeToChooseDialogue = false;
    public bool chooseDialogue1 = false;
    public bool chooseDialogue2 = false;
    void Update()
    {
        if (isTimeToChooseDialogue)
        {
            if (chooseDialogue1)
            {
                // Play dialogue 1
            }
            else if (chooseDialogue2)
            {
                // Play dialogue 2
            }
        }
    }
}
