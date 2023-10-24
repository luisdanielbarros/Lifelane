[System.Serializable]
public class trafficObject
{
    public string prefabURL;
    public float spawnRatio;
    public trafficObject(string _prefabURL, float _spawnRatio)
    {
        prefabURL = _prefabURL;
        spawnRatio = _spawnRatio;
    }
}
