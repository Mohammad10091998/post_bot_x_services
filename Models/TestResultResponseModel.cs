namespace Models
{
    public class TestResultResponseModel
    {
        public string TestData { get; set; } 
        public string? Description { get; set; } 
        public int StatusCode { get; set; } 
        public string ResponseContent { get; set; } 
        public bool IsSuccessful { get; set; } 
        public string? ErrorAnalysis { get; set; }
    }
}
