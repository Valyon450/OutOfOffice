﻿using Microsoft.Data.SqlClient;

namespace BusinessLogic.Services
{
    public class SortOptions
    {
        public string SortBy { get; set; } 
        public SortOrder SortOrder { get; set; }
    }

    public enum SortDirection
    {
        Ascending,
        Descending
    }
}
