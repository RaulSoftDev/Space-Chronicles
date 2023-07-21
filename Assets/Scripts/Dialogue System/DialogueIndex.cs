using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueIndex : Singleton<DialogueIndex>
{
    public enum Dialogue
    {
        System_1_Intro = 0,
        System_1_Outtro = 1,
        Tutorial_Intro = 2,
        Tutorial_Movement = 3,
        Tutorial_Attack = 4,
        Tutorial_Defence = 5,
        Tutorial_Rocket = 6,
        Tutorial_End = 7,
        Unlock_Difficulty = 8,
    }

    public Dialogue dialogue;

    public List<string> DialogueOutput = new List<string>();

    public void SetDialogue(Dialogue dialogue)
    {
        //Clear old dialogues on output
        DialogueOutput.Clear();

        //Set new dialogue from index
        switch (dialogue)
        {
            case Dialogue.System_1_Intro:
                Dialogue_System1_Intro();
                break;
            case Dialogue.System_1_Outtro:
                Dialogue_System1_Outtro();
                break;
            case Dialogue.Tutorial_Intro:
                Dialogue_Tutorial_Intro();
                break;
            case Dialogue.Tutorial_Movement:
                Dialogue_Tutorial_Movement();
                break;
            case Dialogue.Tutorial_Attack:
                Dialogue_Tutorial_Attack();
                break;
            case Dialogue.Tutorial_Defence:
                Dialogue_Tutorial_Defence();
                break;
            case Dialogue.Tutorial_Rocket:
                Dialogue_Tutorial_Rocket();
                break;
            case Dialogue.Tutorial_End:
                Dialogue_Tutorial_End();
                break;
            case Dialogue.Unlock_Difficulty:
                Dialogue_Unlock_Difficulty();
                break;
        }
    }

    #region System 1 Dialogues
    public void Dialogue_System1_Intro()
    {
        string System1_Intro =
            "\n" +
            "We've been discovered during our sabotage of the Imperial Blue System’s seclusion center. " +
            "As we manage to escape from there, several fleets of ships are trying to hinder our passage. \n" +
            "\n" +
            "Face them and get some time!";
        DialogueOutput.Add(System1_Intro);
    }

    public void Dialogue_System1_Outtro()
    {
        string System1_Outtro_P1 =
            "\n" +
            "The Empire of Darkness established its recruiting center years ago in the peaceful Blue Solar System, " +
            "threatening its colonies and leaving a trail of destruction in its wake. " +
            "Adrian and his crew engaged the enemy forces in a thrilling space battle, " +
            "holding off waves of Empire ships buying enough time for many of the refugees to escape, " +
            "however not all suffered the same fate.";
        DialogueOutput.Add(System1_Outtro_P1);
        string System1_Outtro_P2 =
            "\n" +
            "Although the victory was costly, we managed to repel the enemy and protect the innocent. " +
            "\n" +
            "\n" +
            "Now we are going to another system from which we have received a transmission from a local population " +
            "subjugated by the Empire to get hold of an important mining operation, we do not know for what purpose...";
        DialogueOutput.Add(System1_Outtro_P2);
    }
    #endregion

    #region Tutorial Dialogues
    public void Dialogue_Tutorial_Intro()
    {
        string Tutorial_Intro_P1 =
            "\n" +
            "Hello again Adrian..." +
            "\nI understand that you want to hone your combat skills, but you should be more than ready by now!" +
            "\nPay attention to the attack and defense systems of our Centauro combat ship, they will help you to fight and protect us from the Shadow, remember that this body does not stand alone. ;)";
        DialogueOutput.Add(Tutorial_Intro_P1);
        string Tutorial_Intro_P2 = 
            "\n" +
            "As your AI guide system, I will not let you down to make sure you become the ultimate captain, although I do not promise anything...";
        DialogueOutput.Add (Tutorial_Intro_P2);
        string Tutorial_Intro_P3 =
            "\n" +
            "Well, enough of wasting time with instruction manuals. Let's make these bits dance!!!";
        DialogueOutput.Add(Tutorial_Intro_P3);
    }

    public void Dialogue_Tutorial_Movement()
    {
        string Tutorial_Movement_P1 =
            "\n" +
            "The most important thing about to not take damage is to NOT take damage. To achieve this we can move our ship horizontally using these side sliders. " +
            "Pay attention, the closer you get to the end of the sliders, the faster we will move." +
            "\n" +
            "\n Give it a try!";

        DialogueOutput.Add(Tutorial_Movement_P1);
    }

    public void Dialogue_Tutorial_Attack()
    {
        string Tutorial_Attack_P1 = 
            "\n" +
            "Attack is the best form of deffence. In our case, smash the middle button to destroy them all. If you are able...";
        DialogueOutput.Add(Tutorial_Attack_P1);
    }

    public void Dialogue_Tutorial_Defence()
    {
        string Tutorial_Defence_P1 = "\n" +
            "If you've been paying attention, you may have noticed that when shooting at the enemy the shield gauge starts to increase gradually. " +
            "When it is complete, the button will light up and our protection will be available. But be careful, it only lasts a few moments. " +
            "Although you might already be familiar with that...";
        DialogueOutput.Add(Tutorial_Defence_P1);
    }

    public void Dialogue_Tutorial_Rocket()
    {
        string Tutorial_Rocket_P1 =
            "\n" +
            "As with the shield, Rocket's gauge will also increase. You will have a maximum of three shots. Do not fail them." +
            "\n" +
            "\n" +
            "Let me put more enemies to try it out.";
        DialogueOutput.Add(Tutorial_Rocket_P1);
    }

    public void Dialogue_Tutorial_End()
    {
        string Tutorial_End =
            "\n" +
            "Now that you've learned everything I could teach you, there's only one last lesson left. " +
            "\n" +
            "\n" +
            "Survive. " +
            "\n" +
            "Good luck out there.";
        DialogueOutput.Add (Tutorial_End);
    }
    #endregion

    #region Unlock Difficulty
    public void Dialogue_Unlock_Difficulty()
    {
        string Unlock_Difficulty =
            "\n" +
            "\n" +
            "Good job! " +
            "\n" +
            "You have unlocked a new difficulty. Keep smashing does bastards from the Empire!";
        DialogueOutput.Add(Unlock_Difficulty);
    }
    #endregion
}
