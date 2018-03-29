using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using System.Web.OData;
using VinculacionBackend.ActionFilters;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Interfaces;
using VinculacionBackend.Models;
using VinculacionBackend.Security.BasicAuthentication;

namespace VinculacionBackend.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ClassesController : ApiController
    {
        private readonly IClassesServices _classesServices;
        private readonly ILogger _logger;

        public ClassesController(IClassesServices classesServices, ILogger logger)
        {
            _classesServices = classesServices;
            _logger = logger;
        }

        // GET: api/Classes
        [Route("api/Classes")]
        [EnableQuery]
        public IQueryable<Class> GetClasses()
        {
            try
            {
                return _classesServices.All();
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }

        // GET: api/Classes/5
        [ResponseType(typeof(Class))]
        [Route("api/Classes/{id}")]
        public IHttpActionResult GetClass(long id)
        {
            try
            {
                Class @class = _classesServices.Find(id);
                return Ok(@class);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }

        // POST: api/Classes
        [ResponseType(typeof(Class))]
        [Route("api/Classes")]
        [CustomAuthorize(Roles = "Admin")]
        [ValidateModel]
        public IHttpActionResult PostClass(ClassEntryModel classModel)
        {
            try
            {
                var newClass = new Class();
                _classesServices.Map(newClass, classModel);
                _classesServices.Add(newClass);
                return Ok(newClass);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }


        [ResponseType(typeof(Class))]
        [Route("api/Classes/{classId}")]
        [CustomAuthorize(Roles = "Admin")]
        [ValidateModel]
        public IHttpActionResult PutClass(long classId,ClassEntryModel classModel)
        {
            try
            {
                var Class = _classesServices.UpdateClass(classId, classModel);
                return Ok(Class);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }

        // DELETE: api/Classes/5
        [Route("api/Classes/{id}")]
        [ResponseType(typeof(Class))]
        public IHttpActionResult DeleteClass(long id)
        {
            try
            {
                var @class = _classesServices.Delete(id);
                return Ok(@class);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }   
        }
    }
}