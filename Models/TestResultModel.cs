namespace Models
{
    public class TestResultModel
    {
        public string Payload { get; set; } 
        public string? PayloadDescription { get; set; } 
        public int StatusCode { get; set; } 
        public string ResponseContent { get; set; } 
        public bool IsSuccessful { get; set; } 
        public string? ErrorAnalysis { get; set; }
    }
}
