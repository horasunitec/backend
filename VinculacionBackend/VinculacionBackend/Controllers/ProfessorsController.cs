using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using System.Web.OData;
using VinculacionBackend.ActionFilters;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Interfaces;
using VinculacionBackend.Interfaces;
using VinculacionBackend.Models;
using VinculacionBackend.Security.BasicAuthentication;

namespace VinculacionBackend.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ProfessorsController : ApiController
    {
        static string frontEndSite = "http://159.89.229.181";
        static string backEndSite = "http://backend-4.apphb.com";

        //static string frontEndSite = "http://localhost:3000";
        //static string backEndSite = "http://localhost:27011";

        private readonly IProfessorsServices _professorsServices;
        private readonly IEmail _email;
        private readonly IEncryption _encryption;

        public ProfessorsController(IProfessorsServices professorsServices, IEmail email, IEncryption encryption)
        {
            _professorsServices = professorsServices;
            _encryption = encryption;
            _email = email;
        }

        [Route("api/Professors")]
        [CustomAuthorize(Roles = "Admin,Professor,Student")]
        [EnableQuery]
        // GET: api/Professors
        public IQueryable<User> GetUsers()
        {
            return _professorsServices.GetProfessors();
        }

        [ResponseType(typeof(User))]
        [Route("api/Professors/EnableProfessor")]
        [ValidateModel]
        public IHttpActionResult EnableProfessor(EnableProfessorModel model)
        {
            /* check if the account Id exists on the system
                if exists, check 
                    if the status = inactive (0)
                        update account information and send email
                    else
                        cannot update information
                else -> account doesnt exist
                    create account with the model
                    send email
            */
            
            var professor = _professorsServices.FindNullable(model.AccountId);
            // Check if account exists
            if (professor != null)
            {
                // Check if account is inactive
                if (((User)professor).Status == 0)
                {
                    var updatedProfessor = _professorsServices.UpdateProfessor(model.AccountId, model);

                    // Send confirmation email

                    var encryptedTalentoHumano = HexadecimalEncoding.ToHexString(model.AccountId);
                    _email.Send(model.Email,
                        "Hacer click en el siguiente link para activar su cuenta: " +
                            backEndSite +
                            "/api/Professors/EnableProfessors/Activate/" + HttpContext.Current.Server.UrlEncode(encryptedTalentoHumano),
                        "Vinculación");
                    return Ok();
                }
                else
                {
                    throw new Exception("account is already active");
                }
            }
            else
            {
                var newProfessor = new User();

                //mapeo
                newProfessor.AccountId = model.AccountId;
                newProfessor.Email = model.Email;
                newProfessor.Password = _encryption.Encrypt(model.Password);
                newProfessor.Name = model.FirstName + " " + model.LastName;
                newProfessor.Major = null;
                newProfessor.Campus = "USPS";
                newProfessor.CreationDate = DateTime.Now;
                newProfessor.ModificationDate = DateTime.Now;
                newProfessor.Status = Data.Enums.Status.Inactive;
                newProfessor.Finiquiteado = false;

                _professorsServices.AddProfessor(newProfessor);

                // Send confirmation email
                var encryptedTalentoHumano = HexadecimalEncoding.ToHexString(model.AccountId);
                _email.Send(model.Email,
                    "Hacer click en el siguiente link para activar su cuenta: " +
                        backEndSite +
                        "/api/Professors/EnableProfessors/Activate/" + HttpContext.Current.Server.UrlEncode(encryptedTalentoHumano),
                    "Vinculación");
                return Ok();
            }
        }

        [Route("api/Professors/EnableProfessors/Activate/{guid}")]
        public HttpResponseMessage GetActiveProfessor(string guid)
        {
            var accountId = HexadecimalEncoding.FromHexString(HttpContext.Current.Server.UrlDecode(guid));
            var professor = _professorsServices.ActivateUser(accountId);
            var response = Request.CreateResponse(HttpStatusCode.Moved);
            response.Headers.Location = new Uri(frontEndSite);
            return response;
        }

        [Route("api/Professors/alpha")]
        [CustomAuthorize(Roles = "Admin,Professor,Student")]
        [EnableQuery]
        // GET: api/Professors
        public IQueryable<User> GetUsersAlpha()
        {
            return _professorsServices.GetProfessorsAlpha();
        }

        // GET: api/Professors/5
        [Route("api/Professors/{accountId}")]
        [CustomAuthorize(Roles = "Admin,Professor,Student")]
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(string accountId)
        {
            var professor = _professorsServices.Find(accountId);
            return Ok(professor);
        }

        //Put: api/Professors/Verified
        [ResponseType(typeof(User))]
        [Route("api/Professors/Verified")]
        [ValidateModel]
        public IHttpActionResult PostAcceptVerified(VerifiedProfessorModel model)
        {
            _professorsServices.VerifyProfessor(model);
            return Ok();
        }

        // POST: api/Professors
        [ResponseType(typeof(User))]
        [Route("api/Professors")]
        [CustomAuthorize(Roles = "Admin")]
        [ValidateModel]
        public IHttpActionResult PostUser(ProfessorEntryModel professorModel)
        {
            var professor= new User();
            _professorsServices.Map(professor,professorModel);
            _professorsServices.AddProfessor(professor);
            var accountIdParameter = _encryption.Encrypt(professor.AccountId);
            _email.Send(professor.Email,
                "Hacer click en el siguiente link para establecer su contraseña : http://159.89.229.181/registro-maestro/" + 
                    HttpContext.Current.Server.UrlEncode(accountIdParameter),
                "Vinculacion");
            return Ok(professor);
        }

        [ResponseType(typeof(User))]
        [Route("api/Professors/{accountId}")]
        [ValidateModel]
        [CustomAuthorize(Roles = "Admin")]
        public IHttpActionResult PutProfessor(string accountId, ProfessorUpdateModel model)
        {
            var professor = _professorsServices.UpdateProfessor(accountId, model);
            return Ok(professor);
        }

        // DELETE: api/Professors/5
        [ResponseType(typeof(User))]
        [Route("api/Professors/{accountId}")]
        [CustomAuthorize(Roles = "Admin")]
        public IHttpActionResult DeleteUser(string accountId)
        {
            var professor = _professorsServices.DeleteProfessor(accountId);
            return Ok(professor);
        }
    }
}