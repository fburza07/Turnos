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
    public class TrnBocasController : Controller
    {
        private readonly ePlaceDBContext _context;

        public TrnBocasController(ePlaceDBContext context)
        {
            _context = context;
        }

        // GET: TrnBocas
        public async Task<IActionResult> Index()
        {
            var ePlaceDBContext = _context.TrnBoca.Include(t => t.IdTipoBocaNavigation).Include(t => t.TrnCalendarioFeriadoCabeceraNavigation).Include(t => t.TrnCalendarioplantaCabeceraNavigation).Include(t => t.TrnUsuariosBocaNavigation);
            return View(await ePlaceDBContext.ToListAsync());
        }

        // GET: TrnBocas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trnBoca = await _context.TrnBoca
                .Include(t => t.IdTipoBocaNavigation)
                .Include(t => t.TrnCalendarioFeriadoCabeceraNavigation)
                .Include(t => t.TrnCalendarioplantaCabeceraNavigation)
                .Include(t => t.TrnUsuariosBocaNavigation)
                .FirstOrDefaultAsync(m => m.IdBoca == id);
            if (trnBoca == null)
            {
                return NotFound();
            }

            return View(trnBoca);
        }

        // GET: TrnBocas/Create
        public IActionResult Create()
        {
            ViewData["IdTipoBoca"] = new SelectList(_context.TrnBocaTipo, "IdTipoBoca", "Codigo");
            ViewData["IdCalendarioFeriado"] = new SelectList(_context.Set<TrnFeriadoCabecera>(), "IdCalendarioFeriado", "IdCalendarioFeriado");
            ViewData["IdCalendarioPlanta"] = new SelectList(_context.Set<TrnCalendarioPlantaCabecera>(), "IdCalendarioPlanta", "IdCalendarioPlanta");
            ViewData["user_name"] = new SelectList(_context.Set<TrnUsuariosBoca>(), "user_name", "user_name");
            return View();
        }

        // POST: TrnBocas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdBoca,IdPlanta,Empid,BocaEntrega,Descripcion,Estado,SegmentoCantMin,SegmentoCantPalletMax,IdCalendarioPlanta,IdCalendarioFeriado,VerificaSobreposicionHoraria,CantidadCitasSimultaneas,IdTipoBoca,DiasPrevision,user_name,color")] TrnBoca trnBoca)
        {
            if (ModelState.IsValid)
            {
                _context.Add(trnBoca);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdTipoBoca"] = new SelectList(_context.TrnBocaTipo, "IdTipoBoca", "Codigo", trnBoca.IdTipoBoca);
            ViewData["IdCalendarioFeriado"] = new SelectList(_context.Set<TrnFeriadoCabecera>(), "IdCalendarioFeriado", "IdCalendarioFeriado", trnBoca.IdCalendarioFeriado);
            ViewData["IdCalendarioPlanta"] = new SelectList(_context.Set<TrnCalendarioPlantaCabecera>(), "IdCalendarioPlanta", "IdCalendarioPlanta", trnBoca.IdCalendarioPlanta);
            ViewData["user_name"] = new SelectList(_context.Set<TrnUsuariosBoca>(), "user_name", "user_name", trnBoca.user_name);
            return View(trnBoca);
        }

        // GET: TrnBocas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trnBoca = await _context.TrnBoca.FindAsync(id);
            if (trnBoca == null)
            {
                return NotFound();
            }
            ViewData["IdTipoBoca"] = new SelectList(_context.TrnBocaTipo, "IdTipoBoca", "Codigo", trnBoca.IdTipoBoca);
            ViewData["IdCalendarioFeriado"] = new SelectList(_context.Set<TrnFeriadoCabecera>(), "IdCalendarioFeriado", "IdCalendarioFeriado", trnBoca.IdCalendarioFeriado);
            ViewData["IdCalendarioPlanta"] = new SelectList(_context.Set<TrnCalendarioPlantaCabecera>(), "IdCalendarioPlanta", "IdCalendarioPlanta", trnBoca.IdCalendarioPlanta);
            ViewData["user_name"] = new SelectList(_context.Set<TrnUsuariosBoca>(), "user_name", "user_name", trnBoca.user_name);
            return View(trnBoca);
        }

        // POST: TrnBocas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdBoca,IdPlanta,Empid,BocaEntrega,Descripcion,Estado,SegmentoCantMin,SegmentoCantPalletMax,IdCalendarioPlanta,IdCalendarioFeriado,VerificaSobreposicionHoraria,CantidadCitasSimultaneas,IdTipoBoca,DiasPrevision,user_name,color")] TrnBoca trnBoca)
        {
            if (id != trnBoca.IdBoca)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trnBoca);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrnBocaExists(trnBoca.IdBoca))
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
            ViewData["IdTipoBoca"] = new SelectList(_context.TrnBocaTipo, "IdTipoBoca", "Codigo", trnBoca.IdTipoBoca);
            ViewData["IdCalendarioFeriado"] = new SelectList(_context.Set<TrnFeriadoCabecera>(), "IdCalendarioFeriado", "IdCalendarioFeriado", trnBoca.IdCalendarioFeriado);
            ViewData["IdCalendarioPlanta"] = new SelectList(_context.Set<TrnCalendarioPlantaCabecera>(), "IdCalendarioPlanta", "IdCalendarioPlanta", trnBoca.IdCalendarioPlanta);
            ViewData["user_name"] = new SelectList(_context.Set<TrnUsuariosBoca>(), "user_name", "user_name", trnBoca.user_name);
            return View(trnBoca);
        }

        // GET: TrnBocas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trnBoca = await _context.TrnBoca
                .Include(t => t.IdTipoBocaNavigation)
                .Include(t => t.TrnCalendarioFeriadoCabeceraNavigation)
                .Include(t => t.TrnCalendarioplantaCabeceraNavigation)
                .Include(t => t.TrnUsuariosBocaNavigation)
                .FirstOrDefaultAsync(m => m.IdBoca == id);
            if (trnBoca == null)
            {
                return NotFound();
            }

            return View(trnBoca);
        }

        // POST: TrnBocas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trnBoca = await _context.TrnBoca.FindAsync(id);
            _context.TrnBoca.Remove(trnBoca);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrnBocaExists(int id)
        {
            return _context.TrnBoca.Any(e => e.IdBoca == id);
        }
    }
}
