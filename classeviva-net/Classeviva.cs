using System;
using System.Collections.Generic;
using System.Net.Http;
using HtmlAgilityPack;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ClassevivaNet.Internal;
using System.Linq;
using System.Text;
using System.Net.Mime;
using System.Net.Http.Headers;

namespace ClassevivaNet
{
    public class Classeviva
    {
        private const string ApplicationJsonContentType = "application/json";
        private const string BaseUrl = "https://web.spaggiari.eu/rest/v1";

        private const string LoginPath = "/auth/login";

        private HttpClient _http = new HttpClient();

        private Classeviva(StudentInfo responseBody)
        {

            _http.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36");

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
                {"pass", password }
            };

            client.DefaultRequestHeaders.Add("User-Agent", "zorro/1.0");
            client.DefaultRequestHeaders.Add("Z-Dev-Apikey", "+zorro+");

            HttpContent content = new StringContent(JsonConvert.SerializeObject(loginValues), Encoding.UTF8, ApplicationJsonContentType);
            
            HttpResponseMessage msg = await client.PostAsync(BaseUrl + LoginPath, content);
            msg.EnsureSuccessStatusCode();

            return new Classeviva(JsonConvert.DeserializeObject<StudentInfo>(await msg.Content.ReadAsStringAsync()));
        }

        /// <summary>
        /// Gets the student's school name
        /// </summary>
        /// <returns>A string containing the school name</returns>
        public async Task<string> GetSchoolAsync()
        {

            HttpResponseMessage msg = await _http.GetAsync("https://web.spaggiari.eu/home/app/default/menu_webinfoschool_genitori.php");
            //temp
            return null;
        }

        /// <summary>
        /// Gets the student's full name WARNING: This method is slower than the Name and the Surname properties, but is included for consisency
        /// </summary>
        /// <returns>A string containing the students full name</returns>
        public async Task<string> GetFullNameAsync()
        {

            HttpResponseMessage msg = await _http.GetAsync("https://web.spaggiari.eu/home/app/default/menu_webinfoschool_genitori.php");
            return null;
        }

        /// <summary>
        /// Gets the student's full grade history
        /// </summary>
        /// <returns>An array of Grade objects that contain grade data</returns>
        public async Task<Grade[]> GetGradesAsync()
        {
            HttpResponseMessage msg = await _http.GetAsync("https://web.spaggiari.eu/cvv/app/default/genitori_note.php?filtro=tutto");
            return null;
        }

        /// <summary>
        /// Returns all the student's school material files
        /// </summary>
        /// <returns>An array of MaterialFile objects that contain all the file data</returns>
        public async Task<MaterialFile[]> GetFilesAsync()
        {
            HttpResponseMessage msg = await _http.GetAsync("https://web.spaggiari.eu/fml/app/default/didattica_genitori.php");
            return null;
        }

        /// <summary>
        /// Gets all the homework assigned within the DateTime range given
        /// </summary>
        /// <param name="startDate">Start date</param>
        /// <param name="endDate">End date</param>
        /// <returns>An array of homework objects</returns>
        public async Task<Homework[]> GetHomeworkAsync(DateTime startDate, DateTime endDate)
        {
            HttpResponseMessage msg = await _http.GetAsync(
                $"https://web.spaggiari.eu/fml/app/default/agenda_studenti.php?ope=get_events&classe_id=&gruppo_id=&start=" +
                startDate.Subtract(new DateTime(1970, 1, 1)).TotalSeconds.ToString() + "&end=" +
                endDate.Subtract(new DateTime(1970, 1, 1)).TotalSeconds.ToString());
            HomeworkBody[] homeworkBody = JsonConvert.DeserializeObject<HomeworkBody[]>(await msg.Content.ReadAsStringAsync());
            if (homeworkBody != null)
            {
                Homework[] homework = new Homework[homeworkBody.Length];
                for (int i = 0; i < homeworkBody.Length; i++)
                {
                    homework[i] = new Homework(
                        homeworkBody[i].id,
                        homeworkBody[i].title,
                        DateTime.ParseExact(homeworkBody[i].start, "yyyy-MM-dd HH:mm:ss", null),
                        DateTime.ParseExact(homeworkBody[i].end, "yyyy-MM-dd HH:mm:ss", null),
                        homeworkBody[i].allDay,
                        DateTime.ParseExact(homeworkBody[i].data_inserimento, "dd-MM-yyyy HH:mm:ss", null),
                        homeworkBody[i].autore_desc,
                        homeworkBody[i].nota_2);
                }
                return homework;
            }
            else return null;
        }
    }
}