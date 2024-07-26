using Api2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api2.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PicturesController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public IActionResult GetPictures()
        {
            var pictureList = new List<Picture>()
            {
                new Picture() {Id = 1, Name = "Picture 1", Url = "www.mypicture.com"},
                new Picture() {Id = 2, Name = "Picture 2", Url = "www.mypicture2.com"},
                new Picture() {Id = 2, Name = "Picture 3", Url = "www.mypicture3.com"},
                new Picture() {Id = 2, Name = "Picture 4", Url = "www.mypicture4.com"},
                new Picture() {Id = 2, Name = "Picture 5", Url = "www.mypicture5.com"},
            };

            return Ok(pictureList);
        }
    }
}
