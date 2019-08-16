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
    public class TrnCustomizacionController : Controller
    {
        private readonly TurnosContext _context;
        private IConfiguration configuration;

        public TrnCustomizacionController(TurnosContext context, IConfiguration configuration)
        {
            _context = context;
            this.configuration = configuration;
        }

        // GET: TrnCustomizacions
        public async Task<IActionResult> Index(string empid)
        {
            if (empid == null || empid == "")
                empid = configuration.GetSection("empid").Value;
            configuration.GetSection("empid").Value = empid;

            return View(await _context.customizacion.Where(a => a.Empid == empid).ToListAsync());
        }

        // GET: TrnCustomizacions/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trnCustomizacion = await _context.customizacion
                .FirstOrDefaultAsync(m => m.Empid == id);
            if (trnCustomizacion == null)
            {
                return NotFound();
            }

            return View(trnCustomizacion);
        }

        // GET: TrnCustomizacions/Create
        public IActionResult Create()
        {
            /////////////////////////////////////////SEGUIR ACA
            //Traigo plantas que no tengan asignadas customizacion
            _context.customizacion.Where(a => a.Empid == configuration.GetSection("empid").Value).FirstOrDefault();
            ViewData["IdPlanta"] = new SelectList(_context.TrnUsuarioPlanta.Where(a => a.User_Id == configuration.GetSection("empid").Value).ToList(), "Codigo", "Descripcion");
            return View();
        }

        // POST: TrnCustomizacions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Empid,HorarioMinimo,HorarioMaximo,DiasLaborables")] TrnCustomizacion trnCustomizacion)
        {
            if (ModelState.IsValid)
            {
                trnCustomizacion.Empid = configuration.GetSection("empid").Value;
                _context.Add(trnCustomizacion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdPlanta"] = new SelectList(_context.TrnUsuarioPlanta.Where(a => a.User_Id == configuration.GetSection("empid").Value).ToList(), "Codigo", "Descripcion");
            return View(trnCustomizacion);
        }

        // GET: TrnCustomizacions/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trnCustomizacion = await _context.customizacion.FindAsync(id);
            if (trnCustomizacion == null)
            {
                return NotFound();
            }
            ViewData["IdPlanta"] = new SelectList(_context.TrnUsuarioPlanta.Where(a => a.User_Id == configuration.GetSection("empid").Value).ToList(), "Codigo", "Descripcion");
            return View(trnCustomizacion);
        }

        // POST: TrnCustomizacions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Empid,IdPlanta,HorarioMinimo,HorarioMaximo,DiasLaborables")] TrnCustomizacion trnCustomizacion)
        {
            if (id != trnCustomizacion.Empid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trnCustomizacion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrnCustomizacionExists(trnCustomizacion.Empid))
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
            ViewData["IdPlanta"] = new SelectList(_context.TrnUsuarioPlanta.Where(a => a.User_Id == configuration.GetSection("empid").Value).ToList(), "Codigo", "Descripcion");

            return View(trnCustomizacion);
        }

        // GET: TrnCustomizacions/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trnCustomizacion = await _context.customizacion
                .FirstOrDefaultAsync(m => m.Empid == id);
            if (trnCustomizacion == null)
            {
                return NotFound();
            }

            return View(trnCustomizacion);
        }

        // POST: TrnCustomizacions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var trnCustomizacion = await _context.customizacion.FindAsync(id);
            _context.customizacion.Remove(trnCustomizacion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrnCustomizacionExists(string id)
        {
            return _context.customizacion.Any(e => e.Empid == id);
        }              

        [HttpPost]
        public JsonResult TraerDiasSeleccionados(string idPlanta)
        {
            bool lunesActivo = false, martesActivo = false, miercolesActivo = false, juevesActivo = false, viernesActivo = false, sabadoActivo = false, domingoActivo = false;

            TrnCustomizacion trnCustomizacion = _context.customizacion.Where(a => a.Empid == configuration.GetSection("empid").Value && a.IdPlanta == idPlanta).FirstOrDefault();
            if (trnCustomizacion != null)
            {
                String[] activos = trnCustomizacion.DiasLaborables.Split(",");
                foreach (var item in activos)
                {
                    switch (item)
                    {
                        case "1":
                            lunesActivo = true;
                            break;
                        case "2":
                            martesActivo = true;
                            break;
                        case "3":
                            miercolesActivo = true;
                            break;
                        case "4":
                            juevesActivo = true;
                            break;
                        case "5":
                            viernesActivo = true;
                            break;
                        case "6":
                            sabadoActivo = true;
                            break;
                        case "7":
                            domingoActivo = true;
                            break;
                        default:
                            break;
                    }
                }
            }

            var jsonResult = new { lunesActivo, martesActivo, miercolesActivo, juevesActivo, viernesActivo, sabadoActivo, domingoActivo };

            return Json(jsonResult);
        }
    }
}
