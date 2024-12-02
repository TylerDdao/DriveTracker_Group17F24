using System;
using System.Collections.Generic;

using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
//Group17-Rob-Maksym-Ginbot-Dao

namespace Project1
{
    internal class Trip
    {

        public List<SpeedRecord> ExceedingSpeedRecords { get; set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; set; }
        public TimeSpan Duration => EndTime - StartTime;
        public int Score { get; set; }

        public Trip()
        {
            Score = 0;
            StartTime = DateTime.Now; // Set the start time when the trip is created
            ExceedingSpeedRecords = new List<SpeedRecord>();

            //Variables.
            int score;
            string startingLocation;
            string endLocation;
            TimeSpan duration;
            List<int> speedViolation = new List<int>();
        }
        

        public void EndTrip()
        {
            EndTime = DateTime.Now; // Record the end time of the trip
        }

        public void AddSpeedRecordIfExceedsLimit(double currentSpeed, int speedLimit)
        {
            if (currentSpeed > speedLimit + 10)
            {
                ExceedingSpeedRecords.Add(new SpeedRecord
                {
                    Timestamp = DateTime.Now,
                    Speed = currentSpeed
                });
            }
        }

        public void CalculateScore()
        {
            //reducing the score by 10 points for each violation
            Score = 100 - (ExceedingSpeedRecords.Count * 10);
            if (Score < 0)
                Score = 0;
        }
    }


    public class SpeedRecord
    {
        public DateTime Timestamp { get; set; }
        public double Speed { get; set; }

        ////Getter and Setter for endLocation.
        //public string GetEndLocation()
        //{
        //    return endLocation;
        //}
        //public void SetEndLocation(string endLocation)
        //{
        //    this.endLocation = endLocation;
        //}
        //// Getter and Setter for duration.
        //public TimeSpan GetDuration()
        //{
        //    return duration;
        //}
        ////Setter for duration.
        //public void SetDuration(TimeSpan duration)
        //{
        //    this.duration = duration;
        //}

        //public void CalculateScore()
        //{
        //    for (int i = 0; i < speedViolation.LongCount(); i++)
        //    {
        //        if (speedViolation[i]<=10)
        //        {
        //            this.score -= 10;
        //        }
        //        else if (speedViolation[i]<20 && speedViolation[i]>10)
        //        {
        //            this.score -= 20;
        //        }
        //        else
        //        {
        //            this.score -= 30;
        //        }
        //        if(this.score ==0)
        //        {
        //            break;
        //        }
        //    }
        //}

    }
}
