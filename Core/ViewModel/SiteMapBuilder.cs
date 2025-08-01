using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Core.ViewModel
{
    public class SiteMapBuilder
    {
        private readonly XNamespace NS = "http://www.sitemaps.org/schemas/sitemap/0.9";
        private List<SitemapNode> _urls;
        public SiteMapBuilder()
        {
            _urls = new List<SitemapNode>();
        }



        #region Sitemap Generator
        public void AddUrl(string url, DateTime? modified = null, SitemapFrequency? changeFrequency = null, double? priority = null)
        {
            _urls.Add(new SitemapNode()
            {
                Url = url,
                LastModified = modified,
                Frequency = changeFrequency,
                Priority = priority,
            });
        }

        public override string ToString()
        {
            var sitemap = new XDocument(
            new XDeclaration("1.0", "utf-8", "yes"),
            new XElement(NS + "urlset",
            from item in _urls
            select CreateItemElement(item)
            ));

            return sitemap.ToString();
        }

        private XElement CreateItemElement(SitemapNode url)
        {
            XElement itemElement = new XElement(NS + "url", new XElement(NS + "loc", url.Url));

            if (url.LastModified.HasValue)
            {
                itemElement.Add(new XElement(NS + "lastmod", url.LastModified.Value.ToString("yyyy-MM-ddTHH:mm:ss.f") + "+00:00"));
            }

            if (url.Frequency.HasValue)
            {
                itemElement.Add(new XElement(NS + "changefreq", url.Frequency.Value.ToString().ToLower()));
            }

            if (url.Priority.HasValue)
            {
                itemElement.Add(new XElement(NS + "priority", url.Priority.Value.ToString("N1")));
            }

            return itemElement;
        }
        #endregion

    }
}
