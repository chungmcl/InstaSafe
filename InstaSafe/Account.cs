using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaSafe
{
    public enum PostThreshold
    {
        PastMonth, PastThree, PastSix, PastYear, OverYear
    }

    public class Account
    {
        private const double PastMonthWeight = .6;
        private const double PastThreeWeight = .25;
        private const double PastSixWeight = .1;
        private const double PastYearWeight = .04;
        private const double OverYearWeight = .01;
        private List<Post> posts;
        private double[] thresholdAverageSeverities = new double[5];
        public double overallUserSeverity;

        public Account(List<Post> addPosts)
        {
            this.posts = addPosts;
            SetAllSeverity();
            OrganizePostDateThresholds();
            CalculateThresholdAverages();
            OverallUserSeverity();
        }

        private void OrganizePostDateThresholds()
        {
            for (int i = 0; i < this.posts.Count(); i++)
            {
                double howLongAgo = (DateTime.Now - this.posts[i].Date).TotalDays;

                // 0 is highest severity, 4 is lowest
                this.posts[i].DateThreshold = PostThreshold.OverYear;
                // Within a year
                if (howLongAgo <= 365)
                {
                    this.posts[i].DateThreshold = PostThreshold.PastYear;
                }
                // Within half a year
                if (howLongAgo <= 182)
                {
                    this.posts[i].DateThreshold = PostThreshold.PastSix;
                }
                // Within three months
                else if (howLongAgo <= 93)
                {
                    this.posts[i].DateThreshold = PostThreshold.PastThree;
                }
                // Within a month
                if (howLongAgo <= 31)
                {
                    this.posts[i].DateThreshold = PostThreshold.PastMonth;
                }
            }
        }

        private void SetAllSeverity()
        {
            for (int i = 0; i < this.posts.Count; i++)
            {
                this.posts[i].overallSeverity = (CompareCaptSeverity(this.posts[i].CaptionBad) + CompareImgSeverity(this.posts[i].ImageSeverity));
            }
        }

        private int CompareCaptSeverity(bool capt)
        {
            if (capt)
                return 4;
            else
                return 0;
        }

        private int CompareImgSeverity(double img)
        {
            if (img >= 75)
                return 3;
            else if (img < 75 && img >= 50)
                return 2;
            else if (img < 50 && img >= 25)
                return 1;
            else
                return 0;
        }

        private void CalculateThresholdAverages()
        {
            int[] totalPosts = new int[5];
            foreach (Post post in this.posts)
            {
                totalPosts[(int)post.DateThreshold]++;
                this.thresholdAverageSeverities[(int)post.DateThreshold] += post.overallSeverity;
            }
            for (int i = 0; i < thresholdAverageSeverities.Length; i++)
            {
                this.thresholdAverageSeverities[i] /= totalPosts[i];
            }
        }

        private void OverallUserSeverity()
        {
            this.overallUserSeverity = thresholdAverageSeverities[0] * PastMonthWeight + thresholdAverageSeverities[1] * PastThreeWeight
                + thresholdAverageSeverities[2] * PastSixWeight + thresholdAverageSeverities[3] * PastYearWeight + thresholdAverageSeverities[4] * OverYearWeight;
        }
    }
}
