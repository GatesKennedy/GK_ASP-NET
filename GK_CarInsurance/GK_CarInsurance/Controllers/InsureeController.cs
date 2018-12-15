using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GK_CarInsurance.Models;

namespace GK_CarInsurance.Controllers
{
    public class InsureeController : Controller
    {
        private InsuranceEntities db = new InsuranceEntities();

//=====================================================================
//      INDEX
//=====================================================================
        // GET: Insuree
        public ActionResult Index()
        {
            return View(db.Insurees.ToList());
        }


//=====================================================================
//      DETAILS
//=====================================================================
        // GET: Insuree/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }


//=====================================================================
//      CREATE
//=====================================================================
        // GET: Insuree/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Insuree/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,Email,BorthDate,CarYear,CarMake,CarModeL,DUI,Tickets,Coverage,Quote")] Insuree insuree)
        {
            ViewData["Email"] = "Timothy@AjaClark.com";
            if (ModelState.IsValid)
            {

                //CalculateQuote();
                db.Insurees.Add(insuree);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(insuree);
        }

        public int CalculateQuote([Bind(Include = "Id,FirstName,LastName,Email,BorthDate,CarYear,CarMake,CarModeL,DUI,Tickets,Coverage,Quote")] Insuree insuree)
        {
            int quoteValue = 50;
            double daysInYear =365.25;

            //  Insuree Age Fee
            int insureeAgeDays = DateTime.Today.Subtract(insuree.BorthDate).Days;

            if (insureeAgeDays < 18 * daysInYear) quoteValue += 100;
            else if (100*daysInYear < insureeAgeDays || insureeAgeDays < 25*daysInYear) quoteValue += 25;

            //  Car Age Fee
            int carAgeYears = DateTime.Today.Year - insuree.CarYear;
            if (carAgeYears > 18 || carAgeYears < 4) quoteValue += 25;

            //  Car Make Fee
            if (insuree.CarMake.ToLower() == "porsche") quoteValue += 25;

            //  Car Model Fee
            if (insuree.CarMake.ToLower().Contains("Carrera") && insuree.CarMake.ToLower().Contains("911")) quoteValue += 25;

            //  Tickets Fee
            quoteValue += 10 * insuree.Tickets;

            //  DUI Fee
            if (insuree.DUI) quoteValue += (int)(0.25 * quoteValue);

            //  Full Coverage Fee
            if (insuree.Coverage) quoteValue += (int)(0.5 * quoteValue);

            return (quoteValue);
        }

//=====================================================================
//       EDIT
//=====================================================================
        // GET: Insuree/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // POST: Insuree/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Email,BorthDate,CarYear,CarMake,CarModeL,DUI,Tickets,Coverage,Quote")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                db.Entry(insuree).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(insuree);
        }

//=====================================================================
//      DELETE
//=====================================================================
        // GET: Insuree/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // POST: Insuree/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Insuree insuree = db.Insurees.Find(id);
            db.Insurees.Remove(insuree);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

//=====================================================================
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
