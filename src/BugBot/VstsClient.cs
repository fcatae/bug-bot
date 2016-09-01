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
    public class VstsCredentials
    {
        public VstsCredentials(string account, string project, string password)
        {
            this._account = account;
            this._project = project;
            this._password = password;
        }

        string _account;
        string _project;
        string _password;

        public string Url
        {
            get
            {
                return $"https://{_account}.visualstudio.com/{_project}/";
            }
        }
        public string Secret
        {
            get
            {
                return Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "", _password)));
            }
        }
    }

    public class VstsClient
    {
        VstsCredentials _credential;

        public VstsClient(VstsCredentials credential)
        {
            this._credential = credential;
        }

        class VstsBug
        {
            public int id;
            public string url;
        }

        public async Task<string> CreateBugAsync()
        {
            var fields = new dynamic[]
            {
                new { op = "add", path = "/fields/System.Title", value = "Dynamic Test" },
                new { op = "add", path = "/fields/Microsoft.VSTS.TCM.ReproSteps", value = "Description blablablablabla" }
                // new { op = "add", path = "/fields/Microsoft.VSTS.Common.Priority", value = "2" };
            };

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_credential.Url);

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", _credential.Secret);

                //set the httpmethod to Patch
                var method = new HttpMethod("PATCH");

                var body = new StringContent(JsonConvert.SerializeObject(fields), Encoding.UTF8, "application/json-patch+json");

                //send the request
                var request = new HttpRequestMessage(method, "_apis/wit/workitems/$Bug?api-version=1.0") { Content = body };
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
