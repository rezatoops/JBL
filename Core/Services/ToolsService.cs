using Core.Services.Interfaces;
using ServiceReference1;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.Services
{
    public class ToolsService : IToolsService
    {
        public void SendCodeWithSMS(string code, string phoneNumber)
        {
            const string username = "9123125029";
            const string password = "EP1HS";
            SendSoapClient soapClient = new SendSoapClient(SendSoapClient.EndpointConfiguration.SendSoap);
            soapClient.SendByBaseNumberAsync(username, password, new string[] { code }, phoneNumber, 257483);
        }

        public void SendSMS(string message, string phoneNumber)
        {

            //message = "فروشگاه نیش کافه: \n" + message;
            Uri apiBaseAddress = new Uri("https://console.melipayamak.com");
            using (HttpClient client = new HttpClient() { BaseAddress = apiBaseAddress })
            {
                // You may need to Install-Package Microsoft.AspNet.WebApi.Client
                var result = client.PostAsJsonAsync("api/send/simple/18c12848f49040a98b20b6a25f71c437",
                    new { from = "50002710025029", to = phoneNumber, text = message }).Result;
                var response = result.Content.ReadAsStringAsync().Result;
            }
        }
        public List<int> GetNavNumbers(int currentPage, int allPage)
        {
            List<int> navNumbers = new List<int>();
            if (allPage < 5)
            {
                for (int i = 1; i <= allPage; i++)
                {
                    navNumbers.Add(i);
                }
            }
            else
            {
                if (currentPage == 1)
                {
                    navNumbers.Add(currentPage);
                    navNumbers.Add(currentPage + 1);
                    navNumbers.Add(currentPage + 2);
                    navNumbers.Add(currentPage + 3);
                    navNumbers.Add(currentPage + 4);
                }
                else if (currentPage == 2)
                {
                    navNumbers.Add(currentPage - 1);
                    navNumbers.Add(currentPage);
                    navNumbers.Add(currentPage + 1);
                    navNumbers.Add(currentPage + 2);
                    navNumbers.Add(currentPage + 3);
                }
                else if (currentPage == allPage - 1)
                {
                    navNumbers.Add(currentPage - 3);
                    navNumbers.Add(currentPage - 2);
                    navNumbers.Add(currentPage - 1);
                    navNumbers.Add(currentPage);
                    navNumbers.Add(currentPage + 1);
                }
                else if (currentPage == allPage)
                {
                    navNumbers.Add(currentPage - 4);
                    navNumbers.Add(currentPage - 3);
                    navNumbers.Add(currentPage - 2);
                    navNumbers.Add(currentPage - 1);
                    navNumbers.Add(currentPage);
                }
                else
                {
                    navNumbers.Add(currentPage - 2);
                    navNumbers.Add(currentPage - 1);
                    navNumbers.Add(currentPage);
                    navNumbers.Add(currentPage + 1);
                    navNumbers.Add(currentPage + 2);
                }
            }



            return navNumbers;
        }

        public string toEnglishNumber(string input)
        {
            string EnglishNumbers = "";
            for (int i = 0; i < input.Length; i++)
            {
                if (Char.IsDigit(input[i]))
                {
                    EnglishNumbers += char.GetNumericValue(input, i);
                }
                else
                {
                    EnglishNumbers += input[i].ToString();
                }
            }
            return EnglishNumbers;
        }

        public int CalculatePercent(int val1, int val2)
        {
            return (int)(((double)(val1 - val2) / val1) * 100);

        }

        public string GenerateSlug(string phrase)
        {
            var s = RemoveDiacritics(phrase).ToLower();
            s = Regex.Replace(s, @"[^\u0600-\u06FF\uFB8A\u067E\u0686\u06AF\u200C\u200Fa-z0-9\s-]", "");                      // remove invalid characters
            s = Regex.Replace(s, @"\s+", " ").Trim();                       // single space
            s = s.Substring(0, s.Length <= 100 ? s.Length : 45).Trim();      // cut and trim
            s = Regex.Replace(s, @"\s", "-");                               // insert hyphens        
            s = Regex.Replace(s, @"‌", "-");                                // half space
            return s.ToLower();
        }

        public string RemoveDiacritics(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            var normalizedString = text.Normalize(NormalizationForm.FormKC);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public string GetShamsiMotnName()
        {
            PersianCalendar pc = new PersianCalendar();
            DateTime dt = DateTime.Now;

            int ShamsiMonth = pc.GetMonth(dt);
            switch (ShamsiMonth)
            {
                case 1:
                    {
                        return "فروردین";
                    }
                case 2:
                    {
                        return "اردیبهشت";
                    }
                case 3:
                    {
                        return "خرداد";
                    }
                case 4:
                    {
                        return "تیر";
                    }
                case 5:
                    {
                        return "مرداد";
                    }
                case 6:
                    {
                        return "شهریور";
                    }
                case 7:
                    {
                        return "مهر";
                    }
                case 8:
                    {
                        return "آبان";
                    }
                case 9:
                    {
                        return "آذر";
                    }
                case 10:
                    {
                        return "دی";
                    }
                case 11:
                    {
                        return "بهمن";
                    }
                case 12:
                    {
                        return "اسفند";
                    }
                default:
                    return "نامشخص";
            }
        }

    }
}
