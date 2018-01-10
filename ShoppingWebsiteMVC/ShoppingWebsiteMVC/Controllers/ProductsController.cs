using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShoppingWebsiteMVC.Models;
using ShoppingWebsiteMVC.Models.ViewModels;

namespace ShoppingWebsiteMVC.Controllers
{
    public class ProductsController : Controller
    {
        private ShoppingWebsiteMVCEntities4 db = new ShoppingWebsiteMVCEntities4();

        public static List<Cart> Cart = new List<Cart>();

        // GET: Products
        public ActionResult Index()
        {
            ViewBag.Cart = Cart;
            return View(db.Products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Price")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Price")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public ActionResult Init()
        {
            Product product = db.Products.FirstOrDefault();

            // adding dummy data when there is no data in the database

            if (product == null)
            {

                int count = 0;
                int price = 50;
                string name = "My product ";
                while (count < 12)
                {
                    Product p = new Product();
                    p.Name = name + (count + 1);
                    price = (price + (count * 20));
                    p.Price = price.ToString();

                    db.Products.Add(p);
                    db.SaveChanges();

                    count++;
                }
            }

            return RedirectToAction("Index");

        }


        public ActionResult AddToCart(int id)
        {
            Product product = db.Products.Find(id);

            Cart c = Cart.Where(i => i.Product.Id == product.Id).FirstOrDefault();

            if (c == null)
            {
                Cart cart = new Cart();
                cart.Product = product;
                cart.Amount = 1;
                Cart.Add(cart);
            }
            else
            {
                c.Amount += 1;
            }


            return RedirectToAction("Index");
        }


        public ActionResult Expand()
        {
            TempData["cart"] = Cart;
            return RedirectToAction("Index", "Cart");

        }

    }
}
