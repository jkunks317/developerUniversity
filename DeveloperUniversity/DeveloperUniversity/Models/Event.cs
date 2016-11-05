using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DHTMLX.Scheduler;
using DHTMLX.Scheduler.Data;
using DHTMLX.Common;

namespace DeveloperUniversity.Models
{
    public class Event
    {
        [Required]
        public int id { get; set; }

        public string text { get; set; }

        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
    }
}