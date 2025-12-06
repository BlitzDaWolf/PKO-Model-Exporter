namespace PKO_Model_Exporter.Model;

public class OBJ
{
    public List<float> Verts { get; set; } = new();
    public List<float> Normals { get; set; } = new();
    public List<float> UV { get; set; } = new();
    public List<int> Indecies { get; set; } = new();

    public bool ValidVerts => Verts.Count % 3 == 0;
    public bool ValidIndecies => Indecies.Count % 3 == 0;
    public bool ValidUVs => UV.Count % 2 == 0;

    public List<string> SV => Verts.Select(x => x.ToString("N6")).ToList();
    public List<string> SN => Normals.Select(x => x.ToString("N6")).ToList();
    public List<string> ST => UV.Select(x => x.ToString("N6")).ToList();

    public List<string[]> V =>
        Enumerable.Range(0, SV.Count / 3).Select(v => SV.Skip(v * 3).Take(3).ToArray()).ToList();
    public List<string[]> N =>
        Enumerable.Range(0, SN.Count / 3).Select(v => SN.Skip(v * 3).Take(3).ToArray()).ToList();
    public List<int[]> I =>
        Enumerable.Range(0, Indecies.Count / 3).Select(v => Indecies.Skip(v * 3).Take(3).ToArray()).ToList();
    public List<string[]> U =>
        Enumerable.Range(0, ST.Count / 2).Select(v => ST.Skip(v * 2).Take(2).ToArray()).ToList();
}