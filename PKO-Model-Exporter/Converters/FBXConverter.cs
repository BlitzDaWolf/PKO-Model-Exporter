using PKO_Model_Exporter.Model;
using PKO_Model_Exporter.Model.FBXElements;
using PKO_Model_Exporter.Model.FBXModels.Elements;

namespace PKO_Model_Exporter.Converters;

public static class FBXConverter
{
    public static OBJ ToOBJ(this FBX fbx)
    {
        OBJ obj = new OBJ();

        var verts = fbx.GetObjectType<Vertices>().FirstOrDefault()!;
        obj.Verts = ((double[])verts.Data[0]).Select(x => (float)x).ToList();
        
        return obj;
    }
}