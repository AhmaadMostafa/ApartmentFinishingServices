namespace ApartmentFinishingServices.APIs.Dtos
{
    public class RequestToReturnDto
    {
        public int RequestId { get; set; }
        public string WorkerName { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string ServiceName { get; set; }
        public DateTime RequestDate { get; set; }
        public string Comment { get; set; }
        public string Status { get; set; }
        public decimal? CustomerSuggestedPrice { get; set; }
        public decimal? WorkerSuggestedPrice { get; set; }
        public decimal? FinalAgreedPrice { get; set; }
        public string NegotiationStatus { get; set; }
    }
}
