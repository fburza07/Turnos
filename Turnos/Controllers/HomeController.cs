/*********************************************************************************************************************************************************************************************
* Módulo                : Turnos
* Versión               : v2.0.7
* Descripción           : Módulo de gestión y reserva de Turnos
* Autor                 : Fabricio Guardia
* Fecha de Creación     : 09/04/2019
* Base de Datos         : ePlaceDB
*********************************************************************************************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Turnos.Models;
using Microsoft.Extensions.Configuration;

namespace Turnos.Controllers
{
    /// <summary>
    /// Clase encargada del handling de la lógica de negocio del Calendario de Turnos
    /// </summary>
    public class HomeController : Controller
    {
        private readonly TurnosContext _context;
        private IConfiguration configuration;

        public HomeController(TurnosContext context, IConfiguration configuration)
        {
            _context = context;
            this.configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
        public JsonResult ObtenerTurnos()
        {
            var events = _context.Turno.ToList();

            return Json(events);
        }

        [HttpPost]
        public JsonResult GrabarTurno(Turno e)
        {
            var status = false;
            string empid = configuration.GetSection("empid").Value;

            if (e.EventID > 0)
            {
                var v = _context.Turno.Where(a => a.EventID == e.EventID).FirstOrDefault();
                if (v != null)
                {
                    v.Empid = empid;
                    v.Subject = e.Subject;
                    v.Start = e.Start;
                    v.End = e.End;
                    v.Description = e.Description;
                    v.IsFullDay = e.IsFullDay;
                    v.ThemeColor = e.ThemeColor;
                }
            }
            else
            {
                e.Empid = empid;
                _context.Turno.Add(e);
            }

            _context.SaveChanges();
            status = true;

            var jsonResult = new { status = status };
            return Json(jsonResult);
        }

        [HttpPost]
        public JsonResult BorrarTurno(int eventID)
        {
            var status = false;

            var v = _context.Turno.Where(a => a.EventID == eventID).FirstOrDefault();
            if (v != null)
            {
                _context.Turno.Remove(v);
                _context.SaveChanges();
                status = true;
            }

            var jsonResult = new { status = status };
            return Json(jsonResult);
        }
    }
}
