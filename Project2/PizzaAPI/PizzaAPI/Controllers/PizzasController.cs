﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzaAPI.Model;

namespace PizzaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PizzasController : ControllerBase
    {
        private readonly PizzaDbContext _context;

        public PizzasController(PizzaDbContext context)
        {
            _context = context;
        }

        // GET: api/Pizzas
        [HttpGet]
        public IEnumerable<Pizza> GetPizza()
        {
            return _context.Pizza;
        }

        // GET: api/Pizzas/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPizza([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pizza = await _context.Pizza.FindAsync(id);

            if (pizza == null)
            {
                return NotFound();
            }

            return Ok(pizza);
        }

        // PUT: api/Pizzas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPizza([FromRoute] int id, [FromBody] Pizza pizza)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pizza.PizzaId)
            {
                return BadRequest();
            }

            _context.Entry(pizza).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PizzaExists(id))
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

        // POST: api/Pizzas
        [HttpPost]
        public async Task<IActionResult> PostPizza([FromBody] Pizza pizza)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Pizza.Add(pizza);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPizza", new { id = pizza.PizzaId }, pizza);
        }

        // DELETE: api/Pizzas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePizza([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pizza = await _context.Pizza.FindAsync(id);
            if (pizza == null)
            {
                return NotFound();
            }

            _context.Pizza.Remove(pizza);
            await _context.SaveChangesAsync();

            return Ok(pizza);
        }

        private bool PizzaExists(int id)
        {
            return _context.Pizza.Any(e => e.PizzaId == id);
        }
    }
}