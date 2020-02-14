using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Authorization.Infrastructure.CustomPolicy;
using Authorization.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Protected()
        {
            return View("ProtectedPage", new ViewModel { 
                Header = "Protected Page", 
                Description = "Simple Authentication/Authorization used to access to this page" 
            });
        }

        [Authorize(Policy = "Employee")]
        public IActionResult PolicyProtected()
        {
            return View("ProtectedPage", new ViewModel
            {
                Header = "Policy Protected Page",
                Description = "Access allowed for user with specific policy"
            });
        }

        [SecurityLevel(4)]
        public IActionResult SecretLevel()
        {
            return View("ProtectedPage", new ViewModel
            {
                Header = "Secret Page",
                Description = "Access allowed for user with security level 4 and above"
            });
        }

        [SecurityLevel(8)]
        public IActionResult TopSecretLevel()
        {
            return View("ProtectedPage", new ViewModel
            {
                Header = "Top Secret Page",
                Description = "Access allowed for user with security level 8 and above"
            });
        }


        public IActionResult Register()
        {
            return View();
        }

        public IActionResult SignOut()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }

        public IActionResult AccessDenied()
        {
            var username = HttpContext.User.Identity.Name;
            return View("ProtectedPage", new ViewModel
            {
                Header = "Access Denined",
                Description = $"Access Denied for user {username.ToUpper()}"
            });
        }


        [HttpPost]
        public IActionResult Register(string username)
        {
            var primaryClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Email, $"{username}@mail.com")
            };

            var companyClaims = new List<Claim>()
            {
                new Claim("Claim.Employee", "Employee"),
                new Claim(SecurityPolicyType.SecurityLevel, "8")
            };

            var contractorClaims = new List<Claim>()
            {
                new Claim("Claim.Contractor", "NonEmployee"),
                new Claim(SecurityPolicyType.SecurityLevel, "4")
            };

            var primaryIdentity = new ClaimsIdentity(primaryClaims, "Primary Identity");
            var companyIdentity = username == "john" ? new ClaimsIdentity(companyClaims, "Company Identity")
                : new ClaimsIdentity(contractorClaims, "CompanyIdentity");

            var userPrincipal = new ClaimsPrincipal(new[] { primaryIdentity, companyIdentity });

            HttpContext.SignInAsync(userPrincipal);

            return RedirectToAction("Index");
        }
    }
}
