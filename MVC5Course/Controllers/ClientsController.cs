﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC5Course.Models;
using MVC5Course.Models.ViewModel;

namespace MVC5Course.Controllers
{
    public class ClientsController : BaseController
    {
        //private FabricsEntities1 db = new FabricsEntities1();
        //ClientRepository repo = new ClientRepository();
        ClientRepository repo = RepositoryHelper.GetClientRepository();

        public ActionResult Index()
        {
            //var client = db.Client.Include(c => c.Occupation);
            //return View(client.OrderByDescending(c => c.ClientId).Take(100).ToList());
            //改用 Repository 實作
            var client = repo.All();
            return View(client.OrderByDescending(c => c.ClientId).Take(10).ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Client client = db.Client.Find(id);
            //改用 Repository 實作
            Client client = repo.All().FirstOrDefault(c => c.ClientId == id);

            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        public ActionResult Detail2(string name)
        {
            //改成使用 Cache all
            string[] names = name.Split('/');
            var firstName = names[0];
            var middleName = names[1];
            var lastName = names[2];

            if (name == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Client client = db.Client.Find(id);
            //改用 Repository 實作
            Client client = repo.All().FirstOrDefault(c => c.FirstName == firstName &&
                            c.MiddleName == middleName && c.LastName == lastName);

            if (client == null)
            {
                return HttpNotFound();
            }
            return View("Details", client);

            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            ////Client client = db.Client.Find(id);
            ////改用 Repository 實作
            //Client client = repo.All().FirstOrDefault(c => c.ClientId == id);

            //if (client == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(client);
        }

        public ActionResult Create()
        {
            //改用 Repository 實作
            var occupRepo = RepositoryHelper.GetOccupationRepository();
            ViewBag.OccupationId = new SelectList(occupRepo.All(), "OccupationId", "OccupationName");
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ClientId,FirstName,MiddleName,LastName,Gender," +
            "DateOfBirth,CreditRating,XCode,OccupationId,TelephoneNumber,Street1,Street2,City,ZipCode," +
            "Longitude,Latitude,Notes, IdNumber")] Client client)
        {
            if (ModelState.IsValid)
            {
                //db.Client.Add(client);
                //db.SaveChanges();
                //改用 Repository 實作
                repo.Add(client);
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            var occuRepo = RepositoryHelper.GetOccupationRepository();
            ViewBag.OccupationId = new SelectList(occuRepo.All(), "OccupationId", "OccupationName", client.OccupationId);
            return View(client);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Client client = db.Client.Find(id);
            //改用 Repository 實作
            Client client = repo.All().FirstOrDefault(c => c.ClientId == id);
            if (client == null)
            {
                return HttpNotFound();
            }
            var occuRepo = RepositoryHelper.GetOccupationRepository();
            ViewBag.OccupationId = new SelectList(occuRepo.All(), "OccupationId", "OccupationName", client.OccupationId);
            return View(client);
        }

        // POST: Clients/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "ClientId,FirstName,MiddleName,LastName,Gender,DateOfBirth,CreditRating,XCode,OccupationId,TelephoneNumber,Street1,Street2,City,ZipCode,Longitude,Latitude,Notes, IdNumber")] Client client)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        //db.Entry(client).State = EntityState.Modified;
        //        //db.SaveChanges();
        //        //改用 Repository 實作
        //        var db = repo.UnitOfWork.Context;
        //        db.Entry(client).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    var occuRepo = RepositoryHelper.GetOccupationRepository();
        //    ViewBag.OccupationId = new SelectList(occuRepo.All(), "OccupationId", "OccupationName", client.OccupationId);
        //    return View(client);
        //}

        // POST: Clients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection form)
        {
            var client = repo.Find(id);
            if (TryUpdateModel(client, "", null, new string[] { "FirstName" }))
            {
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            var occuRepo = RepositoryHelper.GetOccupationRepository();
            ViewBag.OccupationId = new SelectList(occuRepo.All(), "OccupationId", "OccupationName", client.OccupationId);
            return View(client);
        }

        // GET: Clients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Client client = db.Client.Find(id);
            //改用 Repository 實作
            Client client = repo.All().FirstOrDefault(c => c.ClientId == id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Client client = db.Client.Find(id);
            //db.Client.Remove(client);
            //db.SaveChanges();
            //改用 Repository 實作
            Client client = repo.All().FirstOrDefault(c => c.ClientId == id);
            repo.Delete(client);
            repo.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
                //改用 Repository 實作
                repo.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Search(string keyword)
        {
            //var data = db.Client.Take(100).AsQueryable();
            //改用 Repository 實作
            var data = repo.All().Take(100).AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {   
                data = data.Where(c => c.FirstName.Contains(keyword));

                return View("Index", data);
            }

            return View("Index", data);
        }

        [HttpPost]
        [HandleError(ExceptionType = typeof(DbEntityValidationException), 
            View = "Error_DbEntityValidationException")]
        public ActionResult BatchUpdate(ClientBatchVM[] data)
        {
            if (ModelState.IsValid)
            {
                foreach (var item in data)
                {
                    var client = repo.All().FirstOrDefault(c => c.ClientId == item.Clientid);
                    client.FirstName = item.FirstName;
                    client.MiddleName = item.MiddleName;
                    client.LastName = item.LastName;
                    client.Longitude = default(double);
                    client.Latitude = default(double);
                }
                repo.UnitOfWork.Commit();

                return RedirectToAction("Index");
            }

            ViewData.Model = repo.All().OrderByDescending(c => c.ClientId).Take(10);

            return View("Index");
        }
    }
}
