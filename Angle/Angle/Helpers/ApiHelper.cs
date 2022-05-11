using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;


namespace Angle.Helpers
{
    public class ApiHelper
    {
        static string BASEURL = "https://localhost:44325/";


        public static object Post(string resource, object model, string token="")
        {
            

            //Api projesine bağlanacağımız zaman RestSharp paketini kurmamız gerekiyor
            var client = new RestClient(BASEURL);
            var request = new RestRequest(resource, Method.POST);
            //AddHeader gönderilen mesaja yeni bir değer katar
            request.AddHeader("Content-Type", "application/json");
            if (!String.IsNullOrEmpty(token))
            {
                request.AddHeader("Authorization" , ""+ token);
            }
            request.AddJsonBody(model);
            IRestResponse response = client.Execute(request);

            return response;
        }
    }
}
