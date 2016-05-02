using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfOrigin
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        public Student GetData(string value)
        {
            Student student = new Student();
            student.Id = 1;
            student.Name = value;
            return student;
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
        public User GetUser(string UserId)
        {
            if (Authenticate(WebOperationContext.Current.IncomingRequest))
            {
                return new User
                {
                    UserName = "UserName",
                    UserPassword = "UserPassword"
                };
            }
            else
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Unauthorized;

                throw new WebFaultException<string>("Unauthorized Request.", HttpStatusCode.Unauthorized);

            }
        }
        private static bool Authenticate(IncomingWebRequestContext context)
        {
            bool Authenticated = false;

            string normalizedUrl;
            string normalizedRequestParameters;

            //context.Headers
            NameValueCollection pa = context.UriTemplateMatch.QueryParameters;

            if (pa != null && pa["oauth_consumer_key"] != null)
            {
                // to get uri without oauth parameters
                string uri = context.UriTemplateMatch.RequestUri.OriginalString.Replace
                    (context.UriTemplateMatch.RequestUri.Query, "");

                string consumersecret = "secret";

                OAuthBase oauth = new OAuthBase();

                string hash = oauth.GenerateSignature(
                    new Uri(uri),
                    pa["oauth_consumer_key"],
                    consumersecret,
                    null, // totken
                    null, //token secret
                    "GET",
                    pa["oauth_timestamp"],
                    pa["oauth_nonce"],
                    out normalizedUrl,
                    out normalizedRequestParameters
                    );

                Authenticated = pa["oauth_signature"] == hash;
            }

            return Authenticated;
        }

    }
}
