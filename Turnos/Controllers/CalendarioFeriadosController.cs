using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Turnos.Models;

namespace Turnos.Controllers
{
    public class CalendarioFeriadosController : Controller
    {
        private readonly ePlaceDBContext _context;

        public CalendarioFeriadosController(ePlaceDBContext context)
        {
            _context = context;
        }

        // GET: CalendarioFeriados
        public async Task<IActionResult> Index()
        {
            return View(await _context.TrnCalendarioFeriado.ToListAsync());
        }

        // GET: CalendarioFeriados/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var trnCalendarioFeriado = _context.TrnCalendarioFeriado.Include(t => t.detalle).Where(i => i.IdCalendarioFeriado == id).ToList();
                        
            if (trnCalendarioFeriado == null)
            {
                return NotFound();
            }

            return View(trnCalendarioFeriado[0]);
        }

        // GET: CalendarioFeriados/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CalendarioFeriados/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCalendarioFeriado,Empid,Descripcion")] TrnCalendarioFeriado trnCalendarioFeriado)
        {
            if (ModelState.IsValid)
            {
                _context.Add(trnCalendarioFeriado);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(trnCalendarioFeriado);
        }

        // GET: CalendarioFeriados/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trnCalendarioFeriado = await _context.TrnCalendarioFeriado.FindAsync(id);
            if (trnCalendarioFeriado == null)
            {
                return NotFound();
            }
            return View(trnCalendarioFeriado);
        }

        // POST: CalendarioFeriados/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCalendarioFeriado,Empid,Descripcion")] TrnCalendarioFeriado trnCalendarioFeriado)
        {
            if (id != trnCalendarioFeriado.IdCalendarioFeriado)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trnCalendarioFeriado);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrnCalendarioFeriadoExists(trnCalendarioFeriado.IdCalendarioFeriado))
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
            return View(trnCalendarioFeriado);
        }

        // GET: CalendarioFeriados/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trnCalendarioFeriado = await _context.TrnCalendarioFeriado.FirstOrDefaultAsync(m => m.IdCalendarioFeriado == id);
            if (trnCalendarioFeriado == null)
            {
                return NotFound();
            }

            return View(trnCalendarioFeriado);
        }

        // POST: CalendarioFeriados/Delete/5
        // PRIMERO BORRA EL DETALLE Y LUEGO BORRA EL CALENDARIO (TABLAS RELACIONADAS)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using (var context = new ePlaceDBContext())
            {
                using (IDbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        //ver que funcione y borre todas las lineas del detalle
                        var rnCalendarioFeriadoDetalle = _context.TrnCalendarioFeriadoDetalle.Where(c => c.IdCalendarioFeriado == id);
                        _context.TrnCalendarioFeriadoDetalle.RemoveRange(rnCalendarioFeriadoDetalle);

                        var trnCalendarioFeriado = await _context.TrnCalendarioFeriado.FindAsync(id);
                        _context.TrnCalendarioFeriado.Remove(trnCalendarioFeriado);
                        await _context.SaveChangesAsync();

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
        }

        private bool TrnCalendarioFeriadoExists(int id)
        {
            return _context.TrnCalendarioFeriado.Any(e => e.IdCalendarioFeriado == id);
        }
    }
}
