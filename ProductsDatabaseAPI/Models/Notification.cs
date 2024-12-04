using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


    namespace SomiodAPI.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDateTime { get; set; } // ISO DateTime format
        public int Parent { get; set; } // Container ID (parent)
        public int Event { get; set; } // Event ID
        public string Endpoint { get; set; }
        public bool Enabled { get; set; }
    }
}