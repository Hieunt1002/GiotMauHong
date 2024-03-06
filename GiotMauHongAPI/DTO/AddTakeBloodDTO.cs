namespace GiotMauHongAPI.DTO
{
    public class AddTakeBloodDTO
    {
        public int Hospitalid { get; set; }
        public int Bloodbankid { get; set; }
        public DateTime Datetake { get; set; }
        public List<QuantityTakeDTO> QuantityTake { get; set; } = new List<QuantityTakeDTO>();
    }
}
