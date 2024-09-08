namespace Models
{
    public class APITestRequestModel
    {
        public string ApiType { get; set; }
        public bool? IsAutomated { get; set; }
        public string Url { get; set; }
        public List<string>? Payload { get; set; } 
        public List<Header> Headers { get; set; } 
        public List<Params>? QueryParameters { get; set; }
        public int? NumberOfFields { get; set; }
        public APITestRequestModel()
        {
            Headers = new List<Header>();
            QueryParameters = new List<Params>();
            Payload = new List<string>();
        }
    }
    public class Params
    {
        public string Key { get; set; }
        public List<string> Value { get; set; }
        public Params()
        {
            Value = new List<string>();
        }
    }
    public class Header
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
