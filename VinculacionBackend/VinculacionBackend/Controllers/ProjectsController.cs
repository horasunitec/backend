using System;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Cors;
using System.Web.OData;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.ActionFilters;
using VinculacionBackend.Interfaces;
using VinculacionBackend.Models;
using VinculacionBackend.Security.BasicAuthentication;

namespace VinculacionBackend.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ProjectsController : ApiController
    {
        private readonly IProjectServices _services;
        private readonly ILogger _logger;

        public ProjectsController(IProjectServices services, ILogger logger)
        {
            _services = services;
            _logger = logger;
        }


        // GET: api/Projects
        [Route("api/Projects")]
        [CustomAuthorize(Roles = "Admin,Professor,Student")]
        [EnableQuery]
        public IQueryable<Project> GetProjects()
        {
            try
            {
                return _services.All();
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }   
        }



        // GET: api/Projects
        [Route("api/Projects/ProjectsByUser")]
        [CustomAuthorize(Roles = "Admin,Professor,Student")]
        [EnableQuery]
        public IQueryable<Project> GetProjectsByUser()
        {
            try
            {
                var currentUser = (CustomPrincipal)HttpContext.Current.User;
                return _services.GetUserProjects(currentUser.UserId, currentUser.roles);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
            
        }

        [Route("api/ProjectsCount")]
        [CustomAuthorize(Roles = "Admin,Professor,Student")]
        public int GetProjectsCount()
        {
            try
            {
                var currentUser = (CustomPrincipal)HttpContext.Current.User;
                return _services.GetUserProjects(currentUser.UserId, currentUser.roles).Count();
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }

        // GET: api/Projects/5
        [ResponseType(typeof(Project))]
        [Route("api/Projects/{projectId}")]
        [CustomAuthorize(Roles = "Admin,Professor,Student")]
        public IHttpActionResult GetProject(long projectId)
        {
            try
            {
                Project project = _services.Find(projectId);
                return Ok(project);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }   
        }

        // Get: api/Projects/FinalReport/
        [Route("api/Projects/FinalReport/{projectId}/{sectionId}/{fieldHours}/{calification}/{beneficiariesQuantities}/{beneficiariGroups}")]
        public HttpResponseMessage GetProjectFinalReport(long projectId,long sectionId,int fieldHours, int calification, int beneficiariesQuantities, string beneficiariGroups)
        {
            try
            {
                return _services.GetFinalReport(projectId, sectionId, fieldHours, calification, beneficiariesQuantities, beneficiariGroups);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }

        // GET: api/Projects/5
        [ResponseType(typeof(Project))]
        [Route("api/Projects/Students/{projectId}")]
        [CustomAuthorize(Roles = "Admin,Professor,Student")]
        public IQueryable<User> GetProjectStudents(long projectId)
        {
            try
            {
                return _services.GetProjectStudents(projectId);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }

        // GET: api/Projects
        [Route("api/Projects/ProjectsBySection/{sectionId}")]
        [CustomAuthorize(Roles = "Admin,Professor,Student")]
        [EnableQuery]
        public IQueryable<Project> GetProjectsBySection(long sectionId)
        {
            try
            {
                var projects = _services.GetProjectsBySection(sectionId);
                return projects;
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }   
        }

        // PUT: api/Projects/5
        [ResponseType(typeof(void))]
        [Route("api/Projects/{projectId}")]
        [ValidateModel]
        public IHttpActionResult PutProject(long projectId, ProjectModel model)
        {
            try
            {
                var tmpProject = _services.UpdateProject(projectId, model);
                return Ok(tmpProject);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }

        [ResponseType(typeof(void))]
        [Route("api/Projects/AssignSection")]
        [ValidateModel]
        public IHttpActionResult PostAssignSection(ProjectSectionModel model)
        {
            try
            {
                _services.AssignSection(model);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }

        [ResponseType(typeof(void))]
        [Route("api/Projects/AssignProjectsToSection")]
        [ValidateModel]
        public IHttpActionResult PostAssignSections(ProjectsSectionModel model)
        {
            try
            {
                _services.AssignProjectsToSection(model);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }


        [ResponseType(typeof(void))]
        [Route("api/Projects/RemoveSection")]
        [ValidateModel]
        public IHttpActionResult PostRemoveSection(ProjectSectionModel model)
        {
            try
            {
                _services.RemoveFromSection(model.ProjectId, model.SectionId);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }

        // POST: api/Projects
        [Route("api/Projects")]
        [ResponseType(typeof(Project))]
        [CustomAuthorize(Roles = "Admin")]
        [ValidateModel]
        public IHttpActionResult PostProject(ProjectModel model)
        {
            try
            {
                var project = _services.Add(model);
                return Ok(project);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }

        //DELETE: api/Projects/5
        [Route("api/Projects/{projectId}")]
        [CustomAuthorize(Roles = "Admin")]
        [ResponseType(typeof(Project))]
        public IHttpActionResult DeleteProject(long projectId)
        {
            try
            {
                Project project = _services.Delete(projectId);
                return Ok(project);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }
    }
}