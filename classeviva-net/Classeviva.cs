using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using HtmlAgilityPack;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ClassevivaNet.Internal;
using System.Linq;

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
        public async Task<string> GetSchoolAsync()
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

        /// <summary>
        /// Gets the student's full name WARNING: This method is slower than the Name and the Surname properties, but is included for consisency
        /// </summary>
        /// <returns>A string containing the students full name</returns>
        public async Task<string> GetFullNameAsync()
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

        public async Task GetGrades()
        {
            HttpResponseMessage msg = await http.GetAsync("https://web.spaggiari.eu/cvv/app/default/genitori_note.php?filtro=tutto");
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(await msg.Content.ReadAsStringAsync());
            HtmlNode[] dataTableNodes = doc.GetElementbyId("data_table").ChildNodes.ToArray();
            Console.WriteLine(dataTableNodes.Length);
            for (int i = 10; i < dataTableNodes.Length; i++)
            {
                Console.WriteLine(dataTableNodes[i].GetClasses()?.ToArray()[0]);
            }
        }

        /// <summary>
        /// Gets all the homework assigned within the DateTime range given
        /// </summary>
        /// <param name="startDate">Start date</param>
        /// <param name="endDate">End date</param>
        /// <returns>An array of homework objects</returns>
        public async Task<Homework[]> GetHomeworkAsync(DateTime startDate, DateTime endDate)
        {
            HttpResponseMessage msg = await http.GetAsync(
                $"https://web.spaggiari.eu/fml/app/default/agenda_studenti.php?ope=get_events&classe_id=&gruppo_id=&start=" +
                startDate.Subtract(new DateTime(1970, 1, 1)).TotalSeconds.ToString() + "&end=" +
                endDate.Subtract(new DateTime(1970, 1, 1)).TotalSeconds.ToString());
            HomeworkBody[] homeworkBody = JsonConvert.DeserializeObject<HomeworkBody[]>(await msg.Content.ReadAsStringAsync());
            Homework[] homework = new Homework[ homeworkBody.Length ];
            for (int i = 0; i < homeworkBody.Length; i++)
            {
                homework[ i ] = new Homework(
                    homeworkBody[ i ].id,
                    homeworkBody[ i ].title,
                    DateTime.ParseExact(homeworkBody[ i ].start, "yyyy-MM-dd HH:mm:ss", null),
                    DateTime.ParseExact(homeworkBody[ i ].end, "yyyy-MM-dd HH:mm:ss", null),
                    homeworkBody[ i ].allDay,
                    DateTime.ParseExact(homeworkBody[ i ].data_inserimento, "dd-MM-yyyy HH:mm:ss", null),
                    homeworkBody[ i ].autore_desc,
                    homeworkBody[ i ].nota_2);
            }
            return homework;
        }
    }
}

