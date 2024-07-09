using Ganss.Xss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication58.Services
{
    public class HtmlSanitizationService
    {
        private readonly HtmlSanitizer _htmlSanitizer;

        public HtmlSanitizationService ()
        {
            _htmlSanitizer = new HtmlSanitizer();
        }

        public string SanitizeHtml (string inputHtml)
        {
            return _htmlSanitizer.Sanitize(inputHtml);
        }
    }
}
