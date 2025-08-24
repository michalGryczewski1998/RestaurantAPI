namespace RestaurantAPI.Authentication
{
    public class AuthenticationSettings
    {
        public string JwtKey { get; set; }
        public int JwtExpire { get; set; }
        public string JwtIssuer { get; set; }
    }
}
