using PKO_Model_Exporter.Model;

namespace PKO_Model_Exporter.Converters;

public static class LGOConverter
{
    public static OBJ ToOBJ(this LGO lgo)
    {
        OBJ obj = new OBJ();

        if (lgo.MeshInfo is null) throw new Exception("No mesh found");

        obj.UV = lgo.MeshInfo.TextureCord0
            .SelectMany(x => new float[] { (float)Math.Round(x.X, 5), (float)Math.Round(x.Y, 5) }).ToList();
        
        obj.Verts = lgo.MeshInfo.VertexSeq.SelectMany(x => new float[]{x.X, x.Y, x.Z}).ToList();
        obj.Normals = lgo.MeshInfo.NormalSeq.SelectMany(x => new float[]{x.X, x.Y, x.Z}).ToList();
        obj.Indecies = lgo.MeshInfo.IndexSeq.Select(x => x+1).ToList();
        return obj;
    }
}