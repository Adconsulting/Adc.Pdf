using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

using Adc.Pdf.Handler.Models;

using iTextSharp.text.pdf;

namespace Adc.Pdf.Handler
{
    public static class PdfUtil
    {
        public static IEnumerable<BookMarkItem> GetBookMarks(byte[] pdfData)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                SimpleBookmark.ExportToXML(SimpleBookmark.GetBookmark(new PdfReader(new RandomAccessFileOrArray(pdfData), null)), ms, "UTF-8", false);
                ms.Position = 0;
                var sl = new List<BookMarkItem>();

                



                using (XmlReader xr = XmlReader.Create(ms))
                {
                    xr.MoveToContent();
                    string page = null;
                    string action = null;
                    string text = null;
                    var re = new Regex(@"(^\d+).(\D*\d*\D*\d*.\d*\W\d*)");

                    while (xr.Read())
                    {
                        //Filter by the nodes with depth 1 to remove the report name from toc
                        if (xr.NodeType == XmlNodeType.Element && xr.Name == "Title" && xr.IsStartElement() && xr.Depth != 1)
                        {
                            // extract page number from 'Page' attribute 
                            if (xr.GetAttribute("Page") != null)
                            {
                                var parts = re.Match(xr.GetAttribute("Page"));
                                page = parts.Groups[1].Value;
                                action = parts.Groups[2].Value;
                            }
                                

                            xr.Read();

                            if (xr.NodeType == XmlNodeType.Text)
                            {
                                text = xr.Value.Trim();

                                if (text != null && page != null)
                                    sl.Add(new BookMarkItem
                                    {
                                        Page = Convert.ToInt32(page),
                                        Title = text,
                                        Level = xr.Depth - 3,
                                        Action = action
                                    });
                            }
                        }
                    }

                    return sl;
                }
            }
        }
    }
}
