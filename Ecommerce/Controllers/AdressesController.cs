using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Models;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdressesController : ControllerBase
    {
        private readonly EcommerceDBContext _context;

        public AdressesController(EcommerceDBContext context)
        {
            _context = context;
        }

        [HttpGet("GetAdress")]
        public List<Adress> GetAdress()
        {
            if (_context.Adresses.ToList() == null)
            {
                throw new System.Exception("No Adress Available");
            }
            List<Adress> adress = _context.Adresses.ToList();
            if (adress.Count == 0)
            {
                throw new Exception("No Adress Available");
            }
            return adress;
        }

        [HttpPost]
        public IActionResult Post([FromBody] Adress adress)
        {
            try
            {
                _context.Adresses.Add(adress);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest();
            }
            return Created("Adress Added", adress);

        }

        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
               Adress adress = _context.Adresses.Find(id);
                adress.Isactive = false;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Deletion error. Please try again later.");

            }
            return Ok();
        }

        [HttpPut("UpdateAdress/{id}")]
        public async Task<IActionResult> UpdateAdress(int id, int customerId, string street, string city, string state, string zipCode, string country)
        {
            try
            {
                var ent = await _context.Adresses.FindAsync(id);
                var existingAdress = _context.Adresses.FirstOrDefault(p => p.AddressID == id);

                if (existingAdress == null)
                {
                    return NotFound();
                }
                existingAdress.CustomerID = customerId;
                existingAdress.Street = street;
                existingAdress.City = city;
                existingAdress.State = state;
                existingAdress.ZipCode = zipCode;
                existingAdress.Country = country;


                _context.Entry(existingAdress).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Ok();

        }


    }
}
