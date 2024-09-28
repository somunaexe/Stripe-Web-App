using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe.Checkout;
using StripeWebApp.Data;
using StripeWebApp.Models;

namespace StripeWebApp.Controllers
{
    public class PaymentController : Controller
    {
        private readonly MvcContext _context;
        public PaymentController(MvcContext context){
            _context = context;
        }
        public async Task<IActionResult> Index(){
            return View(await _context.Items.ToListAsync());
        }
         public ActionResult CreateCheckout([Bind("Id, Name, imageUrl, PriceId")] Item item)
        {
            Console.WriteLine(1);
            var domain = "http://localhost:5083/";
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
                {
                  new SessionLineItemOptions
                  {
                    // Provide the exact Price ID (for example, pr_1234) of the product you want to sell
                    Price = $"{item.PriceId}",
                    Quantity = 1,
                  },
                },
                Mode = "payment",
                SuccessUrl = domain + "/Payment/Success",
                CancelUrl = domain + "/Payment/Cancel",
                AutomaticTax = new SessionAutomaticTaxOptions { Enabled = true },
            };
            var service = new SessionService();
            Session session = service.Create(options);

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }
        public IActionResult Success(){
            return View();
        }
        public IActionResult Cancel(){
            return View();
        }
    }
}