using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Turnos.Models;


namespace Turnos.Controllers
{
    public class TrnCalendarioFeriadoDetallesController : Controller
    {
        private readonly ePlaceDBContext _context;

        public TrnCalendarioFeriadoDetallesController(ePlaceDBContext context)
        {
            _context = context;
        }

        // GET: TrnCalendarioFeriadoDetalles
        public async Task<IActionResult> Index()
        {
            var ePlaceDBContext = _context.TrnCalendarioFeriadoDetalle.Include(t => t.trnCalendarioFeriado);
            return View(await ePlaceDBContext.ToListAsync());
        }

        // GET: TrnCalendarioFeriadoDetalles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trnCalendarioFeriadoDetalle = await _context.TrnCalendarioFeriadoDetalle
                .Include(t => t.trnCalendarioFeriado)
                .FirstOrDefaultAsync(m => m.IdDetalle == id);
            if (trnCalendarioFeriadoDetalle == null)
            {
                return NotFound();
            }

            return View(trnCalendarioFeriadoDetalle);
        }

        // GET: TrnCalendarioFeriadoDetalles/Create
        public IActionResult Create()
        {
            ViewData["IdCalendarioFeriado"] = new SelectList(_context.TrnCalendarioFeriado, "IdCalendarioFeriado", "Empid");
            return View();
        }

        // POST: TrnCalendarioFeriadoDetalles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDetalle,IdCalendarioFeriado,Empid,Fecha,Descripcion,DiaCompleto,HoraDesde,HoraHasta")] TrnCalendarioFeriadoDetalle trnCalendarioFeriadoDetalle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(trnCalendarioFeriadoDetalle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCalendarioFeriado"] = new SelectList(_context.TrnCalendarioFeriado, "IdCalendarioFeriado", "Empid", trnCalendarioFeriadoDetalle.IdCalendarioFeriado);
            return View(trnCalendarioFeriadoDetalle);
        }

        // GET: TrnCalendarioFeriadoDetalles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trnCalendarioFeriadoDetalle = await _context.TrnCalendarioFeriadoDetalle.FindAsync(id);
            if (trnCalendarioFeriadoDetalle == null)
            {
                return NotFound();
            }
            ViewData["IdCalendarioFeriado"] = new SelectList(_context.TrnCalendarioFeriado, "IdCalendarioFeriado", "Empid", trnCalendarioFeriadoDetalle.IdCalendarioFeriado);
            return View(trnCalendarioFeriadoDetalle);
        }

        // POST: TrnCalendarioFeriadoDetalles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDetalle,IdCalendarioFeriado,Empid,Fecha,Descripcion,DiaCompleto,HoraDesde,HoraHasta")] TrnCalendarioFeriadoDetalle trnCalendarioFeriadoDetalle)
        {
            if (id != trnCalendarioFeriadoDetalle.IdDetalle)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trnCalendarioFeriadoDetalle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrnCalendarioFeriadoDetalleExists(trnCalendarioFeriadoDetalle.IdDetalle))
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
            ViewData["IdCalendarioFeriado"] = new SelectList(_context.TrnCalendarioFeriado, "IdCalendarioFeriado", "Empid", trnCalendarioFeriadoDetalle.IdCalendarioFeriado);
            return View(trnCalendarioFeriadoDetalle);
        }

        // GET: TrnCalendarioFeriadoDetalles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trnCalendarioFeriadoDetalle = await _context.TrnCalendarioFeriadoDetalle
                .Include(t => t.trnCalendarioFeriado)
                .FirstOrDefaultAsync(m => m.IdDetalle == id);
            if (trnCalendarioFeriadoDetalle == null)
            {
                return NotFound();
            }

            return View(trnCalendarioFeriadoDetalle);
        }

        // POST: TrnCalendarioFeriadoDetalles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trnCalendarioFeriadoDetalle = await _context.TrnCalendarioFeriadoDetalle.FindAsync(id);
            _context.TrnCalendarioFeriadoDetalle.Remove(trnCalendarioFeriadoDetalle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrnCalendarioFeriadoDetalleExists(int id)
        {
            return _context.TrnCalendarioFeriadoDetalle.Any(e => e.IdDetalle == id);
        }
    }
}
