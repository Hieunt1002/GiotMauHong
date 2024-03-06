namespace GiotMauHongAPI.DTO
{
    public class UpdateActive
    {
        public int ActiveId { get; set; }
        public string NameActivate { get; set; }
        public List<UImg> uImgs { get; set; } = new List<UImg>();
    }
}
