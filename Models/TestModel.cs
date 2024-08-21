namespace Models
{
    public class TestModel
    {
        public string ApiType { get; set; }
        public bool IsAutomated { get; set; }
        public string Url { get; set; }
        public List<string> Payload { get; set; } 
        public List<HeaderKeyValue> HeaderPairs { get; set; } 
        public List<KeyValue> QueryParameters { get; set; }
        public TestModel()
        {
            HeaderPairs = new List<HeaderKeyValue>();
            QueryParameters = new List<KeyValue>();
            Payload = new List<string>();
        }
    }
    public class KeyValue
    {
        public string Key { get; set; }
        public List<string> Values { get; set; }
        public KeyValue()
        {
            Values = new List<string>();
        }
    }
    public class HeaderKeyValue
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
