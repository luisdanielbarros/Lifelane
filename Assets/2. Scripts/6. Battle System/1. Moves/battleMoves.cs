public enum battleMoves { None, basicAttack, Goo, Poltergeist }
public class battleMove
{
    //Battle Move
    private battleMoves move;
    public battleMoves Move { get { return move; } }
    //Name
    private string name;
    public string Name { get { return name; } }
    //Type
    private battleMoveTypes type;
    public battleMoveTypes Type { get { return type; } }
    //Range
    private battleMoveRanges range;
    public battleMoveRanges Range { get { return range; } }
    //Contact
    private bool contact;
    public bool Contact { get { return contact; } }
    //Damage
    private int damage;
    public int Damage { get { return damage; } }
    //Accuracy
    private int accuracy;
    public int Accuracy { get { return accuracy; } }
    //Hits
    private int hits;
    public int Hits { get { return hits; } }
    public battleMove(battleMoves _Move, string _Name, battleMoveTypes _Type, battleMoveRanges _Range, bool _Contact, int _Damage, int _Accuracy, int _Hits)
    {
        move = _Move;
        name = _Name;
        type = _Type;
        range = _Range;
        contact = _Contact;
        damage = _Damage;
        accuracy = _Accuracy;
        hits = _Hits;
    }
}
