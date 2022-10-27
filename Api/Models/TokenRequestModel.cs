namespace Api.Models
{
    public class TokenRequestModel
    {
        public string Login { get; set; }
        public string Pass { get; set; }

        public TokenRequestModel(string login, string pass)
        {
            Login = login;
            Pass = pass;
        }
    }

    public class RefreshTokenRequestModel
    {
        public string RefreshToken { get; set; }

        public RefreshTokenRequestModel(string refreshToken)
        {
            RefreshToken = refreshToken;
        }
    }
}
