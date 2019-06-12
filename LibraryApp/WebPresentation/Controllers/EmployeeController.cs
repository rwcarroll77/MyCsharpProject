using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DataObjects;
using LogicLayer;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using WebPresentation.Models;

namespace WebPresentation.Controllers
{
    [Authorize(Roles ="Admin")]
    public class EmployeeController : Controller
    {
        private ApplicationUserManager userManager;
        private EmployeeManager _employeeManager = new EmployeeManager();

        // GET: Employee
        public ActionResult Index()
        {
            userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            return View(userManager.Users.ToList());
        }

        // GET: Employee/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            ApplicationUser applicationUser = userManager.FindById(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }

            var allRoles = new string[] { "Basic", "Admin" };

            var roles = userManager.GetRoles(id);

            var noRoles = allRoles.Except(roles);

            ViewBag.Roles = roles;
            ViewBag.NoRoles = noRoles;
            return View(applicationUser);
        }


        public ActionResult AddRole(string id, string role)
        {
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.Users.First(u => u.Id == id);

            userManager.AddToRole(id, role);

            var allRoles = new string[] { "Basic", "Admin"  };

            var roles = userManager.GetRoles(id);

            var noRoles = allRoles.Except(roles);

            ViewBag.Roles = roles;
            ViewBag.NoRoles = noRoles;

            return View("Details", user);
        }
        public ActionResult RemoveRole(string id, string role)
        {
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.Users.First(u => u.Id == id);

            userManager.RemoveFromRole(id, role);

            var allRoles = new string[] { "Rental", "Checkout",
                "Inspection", "Maintenance",
                "Prep", "Manager", "Admin"  };

            var roles = userManager.GetRoles(id);

            var noRoles = allRoles.Except(roles);

            ViewBag.Roles = roles;
            ViewBag.NoRoles = noRoles;

            return View("Details", user);
        }

        //// GET: Employee/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Employee/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> CreateAsync(RegisterViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = new ApplicationUser
        //        {
        //            UserName = model.Email,
        //            Email = model.Email,
        //            screenName = model.ScreenName,
        //            firstName = model.FirstName,
        //            lastName = model.LastName,
        //            PhoneNumber = model.PhoneNumber
        //        };
        //        var result = await userManager.CreateAsync(user, model.Password);
        //        var employee = new Employee(model.FirstName, model.LastName, model.PhoneNumber, model.Email, "Basic");
        //        try
        //        {
        //            _employeeManager.AddEmployee(employee);
        //        }
        //        catch (Exception ex)
        //        {

        //        }
        //        if (result.Succeeded)
        //        {
        //            //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

        //            // For more information on how to enable account confirmation and password reset please visit
        //            // https://go.microsoft.com/fwlink/?LinkID=320771
        //            // Send an email with this link
        //            // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
        //            // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
        //            // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
        //            return RedirectToAction("Index", "Employee");
        //        }
        //        AddErrors(result);
        //    }
        //    return View(model);
        //}

        //private void AddErrors(IdentityResult result)
        //{
        //    foreach (var error in result.Errors)
        //    {
        //        ModelState.AddModelError("", error);
        //    }

        //}

        // GET: Employee/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: Employee/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: Employee/Delete/5
        //public ActionResult Delete(string id)
        //{
        //    return View();
        //}

        //// POST: Employee/Delete/5
        //[HttpPost]
        //public ActionResult Delete(string id)
        //{
        //    try
        //    {
        //        if (id == null)
        //        {
        //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //        }

        //        userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        //        ApplicationUser applicationUser = userManager.FindById(id);
        //        if (applicationUser == null)
        //        {
        //            return HttpNotFound();
        //        }
        //        userManager.Delete(applicationUser);
        //        _employeeManager.DeactivateEmployeeByEmail(applicationUser.Email);
        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
