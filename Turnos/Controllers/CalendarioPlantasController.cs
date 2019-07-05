using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Turnos.Models;

namespace Turnos.Controllers
{
    public class CalendarioPlantasController : Controller
    {
        private readonly ePlaceDBContext _context;
        private IConfiguration configuration;

        public CalendarioPlantasController(ePlaceDBContext context, IConfiguration configuration)
        {
            _context = context;
            this.configuration = configuration;
        }

        // GET: CalendarioPlantas
        public async Task<IActionResult> Index()
        {
            return View(await _context.TrnCalendarioPlanta.ToListAsync());
        }

        // GET: CalendarioPlantas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trnCalendarioPlanta = await _context.TrnCalendarioPlanta
                .FirstOrDefaultAsync(m => m.IdCalendarioPlanta == id);
            if (trnCalendarioPlanta == null)
            {
                return NotFound();
            }

            return View(trnCalendarioPlanta);
        }

        // GET: CalendarioPlantas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CalendarioPlantas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCalendarioPlanta,CalendarioPlanta,Empid,Descripcion,LunesActivo,LunesDesde,LunesHasta,MartesActivo,MartesDesde,MartesHasta,MiercolesActivo,MiercolesDesde,MiercolesHasta,JuevesActivo,JuevesDesde,JuevesHasta,ViernesActivo,ViernesDesde,ViernesHasta,SabadoActivo,SabadoDesde,SabadoHasta,DomingoActivo,DomingoDesde,DomingoHasta")] TrnCalendarioPlanta trnCalendarioPlanta)
        {
            if (ModelState.IsValid)
            {
                trnCalendarioPlanta.Empid = configuration.GetSection("empid").Value;
                _context.Add(trnCalendarioPlanta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(trnCalendarioPlanta);
        }

        // GET: CalendarioPlantas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trnCalendarioPlanta = await _context.TrnCalendarioPlanta.FindAsync(id);
            if (trnCalendarioPlanta == null)
            {
                return NotFound();
            }
            return View(trnCalendarioPlanta);
        }

        // POST: CalendarioPlantas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCalendarioPlanta,CalendarioPlanta,Empid,Descripcion,LunesActivo,LunesDesde,LunesHasta,MartesActivo,MartesDesde,MartesHasta,MiercolesActivo,MiercolesDesde,MiercolesHasta,JuevesActivo,JuevesDesde,JuevesHasta,ViernesActivo,ViernesDesde,ViernesHasta,SabadoActivo,SabadoDesde,SabadoHasta,DomingoActivo,DomingoDesde,DomingoHasta")] TrnCalendarioPlanta trnCalendarioPlanta)
        {
            if (id != trnCalendarioPlanta.IdCalendarioPlanta)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    trnCalendarioPlanta.Empid = configuration.GetSection("empid").Value;
                    _context.Update(trnCalendarioPlanta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrnCalendarioPlantaExists(trnCalendarioPlanta.IdCalendarioPlanta))
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
            return View(trnCalendarioPlanta);
        }

        // GET: CalendarioPlantas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trnCalendarioPlanta = await _context.TrnCalendarioPlanta
                .FirstOrDefaultAsync(m => m.IdCalendarioPlanta == id);
            if (trnCalendarioPlanta == null)
            {
                return NotFound();
            }

            return View(trnCalendarioPlanta);
        }

        // POST: CalendarioPlantas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trnCalendarioPlanta = await _context.TrnCalendarioPlanta.FindAsync(id);
            _context.TrnCalendarioPlanta.Remove(trnCalendarioPlanta);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrnCalendarioPlantaExists(int id)
        {
            return _context.TrnCalendarioPlanta.Any(e => e.IdCalendarioPlanta == id);
        }
    }
}
