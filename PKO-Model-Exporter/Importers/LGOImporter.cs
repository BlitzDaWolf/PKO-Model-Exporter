using PKO_Model_Exporter.Generics;
using PKO_Model_Exporter.Model;

namespace PKO_Model_Exporter.Importers;

public static class LGOImporter
{
    public static LGO LoadLGO(string path)
    {
        using var stream = File.OpenRead(path);
        using var reader = new BinaryReader(stream);

        LGO lgo = new LGO();

        lgo.Version = reader.ReadInt32();

        lgo.Id = reader.ReadInt32();
        lgo.ParentId = reader.ReadInt32();
        lgo.Type = reader.ReadInt32();

        lgo.MatLocal = Matrix.Parse(reader);
        lgo.ControllCreateInfo = new RenderControllCreateInfo
        {
            ControllId = reader.ReadInt32(),
            DeclId = reader.ReadInt32(),
            VsId = reader.ReadInt32(),
            PsId = reader.ReadInt32(),
        };
        lgo.StatrControll = reader.ReadBytes(8);
        lgo.MtlSize = reader.ReadInt32();
        lgo.MeshSize = reader.ReadInt32();
        lgo.HelperSize = reader.ReadInt32();
        lgo.AnimationSize = reader.ReadInt32();

        if (lgo.MtlSize > 100000)
        {
            throw new Exception("LGO size is too large");
        }

        if (lgo.MtlSize > 0)
        {
            MemoryStream ms = new MemoryStream(reader.ReadBytes(lgo.MtlSize));
            lgo.LoadMtlTextureInfo(new BinaryReader(ms));
        }

        if (lgo.MeshSize > 0)
        {
            MemoryStream ms = new MemoryStream(reader.ReadBytes(lgo.MeshSize));
            lgo.MeshInfoLoad(new BinaryReader(ms));
        }

        if (lgo.HelperSize > 0)
        {
            MemoryStream ms = new MemoryStream(reader.ReadBytes(lgo.HelperSize));
            lgo.HelperData.LoadHelperInfo(new BinaryReader(ms), lgo.Version);
        }

        if (lgo.AnimationSize > 0)
        {
            MemoryStream ms = new MemoryStream(reader.ReadBytes(lgo.AnimationSize));
        }

        return lgo;
    }

    #region Load textureinfo
    private static void LoadMtlTextureInfo(this LGO lgo, BinaryReader reader)
    {
        int version = lgo.Version;
        if (version == 0)
        {
            version = reader.ReadInt32();
        }

        int number = reader.ReadInt32();
        lgo.TextureInfo = new MTLTextureInfo[number];
        for (int i = 0; i < number; i++)
        {
            lgo.TextureInfo[i] = LoadTextureInfo(reader, version);
        }
    }

    private static MTLTextureInfo LoadTextureInfo(BinaryReader reader, int version)
    {
        MTLTextureInfo info = new MTLTextureInfo();
        if (version >= 0x1000)
        {
            info.Opacity = reader.ReadSingle();
            info.Trancerecy = reader.ReadInt32();
            info.Mtl = new Material
            {
                Amb = ColorValue4f.Parse(reader),
                Diff = ColorValue4f.Parse(reader),
                Emi = ColorValue4f.Parse(reader),
                Spe = ColorValue4f.Parse(reader),
                Power = reader.ReadSingle(),
            };
            info.RenderStateSet = Enumerable.Range(0, 8).Select(_ => new RenderStateAtom
            {
                State = reader.ReadInt32(), Value0 = reader.ReadInt32(), Value1 = reader.ReadInt32(),
            }).ToArray();
            info.TextureSequence = Enumerable.Range(0, 4).Select(_ => new TextureInfo
            {
                Stage = reader.ReadInt32(),
                Level = reader.ReadInt32(),
                Usage =  reader.ReadInt32(),
                Format =  reader.ReadInt32(),
                Pool =  reader.ReadInt32(),
                
                ByteAlignmentFlag =   reader.ReadInt32(),
                Type =   reader.ReadInt32(),
                Width =  reader.ReadInt32(),
                Height = reader.ReadInt32(),
                ColorKeyType =  reader.ReadInt32(),
                ColorKey = ColorValue4b.Parse(reader),
                
                FileName = string.Join("", reader.ReadBytes(64).Select(x => (char)x)),
                Data = reader.ReadInt32(),
                
                TssSet = Enumerable.Range(0, 8).Select(_ => new RenderStateAtom
                {
                    State = reader.ReadInt32(), Value0 = reader.ReadInt32(), Value1 = reader.ReadInt32(),
                }).ToArray()
            }).ToArray();
        }
        else if (version == 2)
        {

        }
        else if (version == 1)
        {

        }
        else if (version == 0)
        {
            
        }
        else
        {
            throw new Exception("Invalid version type");
        }

        return info;
    }
    #endregion

    #region Load MeshData
    private static void MeshInfoLoad(this LGO lgo, BinaryReader reader)
    {
        MeshInfo info = new MeshInfo();
        info.Header = new MeshInfoHeader();

        lgo.MeshInfo = info;
        
        int version = lgo.Version;
        if (version == 0)
            version = reader.ReadInt32();
        if (version >= 0x1004)
        {
            info.Header.Fvf = reader.ReadInt32();
            info.Header.PrimitiveType = reader.ReadInt32();

            info.Header.VertexCount = reader.ReadInt32();
            info.Header.IndexCount = reader.ReadInt32();
            info.Header.SubsetCount = reader.ReadInt32();
            info.Header.BoneIndexCount = reader.ReadInt32();
            info.Header.BoneInflFactor = reader.ReadInt32();
            info.Header.VertexElementCount = reader.ReadInt32();
            
            info.Header.rsSet = Enumerable.Range(0, 8).Select(_ => new RenderStateAtom
            {
                State = reader.ReadInt32(), Value0 = reader.ReadInt32(), Value1 = reader.ReadInt32(),
            }).ToArray();
        }
        else if (version == 0x1003)
        {
            info.Header.Fvf = reader.ReadInt32();
            info.Header.PrimitiveType =  reader.ReadInt32();
            info.Header.VertexCount = reader.ReadInt32();
            info.Header.IndexCount = reader.ReadInt32();
            info.Header.SubsetCount = reader.ReadInt32();
            info.Header.BoneIndexCount = reader.ReadInt32();
            
            info.Header.rsSet = Enumerable.Range(0, 8).Select(_ => new RenderStateAtom
            {
                State = reader.ReadInt32(), Value0 = reader.ReadInt32(), Value1 = reader.ReadInt32(),
            }).ToArray();
        }
        else if (version == 0x1000 || version == 1)
        {
            info.Header.Fvf = reader.ReadInt32();
            info.Header.PrimitiveType = reader.ReadInt32();
            info.Header.VertexCount = reader.ReadInt32();
            info.Header.IndexCount = reader.ReadInt32();
            info.Header.SubsetCount = reader.ReadInt32();
            info.Header.BoneIndexCount = reader.ReadInt32();

            info.Header.rsSet = Enumerable.Range(0, 8).Select(_ => new RenderStateAtom
            {
                State = reader.ReadInt32(), Value0 = reader.ReadInt32(), Value1 = reader.ReadInt32(),
            }).ToArray();
        }
        /*else if (version == 0)
        {
            
        }*/
        else
        {
            throw new Exception("invalid version");
        }


        if (version >= 0x1004)
        {
            if (info.Header.VertexElementCount > 0)
            {
                info.VertexElementSeq = Enumerable.Range(0, info.Header.VertexElementCount).Select(_ => new VertexElement
                {
                    Stream = reader.ReadInt16(),
                    Offset = reader.ReadInt16(),
                    Type = reader.ReadByte(),
                    Method = reader.ReadByte(),
                    Usage = reader.ReadByte(),
                    UsageIndex = reader.ReadByte()
                }).ToArray();
            }

            if (info.Header.VertexCount > 0)
            {
                info.VertexSeq = Enumerable.Range(0, info.Header.VertexCount).Select(_ => Vector3.Parse(reader)).ToArray();
            }

            if ((info.Header.Fvf & 0x10) != 0)
            {
                info.NormalSeq = Enumerable.Range(0, info.Header.VertexCount).Select(_ => Vector3.Parse(reader)).ToArray();
            }

            if ((info.Header.Fvf & 0x100) != 0)
            {
                info.TextureCord0 = Enumerable.Range(0, info.Header.VertexCount).Select(_ => Vector2.Parse(reader)).ToArray();
            }
            else if ((info.Header.Fvf & 0x200) != 0)
            {
                info.TextureCord0 = Enumerable.Range(0, info.Header.VertexCount).Select(_ => Vector2.Parse(reader)).ToArray();
                info.TextureCord1= Enumerable.Range(0, info.Header.VertexCount).Select(_ => Vector2.Parse(reader)).ToArray();
            }
            else if ((info.Header.Fvf & 0x300) != 0)
            {
                info.TextureCord0 = Enumerable.Range(0, info.Header.VertexCount).Select(_ => Vector2.Parse(reader)).ToArray();
                info.TextureCord1= Enumerable.Range(0, info.Header.VertexCount).Select(_ => Vector2.Parse(reader)).ToArray();
                info.TextureCord2= Enumerable.Range(0, info.Header.VertexCount).Select(_ => Vector2.Parse(reader)).ToArray();
            }
            else if ((info.Header.Fvf & 0x400) != 0)
            {
                info.TextureCord0 = Enumerable.Range(0, info.Header.VertexCount)
                    .Select(_ => Vector2.Parse(reader)).ToArray();
                info.TextureCord1 = Enumerable.Range(0, info.Header.VertexCount)
                    .Select(_ => Vector2.Parse(reader)).ToArray();
                info.TextureCord2 = Enumerable.Range(0, info.Header.VertexCount)
                    .Select(_ => Vector2.Parse(reader)).ToArray();
                info.TextureCord3 = Enumerable.Range(0, info.Header.VertexCount)
                    .Select(_ => Vector2.Parse(reader)).ToArray();
            }

            if ((info.Header.Fvf & 0x40) != 0)
            {
                info.VerColor = Enumerable.Range(0, info.Header.VertexCount).Select(_ => reader.ReadInt32()).ToArray();
            }


            if (info.Header.BoneIndexCount > 0)
            {
                info.BlendSeq =  Enumerable.Range(0, info.Header.VertexCount).Select(_ => new BlendInfo
                {
                    Index = reader.ReadBytes(4),
                    IndexD = reader.ReadInt32(),
                    Weight = Vector3.Parse(reader),
                }).ToArray();
                info.BlendIndexSeq =  Enumerable.Range(0, info.Header.BoneIndexCount).Select(_ => reader.ReadInt32()).ToArray();
            }

            if (info.Header.IndexCount > 0)
            {
                info.IndexSeq = Enumerable.Range(0, info.Header.IndexCount).Select(_ => reader.ReadInt32()).ToArray();
            }

            if (info.Header.SubsetCount > 0)
            {
                
            }
        }
        else
        {
            // Todo: implement data reading
        }
    }
    #endregion

    #region Load HelperData
    private static void LoadHelperInfo(this HelperInfo info, BinaryReader reader, int version)
    {
        if (version == 0)
            version = reader.ReadInt32();

        int helperType = reader.ReadInt32();
        if ((helperType & 0x1) != 0) info.LoadHelperDummyInfo(reader, version);
        if ((helperType & 0x2) != 0) info.LoadHelperBoxInfo(reader, version);
        if ((helperType & 0x4) != 0) info.LoadHelperMeshInfo(reader, version);
        if ((helperType & 0x10) != 0) info.LoadHelperBoundingBoxInfo(reader, version);
        if ((helperType & 0x20) != 0) info.LoadHelperBoundingShapeInfo(reader, version);
    }

    private static void LoadHelperDummyInfo(this HelperInfo info, BinaryReader reader, int version)
    {
        if (version >= 0x1001)
        {
            int dummyNumber  = reader.ReadInt32();
            info.DummySeq = Enumerable.Range(0, dummyNumber).Select(x => new HelperInfo.HelperDummyInfo
            {
                Id = reader.ReadInt32(),
                Mat = Matrix.Parse(reader),
                MatLocal = Matrix.Parse(reader),
                ParentType =  reader.ReadInt32(),
                ParentId = reader.ReadInt32()
            }).ToArray();
        }
        else
        {
            int dummyNumber  = reader.ReadInt32();
        }
    }

    private static void LoadHelperBoxInfo(this HelperInfo info, BinaryReader reader, int version)
    {
        throw new NotImplementedException();
    }

    private static void LoadHelperMeshInfo(this HelperInfo info, BinaryReader reader, int version)
    {
        throw new NotImplementedException();
    }
    
    private static void LoadHelperBoundingBoxInfo(this HelperInfo info, BinaryReader reader, int version)
    {
        throw new NotImplementedException();
    }

    private static void LoadHelperBoundingShapeInfo(this HelperInfo info, BinaryReader reader, int version)
    {
        int sphereCount = reader.ReadInt32();
        var spheres =  Enumerable.Range(0, sphereCount).Select(_ => new BoundingSphereInfo
        {
            Id = reader.ReadInt32(),
            Sphere = new Sphere{ Position = Vector3.Parse(reader), Radius = reader.ReadSingle() },
            mat =  Matrix.Parse(reader),
        }).ToArray();
    }
    #endregion

    #region Load AnimationData
    #endregion
}