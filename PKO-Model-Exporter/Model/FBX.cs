using PKO_Model_Exporter.Model.FBXElements;

namespace PKO_Model_Exporter.Model
{
    namespace FBXElements
    {
        public class Property70 : RawElement
        {
            public class Property : RawElement
            {
                public string Type => (string)Data[0];

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
        
        public class RawElement
        {
            public string Name { get; set; }
            public List<object> Data { get; set; } = new();
            public List<RawElement> SubElements { get; set; } = new();

            public override string ToString() => $"[{Name}] <{Data.Count}, {SubElements.Count}>";

            public T[] GetObjectsType<T>() where  T : RawElement
            {
                var found = SubElements.SelectMany(x => x.GetObjectsType<T>()).ToArray();
                if (this is T)
                    return [(T)this, ..found];
                return found;
            }

            public T? GetObjectType<T>() where T : RawElement
            {
                if (this is T) return (T)this;
                var found = SubElements.SelectMany(x => x.GetObjectsType<T>()).ToArray();

                return found.FirstOrDefault();
            }
        }

        public class FBXHeaderVersion : RawElement
        {
            public int Version => (int)Data[0];
        }

        public class FBXVersion : RawElement
        {
            public int Version => (int)Data[0];
        }

        public class EncryptionType : RawElement
        {
            public int Encrypt => (int)Data[0];
        }

        public class Connection : RawElement
        {
            public string ConnectionType => (string)Data[0];
            public long Child => (long)Data[1];
            public long Parent => (long)Data[2];

            public override string ToString() => $"[{ConnectionType}] {Parent}->{Child}";
        }
        
        public class Version : RawElement
        {
            public int V => (int)Data[0];
        }

        public class HeaderVersion : RawElement
        {
            public int V => (int)Data[0];
        }

        public class CreationTimeStamp : RawElement
        {
            // Get from sub elements
        }

        public class Vertices : RawElement
        {
            public double[][] Vector => GetVector();
            
            private double[][] GetVector()
            {
                double[] d = (double[])Data[0];
                return Enumerable.Range(0,d.Length/3).Select(i => d.Skip(i*3).Take(3).ToArray()).ToArray();
            }

            public Vertices()
            {
                Name = "Vertices";
            }
        }

        public abstract class SceneObject : RawElement
        {
            public long Id => (long)Data[0];
            public string ObjectName => (string)Data[1];
            public string Type => (string)Data[1];

            public SceneObject? Parrent { get; set; } = null;
            public List<SceneObject> Children { get; set; } = [];
        }

        namespace Geometry
        {
            public class Geometry : SceneObject
            {
                public Geometry(){}

                public Geometry(string name)
                {
                    Name = "Geometry";
                    
                    char zero = (char)0;
                    char one = (char)1;

                    Data.AddRange([
                        (long)Random.Shared.NextInt64(), // ID
                        $"{name}.001{zero}{one}Geometry", // Name
                        "Mesh" // Type
                    ]);
                }
            }

            public class PolyVertexIndex : RawElement
            {
                public void UnPack()
                {
                    int[] a = (int[])Data[0];
                    for (int i = 0; i < a.Length; i++)
                    {
                        if (a[i] < 0)
                        {
                            a[i] ^= -1;
                        }
                    }

                    Data[0] = a;
                }

                public void Pack()
                {
                    int[] a = (int[])Data[0];
                    for (int i = 1; i < a.Length; i++)
                    {
                        var tst = (i+1) % 3;
                        if (tst == 0) a[i] ^= -1;
                    }
                    Data[0] = a;
                }
            }

            public class Edges : RawElement
            {
                public void UnPack()
                {
                    int[] a = (int[])Data[0];
                    for (int i = 0; i < a.Length; i++)
                    {
                        if (a[i] < 0)
                        {
                            a[i] ^= -1;
                        }
                    }

                    Data[0] = a;
                }

                public void Pack()
                {
                    int[] a = (int[])Data[0];
                    for (int i = 1; i < a.Length; i++)
                    {
                        var tst = (i+1) % 3;
                        if (tst == 0) a[i] ^= -1;
                    }
                    Data[0] = a;
                }
            }

            public class UV : RawElement
            {
                
            }
            
            public class UVIndex : RawElement{}
        }

        
        
        public class Model : SceneObject
        {
            public Model(){}

            public Model(string name)
            {
                Name = "Model";
                    
                char zero = (char)0;
                char one = (char)1;

                Data.AddRange([
                    (long)Random.Shared.NextInt64(), // ID
                    $"{name}{zero}{one}Model", // Name
                    "Mesh" // Type
                ]);
            }
        }

        public class NodeAttribute : SceneObject
        {
        }

        public class Pose : SceneObject
        {
            
        }

        public class Deformer : SceneObject
        {
            
        }
        
        public abstract class Animation : SceneObject{}

        public class AnimationStack : Animation{}
        public class AnimationLayer: Animation {}
        public class AnimationCurveNode : Animation {}
        public class AnimationCurve : Animation {}
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