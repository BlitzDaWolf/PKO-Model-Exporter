namespace PKO_Model_Exporter.Model;

public class Skeleton
{
    public List<Bone> Bones { get; set; } = new();
    public List<Bone> RootBones => Bones.Where(x => x.ParentID == -1).ToList();
}