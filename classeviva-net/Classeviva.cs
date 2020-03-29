using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using HtmlAgilityPack;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ClassevivaNet.Internal;

namespace ClassevivaNet
{
    public class Classeviva
    {
        private HttpClient http = new HttpClient();
        private string cookieString;
        private ResponseBody responseBody;

        /// <summary>
        /// The student's name
        /// </summary>
        public string Name
        {
            get
            {
                return responseBody.data.auth.accountInfo.name;
            }
        }

        /// <summary>
        /// The student's surname
        /// </summary>
        public string Surname
        {
            get
            {
                return responseBody.data.auth.accountInfo.surname;
            }
        }

        private Classeviva(string cookieString, ResponseBody responseBody)
        {
            this.cookieString = cookieString;
            this.responseBody = responseBody;

            http.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36");
            http.DefaultRequestHeaders.Add("Set-Cookie", $"PHPSESSID={cookieString}");
            http.DefaultRequestHeaders.Add("Cookie", $"PHPSESSID={cookieString}");
        }

        /// <summary>
        /// Logins and creates a new Classeviva session
        /// </summary>
        /// <param name="email">The user email used to login</param>
        /// <param name="password">The user password</param>
        /// <returns>A Classeviva object</returns>
        public static async Task<Classeviva> LoginAsync(string email, string password)
        {
            HttpClient client = new HttpClient();
            Dictionary<string, string> loginValues = new Dictionary<string, string>
            {
                {"uid", email },
                {"pwd", password }
            };
            FormUrlEncodedContent content = new FormUrlEncodedContent(loginValues);
            HttpResponseMessage msg = await client.PostAsync("https://web.spaggiari.eu/auth-p7/app/default/AuthApi4.php?a=aLoginPwd", content);
            string[] cookies = (string[])msg.Headers.GetValues("Set-Cookie");
            ResponseBody body = JsonConvert.DeserializeObject<ResponseBody>(await msg.Content.ReadAsStringAsync());
            Console.WriteLine(await msg.Content.ReadAsStringAsync());

            if (cookies != null)
            {
                if (body.data.auth.loggedIn && body.data.auth.verified)
                {
                    return new Classeviva(cookies[ 1 ].Substring(10, 32), body);
                }
                else
                {
                    throw new Exception("Failed to authenticate");
                }
            }
            else
            {
                throw new Exception("Invalid session token");
            }
        }

        /// <summary>
        /// Gets the student's school name
        /// </summary>
        /// <returns>A string containing the school name</returns>
        public async Task<string> GetSchool()
        {

            HttpResponseMessage msg = await http.GetAsync("https://web.spaggiari.eu/home/app/default/menu_webinfoschool_genitori.php");
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(await msg.Content.ReadAsStringAsync());
            string school = doc.DocumentNode.SelectNodes("//span[@class='scuola']")[ 0 ].InnerText;
            if (school.Length > 0)
            {
                return school;
            }
            else
            {
                throw new Exception("An error occurred");
            }
        }

        public async Task<string> GetFullName()
        {

            HttpResponseMessage msg = await http.GetAsync("https://web.spaggiari.eu/home/app/default/menu_webinfoschool_genitori.php");
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(await msg.Content.ReadAsStringAsync());
            string school = doc.DocumentNode.SelectNodes("//span[@class='name']")[ 0 ].InnerText;
            if (school.Length > 0)
            {
                return school;
            }
            else
            {
                throw new Exception("An error occurred");
            }
        }
    }
}

