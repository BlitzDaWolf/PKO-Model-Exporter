using PKO_Model_Exporter.Generics;
using PKO_Model_Exporter.Helpers;
using PKO_Model_Exporter.Model;
using PKO_Model_Exporter.Model.FBXModels.Elements;
using PKO_Model_Exporter.Model.FBXModels.Elements.Geometry;

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

    public static FBX ToFBX(this LGO lgo)
    {
        var test = lgo.MeshInfo.TextureCord0;

        var seq = lgo.MeshInfo.BlendSeq.Select(x => x.IndexD).Distinct().ToArray();
        
        FBX fbx = FBXCreator.CreateFBX();
        Geometry emptyGeo = FBXCreator.CreateGeometry("A");
        {
            var testIndex = lgo.MeshInfo.IndexSeq;
            var polyVert = emptyGeo.GetObjectType<PolyVertexIndex>()!;
            polyVert.Data[0] = (object)testIndex.ToArray();
            polyVert.Pack();
            
            var edgesArr = Enumerable.Range(0, testIndex.Length / 2)
                .SelectMany(x => new int[] { testIndex[x * 2], testIndex[x * 2 + 1] }).ToArray();
            
            var edges = emptyGeo.GetObjectType<Edges>()!;
            edges.Data[0] = (object)edgesArr.ToArray();
            edges.Pack();

            var uvInx = emptyGeo.GetObjectType<GeometryIndex>();
            uvInx.Data[0] = edgesArr.ToArray();
            
            
            var norInx = emptyGeo.GetObjectType<GeometryIndex>();
            uvInx.Data[0] = edgesArr.ToArray();
        }
        
        
        var verts = emptyGeo.GetObjectType<Vertices>()!;
        verts.Data[0] = (object)lgo.MeshInfo.VertexSeq.SelectMany(x => x.ToArray().Select(x => (double)x)).ToArray();
        emptyGeo.GetObjectType<UV>()!.Data[0] = (object)lgo.MeshInfo.TextureCord0.Select(x => new Vector2{X = x.X, Y = 1-x.Y}).SelectMany(x => x.ToArray()).Select(x => (double)x).ToArray();
        
        Model.FBXModels.Elements.Objects.Model emptyModel = FBXCreator.CreateModel("A");
        
        fbx.Elements["Connections"].SubElements.AddRange([
            new Connection{ Data = ["OO", emptyModel.Id, (long)0], Name = "C"},
            new Connection{ Data = ["OO", emptyGeo.Id, emptyModel.Id], Name = "C"}
        ]);
        fbx.Elements["Objects"].SubElements.AddRange([emptyGeo, emptyModel]);
           
        
        if (lgo.MeshInfo is not null)
        {
            for (int i = 0; i < lgo.MeshInfo.BlendSeq.Length; i++)
            {
                var blend = lgo.MeshInfo.BlendSeq[i];
                for (int j = 0; j < lgo.MeshInfo.Header.BoneInflFactor; j++)
                {
                    var boneIndex = lgo.MeshInfo.BlendIndexSeq[blend.Index[j]];
                    Console.WriteLine($"[{i}] {boneIndex}: {blend.Weight}");
                }
            }
        }
        
        return fbx;
    }
}