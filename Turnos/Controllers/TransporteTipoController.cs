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
        private readonly TurnosContext _context;
        private IConfiguration configuration;

        public TransporteTipoController(TurnosContext context, IConfiguration configuration)
        {
            _context = context;
            this.configuration = configuration;
        }

        // GET: TransporteTipo
        public async Task<IActionResult> Index(string empid)
        {
            var transporteTipo = _context.TransporteTipo.Where(a => a.Empid == empid && a.IdTransporteTipo != 1); //1 = ninguno
            if (empid == null || empid == "")
                empid = configuration.GetSection("empid").Value;
            configuration.GetSection("empid").Value = empid;

            return View(await transporteTipo.ToListAsync());
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
        [HttpPost]
        public JsonResult Delete(int idTransporteTipo)
        {
            var status = false;

            if (!_context.Turno.Any(a => a.IdTransporteTipo == idTransporteTipo))
            {
                var v = _context.TransporteTipo.Where(a => a.IdTransporteTipo == idTransporteTipo).FirstOrDefault();
                if (v != null)
                {
                    _context.TransporteTipo.Remove(v);
                    _context.SaveChanges();
                    status = true;
                }
            }

            var jsonResult = new { status };
            return Json(jsonResult);
        }

        private bool TransporteTipoExists(int id)
        {
            return _context.TransporteTipo.Any(e => e.IdTransporteTipo == id);
        }
    }
}
