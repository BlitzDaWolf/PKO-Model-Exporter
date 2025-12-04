using PKO_Model_Exporter.Model;

namespace PKO_Model_Exporter.Exporters;

public static class OBJExporter
{
    public static void Export(this OBJ obj, string path)
    {
        if (!obj.ValidVerts || !obj.ValidUVs || !obj.ValidUVs) throw new Exception("");
        
        string output = "# Object exported from [PKOModelExporter]\n#Created by [BlitzDaWolf]\nGithub: [https://github.com/BlitzDaWolf/]\n\n";

        output += string.Join("\n", obj.V.Select(x => "v" + string.Join(" ", x)));
        output += string.Join("\n", obj.U.Select(x => "vt" + string.Join(" ", x)));
        output += string.Join("\n", obj.I.Select(x => "u" + string.Join(" ", x)));
    }
}