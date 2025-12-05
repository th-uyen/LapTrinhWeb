using BanDoGiaDung.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace BanDoGiaDung.Controllers
{
    [RoutePrefix("api/genreapi")]
    public class GenreApiController : ApiController
    {
        private readonly GiaDungDbContext db = new GiaDungDbContext();

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetGenres()
        {
            var genres = db.Genres
                .Select(g => new
                {
                    g.genre_id,
                    g.genre_name
                })
                .ToList();

            return Ok(genres);
        }
    }

}