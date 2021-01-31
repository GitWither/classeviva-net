using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ClassevivaNet.Internal;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

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
        private const string CardsPath = "/students/{0}/cards";

        private StudentInfo _studentInfo;

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

        public bool Reconnect { get; private set; }

        private Classeviva(StudentInfo studentInfo, bool reconnect)
        {
            this._studentInfo = studentInfo;
            this.Reconnect = reconnect;

            _http.DefaultRequestHeaders.Add("User-Agent", "zorro/1.0");
            _http.DefaultRequestHeaders.Add("Z-Dev-Apikey", "+zorro+");
            _http.DefaultRequestHeaders.Add("Z-Auth-Token", studentInfo.Token);
        }

        private static async Task<StudentInfo> LoginAsync(string email, string password)
        {
            using (HttpClient client = new HttpClient())
            {
                Dictionary<string, string> loginValues = new Dictionary<string, string>
                {
                    {"uid", email },
                    {"pass", password }
                };

                client.DefaultRequestHeaders.Add("User-Agent", "zorro/1.0");
                client.DefaultRequestHeaders.Add("Z-Dev-Apikey", "+zorro+");

                using (HttpContent content = new StringContent(JsonConvert.SerializeObject(loginValues), Encoding.UTF8, ApplicationJsonContentType))
                {
                    using (HttpResponseMessage msg = await client.PostAsync(BaseUrl + LoginPath, content))
                    {
                        msg.EnsureSuccessStatusCode();

                        StudentInfo info = JsonConvert.DeserializeObject<StudentInfo>(await msg.Content.ReadAsStringAsync());

                        info.Email = email;
                        info.Password = password;

                        return info;
                    }
                }
            }
        }

        private async Task ReconnectAsync()
        {
            _studentInfo = await LoginAsync(_studentInfo.Email, _studentInfo.Password);

            _http.DefaultRequestHeaders.Add("Z-Auth-Token", _studentInfo.Token);
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
            return new Classeviva(await LoginAsync(email, password), reconnect);
        }

        /// <summary>
        /// Gets the student's school name
        /// </summary>
        /// <returns>A string containing the school name</returns>
        public async Task<string> GetSchoolNameAsync()
        {
            if (!IsValid) await ReconnectAsync();

            using (HttpResponseMessage msg = await _http.GetAsync(BaseUrl + string.Format(CardsPath, _studentInfo.GetFormattedToken())))
            {
                msg.EnsureSuccessStatusCode();
                return JsonConvert.DeserializeObject<CardsReponse>(await msg.Content.ReadAsStringAsync()).Cards[0].SchoolName;
            }
        }

        public async Task<string> GetSchoolTypeAsync()
        {
            if (!IsValid) await ReconnectAsync();

            using (HttpResponseMessage msg = await _http.GetAsync(BaseUrl + string.Format(CardsPath, _studentInfo.GetFormattedToken())))
            {
                msg.EnsureSuccessStatusCode();
                return JsonConvert.DeserializeObject<CardsReponse>(await msg.Content.ReadAsStringAsync()).Cards[0].SchoolType;
            }
        }

        public async Task<string> GetFiscalCodeAsync()
        {
            if (!IsValid) await ReconnectAsync();

            using (HttpResponseMessage msg = await _http.GetAsync(BaseUrl + string.Format(CardsPath, _studentInfo.GetFormattedToken())))
            {
                msg.EnsureSuccessStatusCode();
                return JsonConvert.DeserializeObject<CardsReponse>(await msg.Content.ReadAsStringAsync()).Cards[0].FiscalCode;
            }
        }

        /// <summary>
        /// Gets the student's full grade history
        /// </summary>
        /// <returns>An array of Grade objects that contain grade data</returns>
        public async Task<Grade[]> GetGradesAsync()
        {
            if (!IsValid) await ReconnectAsync();

            using (HttpResponseMessage msg = await _http.GetAsync(BaseUrl + string.Format(GradesPath, _studentInfo.GetFormattedToken())))
            {
                msg.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<GradesResponse>(await msg.Content.ReadAsStringAsync()).Grades;
            }
        }

        public async Task<Lesson[]> GetLessonsAsync(DateTime date)
        {
            if (!IsValid) await ReconnectAsync();

            using (HttpResponseMessage msg = await _http.GetAsync(BaseUrl + string.Format(LessonsPath, _studentInfo.GetFormattedToken()) + date.ToString(DateFormat)))
            {
                msg.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<LessonsResponse>(await msg.Content.ReadAsStringAsync()).Lessons;
            }
        }

        public async Task<Document[]> GetDocumentsAsync()
        {
            if (!IsValid) await ReconnectAsync();

            using (HttpResponseMessage msg = await _http.GetAsync(BaseUrl + string.Format(DocumentsPath, _studentInfo.GetFormattedToken())))
            {
                msg.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<DocumentsResponse>(await msg.Content.ReadAsStringAsync()).Documents;
            }
        }


        public async Task<byte[]> GetDocumentDataAsync(Document document)
        {
            if (!IsValid) await ReconnectAsync();


            using (HttpResponseMessage msg = await _http.GetAsync(BaseUrl + string.Format(DocumentsPath, _studentInfo.GetFormattedToken()) + "/attach/" +
                document.Code + "/" + document.Id))
            {
                msg.EnsureSuccessStatusCode();

                return await msg.Content.ReadAsByteArrayAsync();
            }
        }

        public async Task<Content[]> GetDidacticsAsync()
        {
            if (!IsValid) await ReconnectAsync();

            using (HttpResponseMessage msg = await _http.GetAsync(BaseUrl + string.Format(DidacticsPath, _studentInfo.GetFormattedToken())))
            {
                msg.EnsureSuccessStatusCode();

                DidacticsResponse didactics = JsonConvert.DeserializeObject<DidacticsResponse>(await msg.Content.ReadAsStringAsync());

                List<Content> contents = new List<Content>();
                foreach (Teacher teacher in didactics.Didactics)
                {
                    foreach (Folder folder in teacher.Folders)
                    {
                        foreach (Content content in folder.Contents)
                        {
                            content.FolderName = folder.FolderName;
                            content.TeacherName = teacher.TeacherName;
                            contents.Add(content);
                        }
                    }
                }

                return contents.ToArray();
            }
        }

        public async Task<byte[]> GetDidacticsDataAsync(Content content)
        {
            if (!IsValid) await ReconnectAsync();

            using (HttpResponseMessage msg = await _http.GetAsync(BaseUrl + string.Format(DidacticsPath, _studentInfo.GetFormattedToken()) + "/item/" + content.ContentId))
            {
                msg.EnsureSuccessStatusCode();

                return await msg.Content.ReadAsByteArrayAsync();
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
            if (!IsValid) await ReconnectAsync();

            using (HttpResponseMessage msg = await _http.GetAsync(BaseUrl + string.Format(HomeworkPath, _studentInfo.GetFormattedToken()) +
                startDate.ToString(DateFormat) + "/" + endDate.ToString(DateFormat)))
            {
                msg.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<HomeworkResponse>(await msg.Content.ReadAsStringAsync()).Homework;
            }
        }
    }
}