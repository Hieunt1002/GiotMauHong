namespace GiotMauHongAPI.DTO
{
    public class AddTakeBloodDTO
    {
        public int Hospitalid { get; set; }
        public DateTime Datetake { get; set; }
        public int Bloodtypeid { get; set; }
        public List<QuantityTakeDTO> QuantityTake { get; set; } = new List<QuantityTakeDTO>();
    }
}
