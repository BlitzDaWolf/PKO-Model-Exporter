namespace PKO_Model_Exporter.Generics;

public class Matrix43
{
    public static Matrix43[] ParseArray(BinaryReader reader)
    {
        byte count = reader.ReadByte();
        return Enumerable.Range(0, count).Select(_=>Parse(reader)).ToArray();
    }
    public static Matrix43 Parse(BinaryReader reader)
    {
        Matrix43 m = new Matrix43();

        int i = 0;
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                m.m[x,y] = reader.ReadSingle();
            }
        }
                
        return m;
    }
    public float[,] m { get; set; } = new float[4, 3];
}