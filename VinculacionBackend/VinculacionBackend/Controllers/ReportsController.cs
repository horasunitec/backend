using System.Web.Http;
using System.Web.Http.Cors;
using System.Data;
using VinculacionBackend.Extensions;
using VinculacionBackend.Interfaces;
using System;

namespace VinculacionBackend.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]

    public class ReportsController : ApiController
    {
        private readonly IFacultiesServices _facultiesServices;
        private readonly ISheetsReportsServices _reportsServices;
        private readonly IStudentsServices _studentServices;
        private readonly IClassesServices _classesServices;
        private readonly IProjectServices _projectServices;
        private readonly ILogger _logger;

        public ReportsController(IFacultiesServices facultiesServices, ISheetsReportsServices reportsServices, IStudentsServices studentsServices, IProjectServices projectServices, IClassesServices classesServices, ILogger logger)
        {
            _facultiesServices = facultiesServices;
            _reportsServices = reportsServices;
            _projectServices = projectServices;
            _classesServices = classesServices;
            _studentServices = studentsServices;
            _logger = logger;
        }


        [Route("api/Reports/CostsReport/{year}")]
        public IHttpActionResult GetCostsReport(int year)
        {
            try
            {
                var context = _reportsServices.GenerateReport(_facultiesServices.CreateFacultiesCostReport(year).ToDataTable(),
                "Reporte de Costos por Facultad");
                context.Response.Flush();
                context.Response.End();
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }

        [Route("api/Reports/ProjectsByMajorReport")]
        public IHttpActionResult GetProjectCountByMajorReport()
        {
            try
            {
                var context = _reportsServices.GenerateReport(_projectServices.CreateProjectsByMajor().ToDataTable(),
                "Proyectos por Carrera");
                context.Response.Flush();
                context.Response.End();
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }

        [Route("api/Reports/HoursReport/{year}")]
        public IHttpActionResult GetHoursReport(int year)
        {
            try
            {
                var context = _reportsServices.GenerateReport(_facultiesServices.CreateFacultiesHourReport(year).ToDataTable(),
                "Reporte de Horas por Facultad");
                context.Response.Flush();
                context.Response.End();
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }

        [Route("api/Reports/StudentsReport/{year}")]
        public IHttpActionResult GetStudentsReport(int year)
        {
            try
            {
                var datatables = new DataTable[2];
                datatables[0] = _studentServices.CreateStudentReport(year).ToDataTable();
                datatables[1] = _studentServices.CreateHourNumberReport(year).ToDataTable();
                var context = _reportsServices.GenerateReport(datatables,
                    "Reporte de Alumnos");
                context.Response.Flush();
                context.Response.End();
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }

        [Route("api/Reports/ProjectsByClassReport/{classId}")]
        public IHttpActionResult GetProjectsByClassReport(long classId)
        {
            try
            {
                var context = _reportsServices.GenerateReport(_projectServices.ProjectsByClass(classId).ToDataTable(), "Projectos Por " + _classesServices.Find(classId).Name);
                context.Response.Flush();
                context.Response.End();
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }

        [Route("api/Reports/PeriodReport/{year}")]
        public IHttpActionResult GetPeriodReport(int year)
        {
            try
            {
                var context = _reportsServices.GenerateReport(_projectServices.CreatePeriodReport(year, 1).ToDataTable(),
                1 + " " + year);
                context.Response.Flush();
                context.Response.End();
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }
    }
}