using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugBot
{
    public class BotFrameworkCredentials
    {
        public BotFrameworkCredentials()
        {

        }

        public string MicrosoftAppId { get; set; }
        public string MicrosoftAppPassword { get; set; }
    }

    public class VstsCredentials
    {
        public VstsCredentials(string url, string password)
        {
            this.Url = url;
            this.Secret = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "", password))); ;
        }

        public VstsCredentials(string account, string project, string password)
        {
            this.Url = $"https://{account}.visualstudio.com/{project}/";
            this.Secret = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "", password)));
        }

        public string Url { get; private set; }
        public string Secret { get; private set; }
    }
}
