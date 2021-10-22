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
    public class PlatesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;


        public PlatesController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet("AllPlatesFromRestaurant/{email}")]
        public async Task<object> GetPlate(string email)
        {
            try
            {
                var result = await _context.Plate.Include(x => x.Restaurant).Where(x => x.Restaurant.Email == email).ToListAsync();
                if (result != null)
                {
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", result));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Erro", null));
            }
            catch(Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));

            }

        }

        [HttpGet("Specific/{id}")]
        public async Task<object> GetPlate(int id)
        {
            try
            {
                var plate = await _context.Plate.FindAsync(id);
                if (plate != null)
                {
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", plate));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Invalid Parameters", null));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));

            }

        }
        [HttpGet("PlatesByRestaurant/{email}")]
        public async Task<object> GetAllPlates(string email)
        {
            try
            {
                User restaurante = _userManager.FindByEmailAsync(email).Result;
                if (restaurante != null)
                {
                    var resultado = GetPlatesByRestaurant(restaurante);
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", resultado));

                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Invalid", null));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }

        }

        [HttpPost]
        public async Task<object> PostPlate([FromBody] PlateBindingModel model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.RestaurantEmail);
                var roles = (await _userManager.GetRolesAsync(user)).ToList();
                if(roles.Exists(x => x == "Restaurant"))
                { 
                     var restaurante = await _userManager.FindByEmailAsync(model.RestaurantEmail);
                    if (restaurante != null)
                    {
                        Plate plate = new Plate()
                        {
                            Name = model.Name,
                            Description = model.Description,
                            Price = model.Price,
                            Restaurant = restaurante,
                            PhotoURL = model.PhotoURL

                        };
                        _context.Plate.Add(plate);
                        await _context.SaveChangesAsync();
                        return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", plate));
                    }
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Restaurante não existe", null));

            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));

            }
        }
        [HttpPut("{id}")]
        public async Task<object> PutPlate(int id, Plate plate)
        {
            if (id != plate.PlateId)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Invalid Parameters", null));
            }

            _context.Entry(plate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlateExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", plate));
        }

        [HttpDelete("{id}")]
        public async Task<object> DeletePlate(int id)
        {
            try
            {
                var plate = await _context.Plate.FindAsync(id);
                if (plate == null)
                {
                    return NotFound();
                }

                _context.Plate.Remove(plate);
                await _context.SaveChangesAsync();

                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Plate Delete", null));
            }
            catch(Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }
        private List<PlatesDTO> GetPlatesByRestaurant( User user)
        {
            var ListPlates = _context.Plate.Include(x => x.Restaurant).Where(x => x.Restaurant == user).ToList();
            List<PlatesDTO> ListplatesDTO = new List<PlatesDTO>();
                foreach (var plate in ListPlates)
                {
                    ListplatesDTO.Add(new PlatesDTO(plate.PlateId, plate.Name, plate.Price, plate.Description, plate.PhotoURL));
                }
            return ListplatesDTO;
          
        }

        private bool PlateExists(int id)
        {
            return _context.Plate.Any(e => e.PlateId == id);
        }
    }
}
