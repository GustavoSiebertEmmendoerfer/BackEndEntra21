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
using ProjetoFinalEntra21.Models.DTO;

namespace ProjetoFinalEntra21.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _usermanager;


        public OrdersController(ApplicationDbContext context, UserManager<User> usermanager)
        {
            _usermanager = usermanager;
            _context = context;
        }

        [HttpGet]
        public async Task<object> GetOrder()
        {
            try
            {
                var result = await _context.Order.Include(x => x.Client).ToListAsync();
                if (result != null)
                {
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", result));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Invalid Parameters", null));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }

        }

        [HttpGet("userOrders/user/{id}")]
        public async Task<ActionResult<IEnumerable<OrderClient>>> GetLastOrder(string id)
        {
            var user = await _usermanager.FindByIdAsync(id);
            var roles = (await _usermanager.GetRolesAsync(user)).ToList();
            List<Order> order = new List<Order>();
            if (roles.Exists(x => x == "Client"))
            {
                var orderTest = _context.Order.Include(x => x.Client).Where(x => x.Client.Id == user.Id).ToList();
                order = orderTest;
            }
            else if (roles.Exists(x => x == "Restaurant"))
            {
                var orderTest = _context.Order.Where(x => x.RestaurantEmail == user.Email).ToList();
                order = orderTest;
            };
            List<OrderClient> trueOrders = new List<OrderClient>();
            if (order != null)
            {
                foreach (var item in order)
                {
                    if (item.Status != "AAA")
                    {
                        if (roles.Exists(x => x == "Client"))
                            {
                            var restaurant = await _usermanager.FindByEmailAsync(item.RestaurantEmail);
                            var AllPlates = _context.Plate.Where(x => x.Restaurant == restaurant).ToList();
                            var orderItemFormOrder = _context.OrderItem.Where(x => x.Order.OrderId == item.OrderId).ToList();
                            string plateNameClone = "";
                            double priceClone = 0;
                            foreach (var platetype in AllPlates)
                            {

                                var result = orderItemFormOrder.Where(x => x.Plate == platetype);
                                if (result != null)
                                {
                                    plateNameClone = platetype.Name;
                                    priceClone = platetype.Price;
                                }
                            }

                            trueOrders.Add(new OrderClient()
                            {
                                OrderId = item.OrderId,
                                Status = item.Status,
                                RestaurantName = restaurant.FullName,
                                plateName = plateNameClone,
                                price = priceClone
                            });
                        }
                        else if(roles.Exists(x => x == "Restaurant"))
                        {
                            var AllPlates = _context.Plate.Where(x => x.Restaurant == user).ToList();
                            var orderItemFormOrder = _context.OrderItem.Where(x => x.Order.OrderId == item.OrderId).ToList();

                            string plateNameClone = "";
                            double priceClone = 0;
                            foreach (var platetype in AllPlates)
                            {

                                var result = orderItemFormOrder.Where(x => x.Plate == platetype);
                                if (result != null)
                                {
                                    plateNameClone = platetype.Name;
                                    priceClone = platetype.Price;
                                }
                            }

                            trueOrders.Add(new OrderClient()
                            {
                                OrderId = item.OrderId,
                                Status = item.Status,
                                RestaurantName = user.FullName,
                                plateName = plateNameClone,
                                price = priceClone
                               
                            }); 
                        }
                    }
                }
                return trueOrders;
            }
            return BadRequest();
        }
        [HttpGet("userOrder/{id}")]
        public async Task<ActionResult<Order>> GetOrder(string id)
        {
            try
            {
                var order = await _context.Order.Include(x => x.Client).Where(x => x.Client.Id == id && x.Status == "Ongoing").ToListAsync();
                Order UltimaOrderTeste = new Order();
                foreach (var item in order)
                {
                    if (UltimaOrderTeste.OrderId < item.OrderId)
                    {
                        UltimaOrderTeste.OrderId = item.OrderId;
                    }
                }
                if (order == null)
                {
                    return NotFound();
                }
                return UltimaOrderTeste;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        [HttpPost]
        public async Task<object> PostOrder([FromBody] OrderBindingModel model)
        {
            try
            {
                var client = await _usermanager.FindByEmailAsync(model.ClientEmail);
                var restaurant = await _usermanager.FindByEmailAsync(model.RestaurantEmail);


                var rolesClient = (await _usermanager.GetRolesAsync(client)).ToList();
                var rolesRestaurant = (await _usermanager.GetRolesAsync(restaurant)).ToList();
                if (rolesRestaurant.Exists(x => x == "Restaurant") && rolesClient.Exists(x => x == "Client"))
                {
                    Order foodOrder = new Order()
                    {
                        Status = "Ongoing",
                        OrderTime = DateTime.UtcNow,
                        OrderEnd = DateTime.UtcNow,
                        Client = client,
                        RestaurantEmail = restaurant.Email
                    };
                    _context.Order.Add(foodOrder);
                    await _context.SaveChangesAsync();
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", foodOrder));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Erro", null));

            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));

            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            var currentOrder = _context.Order.Find(id);

            if (id != order.OrderId)
            {
                return BadRequest();
            }

            currentOrder.Status = order.Status;

            _context.Entry(currentOrder).State = EntityState.Modified;

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
