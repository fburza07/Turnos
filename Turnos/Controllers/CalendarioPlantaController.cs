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
    public class CalendarioPlantaController : Controller
    {
        private readonly TurnosContext _context;
        private IConfiguration configuration;

        public CalendarioPlantaController(TurnosContext context, IConfiguration configuration)
        {
            _context = context;
            this.configuration = configuration;
        }

        public IActionResult Index(string empid)
        {
            if (empid == null || empid == "")
                empid = configuration.GetSection("empid").Value;
            configuration.GetSection("empid").Value = empid;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public JsonResult ObtenerCalendarioPlanta()
        {
            var calendarioPlanta = _context.CalendarioPlanta.ToList();

            return Json(calendarioPlanta);
        }

        [HttpPost]
        public JsonResult GrabarCalendario(TrnCalendarioPlantaCabecera e)
        {
            var status = false;
            string empid = configuration.GetSection("empid").Value;

            if (e.IdCalendarioPlanta > 0)
            {
                var v = _context.CalendarioPlantaCabecera.Where(a => a.IdCalendarioPlanta == e.IdCalendarioPlanta).FirstOrDefault();
                if (v != null)
                {
                    v.Empid = empid;
                    v.Descripcion = e.Descripcion;
                }
            }
            else
            {
                e.Empid = empid;
                _context.CalendarioPlantaCabecera.Add(e);
            }

            _context.SaveChanges();
            status = true;

            var jsonResult = new { status = status, id = e.IdCalendarioPlanta, texto = e.Descripcion };
            return Json(jsonResult);
        }

        [HttpPost]
        public JsonResult BorrarCalendario(int idCalendarioPlanta)
        {
            var status = false;

            try
            {
                foreach (var item in _context.CalendarioPlanta.Where(a => a.IdCalendarioPlanta == idCalendarioPlanta).ToList())
                {
                    _context.CalendarioPlanta.Remove(item);
                    status = true;
                }
                _context.CalendarioPlantaCabecera.Remove(_context.CalendarioPlantaCabecera.Where(a => a.IdCalendarioPlanta == idCalendarioPlanta).FirstOrDefault());
                _context.SaveChanges();
                status = true;
            }
            catch (Exception)
            {
                status = false;
                throw;
            }

            var jsonResult = new { status = status };
            return Json(jsonResult);
        }
       

        [HttpPost]
        public JsonResult GrabarCalendarioPlanta(TrnCalendarioPlanta e)
        {            
            var status = false;
            var codigovalido = false;
            string empid = configuration.GetSection("empid").Value;
            if (e.EventID > 0)
            {
                codigovalido = true;
            
                if (ValidarHorarioConfiguracionPlanta(e.Start, e.End))
                {
                    if (e.EventID > 0)
                    {
                        var v = _context.CalendarioPlanta.Where(a => a.EventID == e.EventID).FirstOrDefault();
                        if (v != null)
                        {
                            v.IdCalendarioPlanta = e.IdCalendarioPlanta;
                            v.Empid = empid;
                            v.Subject = e.Subject;
                            v.Start = e.Start;
                            v.End = e.IsFullDay == true ? Convert.ToDateTime(System.DateTime.Now.ToShortDateString() + " " + TraerHorarioMaximo()) : e.End;
                            v.Description = e.Description;
                            v.IsFullDay = e.IsFullDay;
                            v.ThemeColor = e.ThemeColor;
                            v.Dow = e.Dow;
                        }
                    }
                    else
                    {
                        e.Empid = empid;
                        e.End = e.IsFullDay == true ? Convert.ToDateTime(System.DateTime.Now.ToShortDateString() + " " + TraerHorarioMaximo()) : e.End;
                        _context.CalendarioPlanta.Add(e);
                    }

                    _context.SaveChanges();
                    status = true;
                }
                else
                {
                    status = false;
                }
            }
            var jsonResult = new { status = status, codigovalido= codigovalido };
            return Json(jsonResult);
        }

        private bool ValidarHorarioConfiguracionPlanta(DateTime start, DateTime end)
        {
            var rtn = false;
            if (start.TimeOfDay >= Convert.ToDateTime(TraerHorarioMinimo()).TimeOfDay && end.TimeOfDay <= Convert.ToDateTime(TraerHorarioMaximo()).TimeOfDay)
            {
                rtn = true;
            }
            return rtn;
        }

        [HttpPost]
        public JsonResult BorrarCalendarioPlanta(int eventID)
        {
            var status = false;

            var v = _context.CalendarioPlanta.Where(a => a.EventID == eventID).FirstOrDefault();
            if (v != null)
            {
                _context.CalendarioPlanta.Remove(v);
                _context.SaveChanges();
                status = true;
            }

            var jsonResult = new { status = status };
            return Json(jsonResult);
        }

        [HttpPost]
        public string TraerHorarioMinimo()
        {
            var v = _context.customizacion.Where(a => a.Empid == configuration.GetSection("empid").Value).FirstOrDefault();
            if (v != null)
                return v.HorarioMinimo.ToLongTimeString();
            else
                return "";
        }

        [HttpPost]
        public string TraerHorarioMaximo()
        {
            var v = _context.customizacion.Where(a => a.Empid == configuration.GetSection("empid").Value).FirstOrDefault();
            if (v != null)
                return v.HorarioMaximo.ToLongTimeString();
            else
                return "";
        }
    }
}
