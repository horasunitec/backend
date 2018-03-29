using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using VinculacionBackend.Models;
using System.Web.Http.Cors;
using System.Web.OData;
using ClosedXML.Excel;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.ActionFilters;
using VinculacionBackend.Data.Interfaces;
using VinculacionBackend.Interfaces;
using VinculacionBackend.Security.BasicAuthentication;

namespace VinculacionBackend.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class StudentsController : ApiController
    {
        private readonly IStudentsServices _studentsServices;
        private readonly IHoursServices _hoursServices;
        private readonly IEmail _email;
        private readonly IEncryption _encryption;
        private readonly ILogger _logger;

        public StudentsController(IStudentsServices studentServices, IEmail email, IEncryption encryption, IHoursServices hoursServices, ILogger logger)
        {
            _studentsServices = studentServices;
            _email = email;
            _encryption = encryption;
            _hoursServices = hoursServices;
            _logger = logger;
        }

        // GET: api/Students
        [Route("api/Students")]
        [CustomAuthorize(Roles = "Admin,Professor")]
        [EnableQuery]
        public IQueryable<User> GetStudents()
        {
            try
            {
                return _studentsServices.AllUsers();
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }

        // GET: api/Students/5
        [ResponseType(typeof(User))]
        [Route("api/Students/{accountId}")]
        [CustomAuthorize(Roles = "Admin,Professor,Student")]
        public IHttpActionResult GetStudent(string accountId)
        {
            try
            {
                var student = _studentsServices.Find(accountId);
                return Ok(student);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }

        [ResponseType(typeof(User))]
        [Route("api/Students/Me")]
        [CustomAuthorize(Roles = "Student")]
        public IHttpActionResult GetCurrentStudent()
        {
            try
            {
                var currentUser = (CustomPrincipal)HttpContext.Current.User;
                var student = _studentsServices.GetCurrentStudents(currentUser.UserId);
                return Ok(student);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }


        [Route("api/Students/Import")]
        [CustomAuthorize(Roles = "Admin")]
        public IHttpActionResult PostAddManyStudents([FromBody]List<StudentAddManyEntryModel> students)
        {
            try
            {
                _studentsServices.AddMany(students);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }

        [ResponseType(typeof(User))]
        [Route("api/StudentByEmail")]
        [CustomAuthorize(Roles = "Admin,Professor,Student")]
        public IHttpActionResult PostStudentByEmail(EmailModel model)
        {
            try
            {
                var student = _studentsServices.FindByEmail(model.Email);
                return Ok(student);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }

        [Route("api/Students/PendingFiniquitoStudents")]
        [EnableQuery]
        public IQueryable<FiniquitoUserModel> GetStudentsPendingFiniquito()
        {
            try
            {
                return _studentsServices.GetPendingStudentsFiniquito();
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }

        [Route("api/Students/FiniquitoReport/{accountId}")]
        public HttpResponseMessage GetProjectFinalReport(string accountId)
        {
            try
            {
                return _studentsServices.GetFiniquitoReport(accountId);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }

        [ResponseType(typeof(User))]
        [Route("api/Students/{accountId}/Hour")]
        [CustomAuthorize(Roles = "Admin,Professor,Student")]
        public IHttpActionResult GetStudentHour(string accountId)
        {
            try
            {
                var student = _studentsServices.Find(accountId);
                var total = _studentsServices.GetStudentHours(accountId);
                return Ok(total);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }

        [ResponseType(typeof(User))]
        [Route("api/Students/{accountId}/Section/{sectionid}/Hours")]
        [CustomAuthorize(Roles = "Admin,Professor,Student")]
        public IHttpActionResult GetStudentHourBySection(string accountId, long sectionId)
        {
            try
            {
                var total = _studentsServices.GetStudentHoursBySection(accountId, sectionId);
                return Ok(total);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }

        [Route("api/Students/{accountId}/SectionHours")]
        [CustomAuthorize(Roles = "Admin,Professor,Student")]
        public IQueryable<object> GetStudentSection(string accountId)
        {
            try
            {
                return _studentsServices.GetStudentSections(accountId);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }

        [Route("api/Students/Filter/{status}")]
        [CustomAuthorize(Roles = "Admin,Professor")]
        public IQueryable<User> GetStudents(string status)
        {
            try
            {
                return _studentsServices.ListbyStatus(status);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }


        // POST: api/Students
        [ResponseType(typeof(User))]
        [Route("api/Students")]
        [ValidateModel]
        public IHttpActionResult PostStudent(UserEntryModel userModel)
        {
            try
            {
                var newStudent = new User();
                _studentsServices.Map(newStudent, userModel);
                _studentsServices.Add(newStudent);
                var stringparameter = _encryption.Encrypt(newStudent.AccountId);
                _email.Send(newStudent.Email,
                    "Hacer click en el siguiente link para activar su cuenta: " +
                        HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) +
                        "/api/Students/" + HttpContext.Current.Server.UrlEncode(stringparameter) +
                        "/Active",
                    "Vinculación");
                return Ok(newStudent);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }


        [ResponseType(typeof(User))]
        [Route("api/Students/EnableStudent")]
        [ValidateModel]
        public IHttpActionResult PostChangePassword(StudentChangePasswordModel model)
        {
            try
            {
                _studentsServices.ChangePassword(model);
                var stringparameter = _encryption.Encrypt(model.AccountId);
                _email.Send(model.Email,
                    "Hacer click en el siguiente link para activar su cuenta: " +
                        HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) +
                        "/api/Students/" + HttpContext.Current.Server.UrlEncode(stringparameter) +
                        "/Active",
                    "Vinculación");
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }

        }


        //Get: api/Students/Avtive
        [Route("api/Students/{guid}/Active")]
        public HttpResponseMessage GetActiveStudent(string guid)
        {
            try
            {
                var accountId = _encryption.Decrypt(HttpContext.Current.Server.UrlDecode(guid));
                var student = _studentsServices.ActivateUser(accountId);
                var response = Request.CreateResponse(HttpStatusCode.Moved);
                response.Headers.Location = new Uri("http://fiasps.unitec.edu:8096");
                return response;
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }

        }


        [ResponseType(typeof(User))]
        [Route("api/Students/{accountId}")]
        [ValidateModel]
        [CustomAuthorize(Roles = "Admin")]
        public IHttpActionResult PutStudent(string accountId, UserUpdateModel model)
        {
            try
            {
                var student = _studentsServices.UpdateStudent(accountId, model);
                return Ok(student);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }

        }

        // DELETE: api/Students/5
        [ResponseType(typeof(User))]
        [Route("api/Students/{accountId}")]
        [CustomAuthorize(Roles = "Admin,Professor")]
        public IHttpActionResult DeleteStudent(string accountId)
        {
            try
            {
                User user = _studentsServices.DeleteUser(accountId);
                return Ok(user);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }


        }

        [Route("api/StudentHourReport/{accountId}")]
        public HourReportModel GetStudentsHourReport(string accountId)
        {
            try
            {
                return _hoursServices.HourReport(accountId);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }

        }

        [HttpPost]
        [Route("api/Students/Parse")]
        public IQueryable<object> Parse([FromBody]string data)
        {
            try
            {
                var content = Convert.FromBase64String(data);
                MemoryStream stream = new MemoryStream(content);
                var excel = new XLWorkbook(stream);
                return _studentsServices.ParseExcelStudents(excel);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }
    }
}
