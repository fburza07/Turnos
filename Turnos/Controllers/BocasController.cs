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
    public class BocasController : Controller
    {
        private readonly ePlaceDBContext _context;
        private IConfiguration configuration;

        public BocasController(ePlaceDBContext context, IConfiguration configuration)
        {
            _context = context;
            this.configuration = configuration;
        }

        // GET: Bocas
        public async Task<IActionResult> Index()
        {
            var ePlaceDBContext = _context.TrnBoca.Include(t => t.IdTipoBocaNavigation);
            return View(await ePlaceDBContext.ToListAsync());
        }

        // GET: Bocas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trnBoca = await _context.TrnBoca
                .Include(t => t.IdTipoBocaNavigation)
                .FirstOrDefaultAsync(m => m.IdPlanta == id);
            if (trnBoca == null)
            {
                return NotFound();
            }

            return View(trnBoca);
        }

        // GET: Bocas/Create
        public IActionResult Create()
        {
            ViewData["IdTipoBoca"] = new SelectList(_context.TrnBocaTipo, "IdTipoBoca", "Codigo");
            return View();
        }

        // POST: Bocas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPlanta,Empid,BocaEntrega,Descripcion,Estado,SegmentoCantMin,SegmentoCantPalletMax,IdCalendarioPlanta,IdCalendarioFeriado,VerificaSobreposicionHoraria,CantidadCitasSimultaneas,IdTipoBoca,UsuarioResponsableBoca")] TrnBoca trnBoca)
        {
            if (ModelState.IsValid)
            {
                trnBoca.Empid = configuration.GetSection("empid").Value;
                _context.Add(trnBoca);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdTipoBoca"] = new SelectList(_context.TrnBocaTipo, "IdTipoBoca", "Codigo", trnBoca.IdTipoBoca);
            return View(trnBoca);
        }

        // GET: Bocas/Edit/5
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
            return View(trnBoca);
        }

        // POST: Bocas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPlanta,Empid,BocaEntrega,Descripcion,Estado,SegmentoCantMin,SegmentoCantPalletMax,IdCalendarioPlanta,IdCalendarioFeriado,VerificaSobreposicionHoraria,CantidadCitasSimultaneas,IdTipoBoca,UsuarioResponsableBoca")] TrnBoca trnBoca)
        {
            if (id != trnBoca.IdPlanta)
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
                    if (!TrnBocaExists(trnBoca.IdPlanta))
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
            return View(trnBoca);
        }

        // GET: Bocas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trnBoca = await _context.TrnBoca
                .Include(t => t.IdTipoBocaNavigation)
                .FirstOrDefaultAsync(m => m.IdPlanta == id);
            if (trnBoca == null)
            {
                return NotFound();
            }

            return View(trnBoca);
        }

        // POST: Bocas/Delete/5
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
            return _context.TrnBoca.Any(e => e.IdPlanta == id);
        }

        public JsonResult CalendarioFeriadosSelectList()
        {
            List<string> lista = new List<string>();
            foreach (var item in _context.TrnCalendarioFeriado.ToList())
            {
                lista.Add(item.Descripcion);
            }
            return Json(lista);
        }

        public JsonResult CalendarioPlantaSelectList()
        {
            List<string> lista = new List<string>();
            foreach (var item in _context.TrnCalendarioPlanta.ToList())
            {
                lista.Add(item.Descripcion);
            }
            return Json(lista);
        }
    }
}
