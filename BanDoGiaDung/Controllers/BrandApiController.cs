using BanDoGiaDung.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace BanDoGiaDung.Controllers
{
    [RoutePrefix("api/brandapi")]
    public class BrandApiController : ApiController
    {
        private readonly GiaDungDbContext db = new GiaDungDbContext();

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetBrands()
        {
            var brands = db.Brands
                .Select(b => new
                {
                    b.brand_id,
                    b.brand_name
                })
                .ToList();

            return Ok(brands);
        }
    }

}