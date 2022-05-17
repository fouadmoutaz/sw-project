using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using onlinepharmacy.Data;
using onlinepharmacy.Models;

namespace onlinepharmacy.Controllers
{
    public class ordersController : Controller
    {
        private readonly onlinepharmacyContext _context;

        public ordersController(onlinepharmacyContext context)
        {
            _context = context;
        }

        // GET: orders
        public async Task<IActionResult> Index()
        {
              return _context.orders != null ? 
                          View(await _context.orders.ToListAsync()) :
                          Problem("Entity set 'onlinepharmacyContext.orders'  is null.");
        }

        // GET: orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.orders == null)
            {
                return NotFound();
            }

            var orders = await _context.orders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orders == null)
            {
                return NotFound();
            }

            return View(orders);
        }

        // GET: orders/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}
        public async Task<IActionResult> Create(int? id)
        {
            var product = await _context.product.FindAsync(id);

            return View(product);
        }

        // POST: orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int productId, int quantity)
        {
            //if (ModelState.IsValid)
            //{
            orders order = new orders();
            order.productId = productId;
            order.quantity = quantity;
            order.userid = Convert.ToInt32(HttpContext.Session.GetString("userid")); 
            order.orderdate = DateTime.Today;
            SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\yasmi\\Documents\\onlinepharmacy.mdf;Integrated Security=True;Connect Timeout=30");
            string sql;
            sql = "UPDATE product  SET quantity  = quantity   - '" + order.quantity + "'  where (Id ='" + order.productId + "' )";
            SqlCommand comm = new SqlCommand(sql, conn);
            conn.Open();
            comm.ExecuteNonQuery();
            _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(myorders));
            //}
            //return View(orders);
        }

        public async Task<IActionResult> myorders()
        {
            int id = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            var orItems = await _context.orders.FromSqlRaw("select *  from orders where  userid = '" + id + "'  ").ToListAsync();
            return View(orItems);
        }

        public async Task<IActionResult> customerOrders(int? id)
        {


            var orItems = await _context.orders.FromSqlRaw("select *  from orders where  userid = '" + id + "'  ").ToListAsync();
            return View(orItems);

        }

        public async Task<IActionResult> customerreport()
        {
            var orItems = await _context.report.FromSqlRaw("select usersaccounts.id as Id, usersaccounts.name as customername,sum (orders.quantity * product.price)  as total from usersaccounts , orders ,product where usersaccounts.id = orders.userid  and productId= product.Id group by usersaccounts.name,usersaccounts.id,orders.quantity, product.price").ToListAsync();
            return View(orItems);

        }


        // GET: orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.orders == null)
            {
                return NotFound();
            }

            var orders = await _context.orders.FindAsync(id);
            if (orders == null)
            {
                return NotFound();
            }
            return View(orders);
        }

        // POST: orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,productId,userid,quantity,orderdate")] orders orders)
        {
            if (id != orders.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orders);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ordersExists(orders.Id))
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
            return View(orders);
        }

        // GET: orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.orders == null)
            {
                return NotFound();
            }

            var orders = await _context.orders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orders == null)
            {
                return NotFound();
            }

            return View(orders);
        }

        // POST: orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.orders == null)
            {
                return Problem("Entity set 'onlinepharmacyContext.orders'  is null.");
            }
            var orders = await _context.orders.FindAsync(id);
            if (orders != null)
            {
                _context.orders.Remove(orders);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(myorders));
        }

        private bool ordersExists(int id)
        {
          return (_context.orders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
