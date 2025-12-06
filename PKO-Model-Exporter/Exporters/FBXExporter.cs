using PKO_Model_Exporter.Model;
using PKO_Model_Exporter.Model.FBXElements;
using Spectre.Console;
using Version = System.Version;

namespace PKO_Model_Exporter.Exporters;

public static class FBXExporter
{
    public static void Export(FBX fbx, string path)
    {
        if (fbx.Version >= 7500) throw new Exception("VersionNot implemented");
        using var stream = File.Open(path, FileMode.OpenOrCreate);
        using var writer = new BinaryWriter(stream);
        
        writer.Write(FBX.MAGICHEADER);
        writer.Write(fbx.Version);

        foreach (var element in fbx.Elements.Values)
        {
            element.WriteElement(writer);
        }
        
        // EOF
        writer.Write(new byte[((fbx.Version < 7500)? 13: 25)]);
    }

    private static Dictionary<Type, char> Codes = new Dictionary<Type, char>
    {
        {typeof(bool[]), ('b') },
        {typeof(char[]), ('c') },
        {typeof(int[]), ('i') },
        {typeof(float[]), ('f') },
        {typeof(double[]), ('d') },
        {typeof(long[]), ('l') },
    };

    private static Dictionary<Type, Action<BinaryWriter, object>> TypeWriter =
        new Dictionary<Type, Action<BinaryWriter, object>>
        {
            {typeof(byte), (writer, o) => { writer.Write('Z'); writer.Write((byte)o);} },
            {typeof(short), (writer, o) => { writer.Write('Y'); writer.Write((short)o);} },
            {typeof(bool), (writer, o) => { writer.Write('B'); writer.Write((bool)o);} },
            {typeof(char), (writer, o) => { writer.Write('C'); writer.Write((char)o);} },
            {typeof(int), (writer, o) => { writer.Write('I'); writer.Write((int)o);} },
            {typeof(float), (writer, o) => { writer.Write('F'); writer.Write((float)o);} },
            {typeof(double), (writer, o) => { writer.Write('D'); writer.Write((double)o);} },
            {typeof(long), (writer, o) => { writer.Write('L'); writer.Write((long)o);} },
            {typeof(byte[]), (writer, o) =>
            {
                byte[] d = (byte[])o;
                writer.Write('R');
                writer.Write(d.Length);
                writer.Write(d);
            }},
            {typeof(string), (writer, o) =>
            {
                byte[] d = ((string)o).Select(x => (byte)x).ToArray();
                writer.Write('S');
                writer.Write(d.Length);
                writer.Write(d);
            }}
        };
    
    private static Dictionary<Type, Func<object, (int,byte[])>> ArrayTypeWriter =
        new Dictionary<Type, Func<object, (int, byte[])>>
        {
            {typeof(short[]), (o) => (((short[])o).Length,((short[])o).SelectMany(BitConverter.GetBytes).ToArray())},
            {typeof(bool[]), (o) => (((bool[])o).Length,((bool[])o).SelectMany(BitConverter.GetBytes).ToArray())},
            {typeof(char[]), (o) => (((char[])o).Length,((char[])o).SelectMany(BitConverter.GetBytes).ToArray())},
            {typeof(int[]), (o) => (((int[])o).Length,((int[])o).SelectMany(BitConverter.GetBytes).ToArray())},
            {typeof(float[]), (o) => (((float[])o).Length,((float[])o).SelectMany(BitConverter.GetBytes).ToArray()) },
            {typeof(double[]), (o) => (((double[])o).Length,((double[])o).SelectMany(BitConverter.GetBytes).ToArray()) },
            {typeof(long[]), (o) => (((long[])o).Length,((long[])o).SelectMany(BitConverter.GetBytes).ToArray()) }
        };
    
    private static void WriteElement(this RawElement element, BinaryWriter writer,int blockSentinel = 13, bool start = false)
    {
        long startPosition = writer.BaseStream.Position;
        
        // Write base header
        {
            writer.Write(0);
            writer.Write(element.Data.Count);
            writer.Write(0);
            writer.Write((byte)element.Name.Length);
            writer.Write(element.Name.Select(x => (byte)x).ToArray());
        }

        
        long AttributeLen = writer.BaseStream.Position;
        foreach (var data in element.Data)
        {
            var t = data.GetType();
            if (ArrayTypeWriter.ContainsKey(t))
            {
                writer.Write(Codes[t!]);
                var o =  ArrayTypeWriter[t](data);
                writer.Write(o.Item1);
                
                if (o.Item2.Length > 128)
                {
                    writer.Write(1);
                    o.Item2 = Helpers.Compression.CompressZlib(o.Item2);
                }
                else
                {
                    writer.Write(0);
                }
                writer.Write(o.Item2.Length);
                writer.Write(o.Item2);
            }
            else if (TypeWriter.ContainsKey(t))
            {
                TypeWriter[t](writer, data);
            }
        }

        AttributeLen = writer.BaseStream.Position - AttributeLen;

        if (element.SubElements.Count > 0)
        {
            foreach (var subElement in element.SubElements)
            {
                subElement.WriteElement(writer, blockSentinel);
            }
            
            if (!start)
            {
                writer.Write(new byte[blockSentinel]);
            }
        }
        
        if (start)
        {
            writer.Write(new byte[blockSentinel]);
        }
            
        if ((AttributeLen == 0 && element.SubElements.Count == 0))
        {
            writer.Write(new byte[blockSentinel]);
        }
        
        long currentPosition = writer.BaseStream.Position;
        writer.BaseStream.Position = startPosition;
        writer.Write((int)currentPosition);
        writer.Write(element.Data.Count);
        writer.Write((int)AttributeLen);
        writer.BaseStream.Position = currentPosition;
    }
}