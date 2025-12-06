namespace PKO_Model_Exporter.Generics;

public class HelperInfo
{
    public class HelperDummyInfo
    {
        public int Id { get; set; }
        public Matrix Mat { get; set; }
        public Matrix MatLocal { get; set; }
        public int ParentType { get; set; }
        public int ParentId { get; set; }
    }

    public HelperDummyInfo[] DummySeq { get; set; }
}