using PKO_Model_Exporter.Model;
using PKO_Model_Exporter.Model.FBXElements;
using PKO_Model_Exporter.Model.FBXElements.Geometry;
using Version = PKO_Model_Exporter.Model.FBXElements.Version;

namespace PKO_Model_Exporter.Helpers;

public static class FBXCreator
{
    public static FBX CreateFBX()
    {
        DateTime currentTime =  DateTime.Now;
        
        FBX fbx = new FBX();

        fbx.Version = 7400;
        
        RawElement headerExtension = new RawElement{Name = "FBXHeaderExtension"};
        RawElement fieldId = new RawElement { Name = "FieldId" };
        RawElement creationTime = new RawElement { Name = "CreationTime" };
        RawElement creator = new RawElement{ Name = "Creator" };
        RawElement globalSettings = new RawElement { Name = "GlobalSettings" };
        RawElement documents =  new RawElement { Name = "Documents" };
        RawElement references = new RawElement { Name = "References" };
        RawElement definitions = new RawElement { Name = "Definitions" };
        RawElement objects = new RawElement { Name = "Objects" };
        RawElement connections = new RawElement { Name = "Connections" };
        RawElement takes  = new RawElement { Name = "Takes" };

        headerExtension.SubElements.AddRange([
            new RawElement
            {
                Name = "FBXHeaderVersion",
                Data = [1003]
            },
            new RawElement
            {
                Name = "FBXVersion",
                Data = [fbx.Version]
            },
            new RawElement
            {
                Name = "EncryptionType",
                Data = [0]
            },
            new RawElement
            {
                Name = "CreationTimeStamp",
                SubElements = [
                    new RawElement{Name = "Version", Data = [1000]},
                    new RawElement{Name = "Year", Data = [currentTime.Year]},
                    new RawElement{Name = "Month", Data = [currentTime.Month]},
                    new RawElement{Name = "Day", Data = [currentTime.Day]},
                    new RawElement{Name = "Hour", Data = [currentTime.Hour]},
                    new RawElement{Name = "Minute", Data = [currentTime.Minute]},
                    new RawElement{Name = "Second", Data = [currentTime.Second]},
                    new RawElement{Name = "Millisecond", Data = [currentTime.Millisecond]},
                ]
            },
            new RawElement
            {
                Name = "Creator",
                Data = ["Blitz PKO exporter - ({VERSION})"]
            },
            new RawElement
            {
                Name = "SceneInfo", Data = ["GlobalInfo SceneInfo", "UserData"],
                SubElements = [
                    new RawElement{Name = "Type", Data = ["UserData"]},
                    new Version{Name = "Version", Data = [100]},
                    new RawElement{Name = "MetaData"}, // TODO: Add the metadata
                    new Property70
                    {
                        Name = "Properties70",
                        SubElements = [
                            (new Property70.Property()).Add("DocumentUrl", "KString", "Url", "/foobar.fbx"),
                            (new Property70.Property()).Add("SrcDocumentUrl", "KString", "Url", "/foobar.fbx"),
                            (new Property70.Property()).Add("Original", "Compound", ""),
                            (new Property70.Property()).Add("Original|ApplicationVendor", "KString", "", "PKO Exporter"),
                            (new Property70.Property()).Add("Original|ApplicationName", "KString", "", "PKO Exporter (FBX IO)"),
                            (new Property70.Property()).Add("Original|ApplicationVersion", "KString", "", "1.0.0"),
                            (new Property70.Property()).Add("Original|DateTime_GMT", "DateTime", "", "01/01/1970 00:00:00.000"),
                            (new Property70.Property()).Add("Original|FileName", "KString", "", "/foobar.fbx")
                        ]
                    }
                ]
            }
        ]);

        globalSettings.SubElements.AddRange([
            new Version{Name = "Version", Data = [1000]},
            new Property70
            {
                Name = "Property70",
                SubElements = [
                    (new Property70.Property()).Add("UpAxis", "int", "Integer", 1),
                    (new Property70.Property()).Add("UpAxisSign", "int", "Integer", 1),
                    (new Property70.Property()).Add("FrontAxis", "int", "Integer", 2),
                    (new Property70.Property()).Add("FrontAxisSign", "int", "Integer", 1),
                    (new Property70.Property()).Add("CoordAxis", "int", "Integer", 0),
                    (new Property70.Property()).Add("CoordAxisSign", "int", "Integer", 1),
                    (new Property70.Property()).Add("OriginalUpAxis", "int", "Integer", -1),
                    (new Property70.Property()).Add("OriginalUpAxisSign", "int", "Integer", 1),
                    (new Property70.Property()).Add("UnitScaleFactor", "double", "Number", (double)1),
                    (new Property70.Property()).Add("OriginalUnitScaleFactor", "double", "Number", (double)1),
                    (new Property70.Property()).Add("AmbientColor", "ColorRGB", "Color", 0,0,0),
                    (new Property70.Property()).Add("DefaultCamera", "KString", "", "Producer Perspective"),
                    (new Property70.Property()).Add("TimeMode", "enum", "", 11),
                    (new Property70.Property()).Add("TimeSpanStart", "KTime", "Time", (long)0),
                    (new Property70.Property()).Add("TimeSpanStop", "KTime", "Time", (long)46186158000),
                    (new Property70.Property()).Add("CustomFrameRate", "double", "Number", 24),
                ]
            }
        ]);
        
        creationTime.Data.AddRange(["1970-01-01 10:00:00:000"]);
        creator.Data.AddRange(["Blitz PKO exporter - ({VERSION})"]);

        fbx.Elements = new Dictionary<string, RawElement>()
        {
            { headerExtension.Name, headerExtension },
            { fieldId.Name, fieldId },
            { creationTime.Name, creationTime },
            { creator.Name, creator },
            { globalSettings.Name, globalSettings },
            { documents.Name, documents },
            { references.Name, references },
            { definitions.Name, definitions },
            { objects.Name, objects },
            { connections.Name, connections },
            { takes.Name, takes }
        };
        
        return fbx;
    }

    public static Geometry CreateGeometry(string name)
    {
        var element = new Geometry(name);

        element.SubElements.AddRange([
            new RawElement { Name = "Properties70" },
            new RawElement { Name = "GeometryVersion", Data = [124] },
            new Vertices { Data = [Array.Empty<double>()] },
            new PolyVertexIndex { Name = "PolygonVertexIndex", Data = [Array.Empty<int>()] },
            new Edges() { Name = "Edges", Data = [Array.Empty<int>()] },
            new RawElement
            {
                Name = "LayerElementNormal", Data = [0],
                SubElements =
                [
                    new Version { Name = "Version", Data = [101] },
                    new RawElement { Name = "Name", Data = [""] },
                    new RawElement { Name = "MappingInformationType", Data = ["ByPolygonVertex"] },
                    new RawElement { Name = "ReferenceInformationType", Data = ["IndexToDirect"] },
                    new RawElement { Name = "Normals", Data = [Array.Empty<double>()] },
                    /*new RawElement { Name = "NormalsIndex", Data = [Array.Empty<int>()] }*/
                ]
            },
            new RawElement
            {
                Name = "LayerElementUV", Data = [0],
                SubElements =
                [
                    new Version { Name = "Version", Data = [101] },
                    new RawElement { Name = "Name", Data = [""] },
                    new RawElement { Name = "MappingInformationType", Data = ["ByPolygonVertex"] },
                    new RawElement { Name = "ReferenceInformationType", Data = ["IndexToDirect"] },
                    new UV() { Name = "UV", Data = [Array.Empty<double>] },
                    new UVIndex() { Name = "UVIndex", Data = [Array.Empty<int>] }
                ]
            },
            new RawElement
            {
                Name = "Layer", Data = [0],
                SubElements = [
                    new Version { Name = "Version", Data = [100] },
                    new RawElement
                    {
                        Name = "LayerElement",
                        SubElements = [
                            new RawElement{Name = "Type", Data = ["LayerElementNormal"]},
                            new RawElement{Name = "TypedIndex", Data = [0]}
                        ]
                    },
                    new RawElement
                    {
                        Name = "LayerElement",
                        SubElements = [
                            new RawElement{Name = "Type", Data = ["LayerElementUV"]},
                            new RawElement{Name = "TypedIndex", Data = [0]}
                        ]
                    }
                ]
            }
        ]);
        
        return element;
    }

    public static Model.FBXElements.Model CreateModel(string name)
    {
        var element = new Model.FBXElements.Model(name);

        element.SubElements.AddRange([
            new Version{Name = "Version", Data = [232]},
            new Property70
            {
                Name = "Properties70", SubElements = [
                    (new Property70.Property()).Add("Lcl Translation", "Lcl Translation", "", 0d,0d,0d),
                    (new Property70.Property()).Add("Lcl Rotation", "Lcl Rotation", "", 0d,0d,0d),
                    (new Property70.Property()).Add("Lcl Scaling", "Lcl Scaling", "", 100d,100d,100d),
                    
                    (new Property70.Property()).Add("DefaultAttributeIndex", "int", "Integer", 0),
                    (new Property70.Property()).Add("InheritType", "Enum", "", 1),
                ]
            },
            new RawElement{Name = "MultiLayer", Data = [0]},
            new RawElement{Name = "MultiTake", Data = [0]},
            new RawElement{Name = "Shading", Data = [(char)0b1]},
            new RawElement{Name = "Culling", Data = ["CullingOff"]}
        ]);
        
        return element;
    }
}