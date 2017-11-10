namespace Esi.OAuth2.Interfaces
{
    public interface IOAuthAuthorizer
    {

        string ClientId{ get;  }

        string ClientName { get; }

        string RedirectUri { get; }

        string Scope { get; }

        string ClientSecret { get; }

        string CodeRequestUri { get; }

        string GetAuthorizationResponse(string code);

        void GetAuthorizationResponseAsync(string code);

    }
}
