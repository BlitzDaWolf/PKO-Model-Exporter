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

    public Vector3()
    {
        X = 0;
        Y = 0;
        Z = 0;
    }

    public Vector3(float[] points)
    {
        X =  points[0];
        Y =  points[1];
        Z =  points[2];
    }
    
    public Vector3(double[] points)
    {
        X = (float)points[0];
        Y = (float)points[1];
        Z = (float)points[2];
    }
    
    public float[] ToArray() => new[] { X, Y, Z };
    public override string ToString() =>  $"({X}, {Y}, {Z})";
}