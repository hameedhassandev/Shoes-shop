using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shoes_shop.Helpers;
using Shoes_shop.Models.Repositories;
using Shoes_shop.Models;
using Shoes_shop.ViewModels;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Drawing;
using System.IO;
using Syncfusion.Pdf.Grid;

namespace Shoes_shop.Controllers
{
    [Authorize(Roles = RolesName.AdminRole)]
    public class OrderAdminController : Controller
    {
        private readonly IOrderService orderService;
        private readonly IOrderDetailsService orderDetailsService;
        private readonly UserManager<ApplicationUser> UserManager;


        public OrderAdminController(IOrderService _orderService, IOrderDetailsService _orderDetailsService, UserManager<ApplicationUser> _UserManager)
        {
            orderService = _orderService;
            orderDetailsService = _orderDetailsService;
            UserManager = _UserManager;
        }

        public IActionResult Index()
        {
            var allOrders = orderService.All();

            return View(allOrders); 
        }

        public async Task<IActionResult> OrderDetails(int id)
        {
            ViewBag.orderId = id;
            var model = orderService.GetOrder(id);
            if (model == null)
                return NotFound();
            var orderDetails = orderDetailsService.Find(id).ToList();
         
            var user = await UserManager.FindByIdAsync(model.UserId);
            var fullName = user.FullName;
            var userMail = user.Email;
            var gender = user.Gender;

            var orderVM = new OrderAndDetailsVM
            {
                Id = model.Id,
                dateTime = model.dateTime,
                TotalPrice = model.TotalPrice,
                ShippingAddress = model.ShippingAddress,
                Contact = model.Contact,
                FullNam = fullName,
                Email = userMail,
                Gender = gender,
                IsConfirmed = model.IsConfirmed,
                IsShippedAndPay = model.IsShippedAndPay,
                OrderDetails = orderDetails,
            };

            return View(orderVM);
        }
        public IActionResult OrderReports()
        {
            var allConfirmedOrders = orderService.OrderReports();

            return View(allConfirmedOrders);
        }


        public IActionResult ConfirmOrder(int id)
        {
            var order = orderService.GetOrder(id);
            if (order == null)
                return NotFound();

            order.IsConfirmed = true;
            orderService.Update(order);


            return RedirectToAction(nameof(Index));
        }

        public IActionResult ShippedOrder(int id)
        {
            var order = orderService.GetOrder(id);
            if (order == null)
                return NotFound();

            order.IsShippedAndPay = true;
            orderService.Update(order);


            return RedirectToAction(nameof(Index));
        }



        public IActionResult CreateDocument()
        {

            //Generate a new PDF document.
            PdfDocument doc = new PdfDocument();
            //Add a page.
            PdfPage page = doc.Pages.Add();
            //Create a PdfGrid.
            PdfGrid pdfGrid = new PdfGrid();
            //Add values to list
            var allOrder = orderService.OrderPdfReports();

         
            //Add list to IEnumerable
            IEnumerable<object> dataTable = allOrder;
            //Assign data source.
            pdfGrid.DataSource = dataTable;
            //Draw grid to the page of PDF document.
            pdfGrid.Draw(page, new Syncfusion.Drawing.PointF(10, 10));
            //Write the PDF document to stream
            MemoryStream stream = new MemoryStream();
            doc.Save(stream);
            //If the position is not set to '0' then the PDF will be empty.
            stream.Position = 0;
            //Close the document.
            doc.Close(true);
            //Defining the ContentType for pdf file.
            string contentType = "application/pdf";
            //Define the file name.
            string fileName = "Output.pdf";
            //Creates a FileContentResult object by using the file contents, content type, and file name.
            return File(stream, contentType, fileName);
        }



    }
}
