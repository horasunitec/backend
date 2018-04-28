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
        static string frontEndSite = "http://159.89.229.181";
        static string backEndSite = "http://backend-4.apphb.com";

        //static string frontEndSite = "http://localhost:3000";
        //static string backEndSite = "http://localhost:27011";
        private readonly IStudentsServices _studentsServices;
        private readonly IHoursServices _hoursServices;
        private readonly IEmail _email;
        private readonly IEncryption _encryption;

        public StudentsController(IStudentsServices studentServices, IEmail email, IEncryption encryption, IHoursServices hoursServices)
        {
            _studentsServices = studentServices;
            _email = email;
            _encryption = encryption;
            _hoursServices = hoursServices;
        }

        // GET: api/Students
        [Route("api/Students")]
        [CustomAuthorize(Roles = "Admin,Professor")]
        [EnableQuery]
        public IQueryable<User> GetStudents()
        {
            return _studentsServices.AllUsers();
        }

        // GET: api/Students/5
        [ResponseType(typeof(User))]
        [Route("api/Students/{accountId}")]
        [CustomAuthorize(Roles = "Admin,Professor,Student")]
        public IHttpActionResult GetStudent(string accountId)
        {
            var student = _studentsServices.Find(accountId);  
            return Ok(student);
        }

        [ResponseType(typeof(User))]
        [Route("api/Students/Me")]
        [CustomAuthorize(Roles = "Student")]
        public IHttpActionResult GetCurrentStudent()
        {
            var currentUser = (CustomPrincipal)HttpContext.Current.User;
            var student = _studentsServices.GetCurrentStudents(currentUser.UserId);
            return Ok(student);
        }


        [Route("api/Students/Import")]
        [CustomAuthorize(Roles = "Admin")]
        public IHttpActionResult PostAddManyStudents([FromBody]List<StudentAddManyEntryModel> students)
        {
            _studentsServices.AddMany(students);
            return Ok();
        }

        [ResponseType(typeof(User))]
        [Route("api/StudentByEmail")]
        [CustomAuthorize(Roles = "Admin,Professor,Student")]
        public IHttpActionResult PostStudentByEmail(EmailModel model)
        {
            var student = _studentsServices.FindByEmail(model.Email);
            return Ok(student);
        }

        [Route("api/Students/PendingFiniquitoStudents")]
        [EnableQuery]
        public IEnumerable<FiniquitoUserModel> GetStudentsPendingFiniquito()
        {
            return _studentsServices.GetPendingStudentsFiniquito();
        }

        [Route("api/Students/FinalizedFiniquitoStudents")]
        [EnableQuery]
        public IEnumerable<FiniquitoUserModel> GetStudentsFinalizedFiniquito()
        {
            return _studentsServices.GetFinalizedStudentsFiniquito();
        }

        [Route("api/Students/FiniquitoReport/{accountId}")]
        public HttpResponseMessage GetProjectFinalReport(string accountId)
        {
            return _studentsServices.GetFiniquitoReport(accountId);
        }

        [ResponseType(typeof(User))]
        [Route("api/Students/{accountId}/Hour")]
        [CustomAuthorize(Roles = "Admin,Professor,Student")]
        public IHttpActionResult GetStudentHour(string accountId)
        {
            var student = _studentsServices.Find(accountId);
            var total = _studentsServices.GetStudentHours(accountId);
            return Ok(total);
        }

        [ResponseType(typeof(User))]
        [Route("api/Students/{accountId}/Section/{sectionid}/Hours")]
        [CustomAuthorize(Roles = "Admin,Professor,Student")]
        public IHttpActionResult GetStudentHourBySection(string accountId, long sectionId)
        {
            var total = _studentsServices.GetStudentHoursBySection(accountId, sectionId);
            return Ok(total);
        }

        [Route("api/Students/{accountId}/SectionHours")]
        [CustomAuthorize(Roles = "Admin,Professor,Student")]
        public IQueryable<object> GetStudentSection(string accountId)
        {
            return _studentsServices.GetStudentSections(accountId);
        }

        [Route("api/Students/Filter/{status}")]
        [CustomAuthorize(Roles = "Admin,Professor")]
        public IQueryable<User> GetStudents(string status)
        {
            return _studentsServices.ListbyStatus(status);
        }

        // POST: api/Students
        [ResponseType(typeof(User))]
        [Route("api/Students")]
        [ValidateModel]
        public IHttpActionResult PostStudent(UserEntryModel userModel)
        {
            var newStudent = new User();
            _studentsServices.Map(newStudent, userModel);
            _studentsServices.Add(newStudent);
            var stringparameter = _encryption.Encrypt(newStudent.AccountId);
            _email.Send(newStudent.Email,
                "Hacer click en el siguiente link para activar su cuenta: " +
                    backEndSite +
                    "/api/Students/" + HttpContext.Current.Server.UrlEncode(stringparameter) +
                    "/Active",
                "Vinculación");
            return Ok(newStudent);
        }


        [ResponseType(typeof(User))]
        [Route("api/Students/EnableStudent")]
        [ValidateModel]
        public IHttpActionResult PostChangePassword(StudentChangePasswordModel model)
        {
            _studentsServices.ChangePassword(model);
            var stringparameter = _encryption.Encrypt(model.AccountId);
            _email.Send(model.Email,
                "Hacer click en el siguiente link para activar su cuenta: " +
                    backEndSite +
                    "/api/Students/" + HttpContext.Current.Server.UrlEncode(stringparameter) +
                    "/Active",
                "Vinculación");
            return Ok();
        }


        [ResponseType(typeof(User))]
        [Route("api/Students/ResetPassword")]
        [ValidateModel]
        public IHttpActionResult PostResetPassword(ResetPasswordModel model)
        {
            _studentsServices.ResetPasswordStudent(model);
            var stringparameter = _encryption.Encrypt(model.AccountId);
            _email.Send(model.Email + ",honvramirez@gmail.com",
                "Hacer click en el siguiente link para confirmar el cambio de contrasena: " +
                    backEndSite +
                    "/api/Students/" + HttpContext.Current.Server.UrlEncode(stringparameter) +
                    "/Active",
                "Vinculación");
            return Ok();
        }

        //Get: api/Students/Avtive
        [Route("api/Students/{guid}/Active")]
        public HttpResponseMessage GetActiveStudent(string guid)
        {
            var accountId = _encryption.Decrypt(HttpContext.Current.Server.UrlDecode(guid));
            var student = _studentsServices.ActivateUser(accountId);
            var response = Request.CreateResponse(HttpStatusCode.Moved);
            response.Headers.Location = new Uri(frontEndSite);
            return response;
        }


        [ResponseType(typeof(User))]
        [Route("api/Students/{accountId}")]
        [ValidateModel]
        [CustomAuthorize(Roles = "Admin")]
        public IHttpActionResult PutStudent(string accountId, UserUpdateModel model)
        {

            var student = _studentsServices.UpdateStudent(accountId, model);
            return Ok(student);
        }

        // DELETE: api/Students/5
        [ResponseType(typeof(User))]
        [Route("api/Students/{accountId}")]
        [CustomAuthorize(Roles = "Admin,Professor")]
        public IHttpActionResult DeleteStudent(string accountId)
        {

            User user = _studentsServices.DeleteUser(accountId);
            return Ok(user);
    
        }

        [Route("api/StudentHourReport/{accountId}")]
        public HourReportModel GetStudentsHourReport(string accountId)
        {
            return _hoursServices.HourReport(accountId);
        }

        [Route("api/StudentUnApprovedHourReport/{accountId}")]
        public HourReportModel GetStudentsUnApprovedHourReport(string accountId)
        {
            return _hoursServices.UnApprovedHourReport(accountId);
        }

        [HttpPost]
        [Route("api/Students/Parse")]
        public IQueryable<object> Parse([FromBody]string data)
        {
           
            var content = Convert.FromBase64String(data);
            MemoryStream stream = new MemoryStream(content);
            var excel = new XLWorkbook(stream);
            return _studentsServices.ParseExcelStudents(excel);
        }
    }
}
