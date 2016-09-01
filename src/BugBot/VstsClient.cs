using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BugBot
{
    public class VstsClient
    {
        class VstsBug
        {
            public int id;
            public string url;
        }

        public async Task<string> CreateBugAsync(string accountName, string projectName, string personalAccessToken)
        {
            string credentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "", personalAccessToken)));

            var fields = new dynamic[]
            {
                new { op = "add", path = "/fields/System.Title", value = "Dynamic Test" },
                new { op = "add", path = "/fields/Microsoft.VSTS.TCM.ReproSteps", value = "Description blablablablabla" }
                // new { op = "add", path = "/fields/Microsoft.VSTS.Common.Priority", value = "2" };
            };

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);


                //set the httpmethod to Patch
                var method = new HttpMethod("PATCH");

                string url = $"https://{accountName}.visualstudio.com/{projectName}/_apis/wit/workitems/$Bug?api-version=1.0";

                var body = new StringContent(JsonConvert.SerializeObject(fields), Encoding.UTF8, "application/json-patch+json");

                //send the request
                var request = new HttpRequestMessage(method, url) { Content = body };
                var response = await client.SendAsync(request);

                // if error, throw exception
                response.EnsureSuccessStatusCode();

                string data = await response.Content.ReadAsStringAsync();
                var bug = JsonConvert.DeserializeObject<VstsBug>(data);

                return bug.url;
            }
        }

    }
}
