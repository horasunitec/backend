using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VinculacionBackend.Models
{
    public class HourReportUnitModel
    {
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string SectionName { get; set; }
        public int HoursWorked { get; set; }
        public string ProjectDescription { get; set; }

        public int Period { get; set; }
        public int Year { get; set; }
        public string ProfessorName { get; set; }
        public string Organization { get; set; }
        public string ClassName { get; set; }
    }
}