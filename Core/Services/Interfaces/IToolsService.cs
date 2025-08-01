using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Interfaces
{
    public interface IToolsService
    {
        public void SendCodeWithSMS(string code, string phoneNumber);

        public void SendSMS(string message, string phoneNumber);
        List<int> GetNavNumbers(int currentPage, int allPage);

        public string toEnglishNumber(string input);

        public int CalculatePercent(int val1, int val2);

        public string GenerateSlug(string phrase);

        public string GetShamsiMotnName();
    }
}
