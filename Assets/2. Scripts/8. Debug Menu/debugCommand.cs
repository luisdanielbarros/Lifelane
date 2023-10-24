using System;
//Debug Command Base
public class debugCommandBase
{
    //Id
    private string id;
    public string Id { get { return id; } }
    //Description
    private string description;
    public string Description { get { return description; } }
    //Format
    private string format { get; }
    public string Format { get { return format; } }
    //Constructor
    public debugCommandBase(string _Id, string _Description, string _Format)
    {
        id = _Id;
        description = _Description;
        format = _Format;
    }
}
//Debug Command
public class debugCommand : debugCommandBase
{
    private Action Command;
    public debugCommand(string _Id, string _Description, string _Format, Action _Command) : base(_Id, _Description, _Format)
    {
        Command = _Command;
    }
    public void Invoke()
    {
        Command.Invoke();
    }
}
//Debug Command w/ Generic Type
public class debugCommand<T1> : debugCommandBase
{
    private Action<T1> Command;
    public debugCommand(string _Id, string _Description, string _Format, Action<T1> _Command) : base(_Id, _Description, _Format)
    {
        Command = _Command;
    }
    public void Invoke(T1 value)
    {
        Command.Invoke(value);
    }
}
