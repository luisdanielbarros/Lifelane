public class battleSimulator
{
    //Random
    private readonly System.Random Rnd = new System.Random();
    //Constructor
    public battleSimulator()
    {
    }
    public battleSimulation Simulate(ref battleStats _AttackerStats, ref battleEntity _Target, battleMove _Attack)
    {
        //Unwrapping Variables
        battleStats _TargetStats = _Target.Stats;

        //Calculation Variables
        ////Hit or Miss
        int attackAccuracy = _Attack.Accuracy;
        //////Attacker Intuition
        int attackerIntuition = (int)(1 + (_AttackerStats.Intuition * 0.25f));
        //////Target Intuition
        int targetIntuition = (int)(1 + (_Target.Stats.Intuition * 0.25f));
        //////Calculation
        attackAccuracy += attackerIntuition - targetIntuition;
        int rndAccuracy = Rnd.Next(100);

        ////Damage
        int attackDamage = _Attack.Damage;
        bool Contact = _Attack.Contact;
        //////Attacker Offensive & Target Defensive
        int attackerOffensive;
        int targetDefensive;
        if (Contact)
        {
            attackerOffensive = (int)(_AttackerStats.Strength * 0.5f);
            targetDefensive = (int)(_TargetStats.Toughness * 0.5f);
        }
        else
        {
            attackerOffensive = (int)(_AttackerStats.Channelling * 0.5f);
            targetDefensive = (int)(_AttackerStats.Sensitivity * 0.5f);
        }
        //////Calculation
        attackDamage += attackerOffensive - targetDefensive;


        //Return Variables
        bool Died = false;
        int experienceEarned = 0;

        ////Hit or Miss
        if (rndAccuracy <= attackAccuracy)
        {
            ////Died
            Died = _TargetStats.takeDamage(attackDamage);
            ////Experience Earned
            if (Died) experienceEarned = _Target.OnDefeatExperience;
        }

        //Return
        return new battleSimulation(Died, experienceEarned);
    }
    public battleSimulation SimulateRun(ref battleStats _AttackerStats, ref battleEntity _Target)
    {
        return new battleSimulation(true);
    }
}
