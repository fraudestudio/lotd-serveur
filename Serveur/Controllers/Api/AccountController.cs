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
            
            String tempPwd;
            if (await Account.UserExistsEmail(signUpRequest.Email))
            {
                return new ContentResult
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ContentType = "text/plain",
                    Content = "EMAIL EXISTS",
                };
            }
            else if (await Account.UserExists(signUpRequest.Username))
            {
                return new ContentResult
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ContentType = "text/plain",
                    Content = "USERNAME EXISTS",
                };
            }
            else if (!(await captcha.IsValid()))
            {
                return new ContentResult
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ContentType = "text/plain",
                    Content = "INVALID CAPTCHA",
                };
            }
            else
            {
                tempPwd = await Account.CreateTemp(signUpRequest.Email, signUpRequest.Username);

                Email message = new Email(
                    email,
                    "Mot de passe provisoire",
                    Template.Get("signup_email.html").Render(signUpRequest.Username, tempPwd)
                );

                message.Send();

                return new ContentResult
                {
                    StatusCode = StatusCodes.Status201Created,
                    ContentType = "text/plain",
                    Content = "OK",
                };
            }
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
                        String token = Utils.Utils.RandomPassword(30);

                        if (await Account.CreateSession(id, token))
                        {
                            result.Content = JsonSerializer.Serialize<SignInSuccess>(
                                new SignInSuccess
                                {
                                    Validated = true,
                                    SessionToken = token,
                                },
                                Utils.Utils.DefaultJsonOptions
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
                            Utils.Utils.DefaultJsonOptions
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
