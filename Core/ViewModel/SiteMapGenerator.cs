using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Core.ViewModel
{
    public static class SiteMapGenerator
    {
        public static void CreateSitemapXML(IEnumerable<SitemapNode> sitemapNodes, string directoryPath)
        {
            XElement root = new XElement("urlset");

            foreach (var item in sitemapNodes)
            {
                XElement urlElement = new XElement(
                    "url", new XElement("loc", Uri.EscapeUriString(item.Url)),
                    item.LastModified == null? null : new XElement("lastmodified",item.LastModified.Value.ToLocalTime().ToString("yyyy-MM-ddTHH:mm:sszzz")),
                    item.Frequency == null?null: new XElement("changefreq", item.Frequency.Value.ToString().ToLowerInvariant()),
                    item.Priority == null?null: new XElement("priority",item.Priority.Value.ToString("F1")));

                root.Add(urlElement);
            }

            XDocument doc = new XDocument(root);
            doc.Save(Path.Combine(directoryPath, "sitemap.xml"));
        }
    }
}
