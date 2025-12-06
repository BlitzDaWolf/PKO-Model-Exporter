using PKO_Model_Exporter.Generics;

namespace PKO_Model_Exporter.Model
{

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
            public Matrix43[] Mat43
            {
                set
                {
                    // TODO: Convert to Vector3 and Vector4
                }
            } // = new float[4, 3];

            // D3DXMATIX Mat44Seq
            public Matrix[] Mat44
            {
                set
                {
                    // TODO: Convert to Vector3 and Vector4
                }
            } // = new float[3, 3];

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