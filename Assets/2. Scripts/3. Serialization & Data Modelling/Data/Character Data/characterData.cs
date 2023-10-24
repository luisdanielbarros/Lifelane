using UnityEngine;
public class characterData
{
    //Name
    private string name;
    public string Name { get { return name; } }

    //Image Profile
    private Sprite imageprofile;
    public Sprite imageProfile { get { return imageprofile; } }

    //Gender
    private string gender;
    public string Gender { get { return gender; } }

    //Age
    private string age;
    public string Age { get { return age; } }

    //Personality
    private string personality;
    public string Personality { get { return personality; } }

    //Model
    private modelSetData model;
    public modelSetData Model { get { return model; } }

    public characterData(string _Name, Sprite _imageProfile, string _Gender, string _Age, string _Personality, modelSetData _Model)
    {
        name = _Name;
        imageprofile = _imageProfile;
        gender = _Gender;
        age = _Age;
        personality = _Personality;
        model = _Model;
    }
}
