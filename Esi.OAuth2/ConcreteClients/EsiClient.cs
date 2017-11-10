using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Web;
using Esi.OAuth2.Interfaces;
using Esi.OAuth2.Utils;

namespace Esi.OAuth2.ConcreteClients
{
    public class EsiClient : IOAuthClient
    {
        private string _me; 
        public string BaseRequestUrl => "https://esi.uz/oauth2";

        public string ClientName => "Esi";

        public string Response
        {
            set => GetDataFromResponse(value);
        }

        public string Token { get; set; }

        public string MakeRequest(string requesturl)
        {
            return OAuthClientUtils.MakeRequest(OAuthClientUtils.RequestUrl(BaseRequestUrl, requesturl, Token));
        }

        public DateTime TokenExpires { get; set; }

        public string Me()
        {
            if (!string.IsNullOrEmpty(_me))
                return _me;

            return _me = MakeRequest("/api?get=public-info");
        }

        public string FirstName
        {
            get
            {
                var dict = OAuthClientUtils.JsonToDictionary(Me());

                return dict.ContainsKey("first_name") ? dict["first_name"].ToString() : string.Empty;
            }
        }

        public string LastName
        {
            get
            {
                var dict = OAuthClientUtils.JsonToDictionary(Me());

                return dict.ContainsKey("last_name") ? dict["last_name"].ToString() : string.Empty;
            }
        }

        private static Dictionary<string, object> ResponseToDictionary(string response)
        {
            var responseValues = HttpUtility.ParseQueryString(response);

            return responseValues.AllKeys.ToDictionary<string, string, object>(responseValue => responseValue, responseValue => responseValues[responseValue]);
        }

        private void GetDataFromResponse(string response)
        {
            var responseDict = ResponseToDictionary(response);

            if (!responseDict.ContainsKey("access_token"))
                throw new AuthenticationException(response);

            Token = responseDict["access_token"].ToString();
            int expireSeconds;

            if (responseDict.ContainsKey("expires") &&
                int.TryParse(responseDict["expires"].ToString(), out expireSeconds))
                TokenExpires = DateTime.Now.AddSeconds(expireSeconds);
            else
                TokenExpires = DateTime.Now.AddDays(30);
        }
    }
}