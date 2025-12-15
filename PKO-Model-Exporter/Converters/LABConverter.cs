using PKO_Model_Exporter.Helpers;
using PKO_Model_Exporter.Model;
using PKO_Model_Exporter.Model.FBXElements;
using PKO_Model_Exporter.Model.FBXModels.Elements;

namespace PKO_Model_Exporter.Converters;

public static class LABConverter
{
    public static FBX ToFBX(this LAB lab)
    {
        FBX fbx = FBXCreator.CreateFBX();
        Skeleton sklt = lab.ToSkeleton();

        Dictionary<string, Model.FBXModels.Elements.Objects.Model> models =  new Dictionary<string, Model.FBXModels.Elements.Objects.Model>();
        
        foreach (var bone in sklt.Bones)
        {
            Model.FBXModels.Elements.Objects.Model m = FBXCreator.CreateModel(bone.Name);
            m.Data[2] = "null";

            var sc = m.GetObjectType<Property70.ScalingProperty>();
            var tl= m.GetObjectType<Property70.TranslationProperty>();
            var ro = m.GetObjectType<Property70.RotationProperty>();
            
            sc.Data[4] = 1d;
            sc.Data[5] = 1d;
            sc.Data[6] = 1d;

            tl.Data[4] = (double)(bone.Position.X * 50f);
            tl.Data[5] = (double)(bone.Position.Y * 50f);
            tl.Data[6] = (double)(bone.Position.Z * 50f);
            
            ro.Data[4] = (double)bone.Rotation.X;
            ro.Data[5] = (double)bone.Rotation.Y;
            ro.Data[6] = (double)bone.Rotation.Z;
            
            
            
            fbx.Elements["Objects"].SubElements.Add(m);

            var na = FBXCreator.CreateNodeAttribute(bone.Name);

            Connection c1 = new Connection { Name = "C", Data = ["OO", m.Id, (long)0] };
            Connection c2 = new Connection { Name = "C", Data = ["OO", na.Id, m.Id] };
            
            fbx.Elements["Connections"].SubElements.AddRange([c1, c2]);
            
            models.Add(bone.Name, m);
        }

        foreach (var bone in sklt.Bones)
        {
            if(bone.ParentID == -1) continue;
            Connection c = new Connection { Name = "C", Data = ["OO", models[bone.Name].Id, models[bone.ParentBone.Name].Id] };
            fbx.Elements["Connections"].SubElements.Add(c);
        }

        return fbx;
    }
    
    public static Skeleton ToSkeleton(this LAB lab)
    {
        Skeleton sklt = new Skeleton();

        List<Bone> bones = new List<Bone>();

        for (int i = 0; i < lab.BaseInfo.Length; i++)
        {
            Bone b = new Bone();
            
            b.ParentID = lab.BaseInfo[i].ParentId;
            b.Name = lab.BaseInfo[i].Name;
            b.ID = lab.BaseInfo[i].Id;

            b.Position = lab.KeySequnce[i].PositionSeq[0];
            b.Rotation = lab.KeySequnce[i].RotationSeq[0];
            
            bones.Add(b);
        }

        foreach (var bone in bones)
        {
            if (bone.ParentID == -1) continue;
            
            var parentBone = bones.FirstOrDefault(x => x.ID==bone.ParentID);
            if(parentBone is null) continue;
            parentBone.Children.Add(bone);
            bone.ParentBone = parentBone;
        }
        
        sklt.Bones = bones;
        
        return sklt;
    }
}