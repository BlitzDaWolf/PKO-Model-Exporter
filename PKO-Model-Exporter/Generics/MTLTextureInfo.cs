namespace PKO_Model_Exporter.Generics;

public class MTLTextureInfo
{
    public float Opacity { get; set; }
    public int Trancerecy { get; set; }
    public Material Mtl { get; set; }
    public RenderStateAtom[] RenderStateSet { get; set; }
    public TextureInfo[] TextureSequence { get; set; }
}