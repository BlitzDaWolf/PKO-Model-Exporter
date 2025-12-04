using PKO_Model_Exporter.Model.Test;

namespace PKO_Model_Exporter.Model
{

    namespace Test
    {
        public class Matrix
        {
            public static Matrix[] ParseArray(BinaryReader reader)
            {
                byte count = reader.ReadByte();
                return Enumerable.Range(0, count).Select(_=>Parse(reader)).ToArray();
            }
            
            public static Matrix Parse(BinaryReader reader)
            {
                Matrix m = new Matrix();

                int i = 0;
                for (int x = 0; x < 4; x++)
                {
                    for (int y = 0; y < 4; y++)
                    {
                        m.m[x,y] = reader.ReadSingle();
                        i++;
                    }
                }
                
                return m;
            }

            public float[,] m { get; set; } = new float[4, 4];
        }

        public class Matrix43
        {
            public static Matrix43[] ParseArray(BinaryReader reader)
            {
                byte count = reader.ReadByte();
                return Enumerable.Range(0, count).Select(_=>Parse(reader)).ToArray();
            }
            public static Matrix43 Parse(BinaryReader reader)
            {
                Matrix43 m = new Matrix43();

                int i = 0;
                for (int x = 0; x < 4; x++)
                {
                    for (int y = 0; y < 3; y++)
                    {
                        m.m[x,y] = reader.ReadSingle();
                    }
                }
                
                return m;
            }
            public float[,] m { get; set; } = new float[4, 3];
        }

        public class Vector3
        {
            public float X { get; set; }
            public float Y { get; set; }
            public float Z { get; set; }

            public static Vector3[] ParseArray(BinaryReader reader)
            {
                byte count = reader.ReadByte();
                return Enumerable.Range(0, count).Select(_=>Parse(reader)).ToArray();
            }
            public static Vector3 Parse(BinaryReader reader)
            {
                return new Vector3
                {
                    X = reader.ReadSingle(),
                    Y =  reader.ReadSingle(),
                    Z = reader.ReadSingle(),
                };
            }
        }
        public class Vector4
        {
            public float X { get; set; }
            public float Y { get; set; }
            public float Z { get; set; }
            public float W { get; set; }

            public static Vector4[] ParseArray(BinaryReader reader)
            {
                byte count = reader.ReadByte();
                return Enumerable.Range(0, count).Select(_=>Parse(reader)).ToArray();
            }
            public static Vector4 Parse(BinaryReader reader)
            {
                return new Vector4
                {
                    X = reader.ReadSingle(),
                    Y = reader.ReadSingle(),
                    Z = reader.ReadSingle(),
                    W = reader.ReadSingle(),
                };
            }
        }
    }

    public class LAB
    {
        public class BoneInfoHeader
        {
            public int BoneCount { get; set; }
            public int FrameCount { get; set; }
            public int DummyCount { get; set; }
            public int KeyType { get; set; }
        }
        public class BoneBaseInfo
        {
            public string Name { get; set; } // 64 bytes
            public uint Id { get; set; }
            public uint ParentId { get; set; }
        }
        public class BoneDummyInfo
        {
            public uint Id { get; set; }
            public uint ParentBoneId { get; set; }

            // D3DXMATIX Mat
            public Matrix Mat { get; set; } // = new float[4, 4];
        }
        public class BoneKeyInfo
        {
            // Matrix43 Mat43Seq
            public Matrix43[] Mat43 { get; set; } // = new float[4, 3];

            // D3DXMATIX Mat44Seq
            public Matrix[] Mat44 { get; set; } // = new float[3, 3];

            // Vector3 PosSeq
            public Vector3[] PositionSeq { get; set; } // = new float[3];

            // Vector4 RotationSeq
            public Vector4[] RotationSeq { get; set; } // = new float[4];
        }

        public int Version { get; set; }

        public BoneInfoHeader Header { get; set; } = new BoneInfoHeader();
        public BoneBaseInfo[] BaseInfo { get; set; }
        public BoneDummyInfo[] DummyInfo { get; set; }
        public BoneKeyInfo[] KeySequnce { get; set; }

        public Matrix[] InvMat { get; set; }
    }
}