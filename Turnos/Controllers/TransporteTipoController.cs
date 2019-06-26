using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Turnos.Models;
using Microsoft.Extensions.Configuration;

namespace Turnos.Controllers
{
    public class TransporteTipoController : Controller
    {
        private readonly ePlaceDBContext _context;
        private IConfiguration configuration;

        public TransporteTipoController(ePlaceDBContext context, IConfiguration configuration)
        {
            _context = context;
            this.configuration = configuration;
        }

        // GET: TransporteTipo
        public async Task<IActionResult> Index()
        {
            return View(await _context.TransporteTipo.ToListAsync());
        }

        // GET: TransporteTipo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transporteTipo = await _context.TransporteTipo
                .FirstOrDefaultAsync(m => m.IdTransporteTipo == id);
            if (transporteTipo == null)
            {
                return NotFound();
            }

            return View(transporteTipo);
        }

        // GET: TransporteTipo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TransporteTipo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTransporteTipo,Empid,Nombre,Descripcion,Activo")] TransporteTipo transporteTipo)
        {
            if (ModelState.IsValid)
            {
                transporteTipo.Empid = configuration.GetSection("empid").Value;

                _context.Add(transporteTipo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(transporteTipo);
        }

        // GET: TransporteTipo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transporteTipo = await _context.TransporteTipo.FindAsync(id);
            if (transporteTipo == null)
            {
                return NotFound();
            }
            return View(transporteTipo);
        }

        // POST: TransporteTipo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTransporteTipo,Empid,Nombre,Descripcion,Activo")] TransporteTipo transporteTipo)
        {
            if (id != transporteTipo.IdTransporteTipo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    transporteTipo.Empid = configuration.GetSection("empid").Value;
                    _context.Update(transporteTipo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransporteTipoExists(transporteTipo.IdTransporteTipo))
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
            return View(transporteTipo);
        }

        // GET: TransporteTipo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transporteTipo = await _context.TransporteTipo
                .FirstOrDefaultAsync(m => m.IdTransporteTipo == id);
            if (transporteTipo == null)
            {
                return NotFound();
            }

            return View(transporteTipo);
        }

        // POST: TransporteTipo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transporteTipo = await _context.TransporteTipo.FindAsync(id);
            _context.TransporteTipo.Remove(transporteTipo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransporteTipoExists(int id)
        {
            return _context.TransporteTipo.Any(e => e.IdTransporteTipo == id);
        }
    }
}
