namespace PKO_Model_Exporter.Generics;

public class VertexElement
{
    public short Stream { get; set; }
    public short Offset { get; set; }
    public byte Type { get; set; }
    public byte Method { get; set; }
    public byte Usage { get; set; }
    public byte UsageIndex { get; set; }
}