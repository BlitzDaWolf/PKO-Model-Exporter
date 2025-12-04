using PKO_Model_Exporter.Helpers;
using PKO_Model_Exporter.Model;
using PKO_Model_Exporter.Model.FBXElements;
using Version = PKO_Model_Exporter.Model.FBXElements.Version;

namespace PKO_Model_Exporter.Importers;

public static class FBXImport
{
    private static Dictionary<string, Func<RawElement>> GetElementTypes = new Dictionary<string, Func<RawElement>>()
    {
        { "FBXHeaderVersion", () => new FBXHeaderVersion() },
        { "FBXVersion", () => new FBXVersion() },
        { "EncryptionType", () => new EncryptionType() },
        { "Version", () => new Version() },
        { "HeaderVersion", () => new HeaderVersion() },
        { "Vertices", () => new Vertices() },
        { "Geometry", () => new Geometry() },
        { "Model", () => new Model.FBXElements.Model() },
        { "NodeAttribute", () => new NodeAttribute() },
        { "Pose", () => new Pose() },
        { "Deformer", () => new Deformer() },
        
        { "AnimationStack", () => new AnimationStack() },
        { "AnimationLayer", () => new AnimationLayer() },
        { "AnimationCurveNode", () => new AnimationCurveNode() },
        { "AnimationCurve", () => new AnimationCurve() },
        
        { "C", () => new Connection() }
    };
    
    public interface IElementHeader
    {
        public int BlockSentinel { get; }
            
        public (long EndOffset, long Count, long Lentgh, string Id) ReadElement(BinaryReader reader);
        public void WriteElement (BinaryWriter writer, long endOffset, long elementCount, long count, string id);
    }
    public class OldReadElement : IElementHeader
    {
        public int BlockSentinel => 13;

        public (long EndOffset, long Count, long Lentgh, string Id) ReadElement(BinaryReader reader)
        {
            var EndOffset = reader.ReadInt32();
            var Count = reader.ReadInt32();
            var Lentgh = reader.ReadInt32();
            var IdSize = reader.ReadByte();

            return (EndOffset, Count, Lentgh, string.Join("", reader.ReadBytes(IdSize).Select(x => (char)x)));
        }

        public void WriteElement(BinaryWriter writer, long endOffset, long elementCount, long count, string id)
        {
            writer.Write((int)endOffset);
            writer.Write((int)elementCount);
            writer.Write((int)count);
            writer.Write((byte)id.Length);
            writer.Write(id.Select(x => (byte)x).ToArray());
        }
    }
    public class NewReadElement : IElementHeader
    {
        public int BlockSentinel => 25;
        public long EndOffset { get; set; }
        public long Count { get; set; }
        public long Lentgh { get; set; }
        public byte IdSize { get; set; }

        public (long EndOffset, long Count, long Lentgh, string Id) ReadElement(BinaryReader reader)
        {
            var EndOffset = reader.ReadInt64();
            var Count = reader.ReadInt64();
            var Lentgh = reader.ReadInt64();
            var IdSize = reader.ReadByte();

            return (EndOffset, Count, Lentgh, string.Join("", reader.ReadBytes(IdSize).Select(x => (char)x)));
        }

        public void WriteElement(BinaryWriter writer, long endOffset, long elementCount, long count, string id)
        {
            writer.Write(endOffset);
            writer.Write(elementCount);
            writer.Write(count);
            writer.Write((byte)id.Length);
            writer.Write(id.Select(x => (byte)x).ToArray());
        }
    }

    private static Dictionary<char, Func<int, byte[], object>> ArrayTypeReader =
        new Dictionary<char, Func<int, byte[], object>>()
        {
            { 'b', (size, data) => data.Select(x => x == 1).ToArray() },
            { 'c', (size, data) => data },
            {
                'i',
                (size, data) => Enumerable.Range(0, size)
                    .Select(x => BitConverter.ToInt32(data.Skip(x * 4).Take(4).ToArray())).ToArray()
            },
            {
                'l',
                (size, data) => Enumerable.Range(0, size)
                    .Select(x => BitConverter.ToInt64(data.Skip(x * 8).Take(8).ToArray())).ToArray()
            },
            {
                'f',
                (size, data) => Enumerable.Range(0, size)
                    .Select(x => BitConverter.ToSingle(data.Skip(x * 4).Take(4).ToArray())).ToArray()
            },
            {
                'd',
                (size, data) => Enumerable.Range(0, size)
                    .Select(x => BitConverter.ToDouble(data.Skip(x * 8).Take(8).ToArray())).ToArray()
            },
        };
    private static Dictionary<char, Func<BinaryReader, object>> SingleTypeReader =
        new Dictionary<char, Func<BinaryReader,  object>>()
        {
            {'Z', (reader) => reader.ReadByte()},
            {'Y', (reader) => reader.ReadInt16()},
            {'B', (reader) => reader.ReadByte()==1},
            {'C', (reader) => reader.ReadChar()},
            {'I', (reader) => reader.ReadInt32()},
            {'F', (reader) => reader.ReadSingle()},
            {'D', (reader) => reader.ReadDouble()},
            {'L', (reader) => reader.ReadInt64()},
            {'R', (reader) => reader.ReadBytes(reader.ReadInt32())},
            {'S', (reader) => string.Join("", reader.ReadBytes(reader.ReadInt32()).Select(x => (char)x))},
        };
    
    public static void LoadFBX(string path)
    {
        using var stream = File.OpenRead(path);
        using var reader = new BinaryReader(stream);
        
        FBX fbx = new FBX();
        fbx.Header =  reader.ReadBytes(23);
        if(!fbx.IsValidHeader()) throw new Exception("Invalid FBX header");
        fbx.Version = reader.ReadInt32();

        IElementHeader ReadElementFunc = (fbx.Version < 7500)? new OldReadElement() : new NewReadElement();
        
        while (true)
        {
            var element = ReadElement(reader, ReadElementFunc);
            if(element is null) break;
            fbx.Elements.Add(element);
        }

        var objects = fbx.GetObjectType<SceneObject>();
        var Connections = fbx.GetObjectType<Connection>();

        foreach (var connect in Connections)
        {
            var parrent = objects.FirstOrDefault(x => x.Id == connect.Parent);
            var child = objects.FirstOrDefault(x => x.Id == connect.Child);
            
            if(child==null) continue;
            parrent?.Children.Add(child);
            child.Parrent = parrent;
            if (connect.ConnectionType == "OO")
            {
            }
            else
            {
                
            }
        }

        var rootObjects = objects.Where(x => x.Parrent == null).ToArray();
    }

    private static RawElement? ReadElement(BinaryReader reader, IElementHeader headerRead, long fileOffset=0)
    {
        (long EndOffset, long Count, long Lentgh, string Id) = headerRead.ReadElement(reader);
        if (EndOffset == 0) return null;
        RawElement element = new  RawElement();
        if (GetElementTypes.ContainsKey(Id))
        {
            element = GetElementTypes[Id]();
        }
        element.Name = Id;

        for (int i = 0; i < Count; i++)
        {
            char dataType = reader.ReadChar();
            if (ArrayTypeReader.ContainsKey(dataType))
            {
                var arrayInfo = ReadArrayParams(reader);
                var data = reader.ReadBytes(arrayInfo[2]);
                if (arrayInfo[1] == 1) // Is comressed?
                {
                    data = Compression.DecompressZlib(data);
                }

                element.Data.Add(ArrayTypeReader[dataType](arrayInfo[0], data));
            }
            else
            {
                element.Data.Add(SingleTypeReader[dataType](reader));
            }
        }

        if (element is RawElement && Count != 0)
        {
            
        }
        
        long pos = reader.BaseStream.Position;
        long localEndOffset = EndOffset - fileOffset;

        long startSubPos = 0;
        long subTreeEnd = 0;

        BinaryReader rd = reader;
        
        if (pos < localEndOffset)
        {
            if (fileOffset == 0 && element.Name != "Objects")
            {
                long blockBytesRemain = localEndOffset - pos;

                MemoryStream ms = new MemoryStream(reader.ReadBytes((int)blockBytesRemain));
                rd = new BinaryReader(ms);

                startSubPos = 0;
                fileOffset = pos;
                subTreeEnd = blockBytesRemain - headerRead.BlockSentinel;
            }
            else
            {
                startSubPos = pos;
                subTreeEnd = localEndOffset - headerRead.BlockSentinel;
            }
            
            long subPos = startSubPos;
            while (subPos < subTreeEnd)
            {
                element.SubElements.Add(ReadElement(rd, headerRead, fileOffset));
                subPos = rd.BaseStream.Position;
            }

            if (startSubPos != 0)
            {
                var sentinalBlock = reader.ReadBytes(headerRead.BlockSentinel);
                if (sentinalBlock.Sum(x => (short)x) != 0)
                {
                    throw new Exception("Failed to read nested block sentinel");
                }
            }
            
            pos += (subPos - startSubPos) + headerRead.BlockSentinel;
        }

        if (pos != localEndOffset)
        {
            throw new Exception("Scope length not reached");
        }
        
        return element;
    }
    
    private static int[] ReadArrayParams(BinaryReader reader) => [ reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32() ];
}