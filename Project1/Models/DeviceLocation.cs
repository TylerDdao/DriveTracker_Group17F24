using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1.Models
{
    public class DeviceLocation
    {

        public double latitude { get; set; }
        public double longitude { get; set; }

        public double speed { get; set; }
   
        public DeviceLocation(double latitude, double longitude, double speed)
        {
            this.latitude = latitude;
            this.longitude = longitude;
            this.speed = speed;
        }
    }
}
