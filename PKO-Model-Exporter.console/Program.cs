using PKO_Model_Exporter.Converters;
using PKO_Model_Exporter.Exporters;
using PKO_Model_Exporter.Helpers;
using PKO_Model_Exporter.Importers;


// OBJExporter.Export(LGOImporter.LoadLGO("./testModels/0002011702.lgo").ToOBJ(), "");
// LABImporter.LoadLAB("./testModels/0042.lab");
//OBJExporter.Export(FBXImport.LoadFBX("./testModels/test.fbx").ToOBJ(), "./test.obj");

var loaded = FBXImport.LoadFBX("./testModels/test.fbx");
var test = FBXCreator.CreateFBX();

try
{
    FBXExporter.Export(loaded, "./out.fbx");
    FBXImport.LoadFBX("./out.fbx");
}
catch
{
    
}

var t = "";