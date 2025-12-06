namespace PKO_Model_Exporter.Generics;

public class MeshInfo
{
    public MeshInfoHeader Header { get; set; }
    public VertexElement[] VertexElementSeq { get; set; }
    public Vector3[] VertexSeq { get; set; }
    public Vector3[] NormalSeq { get; set; }
    
    public Vector2[] TextureCord0 { get; set; }
    public Vector2[] TextureCord1 { get; set; }
    public Vector2[] TextureCord2 { get; set; }
    public Vector2[] TextureCord3 { get; set; }
    
    public int[] VerColor { get; set; }
    public BlendInfo[] BlendSeq { get; set; }
    public int[] BlendIndexSeq { get; set; }
    public int[] IndexSeq { get; set; }
}