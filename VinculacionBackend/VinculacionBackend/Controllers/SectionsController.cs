using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Cors;
using System.Web.OData;
using VinculacionBackend.ActionFilters;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Interfaces;
using VinculacionBackend.Models;
using VinculacionBackend.Security.BasicAuthentication;
using System;

namespace VinculacionBackend.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SectionsController : ApiController
    {
        private readonly ISectionsServices _sectionServices;
        private readonly ILogger _logger;

        public SectionsController( ISectionsServices sectionServices, ILogger logger)
        {
            _sectionServices = sectionServices;
            _logger = logger;
        }

        // GET: api/Sections
        [Route("api/Sections")]
        [CustomAuthorize(Roles = "Admin,Professor,Student")]
        [EnableQuery]
        public IQueryable<Section> GetSections()
        {
            try
            {
                var currentUser = (CustomPrincipal)HttpContext.Current.User;
                return _sectionServices.AllByUser(currentUser.UserId, currentUser.roles);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }


        // GET: api/Sections
        [Route("api/Sections/CurrentPeriodSections")]
        [CustomAuthorize(Roles = "Admin,Professor,Student")]
        [EnableQuery]
        public IQueryable<Section> GetCurrentPeriodSections()
        {
            try
            {
                var currentUser = (CustomPrincipal)HttpContext.Current.User;
                return _sectionServices.GetCurrentPeriodSectionsByUser(currentUser.UserId, currentUser.roles.Single());
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }


        // GET: api/Sections
        [Route("api/Sections/SectionsByProject/{projectId}")]
        [CustomAuthorize(Roles = "Admin,Professor,Student")]
        [EnableQuery]
        public IQueryable<Section> GetSectionsByProject(long projectId)
        {
            try
            {
                var currentUser = (CustomPrincipal)HttpContext.Current.User;
                var sections = _sectionServices.GetSectionsByProject(projectId, currentUser.roles.Single(), currentUser.UserId);
                return sections;
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }

        // GET: api/Sections/5
        [Route("api/Sections/{sectionId}")]
        [ResponseType(typeof(Section))]
        [CustomAuthorize(Roles = "Admin,Professor,Student")]
        public IHttpActionResult GetSection(long sectionId)
        {
            try
            {
                var section = _sectionServices.Find(sectionId);
                return Ok(section);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }

        // GET: api/Sections/5
        [ResponseType(typeof(Project))]
        [Route("api/Sections/Students/{sectionId}")]
        [CustomAuthorize(Roles = "Admin,Professor,Student")]
        public IQueryable<User> GetSectionStudents(long sectionId)
        {
            try
            {
                return _sectionServices.GetSectionStudents(sectionId);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }


        [ResponseType(typeof(Project))]
        [Route("api/Sections/StudentsHour/{sectionId}/{projectId}")]
        [CustomAuthorize(Roles = "Admin,Professor,Student")]
        public object GetSectionStudentsHour(long sectionId,long projectId)
        {
            try
            {
                return _sectionServices.GetSectionStudentsHour(sectionId, projectId);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }

        // GET: api/Sections/5
        [ResponseType(typeof(Project))]
        [Route("api/Sections/Projects/{sectionId}")]
        [CustomAuthorize(Roles = "Admin,Professor,Student")]
        public IQueryable<Project> GetSectionProjects(long sectionId)
        {
            try
            {
                return _sectionServices.GetSectionsProjects(sectionId);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }


        // POST: api/Sections
        [Route("api/Sections")]
        [ResponseType(typeof(Section))]
        [CustomAuthorize(Roles = "Admin,Professor")]
        [ValidateModel]
        public IHttpActionResult PostSection(SectionEntryModel sectionModel)
        {
            try
            {
                var section = new Section();
                _sectionServices.Map(section, sectionModel);
                _sectionServices.Add(section);
                return Ok(section);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }

        [Route("api/Sections/AssignStudents")]
        [ResponseType(typeof(Section))]
        [CustomAuthorize(Roles = "Admin,Professor")]
        [ValidateModel]
        public IHttpActionResult PostAssignStudents(SectionStudentModel model)
        {
            try
            {
                _sectionServices.AssignStudents(model);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }
        
        [Route("api/Sections/RemoveStudents")]
        [ResponseType(typeof(Section))]
        [CustomAuthorize(Roles = "Admin,Professor")]
        [ValidateModel]
        public IHttpActionResult PostRemoveStudents(SectionStudentModel model)
        {
            try
            {
                _sectionServices.RemoveStudents(model);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }

        // PUT: api/Sections/5
        [ResponseType(typeof(Section))]
        [Route("api/Sections/{sectionId}")]
        [ValidateModel]
        public IHttpActionResult PutSection(long sectionId,SectionEntryModel model)
        {
            try
            {
                var tmpSection = _sectionServices.UpdateSection(sectionId, model);
                return Ok(tmpSection);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }

        // DELETE: api/Sections/5
        [ResponseType(typeof(Section))]
        [Route("api/Sections/{sectionId}")]
        [CustomAuthorize(Roles = "Admin,Professor")]
        public IHttpActionResult DeleteSection(long sectionId)
        {
            try
            {
                Section section = _sectionServices.Delete(sectionId);
                return Ok(section);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }

        [Route("api/Sections/Reassign")]
        [ResponseType(typeof(Section))]
        [CustomAuthorize(Roles = "Admin,Professor")]
        [ValidateModel]
        public IHttpActionResult PostReassignStudents(SectionStudentModel model)
        {
            try
            {
                _sectionServices.RebuildSectionStudentRelationships(model);
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