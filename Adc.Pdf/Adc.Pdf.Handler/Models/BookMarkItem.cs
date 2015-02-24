using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adc.Pdf.Handler.Models
{
    public class BookMarkItem
    {
        /// <summary>
        /// Gets or Sets the Page
        /// </summary>
        public virtual int Page { get; set; }

        /// <summary>
        /// Gets or Sets the Title
        /// </summary>
        public virtual string Title { get; set; }

        /// <summary>
        /// Gets or Sets the Level
        /// </summary>
        public virtual int Level { get; set; }

        /// <summary>
        /// Gets or Sets the Action
        /// </summary>
        public virtual string Action { get; set; }
    }
}
