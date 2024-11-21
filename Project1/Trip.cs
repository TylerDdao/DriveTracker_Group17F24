using System;
using System.Collections.Generic;

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
            // Implement your scoring logic here
            // For example, reducing the score by 10 points for each violation
            Score = 100 - (ExceedingSpeedRecords.Count * 10);
            if (Score < 0)
                Score = 0;
        }
    }

    public class SpeedRecord
    {
        public DateTime Timestamp { get; set; }
        public double Speed { get; set; }
    }
}
