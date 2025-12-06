namespace PKO_Model_Exporter.Generics;

public class Sphere
{
    public Vector3 Position { get; set; }
    public float Radius { get; set; }
}

public class BoundingSphereInfo
{
    public int Id { get; set; }
    public Sphere Sphere { get; set; }
    public Matrix mat { get; set; }
}