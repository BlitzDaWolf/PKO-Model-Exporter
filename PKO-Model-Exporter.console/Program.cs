using PKO_Model_Exporter.Converters;
using PKO_Model_Exporter.Exporters;
using PKO_Model_Exporter.Helpers;
using PKO_Model_Exporter.Importers;
using PKO_Model_Exporter.Model.FBXElements;
using PKO_Model_Exporter.Model.FBXModels.Elements;
using PKO_Model_Exporter.Model.FBXModels.Elements.Animation;
using PKO_Model_Exporter.Model.FBXModels.Elements.Animation.Keys;
using PKO_Model_Exporter.Model.FBXModels.Elements.Roots;

var loaded = FBXImport.LoadFBX("./testModels/Untitled.fbx");
var rootObject = loaded.GetObjectType<SceneObject>().Where(x => x.Parrent is null).ToArray();

var root = ((Objects)loaded.Elements["Objects"]).RootObjects;

var r = loaded.GetObjectType<Key>()
    // .Where(x => x.GetType() == typeof(RawElement))
    .GroupBy(x => x.Name)
    .OrderByDescending(x => x.Count())
    .ToDictionary(x => x.Key, x => (x.Count(), x));

var lab = LABImporter.LoadLAB("./testModels/0000.lab");
FBXExporter.Export(lab.ToFBX(), "./boneExport.fbx");

// OBJExporter.Export(LGOImporter.LoadLGO("./testModels/0002011702.lgo").ToOBJ(), "./lgoexport.obj");
var costume = LGOImporter.LoadLGO("./testModels/0002011702.lgo").ToFBX();
// var costume = LGOImporter.LoadLGO("./testModels/0000000000.lgo").ToFBX();
FBXExporter.Export(costume, "./lgoExport.fbx");
FBXImport.LoadFBX("./lgoExport.fbx");

var t = "";