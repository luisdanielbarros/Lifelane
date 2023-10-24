public class battleSimulation
{
    //Died
    private bool died;
    public bool Died { get { return died; } }
    //Experience Earned
    private int experienceearned;
    public int experienceEarned { get { return experienceearned; } }
    //Ran
    private bool ran;
    public bool Ran { get { return ran; } }

    //Constructor
    public battleSimulation(bool _Died, int _experienceEarned)
    {
        died = _Died;
        experienceearned = _experienceEarned;
        ran = false;
    }
    public battleSimulation(bool _Ran)
    {
        died = false;
        experienceearned = 0;
        ran = _Ran;
    }
}
