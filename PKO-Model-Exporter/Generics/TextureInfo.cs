namespace PKO_Model_Exporter.Generics;

public class TextureInfo
{
    public int Stage { get; set; }
    public int Level { get; set; }
    public int Usage { get; set; }

    public int Format { get; set; }
    public int Pool { get; set; }

    public int ByteAlignmentFlag { get; set; }
    public int Type { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public int ColorKeyType { get; set; }
    public ColorValue4b ColorKey { get; set; }
    public string FileName { get; set; } // 64 bytes long

    public int Data { get; set; }
    public RenderStateAtom[] TssSet { get; set; }
}