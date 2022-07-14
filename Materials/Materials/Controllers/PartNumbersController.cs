using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Materials.Models;
using OfficeOpenXml;

namespace Materials.Controllers
{
    public class PartNumbersController : Controller
    {
        private MaterialsEntities db = new MaterialsEntities();

        // GET: PartNumbers
        public ActionResult Index()
        {
            var partNumbers = db.PartNumbers.Include(p => p.Customers);
            return View(partNumbers.ToList());
        }

        // GET: PartNumbers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PartNumbers partNumbers = db.PartNumbers.Find(id);
            if (partNumbers == null)
            {
                return HttpNotFound();
            }
            return View(partNumbers);
        }

        // GET: PartNumbers/Create
        public ActionResult Create()
        {
            ViewBag.FKCustomer = new SelectList(db.Customers, "PKCustomers", "Customer");
            return View();
        }

        // POST: PartNumbers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PKPartNumber,PartNumber,FKCustomer,Available")] PartNumbers partNumbers)
        {
            if (ModelState.IsValid)
            {
                db.PartNumbers.Add(partNumbers);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FKCustomer = new SelectList(db.Customers, "PKCustomers", "Customer", partNumbers.FKCustomer);
            return View(partNumbers);
        }

        // GET: PartNumbers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PartNumbers partNumbers = db.PartNumbers.Find(id);
            if (partNumbers == null)
            {
                return HttpNotFound();
            }
            ViewBag.FKCustomer = new SelectList(db.Customers, "PKCustomers", "Customer", partNumbers.FKCustomer);
            return View(partNumbers);
        }

        // POST: PartNumbers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PKPartNumber,PartNumber,FKCustomer,Available")] PartNumbers partNumbers)
        {
            if (ModelState.IsValid)
            {
                db.Entry(partNumbers).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FKCustomer = new SelectList(db.Customers, "PKCustomers", "Customer", partNumbers.FKCustomer);
            return View(partNumbers);
        }

        // GET: PartNumbers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PartNumbers partNumbers = db.PartNumbers.Find(id);
            if (partNumbers == null)
            {
                return HttpNotFound();
            }
            return View(partNumbers);
        }

        // POST: PartNumbers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PartNumbers partNumbers = db.PartNumbers.Find(id);
            db.PartNumbers.Remove(partNumbers);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public void DownloadExcel()
        {
            var collection = db.PartNumbers.Include(p => p.Customers).ToList();


            ExcelPackage Ep = new ExcelPackage();
            ExcelWorksheet Sheet = Ep.Workbook.Worksheets.Add("Report");
            Sheet.Cells["A1"].Value = "Part Number";
            Sheet.Cells["B1"].Value = "Available";
            Sheet.Cells["C1"].Value = "Customer";
            Sheet.Cells["D1"].Value = "Building";
            int row = 2;
            foreach (var item in collection)
            {

                Sheet.Cells[string.Format("A{0}", row)].Value = item.PartNumber;
                Sheet.Cells[string.Format("B{0}", row)].Value = item.Available;
                Sheet.Cells[string.Format("C{0}", row)].Value = item.Customers.Customer;
                Sheet.Cells[string.Format("D{0}", row)].Value = item.Customers.Buildings.Building;
                row++;
            }


            Sheet.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "Report.xlsx");
            Response.BinaryWrite(Ep.GetAsByteArray());
            Response.End();
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
