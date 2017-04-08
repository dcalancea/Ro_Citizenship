using System.Net.Http;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using WebApplication1.Models;
using System.Globalization;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using HtmlAgilityPack;

namespace WebApplication1.Checker
{
    public class Checker : IChecker
    {
        private Uri _dossierStateUrl;
        private Uri _ordersUrl;
        private string _dossierDownloadUrl;
        private string _ordersDownloadUrl;

        public Checker(IOptions<AppSettings> settings)
        {
            //TODO: initialize stuff like connections, clients, PDF parsers etc....
            var ConfigSettings = settings.Value;
            _dossierStateUrl = new Uri(ConfigSettings.DossierStateUrl);
            _ordersUrl = new Uri(ConfigSettings.OrdersUrl);
            _dossierDownloadUrl = ConfigSettings.DossierDownloadUrl;
            _ordersDownloadUrl = ConfigSettings.OrdersDownloadUrl;
        }

        public bool CheckName(string firstName, string lastName)
        {
            //var dossiersDownloaded = DownloadFiles(_dossierStateUrl, _dossierDownloadUrl);
            //var ordersDownloaded = DownloadFiles(_ordersUrl, _ordersDownloadUrl);

            //Task.WaitAll(new Task[2] { dossiersDownloaded, ordersDownloaded });

            //TODO: implement
            
            return true;
        }

        public IEnumerable<User> GetRemoteUsers()
        {
            var orders = Directory.GetFiles(_ordersDownloadUrl)
                .Select(fileName => GetOrder(fileName)).ToList();

            var dossiers = Directory.GetFiles(_dossierDownloadUrl)
                .Select(fileName => GetDossierFileUsers(fileName)).ToList();

            var joinedUsers = from du in dossiers.SelectMany(x => x)
                              join ou in orders.SelectMany(o => o.Users).DefaultIfEmpty() on du.DossierNr equals ou.DossierNr
                              select new User
                              {
                                  DossierNr = du.DossierNr,
                                  FirstName = ou.FirstName,
                                  LastName = ou.LastName,
                                  OrderNr = ou.OrderNr,
                                  RegisterDate = du.RegisterDate,
                                  ResolutionDate = du.ResolutionDate,
                                  Term = du.Term
                              };

            return joinedUsers.ToList();
        }

        public Order GetOrder(string filePath)
        {
            var lines = GetPdfLines(filePath);
            var order = new Order();
            order.Id = lines.FirstOrDefault(l => l.StartsWith("O R D I N") || l.StartsWith("ORDIN"))
                .Split(new string[] { "Nr.", "NR." }, StringSplitOptions.None)
                .Last()
                .Trim();

            var userLines = lines.Where(l => Regex.Match(l, "^\\d+\\.\\D").Success);
            order.Users = userLines
                .Select(l => 
                {
                    var firstSplit = l.Split('.')[1].Trim(new char[] { ';', ' ' }).Split('(');
                    var firstLast = firstSplit.First().Trim().Split(' ');
                    string name = firstLast.Last();
                    string surname = String.Join(" ", firstLast.Take(firstLast.Length - 1));
                    string dossierNr = firstSplit.Last().Split(')').First().Trim();

                    return new User()
                    {
                        FirstName = name,
                        LastName = surname,
                        DossierNr = dossierNr,
                        OrderNr = order.Id
                    };
                });
            return order;
        }

        public IEnumerable<User> GetDossierFileUsers(string filePath)
        {
            var lines = GetPdfLines(filePath);
            var userLines = lines.Where(l => Regex.Match(l, "^\\d+/").Success).ToList();
            
            var users = userLines.Select(l =>
            {
                var columns = l.Split(' ');

                var dossierNumberSplit = columns[0].Split(new string[] { "RD/" }, StringSplitOptions.None);
                DateTime registerDate = DateTime.MinValue;
                DateTime.TryParse(columns[1], out registerDate);

                DateTime term = DateTime.MinValue;
                DateTime resolutionDate = DateTime.MinValue;
                string orderNumber = String.Empty;
                string solutionString = String.Empty;

                DateTime value;
                if(columns.Length >= 3)
                {
                    if (DateTime.TryParse(columns[2], out value))
                    {
                        term = value;

                        if (columns.Length >= 4)
                        {
                            solutionString = columns[3];
                        }
                        else
                        {
                            solutionString = columns[2];
                        }

                    }
                    else
                    {
                        solutionString = columns[2];
                    }
                }

                var splitResolution = solutionString.Split('/');
                DateTime.TryParseExact(splitResolution.Last(), "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out resolutionDate);
                orderNumber = String.Join("/", splitResolution.Take(splitResolution.Length - 1));

                return new User()
                {
                    DossierNr = String.Join("", dossierNumberSplit),
                    OrderNr = orderNumber,
                    RegisterDate = registerDate,
                    Term = term,
                    ResolutionDate = resolutionDate
                };
            })
            .Where(u => u != null)
            .ToList();

            return users;
        }

        public IEnumerable<string> GetPdfLines(string filePath)
        {
            PdfReader reader = new PdfReader(filePath);
            var sb = new StringBuilder();
            for (int i = 0; i < reader.NumberOfPages; i++)
            {
                sb.Append(PdfTextExtractor.GetTextFromPage(reader, i + 1));
            }

            return sb.ToString().Replace("NR. DOSAR", "\nNR. DOSAR").Split('\n').Select(l => l.Trim());
        }

        public Task DownloadDossierFiles()
        {
            return DownloadFiles(_dossierStateUrl, _dossierDownloadUrl);
        }

        public Task DownloadOrderFiles()
        {
            return DownloadFiles(_ordersUrl, _ordersDownloadUrl);
        }

        public async Task DownloadFiles(Uri sourceUrl, string outputFolder)
        {
            var html = new HtmlDocument();
            html.LoadHtml(await new HttpClient().GetStringAsync(sourceUrl));
            var nodes = html.DocumentNode.Descendants("div")
                .Where(d =>
                    d.Attributes.Contains("class")
                        &&
                    d.Attributes["class"].Value.Contains("item-page")
                ).ToList();

            string dossierBaseUrl = _dossierStateUrl.GetComponents(UriComponents.Scheme | UriComponents.StrongAuthority, UriFormat.Unescaped);
            var links = nodes[0].Descendants("a")
                .Where(n => n.Attributes.Contains("href") && n.Attributes["href"].Value.EndsWith(".pdf"))
                .Select(a => new Uri(dossierBaseUrl + a.Attributes["href"].Value)).ToList();

            Directory.GetFiles(outputFolder).ToList().ForEach(path => File.Delete(path));
            Task.WaitAll(links.Select(l => DownloadFile(l, outputFolder)).ToArray());
        }

        public async Task DownloadFile(Uri fileUri, string outputFolder)
        {
            var client = new HttpClient();
            var inputStream = await client.GetStreamAsync(fileUri);
            
            string fileName = fileUri.ToString().Split('/').Last();
            string outputPath = System.IO.Path.Combine(outputFolder, fileName);


            using (var fileStream = File.Create(outputPath))
            {
                inputStream.CopyTo(fileStream);
            }
        }
    }
    
}
