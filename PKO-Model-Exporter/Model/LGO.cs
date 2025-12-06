using PKO_Model_Exporter.Generics;

namespace PKO_Model_Exporter.Model
{
    public class LGO
    {
        
        public int Version { get; set; }

        public int Id { get; set; }
        public int ParentId { get; set; }
        public int Type { get; set; }

        public Matrix MatLocal { get; set; }
        public RenderControllCreateInfo ControllCreateInfo { get; set; }
        public byte[] StatrControll { get; set; } // 8 bytes

        public int MtlSize { get; set; }
        public int MeshSize { get; set; }
        public int HelperSize { get; set; }
        public int AnimationSize { get; set; }

        public HelperInfo HelperData { get; set; } = new HelperInfo();
        public MTLTextureInfo[] TextureInfo { get; set; }
        public MeshInfo MeshInfo { get; set; }
    }
}