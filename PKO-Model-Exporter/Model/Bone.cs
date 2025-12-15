using PKO_Model_Exporter.Generics;

namespace PKO_Model_Exporter.Model;

public class Bone
{
    public int ID { get; set; }
    public int ParentID { get; set; }

    public Bone ParentBone { get; set; }
    public List<Bone> Children { get; set; } = new();
    
    public string Name { get; set; }
    public Vector3 Position { get; set; }
    public Vector4 Rotation { get; set; }
}