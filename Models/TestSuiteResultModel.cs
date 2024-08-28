namespace Models
{
    public class TestSuiteResultModel
    {
        public List<TestResultResponseModel> TestResults { get; set; } 

        public TestSuiteResultModel()
        {
            TestResults = new List<TestResultResponseModel>();
        }
    }
}
