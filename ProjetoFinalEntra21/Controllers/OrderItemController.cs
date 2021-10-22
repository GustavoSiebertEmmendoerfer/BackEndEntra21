using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoFinalEntra21.Data;
using ProjetoFinalEntra21.Enum;
using ProjetoFinalEntra21.Models;
using ProjetoFinalEntra21.Models.BindingModels;

namespace ProjetoFinalEntra21.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _usermanager;

        public OrderItemController(ApplicationDbContext context, UserManager<User> usermanager)
        {
            _usermanager = usermanager;
            _context = context;
        }

        // GET: api/Orders
        [HttpGet("Usuario/{email}")]
        public async Task<object> GetOrderItem(string email)
        {
            try
            {
                var user = await _usermanager.FindByEmailAsync(email);
                if (user == null)
                {

                    var result = _context.OrderItem.Include(X => X.Order).Include(X => X.Plate).Where(x=>x.Order.Client.Email == email).ToList();
                    return await Task.FromResult(new ResponseModel(ResponseCode.Error, "deu bom", result));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "erro", null));
            }
            catch(Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
            
        }

        // GET: api/Orders/5
        [HttpGet("GetAll")]
        public async Task<object> GetOrder()
        {
            var order = _context.Order.ToList();

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }
        [HttpPost]
        public async Task<object> PostPlate([FromBody] OrderItemBindingModel model)
        {
            try
            {              
                var order = await _context.Order.FindAsync(model.OrderID);
                var plate = await _context.Plate.FindAsync(model.PlateID);
                if (order != null)
                {

                    OrderItem orderItem = new OrderItem()
                    {
                        Quantity = model.Quantity,
                        Order = order,
                        Plate = plate
                    };
                    _context.OrderItem.Add(orderItem);
                    await _context.SaveChangesAsync();
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", orderItem));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.OK, "Erro", null));

            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));

            }
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.OrderId)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Order.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.OrderId == id);
        }
    }
}
