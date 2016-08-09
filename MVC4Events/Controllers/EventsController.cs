using System;
using System.Collections;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

using MVC4Events.Models;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace MVC4Events.Controllers
{
    public class EventsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Events
        public ActionResult Index()
        {
            return View(db.Events.ToList());
        }

        // GET: Events/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventModel eventModel = db.Events.Find(id);
            if (eventModel == null)
            {
                return HttpNotFound();
            }
            return View(eventModel);
        }

        // GET: Events/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,Technology,StartingDate,RegistrationLink")] EventModel eventModel)
        {
            if (ModelState.IsValid)
            {
                db.Events.Add(eventModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(eventModel);
        }

        // GET: Events/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventModel eventModel = db.Events.Find(id);
            if (eventModel == null)
            {
                return HttpNotFound();
            }
            return View(eventModel);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,Technology,StartingDate,RegistrationLink")] EventModel eventModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eventModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(eventModel);
        }

        // GET: Events/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventModel eventModel = db.Events.Find(id);
            if (eventModel == null)
            {
                return HttpNotFound();
            }
            return View(eventModel);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EventModel eventModel = db.Events.Find(id);
            db.Events.Remove(eventModel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Return event array for DataTables server-side processing mode
        /// </summary>
        /// <param name="sEcho">Information for DataTables to use for rendering</param>
        /// <param name="iDisplayStart">Display start point in the current data set</param>
        /// <param name="iDisplayLength">
        ///     Number of records that the table can display in the current draw. It is expected that the 
        ///     number of records returned will be equal to this number, unless the server has fewer records 
        ///     to return.
        /// </param>
        /// <param name="sSearch">Global search field</param>
        /// <returns>Array of events in JSON format</returns>
        [OutputCache(CacheProfile = "CacheEvents")]
        public string Query(string sEcho, int iDisplayStart, int iDisplayLength, string sSearch = null)
        {
            //  TODO: Do this job in the database
            Expression<Func<EventModel, bool>> searchExpresion = 
                e => sSearch.Equals(null) || e.Title.Contains(sSearch);

            var filteredRecordCount = db.Events
                .Where(searchExpresion)
                .Count();

            var displayEvents = db.Events
                .Where(searchExpresion)
                .OrderBy(e => e.ID)
                .Skip(iDisplayStart)
                .Take(iDisplayLength)
                .ToList();
            //  TODO: End

            var response = new
            {
                iTotalRecords = db.Events.Count(),
                iTotalDisplayRecords = filteredRecordCount,
                sEcho = sEcho,
                aaData = displayEvents
                    .Select(e => new ArrayList { e.Title, e.Technology, e.StartingDate, e.RegistrationLink })
            };

            return JsonConvert.SerializeObject(response);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}