using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaSafe
{
    public class Post
    {
        /// <summary>
        /// Whether the caption is considered to have risk or not
        /// </summary>
        public double CaptionBad { get; set; }

        /// <summary>
        /// The severity score of the image posted
        /// </summary>
        public double ImageSeverity { get; set; }

        /// <summary>
        /// The date the post was made on.
        /// </summary>
        public DateTime Date { get; set; }

        public string DateString
        {
            get
            {
                return this.Date.ToShortDateString();
            }
        }


        public PostThreshold DateThreshold { get; set; }

        /// <summary>
        /// The overall severity of the post;
        /// </summary>
        public double OverallSeverity { get; set; }

        public Post(double setCapt, double setImg, DateTime setDate)
        {
            this.CaptionBad = setCapt;
            this.ImageSeverity = setImg;
            this.Date = setDate;
        }
    }
}
