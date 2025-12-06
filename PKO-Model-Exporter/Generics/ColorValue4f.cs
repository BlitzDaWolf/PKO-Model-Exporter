namespace PKO_Model_Exporter.Generics;

public class ColorValue4f
{
    public float R { get; set; }
    public float G { get; set; }
    public float B { get; set; }
    public float A { get; set; }

    public static ColorValue4f Parse(BinaryReader reader)
    {
        return new ColorValue4f
        {
            R = reader.ReadSingle(),
            G = reader.ReadSingle(),
            B = reader.ReadSingle(),
            A = reader.ReadSingle()
        };
    }
}