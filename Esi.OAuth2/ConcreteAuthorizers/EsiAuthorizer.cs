namespace Esi.OAuth2.ConcreteAuthorizers
{
    public sealed class EsiAuthorizer : OAuthAuthorizer
    {

        private const string BaseUrl = "https://esi.uz";

        public override string ClientId => "6F3572A1014859C2";

        public override string ClientName => "Esi";

        public override string RedirectUri => "http://www.uzex.uz/welcome";

        public override string ClientSecret => "5B8DB3174F83AEA62E729650A0C4BE7736FB3965999E679D611039C3443A823C";

        protected override string AuthorizeUri => BaseUrl + "/oauth2/authorize";

        protected override string TokenUri => BaseUrl + "/oauth2/token";

        public override string Scope => "public-info";


    }
}