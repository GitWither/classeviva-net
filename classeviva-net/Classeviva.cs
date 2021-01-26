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
using System.IO;
using ClassevivaNet.Objects;

namespace ClassevivaNet
{
    public class Classeviva
    {
        private const string DateFormat = "yyyyMMdd";

        private const string ApplicationJsonContentType = "application/json";
        private const string BaseUrl = "https://web.spaggiari.eu/rest/v1";

        private const string LoginPath = "/auth/login/";
        private const string HomeworkPath = "/students/{0}/agenda/all/";
        private const string GradesPath = "/students/{0}/grades/";
        private const string LessonsPath = "/students/{0}/lessons/";
        private const string DocumentsPath = "/students/{0}/noticeboard";
        private const string DidacticsPath = "/students/{0}/didactics";

        private readonly StudentInfo _studentInfo;

        private readonly HttpClient _http = new HttpClient();

        public string FirstName
        {
            get => _studentInfo.FirstName;
        }

        public string LastName
        {
            get => _studentInfo.LastName;
        }

        public bool IsValid
        {
            get
            {
                return (this._studentInfo.ExpireTime - this._studentInfo.LoggedIn).TotalMinutes <= 90;
            }
        }

        private Classeviva(StudentInfo studentInfo)
        {
            this._studentInfo = studentInfo;

            _http.DefaultRequestHeaders.Add("User-Agent", "zorro/1.0");
            _http.DefaultRequestHeaders.Add("Z-Dev-Apikey", "+zorro+");
            _http.DefaultRequestHeaders.Add("Z-Auth-Token", studentInfo.Token);
        }

        /// <summary>
        /// Logins and creates a new Classeviva session
        /// </summary>
        /// <param name="email">The user email used to login</param>
        /// <param name="password">The user password</param>
        /// <param name="reconnect">If true, will automatically reconnect if the token expires</param>
        /// <returns>A Classeviva object</returns>
        public static async Task<Classeviva> LoginAsync(string email, string password, bool reconnect)
        {
            HttpClient client = new HttpClient();

            Dictionary<string, string> loginValues = new Dictionary<string, string>
            {
                {"uid", email },
                {"pass", password }
            };

            //client.DefaultRequestHeaders.Add("User-Agent", "zorro/1.0");
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
            HttpResponseMessage msg = await _http.GetAsync(BaseUrl + string.Format(GradesPath, _studentInfo.GetFormattedToken()));
            msg.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<GradesResponse>(await msg.Content.ReadAsStringAsync()).Grades;
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

        public async Task<Lesson[]> GetLessonsAsync(DateTime date)
        {
            HttpResponseMessage msg = await _http.GetAsync(BaseUrl + string.Format(LessonsPath, _studentInfo.GetFormattedToken()) + date.ToString(DateFormat));
            msg.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<LessonsResponse>(await msg.Content.ReadAsStringAsync()).Lessons;
        }

        public async Task<Document[]> GetDocumentsAsync()
        {
            HttpResponseMessage msg = await _http.GetAsync(BaseUrl + string.Format(DocumentsPath, _studentInfo.GetFormattedToken()));
            msg.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<DocumentsResponse>(await msg.Content.ReadAsStringAsync()).Documents;
        }


        public async Task<byte[]> GetDocumentDataAsync(Document document)
        {
            HttpResponseMessage msg = await _http.GetAsync(BaseUrl + string.Format(DocumentsPath, _studentInfo.GetFormattedToken()) + "/attach/" + 
                document.Code + "/" + document.Id);
            msg.EnsureSuccessStatusCode();

            return await msg.Content.ReadAsByteArrayAsync();
        }

        public async Task<Content[]> GetDidacticsAsync()
        {
            HttpResponseMessage msg = await _http.GetAsync(BaseUrl + string.Format(DidacticsPath, _studentInfo.GetFormattedToken()));
            msg.EnsureSuccessStatusCode();

            Console.WriteLine(await msg.Content.ReadAsStringAsync());

            DidacticsResponse test = JsonConvert.DeserializeObject<DidacticsResponse>(await msg.Content.ReadAsStringAsync());

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
            HttpResponseMessage msg = await _http.GetAsync(BaseUrl + string.Format(HomeworkPath, _studentInfo.GetFormattedToken()) + 
                startDate.ToString(DateFormat) + "/" + endDate.ToString(DateFormat)
                );
            msg.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<HomeworkResponse>(await msg.Content.ReadAsStringAsync()).Homework;
        }
    }
}