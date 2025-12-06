namespace PKO_Model_Exporter.Generics;

public class MeshInfoHeader
{
    public int Fvf { get; set; }
    public int PrimitiveType { get; set; }

    public int VertexCount { get; set; }

    public int IndexCount { get; set; }

    public int SubsetCount { get; set; }

    public int BoneIndexCount { get; set; }

    public int BoneInflFactor { get; set; }

    public int VertexElementCount { get; set; }
    public RenderStateAtom[] rsSet { get; set; }
}