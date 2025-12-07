using PKO_Model_Exporter.Converters;
using PKO_Model_Exporter.Exporters;
using PKO_Model_Exporter.Helpers;
using PKO_Model_Exporter.Importers;

Directory.GetFiles("/mnt/Vore/topsrcs/topsrcs/Client/model/character").ToList().ForEach(x =>
{
    try
    {
        var f = Path.GetFileName(x);
        var fbx = LGOImporter.LoadLGO(x).ToFBX();
        
        FBXExporter.Export(fbx, $"./exports/{f.Replace(".lgo", ".fbx")}");
    }
    catch
    {

    }
});

var loaded = FBXImport.LoadFBX("./testModels/test.fbx");

// OBJExporter.Export(LGOImporter.LoadLGO("./testModels/0002011702.lgo").ToOBJ(), "./lgoexport.obj");
var costume = LGOImporter.LoadLGO("./testModels/0002011702.lgo").ToFBX();
FBXExporter.Export(costume, "./lgoExport.fbx");
FBXImport.LoadFBX("./lgoExport.fbx");

var t = "";