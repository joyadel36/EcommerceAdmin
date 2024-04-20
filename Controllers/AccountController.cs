using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Repositories;
using WebApplication1.ViewModels;
using System.Security.Claims;

namespace WebApplication1.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _UManager;
        private readonly SignInManager<ApplicationUser> _SIManager;
        private readonly ISessionRepo _Session;
        public AccountController(UserManager<ApplicationUser> umanager, SignInManager<ApplicationUser> simanager, ISessionRepo session)
        {       
            _UManager = umanager;
            _SIManager = simanager;
            _Session = session;

        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(SignUpAuthViewModel SUUser)
        {
            //add new user
            ApplicationUser user = new ApplicationUser();
            user.FirstName = SUUser.FirstName;
            user.LastName = SUUser.LastName;
            user.UserName = SUUser.FirstName + SUUser.LastName;
            user.BuildingNumber = SUUser.BuildingNumber;
            user.City = SUUser.City;
            user.Street = SUUser.Street;
            user.Region = SUUser.Region;
            user.PhoneNumber = SUUser.PhoneNumber;
            user.Email = SUUser.Email;
            user.PasswordHash = SUUser.Password;
            IdentityResult IResult = await _UManager.CreateAsync(user, SUUser.Password);

            if (IResult.Succeeded)
            {
                //add to session
                string userid = await _UManager.GetUserIdAsync(user);
                Sessions session = new Sessions();
                session.ApplicationUserId = userid;
                session.LastSessionTime = DateTime.Now;
                session.UserEmail = SUUser.FirstName + SUUser.LastName;
                _Session.CreateSession(session);

                await _SIManager.SignInAsync(user, true);
                return RedirectToAction("Home", "Index");
            }
            else
            {
                foreach (var error in IResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View();
        }

        
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LogInAuthViewModel LIUser, string returnUrl)
        {
            ApplicationUser? user = await _UManager.FindByNameAsync(LIUser.UserName);
            
            if (user != null)
            {
                bool found = await _UManager.CheckPasswordAsync(user, LIUser.Password);
                if (found)
                {
                    //addtosession
                    bool sessionisnotfound = CheckAndAddSession(user.Id, user.UserName);

                    if (sessionisnotfound) {

                        await _SIManager.SignInAsync(user, true);
                        if (!string.IsNullOrEmpty(returnUrl))
                        {

                            return LocalRedirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Home", "Index");
                        }


                    }
                    else {

                        ModelState.AddModelError("", "You're already signed in from somewhere else..");
                        return View();
                    }
                    

                }

            }

            ModelState.AddModelError("", "Wrong UserName Or Password");
            return View();
        }


        public async Task<IActionResult> Logout()
        {
            //Removesession
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _Session.DeleteSessions(UserId);
            await _SIManager.SignOutAsync();
            return RedirectToAction("Home", "Index");
        }


        private bool CheckAndAddSession(string UserId, string UserName)
        {
            Sessions? session = _Session.GetSessionByUserId(UserId);

            if (session == null)
            {
                Sessions newsession = new Sessions();
                newsession.ApplicationUserId = UserId;
                newsession.LastSessionTime = DateTime.Now;
                newsession.UserEmail = UserName;
                _Session.CreateSession(newsession);
                return true;
            }
            else
            {
                if (
                    (((DateTime.Now.Day) - (session.LastSessionTime.Day)) > 0) ||
                    (((DateTime.Now.Hour) - (session.LastSessionTime.Hour)) >= 3) ||
                  (
                    (((DateTime.Now.Hour) - (session.LastSessionTime.Hour)) == 2) &&
                    (((DateTime.Now.Minute) - (session.LastSessionTime.Minute)) >= 58)
                  )

                )
                {
                    Sessions newsession = new Sessions();
                    newsession.ApplicationUserId = UserId;
                    newsession.LastSessionTime = DateTime.Now;
                    newsession.UserEmail = UserName;
                    _Session.EditSession(UserId, newsession);
                    return true;

                }
                else
                {
                    return false;
                }



            }
        }

      
    }
}
