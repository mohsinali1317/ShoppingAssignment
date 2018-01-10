using ShoppingWebsiteMVC.Models;
using ShoppingWebsiteMVC.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingWebsiteMVC.Controllers
{
    public class CartController : Controller
    {
        private ShoppingWebsiteMVCEntities4 db = new ShoppingWebsiteMVCEntities4();

        public static List<Cart> Cart = new List<Cart>();

        public static string Message;

        // GET: Cart
        public ActionResult Index()
        {
            if (TempData["cart"] as List<Cart> != null)
                Cart = TempData["cart"] as List<Cart>;


            int totalPrice = 0;

            foreach(Cart c in Cart)
            {
                totalPrice += Int32.Parse(c.Product.Price) * c.Amount;
            }

            ViewBag.TotalPrice = totalPrice;
            ViewBag.Message = Message;
            return View(Cart);
        }

        public ActionResult Remove(int id)
        {
            var c = Cart.Where(i => i.Product.Id == id).FirstOrDefault();

            if (c == null)
                return RedirectToAction("Index");

            if (c.Amount == 1)
                Cart.Remove(c);
            else
                c.Amount -= 1;


            return RedirectToAction("Index");

        }

        public ActionResult CheckOut()
        {
            int totalPrice = 0;

            foreach (Cart c in Cart)
            {
                totalPrice += Int32.Parse(c.Product.Price) * c.Amount;
            }

            order Order = new order();
            Order.Paid = false;
            Order.Price = totalPrice.ToString();

            db.orders.Add(Order);


            foreach (Cart c in Cart)
            {

                OrderDetail orderDetails = new OrderDetail();
                orderDetails.OrderId = Order.Id;
                orderDetails.ProductId = c.Product.Id;
                orderDetails.Amount = c.Amount;

                db.OrderDetails.Add(orderDetails);

            }

            db.SaveChanges();

            Cart.Clear();

            Message = "Your order is placed. Your order Id is: " + Order.Id;


            return RedirectToAction("Index");
        }


        public ActionResult ToProduct()
        {
            Message = String.Empty;

            return RedirectToAction("Index", "Products");

        }

    }
}