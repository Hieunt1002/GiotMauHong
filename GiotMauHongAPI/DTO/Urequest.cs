namespace GiotMauHongAPI.DTO
{
    public class Urequest
    {
        public int Requestid { get; set; }
        public DateTime RequestDate { get; set; }
        public int quantity { get; set; }
        public string Contact { get; set; }
        public string Starttime { get; set; }
        public string Endtime { get; set; }
        public string City { get; set; }
        public string Ward { get; set; }
        public string District { get; set; }
        public string Address { get; set; }
    }
}
