using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Fanview.API.Repository.Interface;
using Fanview.API.Model;

namespace Fanview.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class TakeDamageController : Controller
    {
        private ITakeDamageRepository _takeDamageRepository;

        public TakeDamageController(ITakeDamageRepository takeDamageRepository)
        {
            _takeDamageRepository = takeDamageRepository;
        }
        //// GET: api/TakeDamage        
        //[HttpGet("PlayerTakeDamaged")]
        //public IEnumerable<TakeDamage> Get()
        //{
        //    var result = _takeDamageRepository.GetPlayerTakeDamage();

        //    // return result.Result.Count() > 0 ? result.Result : null;
        //    return result.Result;
        //}
    }
}
