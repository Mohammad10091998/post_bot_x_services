namespace Models
{
    public class TestModel
    {
        public string ApiType { get; set; }
        public string Url { get; set; }
        public string Payload { get; set; } 
        public List<KeyValue> HeaderPairs { get; set; } 
        public List<KeyValue> QueryParameters { get; set; }
        public TestModel()
        {
            HeaderPairs = new List<KeyValue>();
            QueryParameters = new List<KeyValue>();
        }
    }

    public class KeyValue
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
