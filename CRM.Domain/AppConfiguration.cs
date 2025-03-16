namespace CRM.Domain
{
    public class AppConfiguration(string? realm, string? authServerUrl, string? resource, string? credentialSecret)
    {
        public string Realm { get; set; } = realm;
        public string AuthServerUrl { get; set; } = authServerUrl;
        public string Resource { get; set; } = resource;
        public string CredentialSecret { get; set; } = credentialSecret;
    }
}
