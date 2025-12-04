namespace PKO_Model_Exporter.Model;

public class OBJ
{
    public List<float> Verts { get; set; } = new();
    public List<float> UV { get; set; } = new();
    public List<int> Indecies { get; set; } = new();

    public bool ValidVerts => Verts.Count % 3 == 0;
    public bool ValidIndecies => Indecies.Count % 3 == 0;
    public bool ValidUVs => Verts.Count % 2 == 0;

    public List<float[]> V =>
        Enumerable.Range(0, Verts.Count / 3).Select(v => Verts.Skip(v * 3).Take(3).ToArray()).ToList();
    public List<int[]> I =>
        Enumerable.Range(0, Indecies.Count / 3).Select(v => Indecies.Skip(v * 3).Take(3).ToArray()).ToList();
    public List<float[]> U =>
        Enumerable.Range(0, UV.Count / 2).Select(v => UV.Skip(v * 2).Take(2).ToArray()).ToList();
}