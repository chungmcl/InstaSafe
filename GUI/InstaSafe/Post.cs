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
        public bool CaptionBad { get; set; }

        /// <summary>
        /// The severity score of the image posted
        /// </summary>
        public double ImageSeverity { get; set; }

        /// <summary>
        /// The date the post was made on.
        /// </summary>
        public DateTime Date { get; set; }

        public PostThreshold DateThreshold { get; set; }

        /// <summary>
        /// The overall severity of the post;
        /// </summary>
        public double overallSeverity { get; set; }

        public Post(bool setCapt, double setImg, DateTime setDate)
        {
            this.CaptionBad = setCapt;
            this.ImageSeverity = setImg;
            this.Date = setDate;
        }
    }
}
