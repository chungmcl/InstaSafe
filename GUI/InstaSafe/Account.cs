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

    public class Account : IComparable
    {
        private const double PastMonthWeight = 1;//.6;
        private const double PastThreeWeight = 1;//.25;
        private const double PastSixWeight = 1;//.1;
        private const double PastYearWeight = 1;//.04;
        private const double OverYearWeight = 1;//.01;
        private const double CaptionWeight = .5;
        private const double ImageWeight = 2;
        public List<Post> Posts { get; set; }
        private double[] thresholdAverageSeverities = new double[5];
        public double OverallUserSeverity { get; set; }
        public string Username { get; set; }

        public Account(List<Post> addPosts, string username)
        {
            this.Posts = addPosts;
            this.Username = username;
            this.SetAllSeverity();
            this.OrganizePostDateThresholds();
            this.CalculateThresholdAverages();
            this.CalculateOverallUserSeverity();
        }

        private void OrganizePostDateThresholds()
        {
            for (int i = 0; i < this.Posts.Count(); i++)
            {
                double howLongAgo = (DateTime.Now - this.Posts[i].Date).TotalDays;

                // 0 is highest severity, 4 is lowest
                this.Posts[i].DateThreshold = PostThreshold.OverYear;
                // Within a year
                if (howLongAgo <= 365)
                {
                    this.Posts[i].DateThreshold = PostThreshold.PastYear;
                }
                // Within half a year
                if (howLongAgo <= 182)
                {
                    this.Posts[i].DateThreshold = PostThreshold.PastSix;
                }
                // Within three months
                else if (howLongAgo <= 93)
                {
                    this.Posts[i].DateThreshold = PostThreshold.PastThree;
                }
                // Within a month
                if (howLongAgo <= 31)
                {
                    this.Posts[i].DateThreshold = PostThreshold.PastMonth;
                }
            }
        }

        private void SetAllSeverity()
        {
            for (int i = 0; i < this.Posts.Count; i++)
            {
                this.Posts[i].OverallSeverity = this.Posts[i].CaptionSeverity * CaptionWeight + this.Posts[i].ImageSeverity * ImageWeight;
            }
        }

        private void CalculateThresholdAverages()
        {
            int[] totalPosts = new int[5];
            foreach (Post post in this.Posts)
            {
                totalPosts[(int)post.DateThreshold]++;
                this.thresholdAverageSeverities[(int)post.DateThreshold] += post.OverallSeverity;
            }
            for (int i = 0; i < this.thresholdAverageSeverities.Length; i++)
            {
                this.thresholdAverageSeverities[i] /= totalPosts[i];
                if (totalPosts[i] == 0)
                    this.thresholdAverageSeverities[i] = 0;
            }
        }

        private void CalculateOverallUserSeverity()
        {
            this.OverallUserSeverity = this.thresholdAverageSeverities[0] * PastMonthWeight + this.thresholdAverageSeverities[1] * PastThreeWeight
                + this.thresholdAverageSeverities[2] * PastSixWeight + this.thresholdAverageSeverities[3] * PastYearWeight + this.thresholdAverageSeverities[4] * OverYearWeight;
        }

        public int CompareTo(object other)
        {
            return ((Account)other).OverallUserSeverity.CompareTo(this.OverallUserSeverity);
        }
    }
}
