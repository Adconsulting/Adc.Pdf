using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Adc.Pdf.Handler;

namespace Adc.Pdf.Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            var file1 = @"C:\Temp\Report\tmp\23\main.pdf";
            var file2 = @"C:\Temp\Report\tmp\23\toc.pdf";
            var destination = @"c:\Temp\output.pdf";

            var merger = new PdfMerger();

            var bookmarks = PdfUtil.GetBookMarks(File.ReadAllBytes(file1));

            foreach (var bookMarkItem in bookmarks)
            {
                bookMarkItem.Page += 1;
            }


            merger.AddFile(file2)
                .AddFile(file1)
                .AddBookmarks(bookmarks)
                .Merge(destination);
        }
    }
}
