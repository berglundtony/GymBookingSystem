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
        private readonly UserManager<ApplicationUser> _userManager;
        //private readonly IMapper mapper;

        public GymClassesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            db = context;
            _userManager = userManager;
            //this.mapper = mapper;
        }

        // GET: GymClasses
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            //var model = mapper.ProjectTo<GymClassViewModel>(db.GymClass);
            bool isBooked = false;
            var gymclasses = await db.GymClass.ToListAsync();
           
            var _gymlist = new List<GymClassViewModel>();

            foreach (var gymclass in gymclasses)
            {
                var id = gymclass.Id;
                isBooked = await Attending(userId, id);
                _gymlist.Add(new GymClassViewModel
                {
                     Id = gymclass.Id,
                     Name = gymclass.Name,
                     Description = gymclass.Description,
                     StartTime = gymclass.StartTime,
                     Duration = gymclass.Duration,
                     Booked = isBooked
                });
            }

            return View(_gymlist);
        }

        private async Task<bool> Attending(string userId, int id)
        {
            var attending = await db.ApplicationGyms.FindAsync(userId, id);

            if (attending is null)
            {
               return false;
            }
            else
            {
                return true;
            }
        }

        [Authorize]
        public async Task<IActionResult> BookingToggle(int id)
        {
            if (id == null) { return NotFound(); }

            var userGuid = _userManager.GetUserId(User);
            if (userGuid == null) {
                return LocalRedirect("/Identity/Account/Login");
            }

            var attendingMember = await db.GymClass.Include(u => u.AttendingMembers)
                .FirstOrDefaultAsync(gc => gc.Id == id);

            var presentedMember = await db.ApplicationGyms.FindAsync(userGuid, id);


            if (attendingMember.AttendingMembers.Any(a => a.ApplicationUserId == userGuid))
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
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gymClass == null)
            {
                return NotFound();
            }

            return View(gymClass);
        }

        // GET: GymClasses/Create
        [Authorize]
        public IActionResult Create()
        {
            return Request.IsAjax() ? PartialView("CreatePartial") : View();
        }

        // POST: GymClasses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
