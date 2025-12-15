using PKO_Model_Exporter.Model.FBXElements;
using PKO_Model_Exporter.Model.FBXModels.Elements;

namespace PKO_Model_Exporter.Model
{
    namespace FBXElements
    {
        public class Property70 : RawElement
        {
            public class TranslationProperty : Property;
            public class RotationProperty : Property;
            public class ScalingProperty : Property;
            
            public class Property : RawElement
            {
                public string Type
                {
                    get => (string)Data[0];
                    set => Data[0] = value;
                }

                public Property()
                {
                    Name = "P";
                }
                
                public override string ToString() => $"Property: {Type}";

                public Property Add(string name, string type, string type2, params object[] value)
                {
                    Data.AddRange([
                        name, 
                        type,
                        type2,
                        name.StartsWith("Lcl") ? "A" : ""
                    ]);
                    if (value.Length > 0)
                        Data.AddRange(value);
                    
                    return this;
                }
            }
        }

        public class CreationTimeStamp : RawElement
        {
            // Get from sub elements
        }

    }

    public class FBX
    {
        public static byte[] MAGICHEADER =>
        [
            0x4B, 0x61, 0x79, 0x64, 0x61, 0x72, 0x61, 0x20,
            0x46, 0x42, 0x58, 0x20, 0x42, 0x69, 0x6e, 0x61,
            0x72, 0x79, 0x20, 0x20, 0x00, 0x1a, 0x00
        ];

        public int Version { get; set; }
        public byte[] Header { get; set; }
        public Dictionary<string, RawElement> Elements { get; set; } = new();

        public bool IsValidHeader()
        {
            if (Header.Length != 23) return false;
            return Header.SequenceEqual(MAGICHEADER);
        }

        public T[] GetObjectType<T>() where T : RawElement => Elements.Values.SelectMany(x => x.GetObjectsType<T>()).ToArray();
    }
}