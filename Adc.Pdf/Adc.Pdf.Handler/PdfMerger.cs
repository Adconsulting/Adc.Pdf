using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Adc.Pdf.Handler.Models;

using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Adc.Pdf.Handler
{
    public class PdfMerger
    {
        private readonly ICollection<string> _filenames;

        private readonly ICollection<BookMarkItem> _bookmarks;


        public PdfMerger()
        {
            _filenames = new Collection<string>();
            _bookmarks = new Collection<BookMarkItem>();
        }


        public PdfMerger AddFile(string fullPath)
        {
            if (File.Exists(fullPath))
            {
                _filenames.Add(fullPath);
                return this;
            }
            throw new FileNotFoundException("File not found", fullPath);

        }

        public PdfMerger AddFiles(IEnumerable<string> fullPath)
        {
            foreach (var path in fullPath)
            {
                AddFile(path);
            }
            return this;
        }

        public PdfMerger AddBookmarks(IEnumerable<BookMarkItem> bookMarks)
        {
            foreach (var bookMarkItem in bookMarks)
            {
                _bookmarks.Add(bookMarkItem);
            }
            return this;
        }

        public bool Merge(string targetPdf)
        {
            if (!_filenames.Any())
                throw new FileNotFoundException("No files to merge");
            var merged = true;
            using (var stream = new FileStream(targetPdf, FileMode.Create))
            {
                var document = new Document();
                var pdf = new PdfCopy(document, stream);
                PdfReader reader = null;
                try
                {
                    document.Open();
                    foreach (var file in _filenames)
                    {
                        reader = new PdfReader(file);
                        pdf.AddDocument(reader);
                        reader.Close();
                    }

                    if (_bookmarks.Any())
                    {
                        pdf.Outlines = CreateBookmarks();
                    }
                }
                catch (Exception ex)
                {
                    ErrorMessage = ex.Message;
                    merged = false;
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
                finally
                {
                    if (document != null)
                    {
                        document.Close();
                    }
                }
            }

            return merged;


        }

        public string ErrorMessage { get; set; }

        private IList<Dictionary<string, object>> CreateBookmarks()
        {
            var bookmarks = new List<Dictionary<string, object>>();

            foreach (var bookMarkItem in _bookmarks)
            {
                bookmarks.Add(new Dictionary<string, object>
                                  {
                                      { "Title" , bookMarkItem.Title },
                                      { "Action" , "GoTo" },
                                      { "Page", String.Format("{0} {1}", bookMarkItem.Page, bookMarkItem.Action) }
                                  });
            }


            return bookmarks;
        }
    }
}
