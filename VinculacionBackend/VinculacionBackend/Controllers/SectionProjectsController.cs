using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using VinculacionBackend.ActionFilters;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Interfaces;
using VinculacionBackend.Models;
using VinculacionBackend.Security.BasicAuthentication;

namespace VinculacionBackend.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SectionProjectsController : ApiController
    {
        private readonly ISectionProjectServices _sectionProjectServices;
        private readonly ILogger _logger;

        public SectionProjectsController(ISectionProjectServices sectionProjectServices, ILogger logger)
        {
            _sectionProjectServices = sectionProjectServices;
            _logger = logger;
        }


        // GET: api/SectionProjects
        [Route("api/SectionProjects/UnApproved/")]
        [CustomAuthorize(Roles = "Admin")]
        public IQueryable<SectionProject> GetSectionProjecstUnApproved()
        {
            try
            {
                return _sectionProjectServices.GetUnapproved();
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }


        [ResponseType(typeof(SectionProject))]
        [Route("api/SectionProjects/Info/{sectionprojectId}")]
        [CustomAuthorize(Roles = "Admin,Professor")]
        public SectionProject GetSectionProject(long sectionprojectId)
        {
            try
            {
                return _sectionProjectServices.GetInfo(sectionprojectId);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }

        [ResponseType(typeof(SectionProject))]
        [Route("api/SectionProjects/Info/{sectionId}/{projectId}")]
        [CustomAuthorize(Roles = "Admin,Professor")]
        public SectionProject GetSectionProject(long sectionId,long projectId)
        {
            try
            {
                return _sectionProjectServices.GetInfo(sectionId, projectId);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }

        // POST: api/SectionProjects
        [ResponseType(typeof(SectionProject))]
        [Route("api/SectionProjects")]
        [CustomAuthorize(Roles = "Admin,Professor")]
        [ValidateModel]
        public IHttpActionResult PostSectionProject(SectionProjectEntryModel sectionProjectEntryModel)
        {
            try
            {
                return Ok(_sectionProjectServices.AddOrUpdate(sectionProjectEntryModel));
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }

        [ResponseType(typeof(void))]
        [Route("api/SectionProjects/Approve/{sectionprojectId}")]
        [CustomAuthorize(Roles = "Admin")]
        public IHttpActionResult PutSectionProject(long sectionprojectId)
        {
            try
            {
                _sectionProjectServices.Approve(sectionprojectId);
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