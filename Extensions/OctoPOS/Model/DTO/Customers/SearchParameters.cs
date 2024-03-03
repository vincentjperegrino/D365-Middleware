using System;


namespace KTI.Moo.Extensions.OctoPOS.Model.DTO.Customers
{
    public class SearchParameters
    {
        public int Pageno { get; set; } = 1;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime LastEditDateTime { get; set; }
        public string Email { get; set; }
        public string HandPhone { get; set; }
    }
}
