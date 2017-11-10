using System;
using Esi.OAuth2.Utils;

namespace Esi.OAuth2.Interfaces
{
    public interface IOAuthClient
    {
        string ClientName { get; }
        string Response { set; }
        string Token { get; set; } 
        string BaseRequestUrl { get; }
        string MakeRequest(string requesturl);
        DateTime TokenExpires { get; set; } 
        string Me();
        string FirstName { get; }
        string LastName { get; } 
    }
}
