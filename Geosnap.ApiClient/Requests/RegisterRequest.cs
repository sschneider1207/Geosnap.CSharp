using System;

namespace Geosnap.ApiClient.Requests
{
    internal sealed class RegisterRequest
    {
        public string Username { get; }
        public string Password { get; }
        public string Email { get; }
        public Guid Uuid { get; }

        public RegisterRequest(string username, string password, string email, Guid uuid)
        {
            Username = username;
            Password = password;
            Email = email;
            Uuid = uuid;
        }
    }
}
