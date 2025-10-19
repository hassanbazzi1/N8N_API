using Public.DTO.Credentials.Google;

namespace Google_OAuth2.Helpers
{
    public static class GoogleOAuthStateHelper
    {
        public static string Encode(GoogleState state)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(state);
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(json))
                .TrimEnd('=').Replace('+', '-').Replace('/', '_'); // URL-safe
        }

        public static GoogleState Decode(string encoded)
        {
            string padded = encoded.PadRight(encoded.Length + (4 - encoded.Length % 4) % 4, '=')
                                    .Replace('-', '+').Replace('_', '/');
            var bytes = Convert.FromBase64String(padded);
            return System.Text.Json.JsonSerializer.Deserialize<GoogleState>(System.Text.Encoding.UTF8.GetString(bytes));
        }
    }
}
