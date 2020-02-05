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
            var trnBoca = _context.TrnBoca.Include(t => t.IdTipoBocaNavigation)
                .Include(t => t.TrnCalendarioplantaCabeceraNavigation)
                .Include(t => t.TrnCalendarioFeriadoCabeceraNavigation)
                .Where(a => a.Empid == empid);
            if (empid == null || empid == "")
                empid = configuration.GetSection("empid").Value;
                
            configuration.GetSection("empid").Value = empid;

            return View(await trnBoca.ToListAsync());
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
                .Include(t => t.TrnCalendarioplantaCabeceraNavigation)
                .Include(t => t.TrnCalendarioFeriadoCabeceraNavigation)
                .Include(t => t.TrnUsuariosBocaNavigation)
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
            ViewData["user_name"] = new SelectList(_context.TrnUsuariosBoca.Where(a => a.user_id == configuration.GetSection("empid").Value && a.perfil != 1).ToList(), "user_name", "datosCompletos");
            return View();
        }

        // POST: Bocas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdBoca,IdPlanta,Empid,BocaEntrega,Descripcion,Estado,SegmentoCantMin,SegmentoCantPalletMax,IdCalendarioPlanta,IdCalendarioFeriado,VerificaSobreposicionHoraria,CantidadCitasSimultaneas,IdTipoBoca,DiasPrevision,user_name,color")] TrnBoca trnBoca)
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
            ViewData["user_name"] = new SelectList(_context.TrnUsuariosBoca.Where(a => a.user_id == configuration.GetSection("empid").Value && a.perfil != 1).ToList(), "user_name", "datosCompletos");
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
            if (_context.Turno.Any(a => a.IdBoca == id))
                ViewData["deshabilitar"] = true;
            ViewData["IdPlanta"] = new SelectList(_context.TrnUsuarioPlanta.Where(a => a.User_Id == configuration.GetSection("empid").Value).ToList(), "Codigo", "Descripcion");
            ViewData["IdTipoBoca"] = new SelectList(_context.TrnBocaTipo.Where(a => a.Empid == configuration.GetSection("empid").Value).ToList(), "IdTipoBoca", "Nombre", trnBoca.IdTipoBoca);
            ViewData["IdCalendarioFeriado"] = new SelectList(_context.FeriadoCabecera.Where(a => a.Empid == configuration.GetSection("empid").Value).ToList(), "IdCalendarioFeriado", "Descripcion", trnBoca.IdCalendarioFeriado);
            ViewData["IdCalendarioPlanta"] = new SelectList(_context.CalendarioPlantaCabecera.Where(a => a.Empid == configuration.GetSection("empid").Value).ToList(), "IdCalendarioPlanta", "Descripcion", trnBoca.IdCalendarioPlanta);
            ViewData["user_name"] = new SelectList(_context.TrnUsuariosBoca.Where(a => a.user_id == configuration.GetSection("empid").Value && a.perfil != 1).ToList(), "user_name", "datosCompletos");
            return View(trnBoca);
        }

        // POST: Bocas/Edit/5
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
            ViewData["user_name"] = new SelectList(_context.TrnUsuariosBoca.Where(a => a.user_id == configuration.GetSection("empid").Value && a.perfil != 1).ToList(), "user_name", "datosCompletos");
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
                .Include(t => t.TrnCalendarioplantaCabeceraNavigation)
                .Include(t => t.TrnCalendarioFeriadoCabeceraNavigation)
                .Include(t => t.TrnUsuariosBocaNavigation)
                .FirstOrDefaultAsync(m => m.IdBoca == id);
            if (trnBoca == null)
            {
                return NotFound();
            }

            return View(trnBoca);
        }

        [HttpPost]
        public JsonResult Delete(int idBoca)
        {
            var status = false;

            if (!_context.Turno.Any(a => a.IdBoca == idBoca))
            {
                var v = _context.TrnBoca.Where(a => a.IdBoca == idBoca).FirstOrDefault();
                if (v != null)
                {
                    _context.TrnBoca.Remove(v);
                    _context.SaveChanges();
                    status = true;
                }
            }

            var jsonResult = new { status };
            return Json(jsonResult);
        }

        [HttpPost]
        public JsonResult DeshabilitarBoca(TrnBoca boca, DateTime hasta)
        {
            var status = false;
            var turnos = _context.Turno.Where(a => a.IdBoca == boca.IdBoca && a.Start <= hasta).ToList();
            if (turnos != null)
            {
                foreach (var item in turnos)
                {
                    var proveedor = _context.TrnUsuarioMaestros.Where(a => a.usr_Id == item.Provid).FirstOrDefault();

                    MailsEnvios mailsEnvios = new MailsEnvios();
                    mailsEnvios.idFrom = item.Empid;
                    mailsEnvios.idUsFrom = "ADMIN";
                    mailsEnvios.idTo = item.Provid;
                    mailsEnvios.idUsTo = "ADMIN";
                    mailsEnvios.idTipo = 1500;
                    mailsEnvios.fhAlta = DateTime.Now;
                    mailsEnvios.param1 = proveedor.nombre;
                    mailsEnvios.param2 = item.Start.ToString();
                    mailsEnvios.param3 = "La boca donde se encontraba asignado el turno fue deshabilitada temporalmente";
                    mailsEnvios.estado = "N";
                    mailsEnvios.fhProc = DateTime.Now;
                    mailsEnvios.fhModif = DateTime.Now;
                    mailsEnvios.CuerpoLibre = "";
                    mailsEnvios.AsuntoLibre = "";

                    _context.MailsEnvios.Add(mailsEnvios);

                    _context.Turno.Remove(item);
                }
                boca.Empid = configuration.GetSection("empid").Value;
                _context.TrnBoca.Update(boca);

                _context.SaveChanges();
                status = true;
            }

            var jsonResult = new { status };
            return Json(jsonResult);
        }

        [HttpPost]
        public JsonResult Modificar(TrnBoca boca)
        {
            var status = false;

            boca.Empid = configuration.GetSection("empid").Value;
            _context.TrnBoca.Update(boca);
            _context.SaveChanges();
            status = true;

            var jsonResult = new { status };
            return Json(jsonResult);
        }


        [HttpPost]
        public JsonResult DeshabilitarTipo(int idBoca)
        {
            var status = false;

            if (!_context.Turno.Any(a => a.IdBoca == idBoca))
            {
                status = true;
            }

            var jsonResult = new { status };
            return Json(jsonResult);
        }

        private bool TrnBocaExists(int id)
        {
            return _context.TrnBoca.Any(e => e.IdBoca == id);
        }

    }
}
