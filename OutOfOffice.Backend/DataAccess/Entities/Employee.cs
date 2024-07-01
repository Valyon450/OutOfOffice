﻿namespace DataAccess.Entities
{
    public class Employee
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public string Subdivision { get; set; }
        public string Position { get; set; }
        public string Status { get; set; }
        public int? PeoplePartner { get; set; }
        public int OutOfOfficeBalance { get; set; }
        public byte[] Photo { get; set; }
    }
}
