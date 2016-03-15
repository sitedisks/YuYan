using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YuYan.Domain.Database
{
    [Table("ip2location_db3")]
    public class ip2location_db3
    {
        [Key]
        public double ip_from { get; set; }
        public double ip_to { get; set; }
        public string country_code { get; set; }
        public string country_name { get; set; }
        public string region_name { get; set; }
        public string city_name { get; set; }
        public Nullable<double> latitude { get; set; }
        public Nullable<double> longitude { get; set; }
    }
}
