using PKO_Model_Exporter.Model;
using PKO_Model_Exporter.Model.FBXElements;

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
}