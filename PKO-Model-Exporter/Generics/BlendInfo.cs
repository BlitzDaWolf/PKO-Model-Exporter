namespace PKO_Model_Exporter.Generics;

public class BlendInfo
{
    public byte[] Index { get; set; }
    public int IndexD { get; set; }
    public Vector3 Weight { get; set; }

    public override string ToString() => $"[{string.Join(",", Index)}] => {Weight}";
}