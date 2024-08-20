namespace Services.Interfaces
{
    public interface IHelperService
    {
        List<(string Payload, string Description)> ParsePayloads(string input);
    }
}
