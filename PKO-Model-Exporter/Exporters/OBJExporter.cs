using PKO_Model_Exporter.Model;

namespace PKO_Model_Exporter.Exporters;

public static class OBJExporter
{
    public static void Export(this OBJ obj, string path)
    {
        //if (!obj.ValidVerts || !obj.ValidUVs || !obj.ValidIndecies) throw new Exception("");
        
        string output = "# Object exported from [PKOModelExporter]\n#Created by [BlitzDaWolf]\n#Github: [https://github.com/BlitzDaWolf/]\n\n\n";

        output += string.Join("\n", obj.V.Select(x => "v " + string.Join(" ", x)));
        output += "\n\n";
        output += string.Join("\n", obj.N.Select(x => "vn " + string.Join(" ", x)));
        output += "\n\n";
        
        output += string.Join("\n", obj.U.Select(x => "vt " + string.Join(" ", x)));
        output += "\ns 0\n";
        
        output += string.Join("\n", obj.I.Select(x => "f " + string.Join(" ", x)));
        
        File.WriteAllText(path, output);
    }
}