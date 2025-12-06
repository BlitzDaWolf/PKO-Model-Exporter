namespace PKO_Model_Exporter.Generics;

public class Vector3
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }

    public static Vector3[] ParseArray(BinaryReader reader)
    {
        byte count = reader.ReadByte();
        return Enumerable.Range(0, count).Select(_=>Parse(reader)).ToArray();
    }
    public static Vector3 Parse(BinaryReader reader)
    {
        return new Vector3
        {
            X = reader.ReadSingle(),
            Y =  reader.ReadSingle(),
            Z = reader.ReadSingle(),
        };
    }
    public override string ToString() =>  $"({X}, {Y}, {Z})";
}