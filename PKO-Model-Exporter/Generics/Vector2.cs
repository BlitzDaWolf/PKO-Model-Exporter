namespace PKO_Model_Exporter.Generics;

public class Vector2
{
    public float X { get; set; }
    public float Y { get; set; }

    public static Vector2[] ParseArray(BinaryReader reader)
    {
        byte count = reader.ReadByte();
        return Enumerable.Range(0, count).Select(_=>Parse(reader)).ToArray();
    }
    public static Vector2 Parse(BinaryReader reader)
    {
        return new Vector2
        {
            X = reader.ReadSingle(),
            Y =  reader.ReadSingle()
        };
    }

    public float[] ToArray() => new[] { X, Y };

    public override string ToString() =>  $"({X}, {Y})";
}