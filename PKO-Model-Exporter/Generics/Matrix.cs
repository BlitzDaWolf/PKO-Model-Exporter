namespace PKO_Model_Exporter.Generics;

public class Matrix
{
    public static Matrix[] ParseArray(BinaryReader reader)
    {
        byte count = reader.ReadByte();
        return Enumerable.Range(0, count).Select(_=>Parse(reader)).ToArray();
    }
            
    public static Matrix Parse(BinaryReader reader)
    {
        Matrix m = new Matrix();

        int i = 0;
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                m.m[x,y] = reader.ReadSingle();
                i++;
            }
        }
                
        return m;
    }

    public float[,] m { get; set; } = new float[4, 4];
}