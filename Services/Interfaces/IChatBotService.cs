namespace Services.Interfaces
{
    public interface IChatBotService
    {
        Task<string> UserQueryResolver(string query);
    }
}
