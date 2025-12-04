using System.IO.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace PKO_Model_Exporter.Helpers;

public static class Compression
{
    public static byte[] CompressZlib(byte[] input)
    {
        var deflater = new Deflater(1, false);
        using var output = new MemoryStream();
        using (var zlib = new DeflaterOutputStream(output, deflater))
        {
            zlib.Write(input, 0, input.Length);
            zlib.Finish();
        }

        return output.ToArray();
    }
    
    public static byte[] DecompressZlib(byte[] input)
    {
        using var inputStream = new MemoryStream(input);
        using var zlib = new ZLibStream(inputStream, CompressionMode.Decompress);
        using var output = new MemoryStream();
        zlib.CopyTo(output);
        return output.ToArray();
    }
}