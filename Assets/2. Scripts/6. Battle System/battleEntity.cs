public class battleEntity
{
    //Name
    private string name;
    public string Name { get { return name; } }
    //Given Experience
    private int ondefeatexperience;
    public int OnDefeatExperience { get { return ondefeatexperience; } }
    //Stats
    private battleStats stats;
    public battleStats Stats { get { return stats; } }
    //Movepool
    private battleMove[] movepool;
    public battleMove[] Movepool { get { return movepool; } }
    // Start is called before the first frame update
    public battleEntity(string _Name, battleStats _Stats, battleMove[] _Movepool)
    {
        name = _Name;
        stats = _Stats;
        movepool = _Movepool;
        ondefeatexperience = stats.Health * stats.Level;
    }
}
