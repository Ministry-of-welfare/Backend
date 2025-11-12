using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server_pra.Services;
using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        // אין תלות על־ידי Constructor כדי למנוע בעיות DI/Startup.
        // JwtService נשלף רק בזמן קריאת הפעולה, עם טיפול בשגיאות.
        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // בדיקה זמנית בלבד — החלף בבדיקה מול DB/BL
            if (model.Username == "admin" && model.Password == "1234")
            {
                // prefer fail-fast so a missing registration is obvious
                var jwt = HttpContext.RequestServices.GetRequiredService<JwtService>();

                try
                {
                    var token = jwt.GenerateToken("1", "Admin");
                    return Ok(new { token });
                }
                catch (Exception ex)
                {
                    // החזרת שגיאה מסודרת במקום לזרוק חריגה שתשבר את Swagger
                    return StatusCode(500, new { message = "Failed to generate token", details = ex.Message });
                }
            }

            return Unauthorized();
        }

       
    }

    public class LoginModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}