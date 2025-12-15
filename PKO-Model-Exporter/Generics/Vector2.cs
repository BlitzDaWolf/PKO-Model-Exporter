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
    
    public Vector2()
    {
        X = 0;
        Y = 0;
    }

    public Vector2(float[] points)
    {
        X =  points[0];
        Y =  points[1];
    }
    
    public Vector2(double[] points)
    {
        X = (float)points[0];
        Y = (float)points[1];
    }

    public float[] ToArray() => new[] { X, Y };

    public override string ToString() =>  $"({X}, {Y})";
}