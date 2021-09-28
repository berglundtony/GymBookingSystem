using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GymBookingSystem.Data;
using GymBookingSystem.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using GymBookingSystem.Extensions;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using GymBookingSystem.Models.ViewModels;

namespace GymBookingSystem.Controllers
{
    public class GymClassesController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;

        public GymClassesController(ApplicationDbContext context, IMapper mapper,  UserManager<ApplicationUser> userManager)
        {
            db = context;
            this.mapper = mapper;
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        // GET: GymClasses
        [AllowAnonymous]
        public async Task<IActionResult> Index(IndexViewModel ivm, string type)
        {
            var userId = userManager.GetUserId(User);

            if (type == "Booked")
            {
                var classes = db.GymClass.Include(g => g.AttendingMembers).ToList();
                var data = mapper.Map<IEnumerable<GymClassViewModel>>(classes,
                    opt => opt.Items.Add("Id", userId)).Where(a => a.Attending == true);
                return View(mapper.Map<IndexViewModel>(data));
            }
            else if (type == "History")
            {
                var classes = await db.GymClass.Include(g => g.AttendingMembers)
                    .IgnoreQueryFilters().ToListAsync();

                var data =  mapper.Map<IEnumerable<GymClassViewModel>>(classes,
                    opt => opt.Items.Add("Id", userId)).Where(a => a.Attending == true && a.StartTime < DateTime.Now);
                return View(mapper.Map<IndexViewModel>(data));
            }
            else if (ivm.ShowAll)
            {
                var classes = await db.GymClass.Include(g => g.AttendingMembers).IgnoreQueryFilters().ToListAsync();
                var result = mapper.Map <IndexViewModel>(classes);
                return View(result);
            }
            else
            {
                if (User.Identity.IsAuthenticated)
                {
                    var classes = db.GymClass.Include(g => g.AttendingMembers).ToList();
                    var result = mapper.Map<IndexViewModel>(classes,
                        opt => opt.Items.Add("Id", userId));

                    return View(result);
                }
                else
                {
                    var model = mapper.Map<IndexViewModel>(db.GymClass.ToList());
                    return View(model);
                }
            }
        }

        public async Task<IActionResult> BookingToggle(int id)
        {
            if (id == null) { return NotFound(); }

            var userGuid = userManager.GetUserId(User);
            if (userGuid == null) {
                return LocalRedirect("/Identity/Account/Login");
            }

            var presentedMember = await db.ApplicationGyms.FindAsync(userGuid, id);

            if (presentedMember is not null)
            {
                    db.Remove(presentedMember);
                    db.SaveChanges();
            }
            else
            {
                var applicationUser = new ApplicationUserGymClass
                {
                    ApplicationUserId = userGuid,
                    GymClassId = id
                };
                await db.AddAsync(applicationUser);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        // GET: GymClasses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gymClass = await db.GymClass
                .Include(a => a.AttendingMembers)
                .ThenInclude(a => a.ApplicationUser)
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gymClass == null)
            {
                return NotFound();
            }

            return View(gymClass);
        }

        // GET: GymClasses/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return Request.IsAjax() ? PartialView("CreatePartial") : View();
        }

        // POST: GymClasses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name,StartTime,Duration,Description")] GymClass gymClass)
        {
            if (ModelState.IsValid)
            {
                db.Add(gymClass);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gymClass);
        }

        // GET: GymClasses/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gymClass = await db.GymClass.FindAsync(id);
            if (gymClass == null)
            {
                return NotFound();
            }
            return View(gymClass);
        }

        // POST: GymClasses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StartTime,Duration,Description")] GymClass gymClass)
        {
            if (id != gymClass.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(gymClass);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GymClassExists(gymClass.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(gymClass);
        }

        // GET: GymClasses/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gymClass = await db.GymClass
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gymClass == null)
            {
                return NotFound();
            }

            return View(gymClass);
        }

        // POST: GymClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gymClass = await db.GymClass.FindAsync(id);
            db.GymClass.Remove(gymClass);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GymClassExists(int id)
        {
            return db.GymClass.Any(e => e.Id == id);
        }
    }
}
