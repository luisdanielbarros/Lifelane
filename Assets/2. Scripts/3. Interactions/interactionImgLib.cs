[System.Serializable]
public enum interactionImgLibEntry
{
    //None
    None,
    //Story
    ////Introduction
    Story1PG1, Story1PG2, Story1PG3, Story1PG4, Story1PG5, Story1PG6, Story1PG7, Story1PG8, Story1PG9, Story1PG10, Story1PG11, Story1PG12,
    //Characters
    ////Samuel
    CharSamNeutral, CharSamNeutral2, CharSamSpeaking, CharSamSpeaking2, CharSamThinking, CharSamThinking2,
    CharSamPsychologicalPain, CharSamPsychologicalPain2, CharSamPhysicalPain, CharSamPhysicalPain2, CharSamApology, CharSamApology2,
    CharSamGettingUp, CharSamGettingUp2, CharSamConfused, CharSamConfused2,
    ////Marie
    CharMarieNeutral, CharMarieSpeaking, CharMarieThinking, CharMariePain
}
public static class interactionImgLib
{
    public static string getEntry(interactionImgLibEntry Entry)
    {
        switch (Entry)
        {
            //None
            case interactionImgLibEntry.None:
                return "Sprites/Interaction Images/None";
            //Story
            ////Introduction
            case interactionImgLibEntry.Story1PG1:
                return "Sprites/Interaction Images/Story/1, Introduction/1";
            case interactionImgLibEntry.Story1PG2:
                return "Sprites/Interaction Images/Story/1, Introduction/2";
            case interactionImgLibEntry.Story1PG3:
                return "Sprites/Interaction Images/Story/1, Introduction/3";
            case interactionImgLibEntry.Story1PG4:
                return "Sprites/Interaction Images/Story/1, Introduction/4";
            case interactionImgLibEntry.Story1PG5:
                return "Sprites/Interaction Images/Story/1, Introduction/5";
            case interactionImgLibEntry.Story1PG6:
                return "Sprites/Interaction Images/Story/1, Introduction/6";
            case interactionImgLibEntry.Story1PG7:
                return "Sprites/Interaction Images/Story/1, Introduction/7";
            case interactionImgLibEntry.Story1PG8:
                return "Sprites/Interaction Images/Story/1, Introduction/8";
            case interactionImgLibEntry.Story1PG9:
                return "Sprites/Interaction Images/Story/1, Introduction/9";
            case interactionImgLibEntry.Story1PG10:
                return "Sprites/Interaction Images/Story/1, Introduction/10";
            case interactionImgLibEntry.Story1PG11:
                return "Sprites/Interaction Images/Story/1, Introduction/11";
            case interactionImgLibEntry.Story1PG12:
                return "Sprites/Interaction Images/Story/1, Introduction/12";
            //Characters
            ////Samuel
            case interactionImgLibEntry.CharSamNeutral:
                return "Sprites/Interaction Images/Sam/Neutral";
            case interactionImgLibEntry.CharSamNeutral2:
                return "Sprites/Interaction Images/Sam/Neutral, 2";
            case interactionImgLibEntry.CharSamSpeaking:
                return "Sprites/Interaction Images/Sam/Speaking";
            case interactionImgLibEntry.CharSamSpeaking2:
                return "Sprites/Interaction Images/Sam/Speaking, 2";
            case interactionImgLibEntry.CharSamThinking:
                return "Sprites/Interaction Images/Sam/Thinking";
            case interactionImgLibEntry.CharSamThinking2:
                return "Sprites/Interaction Images/Sam/Thinking, 2";
            case interactionImgLibEntry.CharSamPsychologicalPain:
                return "Sprites/Interaction Images/Sam/Psychological Pain";
            case interactionImgLibEntry.CharSamPsychologicalPain2:
                return "Sprites/Interaction Images/Sam/Psychological Pain, 2";
            case interactionImgLibEntry.CharSamPhysicalPain:
                return "Sprites/Interaction Images/Sam/Physical Pain";
            case interactionImgLibEntry.CharSamPhysicalPain2:
                return "Sprites/Interaction Images/Sam/Physical Pain, 2";
            case interactionImgLibEntry.CharSamApology:
                return "Sprites/Interaction Images/Sam/Apology";
            case interactionImgLibEntry.CharSamApology2:
                return "Sprites/Interaction Images/Sam/Apology, 2";
            case interactionImgLibEntry.CharSamGettingUp:
                return "Sprites/Interaction Images/Sam/Getting Up";
            case interactionImgLibEntry.CharSamGettingUp2:
                return "Sprites/Interaction Images/Sam/Getting Up, 2";
            case interactionImgLibEntry.CharSamConfused:
                return "Sprites/Interaction Images/Sam/Confused";
            case interactionImgLibEntry.CharSamConfused2:
                return "Sprites/Interaction Images/Sam/Confused, 2";
            ////Marie
            case interactionImgLibEntry.CharMarieNeutral:
                return "Sprites/Interaction Images/Marie/Neutral";
            case interactionImgLibEntry.CharMarieSpeaking:
                return "Sprites/Interaction Images/Marie/Speaking";
            case interactionImgLibEntry.CharMarieThinking:
                return "Sprites/Interaction Images/Marie/Thinking";
            case interactionImgLibEntry.CharMariePain:
                return "Sprites/Interaction Images/Marie/Pain";

            default:
                return "Sprites/Interaction Images/None";
        }
    }
}
