using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Server.Utils;
using Server.Model;
using Server.Auth;
using Server.Database;
using System.Net.Mail;
using System.Text.Json;
using System.Text;
using System.Security.Principal;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Server.Controllers.Api
{
    [ApiController]
    [Route("api/account")]          
    public class AccountController : Controller
    {
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(SignUpRequest signUpRequest)
        {
            Captcha captcha = new Captcha(signUpRequest.CaptchaToken);
            int? maybeId = await Database.Account.UserExistsEmail(signUpRequest.Email);

            if (maybeId is int id)
            {
                if (await Database.Account.UserInfo(id) is Model.Account info)
                {
                    if (info.IsValidated || info.Username != signUpRequest.Username)
                    {
                        return new ContentResult
                        {
                            StatusCode = StatusCodes.Status400BadRequest,
                            ContentType = "text/plain",
                            Content = "Cette adresse mail est déja utilisée",
                        };
                    }
                }
                else
                {
                    return new ContentResult
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        ContentType = "text/plain",
                        Content = "Cette adresse mail est déja utilisée",
                    };
                }
            }
            
            if (await Database.Account.UserExists(signUpRequest.Username))
            {
                return new ContentResult
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ContentType = "text/plain",
                    Content = "Ce nom d'utilisateur est déja utilisé",
                };
            }
            
            if (!(await captcha.IsValid()))
            {
                return new ContentResult
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ContentType = "text/plain",
                    Content = "Le CAPTCHA n'a pas été validé ou a expiré",
                };
            }
            
            String tempPwd = await Database.Account.CreateTemp(signUpRequest.Email, signUpRequest.Username);

            Email message = new Email(
                signUpRequest.Email,
                "Mot de passe provisoire",
                Template.Get("signup_email.html").Render(signUpRequest.Username, tempPwd)
            );

            message.Send();

            return new StatusCodeResult(201);
        }

        [Authorize(AuthenticationSchemes="Basic")]
        [HttpPost("signin")]
        public async Task<IActionResult> SignIn()
        {
            ContentResult result = new ContentResult
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                ContentType = "text/plain",
                Content = "No result",
            };

            int? maybeId = HttpContext.User.UserId();
            bool? maybeValidated = HttpContext.User.IsValidated();

            if (maybeId is int id)
            {
                if (maybeValidated is bool validated)
                {
                    if (validated)
                    {
                        String token = Util.RandomPassword(30);

                        if (await Database.Account.CreateSession(id, token))
                        {
                            result.Content = JsonSerializer.Serialize<SignInSuccess>(
                                new SignInSuccess
                                {
                                    Validated = true,
                                    SessionToken = token,
                                },
                                Util.DefaultJsonOptions
                            );
                            result.ContentType = "application/json; charset=UTF-8";
                            result.StatusCode = StatusCodes.Status200OK;
                        }
                        else
                        {
                            result.Content = "Couldn't create session";
                            result.StatusCode = StatusCodes.Status503ServiceUnavailable;
                        }
                    }
                    else
                    {
                        result.Content = JsonSerializer.Serialize<SignInSuccess>(
                            new SignInSuccess
                            {
                                Validated = false,
                            },
                            Util.DefaultJsonOptions
                        );
                        result.ContentType = "application/json; charset=UTF-8";
                        result.StatusCode = StatusCodes.Status200OK;
                    }

                }
            }

            return result;
        }
    }
}
