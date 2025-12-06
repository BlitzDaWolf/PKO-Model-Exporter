namespace PKO_Model_Exporter.Generics;

public class ColorValue4b
{
    public byte R { get; set; }
    public byte G { get; set; }
    public byte B { get; set; }
    public byte A { get; set; }

    public static ColorValue4b Parse(BinaryReader reader)
    {
        return new ColorValue4b
        {
            R = reader.ReadByte(),
            G = reader.ReadByte(),
            B = reader.ReadByte(),
            A = reader.ReadByte()
        };
    }
}