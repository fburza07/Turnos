using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Turnos.Models;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Turnos.Controllers
{
    public class BocasController : Controller
    {
        private readonly TurnosContext _context;
        private IConfiguration configuration;

        public BocasController(TurnosContext context, IConfiguration configuration)
        {
            _context = context;
            this.configuration = configuration;
        }

        // GET: Bocas
        public async Task<IActionResult> Index(string empid)
        {            
            var ePlaceDBContext = _context.TrnBoca.Include(t => t.IdTipoBocaNavigation);
            if (empid == null || empid == "")
                empid = configuration.GetSection("empid").Value;
            configuration.GetSection("empid").Value = empid;
            
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
                .FirstOrDefaultAsync(m => m.IdBoca == id);
            if (trnBoca == null)
            {
                return NotFound();
            }

            return View(trnBoca);
        }

        // GET: Bocas/Create
        public IActionResult Create()
        {            
            ViewData["IdPlanta"] = new SelectList(_context.TrnUsuarioPlanta.Where(a => a.User_Id == configuration.GetSection("empid").Value).ToList(), "Codigo", "Descripcion");
            ViewData["IdTipoBoca"] = new SelectList(_context.TrnBocaTipo.Where(a => a.Empid == configuration.GetSection("empid").Value).ToList(), "IdTipoBoca", "Nombre");
            ViewData["IdCalendarioFeriado"] = new SelectList(_context.FeriadoCabecera.Where(a => a.Empid == configuration.GetSection("empid").Value).ToList(), "IdCalendarioFeriado", "Descripcion");
            ViewData["IdCalendarioPlanta"] = new SelectList(_context.CalendarioPlantaCabecera.Where(a => a.Empid == configuration.GetSection("empid").Value).ToList(), "IdCalendarioPlanta", "Descripcion");
            return View();
        }

        // POST: Bocas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdBoca,IdPlanta,Empid,BocaEntrega,Descripcion,Estado,SegmentoCantMin,SegmentoCantPalletMax,IdCalendarioPlanta,IdCalendarioFeriado,VerificaSobreposicionHoraria,CantidadCitasSimultaneas,IdTipoBoca,DiasPrevision,UsuarioResponsableBoca,color")] TrnBoca trnBoca)
        {
            if (ModelState.IsValid)
            {
                trnBoca.Empid = configuration.GetSection("empid").Value;                
                _context.Add(trnBoca);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }           
            ViewData["IdPlanta"] = new SelectList(_context.TrnUsuarioPlanta.Where(a => a.User_Id == trnBoca.Empid).ToList(), "Codigo", "Descripcion");
            ViewData["IdTipoBoca"] = new SelectList(_context.TrnBocaTipo.Where(a => a.Empid == trnBoca.Empid).ToList(), "IdTipoBoca", "Nombre", trnBoca.IdTipoBoca);
            ViewData["IdCalendarioFeriado"] = new SelectList(_context.FeriadoCabecera.Where(a => a.Empid == trnBoca.Empid).ToList(), "IdCalendarioFeriado", "Descripcion", trnBoca.IdCalendarioFeriado);
            ViewData["IdCalendarioPlanta"] = new SelectList(_context.CalendarioPlantaCabecera.Where(a => a.Empid == trnBoca.Empid).ToList(), "IdCalendarioPlanta", "Descripcion", trnBoca.IdCalendarioPlanta);
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
          
            ViewData["IdPlanta"] = new SelectList(_context.TrnUsuarioPlanta.Where(a => a.User_Id == configuration.GetSection("empid").Value).ToList(), "Codigo", "Descripcion");
            ViewData["IdTipoBoca"] = new SelectList(_context.TrnBocaTipo.Where(a => a.Empid == configuration.GetSection("empid").Value).ToList(), "IdTipoBoca", "Nombre", trnBoca.IdTipoBoca);
            ViewData["IdCalendarioFeriado"] = new SelectList(_context.FeriadoCabecera.Where(a => a.Empid == configuration.GetSection("empid").Value).ToList(), "IdCalendarioFeriado", "Descripcion", trnBoca.IdCalendarioFeriado);
            ViewData["IdCalendarioPlanta"] = new SelectList(_context.CalendarioPlantaCabecera.Where(a => a.Empid == configuration.GetSection("empid").Value).ToList(), "IdCalendarioPlanta", "Descripcion", trnBoca.IdCalendarioPlanta);
            return View(trnBoca);
        }

        // POST: Bocas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdBoca,IdPlanta,Empid,BocaEntrega,Descripcion,Estado,SegmentoCantMin,SegmentoCantPalletMax,IdCalendarioPlanta,IdCalendarioFeriado,VerificaSobreposicionHoraria,CantidadCitasSimultaneas,IdTipoBoca,DiasPrevision,UsuarioResponsableBoca,color")] TrnBoca trnBoca)
        {
            if (id != trnBoca.IdBoca)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    trnBoca.Empid = configuration.GetSection("empid").Value;
                    
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
            
            ViewData["IdPlanta"] = new SelectList(_context.TrnUsuarioPlanta.Where(a => a.User_Id == trnBoca.Empid).ToList(), "Codigo", "Descripcion");
            ViewData["IdTipoBoca"] = new SelectList(_context.TrnBocaTipo.Where(a => a.Empid == trnBoca.Empid).ToList(), "IdTipoBoca", "Nombre", trnBoca.IdTipoBoca);
            ViewData["IdCalendarioFeriado"] = new SelectList(_context.FeriadoCabecera.Where(a => a.Empid == trnBoca.Empid).ToList(), "IdCalendarioFeriado", "Descripcion", trnBoca.IdCalendarioFeriado);
            ViewData["IdCalendarioPlanta"] = new SelectList(_context.CalendarioPlantaCabecera.Where(a => a.Empid == trnBoca.Empid).ToList(), "IdCalendarioPlanta", "Descripcion", trnBoca.IdCalendarioPlanta);
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
                .FirstOrDefaultAsync(m => m.IdBoca == id);
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
            return _context.TrnBoca.Any(e => e.IdBoca == id);
        }
        
    }
}
