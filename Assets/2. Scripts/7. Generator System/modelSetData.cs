public class modelSetData
{
    //Physic
    private modelData physic;
    public modelData Physic { get { return physic; } }
    //Head
    private modelData head;
    public modelData Head { get { return head; } }
    //Eyes
    private modelData eyes;
    public modelData Eyes { get { return eyes; } }
    //Hair
    private modelData hair;
    public modelData Hair { get { return hair; } }
    //Jacket
    private modelData jacket;
    public modelData Jacket { get { return jacket; } }
    //Shirt
    private modelData shirt;
    public modelData Shirt { get { return shirt; } }
    //Pants
    private modelData pants;
    public modelData Pants { get { return pants; } }
    //Socks
    private modelData socks;
    public modelData Socks { get { return socks; } }
    //Shoes
    private modelData shoes;
    public modelData Shoes { get { return shoes; } }
    //Acessories
    private modelData[] acessories;
    public modelData[] Acessories { get { return acessories; } }
    //Head Scale
    private float headscale;
    public float headScale { get { return headscale; } }
    //Body Scale
    private float bodyscale;
    public float bodyScale { get { return bodyscale; } }
    public modelSetData(modelData _Physic, modelData _Head, modelData _Eyes, modelData _Hair, modelData _Jacket, modelData _Shirt, modelData _Pants, modelData _Socks, modelData _Shoes, modelData[] _Acessories, float _headScale, float _bodyScale)
    {
        physic = _Physic;
        head = _Head;
        eyes = _Eyes;
        hair = _Hair;
        jacket = _Jacket;
        shirt = _Shirt;
        pants = _Pants;
        socks = _Socks;
        shoes = _Shoes;
        acessories = _Acessories;
        headscale = _headScale;
        bodyscale = _bodyScale;
        //Apply Scale
        physic.modelScale *= bodyscale;
        head.modelScale *= bodyscale * headscale;
        hair.modelScale *= bodyscale * headscale;
        eyes.modelScale *= bodyscale * headscale;
        jacket.modelScale *= bodyscale;
        shirt.modelScale *= bodyscale;
        pants.modelScale *= bodyscale;
        socks.modelScale *= bodyscale;
        shoes.modelScale *= bodyscale;
        for (int i = 0; i < acessories.Length; i++) acessories[i].modelScale *= bodyscale;
    }
}
