using UnityEngine;

public class modelData
{
    //Id
    private string modelid;
    public string modelId { get { return modelid; } }
    //Name
    private string name;
    public string Name { get { return name; } }
    //Type
    private modelDataType type;
    public modelDataType Type { get { return type; } }
    //Path
    private string path;
    public string Path { get { return path; } }
    //Materials
    ////Material 1
    private modelDataMaterial material1;
    public modelDataMaterial Material1 { get { return material1; } }
    ////Material 2
    private modelDataMaterial material2;
    public modelDataMaterial Material2 { get { return material2; } }
    ////Material 3
    private modelDataMaterial material3;
    public modelDataMaterial Material3 { get { return material3; } }
    //Transform
    ////Position
    private Vector3 modelposition;
    public Vector3 modelPosition { get { return modelposition; } }
    ////Rotation
    private Quaternion modelrotation;
    public Quaternion modelRotation { get { return modelrotation; } }
    ////Scale
    private Vector3 modelscale;
    public Vector3 modelScale { get { return modelscale; } set { modelscale = value; } }
    public modelData(string _modelId, string _Name, modelDataType _Type, string _Path, modelDataMaterial[] _Materials)
    {
        modelid = _modelId;
        name = _Name;
        type = _Type;
        path = _Path;
        material1 = _Materials[0];
        material2 = _Materials[1];
        material3 = _Materials[2];
        modelposition = new Vector3(0, 0, 0);
        modelrotation = Quaternion.AngleAxis(0, Vector3.right);
        modelscale = new Vector3(1, 1, 1);
    }
}
