namespace PKO_Model_Exporter.Generics;

public class Vector4
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
    public float W { get; set; }

    public static Vector4[] ParseArray(BinaryReader reader)
    {
        byte count = reader.ReadByte();
        return Enumerable.Range(0, count).Select(_=>Parse(reader)).ToArray();
    }
    public static Vector4 Parse(BinaryReader reader)
    {
        return new Vector4
        {
            X = reader.ReadSingle(),
            Y = reader.ReadSingle(),
            Z = reader.ReadSingle(),
            W = reader.ReadSingle(),
        };
    }
    
    public override string ToString() =>  $"({X}, {Y}, {Z}, {W})";
}