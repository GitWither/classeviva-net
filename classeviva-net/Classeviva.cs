using System;
using System.Collections.Generic;
using System.Net.Http;
using HtmlAgilityPack;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ClassevivaNet.Internal;
using System.Linq;

namespace ClassevivaNet
{
    public class Classeviva
    {
        private HttpClient _http = new HttpClient();
        private ResponseBody _responseBody;

        /// <summary>
        /// The student's name
        /// </summary>
        public string Name
        {
            get
            {
                return _responseBody.data.auth.accountInfo.name;
            }
        }

        /// <summary>
        /// The student's surname
        /// </summary>
        public string Surname
        {
            get
            {
                return _responseBody.data.auth.accountInfo.surname;
            }
        }

        private Classeviva(string cookieString, ResponseBody responseBody)
        {
            this._responseBody = responseBody;

            _http.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36");
            _http.DefaultRequestHeaders.Add("Set-Cookie", $"PHPSESSID={cookieString}");
            _http.DefaultRequestHeaders.Add("Cookie", $"PHPSESSID={cookieString}");
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
                    return new Classeviva(cookies[1].Substring(10, 32), body);
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

            HttpResponseMessage msg = await _http.GetAsync("https://web.spaggiari.eu/home/app/default/menu_webinfoschool_genitori.php");
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(await msg.Content.ReadAsStringAsync());
            string school = doc.DocumentNode.SelectNodes("//span[@class='scuola']")[0].InnerText;
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

            HttpResponseMessage msg = await _http.GetAsync("https://web.spaggiari.eu/home/app/default/menu_webinfoschool_genitori.php");
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(await msg.Content.ReadAsStringAsync());
            string school = doc.DocumentNode.SelectNodes("//span[@class='name']")[0].InnerText;
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
        /// Gets the student's full grade history
        /// </summary>
        /// <returns>An array of Grade objects that contain grade data</returns>
        public async Task<Grade[]> GetGradesAsync()
        {
            HttpResponseMessage msg = await _http.GetAsync("https://web.spaggiari.eu/cvv/app/default/genitori_note.php?filtro=tutto");
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(await msg.Content.ReadAsStringAsync());
            HtmlNode[] dataTableNodes = doc.GetElementbyId("data_table").ChildNodes.ToArray();
            List<Grade> grades = new List<Grade>();
            string currentSubject = string.Empty;
            for (int i = 10; i < dataTableNodes.Length; i++)
            {

                bool isTitle =
                    dataTableNodes[i].GetAttributeValue("align", "none") == "center" &&
                    dataTableNodes[i].GetAttributeValue("height", "none") == "38" &&
                    !dataTableNodes[i].HasClass("griglia");
                bool isGradeObject =
                    dataTableNodes[i].ChildNodes.Count > 0 &&
                    dataTableNodes[i].Attributes.Count == 0;

                if (isTitle && currentSubject != dataTableNodes[i].InnerText)
                {
                    currentSubject = dataTableNodes[i].InnerText.Trim();
                }
                else if (isGradeObject)
                {
                    DateTime date = DateTime.MinValue;
                    string comment = string.Empty;
                    string type = string.Empty;
                    string stringGrade = string.Empty;
                    bool countsTowardsAverage = true;
                    foreach (HtmlNode node in dataTableNodes[i].ChildNodes)
                    {
                        bool isDate =
                            node.GetAttributeValue("colspan", "none") == "6";
                        bool isType =
                            node.GetAttributeValue("colspan", "none") == "5";
                        bool isGrade =
                            node.GetAttributeValue("colspan", "none") == "2" &&
                            node.HasClass("voto_");
                        bool isComment =
                            node.GetAttributeValue("colspan", "none") == "32" &&
                            node.HasClass("handwriting") &&
                            node.HasClass("graytext") &&
                            node.ChildNodes.Count > 0;
                        if (isDate)
                        {
                            date = DateTime.ParseExact(node.InnerText.Trim(), "dd/MM/yyyy", null);
                        }
                        else if (isType)
                        {
                            type = node.InnerText.Trim();
                        }
                        else if (isComment)
                        {
                            string temp = node.InnerText.Trim();
                            comment = temp;
                        }
                        else if (isGrade)
                        {
                            countsTowardsAverage = !node.ChildNodes[1].HasClass("f_reg_voto_dettaglio");
                            stringGrade = node.InnerText.Trim().Replace("½", ".5").Replace(",", ".").Replace(" ", "");
                        }
                    }

                    if (date != DateTime.MinValue && stringGrade != string.Empty && currentSubject != string.Empty)
                    {
                        if (double.TryParse(stringGrade, out double numberGrade))
                        {
                            grades.Add(new NumericGrade(numberGrade, comment, date, currentSubject, type, countsTowardsAverage));
                        }
                        else
                        {
                            grades.Add(new TextGrade(stringGrade, comment, date, currentSubject, type, countsTowardsAverage));
                        }
                    }
                }
            }
            return grades.ToArray();
        }

        /// <summary>
        /// Returns all the student's school material files
        /// </summary>
        /// <returns>An array of MaterialFile objects that contain all the file data</returns>
        public async Task<MaterialFile[]> GetFilesAsync()
        {
            HttpResponseMessage msg = await _http.GetAsync("https://web.spaggiari.eu/fml/app/default/didattica_genitori.php");
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(await msg.Content.ReadAsStringAsync());
            HtmlNode[] dataTableNodes = doc.GetElementbyId("data_table").ChildNodes.ToArray();
            List<MaterialFile> files = new List<MaterialFile>();
            string currentAuthor = string.Empty;
            string currentFolder = string.Empty;
            for (int i = 10; i < dataTableNodes.Length; i++)
            {
                bool isFileNode =
                    dataTableNodes[i].HasClass("row") &&
                    dataTableNodes[i].HasClass("contenuto");

                bool isFolder =
                    dataTableNodes[i].HasClass("row") &&
                    dataTableNodes[i].HasClass("row_parent");

                bool isAuthor =
                    dataTableNodes[i].GetAttributeValue("style", "none") == "height: 40px;";


                if (isAuthor)
                {
                    foreach (HtmlNode node in dataTableNodes[i].ChildNodes)
                    {
                        bool isAuthorNode =
                            node.GetAttributeValue("colspan", "none") == "12";

                        if (isAuthorNode)
                        {
                            currentAuthor = node.InnerText.Trim();
                        }
                    }
                }

                if (isFolder)
                {
                    foreach (HtmlNode node in dataTableNodes[i].ChildNodes)
                    {
                        bool isFolderNode =
                            node.GetAttributeValue("colspan", "none") == "44" &&
                            node.HasClass("folder");
                        if (isFolderNode)
                        {
                            currentFolder = node.ChildNodes[0].InnerText.Trim();
                        }
                    }
                }

                if (isFileNode)
                {
                    string description = string.Empty;
                    string link = string.Empty;
                    MaterialFileType materialFileType = MaterialFileType.File;
                    DateTime date = DateTime.MinValue;
                    foreach (HtmlNode node in dataTableNodes[i].ChildNodes)
                    {
                        bool isContent =
                            node.GetAttributeValue("colspan", "none") == "36" &&
                            node.HasClass("contenuto_desc");
                        bool isButton =
                            node.GetAttributeValue("colspan", "none") == "4" &&
                            node.HasClass("contenuto_action");

                        if (isContent)
                        {
                            foreach (HtmlNode subNode in node.ChildNodes[1].ChildNodes) {
                                bool isDescription =
                                    subNode.HasClass("row_contenuto_desc") &&
                                    subNode.HasClass("font_size_16");
                                bool isDate =
                                    !subNode.HasClass("row_contenuto_desc") &&
                                    subNode.HasClass("font_size_9");

                                if (isDescription)
                                {
                                    description = subNode.InnerText.Trim();
                                }
                                if (isDate)
                                {
                                    date = DateTime.ParseExact(subNode.InnerText.Replace("condiviso il: ", "").Trim(), "dd-MM-yyyy HH:mm:ss", null);
                                }
                            }
                        }
                        if (isButton)
                        {
                            bool isLink = node.ChildNodes[1].HasClass("action_link");
                            bool isFile = node.ChildNodes[1].HasClass("action_download");
                            if (isLink)
                            {
                                link = node.ChildNodes[1].GetAttributeValue("ref", "none");
                                materialFileType = MaterialFileType.Link;
                            }
                            if (isFile)
                            {
                                link = $"https://web.spaggiari.eu/fml/app/default/didattica_genitori.php?a=downloadContenuto&contenuto_id={node.ChildNodes[1].GetAttributeValue("contenuto_id", "none")}&cksum={node.ChildNodes[1].GetAttributeValue("cksum", "none")}";
                                materialFileType = MaterialFileType.File;
                            }
                        }
                    }
                    files.Add(new MaterialFile(currentAuthor, description, link, currentFolder, materialFileType, date));
                }
            }
            return files.ToArray();
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