namespace GiotMauHongAPI.DTO
{
    public class AddSendBloodDTO
    {
        public int Hospitalid { get; set; }
        public DateTime Datesend { get; set; }

        public List<QuantitySendDTO> QuantitySend { get; set; } = new List<QuantitySendDTO>();
    }
}
