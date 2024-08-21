namespace Models
{
    public class TestSuiteResultModel
    {
        public string ApiUrl { get; set; } 
        public string ApiType { get; set; } 
        public List<TestResultResponseModel> TestResults { get; set; } 

        public TestSuiteResultModel()
        {
            TestResults = new List<TestResultResponseModel>();
        }
    }
}
