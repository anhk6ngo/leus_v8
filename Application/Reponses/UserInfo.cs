namespace LeUs.Application.Reponses
{
    // Add properties to this class and update the server and client AuthenticationStateProviders
    // to expose more information about the authenticated user to the client.
    public class UserInfo
    {
        public required string UserId { get; set; }
        public required string Email { get; set; }
        public List<string>? Role { get; set; }
        public string? FullName { get; set; }
        public string? Receiver { get; set; }
        public string? SentTo { get; set; }
        public string? Phone { get; set; }
        public string? SignIn { get; set; }
        public string? Airport { get; set; }
    }
}