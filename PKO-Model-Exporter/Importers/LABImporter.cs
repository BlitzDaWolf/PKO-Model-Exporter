using PKO_Model_Exporter.Model;
using PKO_Model_Exporter.Model.Test;

namespace PKO_Model_Exporter.Importers;

public static class LABImporter
{
    public static void LoadLAB(string path)
    {
        using var stream = File.OpenRead(path);
        using var reader = new BinaryReader(stream);

        LAB lab = new LAB();

        lab.Version = reader.ReadInt32();

        if (lab.Version < 0x1000)
        {
            throw new Exception("Old Animation version. Need to re-export");
        }

        lab.Header.BoneCount = reader.ReadInt32();
        lab.Header.FrameCount = reader.ReadInt32();
        lab.Header.DummyCount = reader.ReadInt32();
        lab.Header.KeyType = reader.ReadInt32();

        lab.BaseInfo = Enumerable.Range(0, lab.Header.BoneCount).Select(x => new LAB.BoneBaseInfo
        {
            Name = string.Join("", reader.ReadBytes(64).Select(x => (char)x)),
            Id = reader.ReadUInt32(), ParentId = reader.ReadUInt32(),
        }).ToArray(); //new LAB.BoneBaseInfo[lab.Header.BoneCount];
        lab.InvMat = Enumerable.Range(0, lab.Header.BoneCount).Select(_ =>  Matrix.Parse(reader)).ToArray();

        lab.DummyInfo = Enumerable.Range(0, lab.Header.DummyCount).Select(x => new LAB.BoneDummyInfo
        {
            Id = reader.ReadUInt32(),
            ParentBoneId =  reader.ReadUInt32(),
            Mat = Matrix.Parse(reader),
        }).ToArray(); // new LAB.BoneDummyInfo[lab.Header.BoneCount];
        lab.KeySequnce = new LAB.BoneKeyInfo[lab.Header.BoneCount];

        if (lab.Header.KeyType == 1)
        {
            for (int i = 0; i < lab.Header.BoneCount; i++)
            {
                LAB.BoneKeyInfo key = new();
                key.Mat43 = new Matrix43[lab.Header.FrameCount];
                
                key.Mat43 = Enumerable.Range(0, lab.Header.FrameCount).Select(_ => Matrix43.Parse(reader)).ToArray();
                
                lab.KeySequnce[i] = key;
            }
        }
        else if (lab.Header.KeyType == 2)
        {
            for (int i = 0; i < lab.Header.BoneCount; i++)
            {
                LAB.BoneKeyInfo key = new();
                key.Mat44 = new Matrix[lab.Header.FrameCount];
                
                key.Mat44 = Enumerable.Range(0, lab.Header.FrameCount).Select(_ => Matrix.Parse(reader)).ToArray();
                
                lab.KeySequnce[i] = key;
            }
        }
        else if (lab.Header.KeyType == 3)
        {
            if (lab.Version >= 0x1003)
            {
                for (int i = 0; i < lab.Header.BoneCount; i++)
                {
                    LAB.BoneKeyInfo key = new();
                    key.PositionSeq = Enumerable.Range(0, lab.Header.FrameCount).Select(_ => Vector3.Parse(reader)).ToArray();
                    key.RotationSeq = Enumerable.Range(0, lab.Header.FrameCount).Select(_ => Vector4.Parse(reader)).ToArray();
                    lab.KeySequnce[i] = key;
                }
            }
            else
            {
                for (int i = 0; i < lab.Header.BoneCount; i++)
                {
                    LAB.BoneKeyInfo key = new();
                    int posNumber = lab.BaseInfo[i].ParentId == 1 ? lab.Header.FrameCount : 1;
                    key.PositionSeq = Enumerable.Range(0, posNumber).Select(_ => Vector3.Parse(reader)).ToArray();
                    if (posNumber == 1)
                    {
                        // TODO: Copy till frame count
                        
                    }
                    key.RotationSeq = Enumerable.Range(0, lab.Header.FrameCount).Select(_ => Vector4.Parse(reader)).ToArray();
                    lab.KeySequnce[i] = key;
                }
            }
        }
    }
}