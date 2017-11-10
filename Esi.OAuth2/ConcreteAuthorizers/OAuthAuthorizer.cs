using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Esi.OAuth2.Interfaces;

namespace Esi.OAuth2.ConcreteAuthorizers
{
    public abstract class OAuthAuthorizer : IOAuthAuthorizer
    {
        public delegate void AuthorizationComplete(DownloadStringCompletedEventArgs args);

        protected abstract string AuthorizeUri { get; }

        protected abstract string TokenUri { get; }

        protected virtual Dictionary<string, string> Params { get;   }

        public abstract string ClientId { get; }

        public abstract string ClientName { get; }

        public abstract string RedirectUri { get;  }

        public virtual string Scope { get; }

        public abstract string ClientSecret { get; }

        public string CodeRequestUri
        {
            get
            {
                var query = new StringBuilder();

                query.AppendFormat("?client_id={0}", ClientId);
                query.AppendFormat("&redirect_uri={0}", RedirectUri);

                if (!string.IsNullOrEmpty(Scope))
                    query.AppendFormat("&scope={0}", Scope);

                if (Params != null)
                {
                    foreach (var param in Params)
                    {
                        query.AppendFormat("&{0}={1}", param.Key, param.Value);
                    }
                }

                return AuthorizeUri + query;
            }
        }

        public string GetAuthorizationResponse(string code)
        {
            var uri = TokenRequestUri(code);
            var request = WebRequest.Create(uri);
            WebResponse response;
            try
            {
                response = request.GetResponse();
            }
            catch (WebException e)
            {
                response = e.Response;
            }
            var receiveStream = response.GetResponseStream();
            var encode = Encoding.GetEncoding("utf-8");
            var readStream = new StreamReader(receiveStream, encode);
            var result = readStream.ReadToEnd();
            readStream.Close();
            response.Close();
            return result;
        }

        public void GetAuthorizationResponseAsync(string code)
        {
            var wc = new WebClient();
            var uri = TokenRequestUri(code);
            wc.DownloadStringCompleted += delegate (object o, DownloadStringCompletedEventArgs eventArgs)
            {
                EndAuthorization?.Invoke(eventArgs);
            };
            wc.DownloadStringAsync(uri);
        }

        public event AuthorizationComplete EndAuthorization;

        private Uri TokenRequestUri(string code)
        {
            var query = new StringBuilder();

            query.AppendFormat("?client_id={0}", ClientId);
            query.AppendFormat("&client_secret={0}", ClientSecret);
            query.AppendFormat("&redirect_uri={0}", RedirectUri);
            query.AppendFormat("&code={0}", code);

            if (Params != null)
            {
                foreach (var param in Params)
                {
                    query.AppendFormat("&{0}={1}", param.Key, param.Value);
                }
            }

            return new Uri(TokenUri + query);
        }
    }
}