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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

namespace Turnos.Controllers
{
    /// <summary>
    /// Clase encargada del handling de la lógica de negocio del Calendario de Turnos
    /// </summary>
    public class FeriadosController : Controller
    {
        private readonly TurnosContext _context;
        private IConfiguration configuration;

        public FeriadosController(TurnosContext context, IConfiguration configuration)
        {
            _context = context;
            this.configuration = configuration;
        }

        public IActionResult Index(string empid)
        {            
            if (empid == null || empid == "")
                empid = configuration.GetSection("empid").Value;
            configuration.GetSection("empid").Value = empid;

            ViewData["IdCalendarioFeriado"] = new SelectList(_context.FeriadoCabecera.Where(a => a.Empid == empid).ToList(), "IdCalendarioFeriado", "Descripcion");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public JsonResult ObtenerFeriados(int idCalendarioFeriado)
        {
            var feriados = _context.Feriado.Where(a => a.Empid == configuration.GetSection("empid").Value && a.IdCalendarioFeriado == idCalendarioFeriado).ToList();

            return Json(feriados);
        }

        [HttpPost]
        public JsonResult GrabarCalendario(TrnFeriadoCabecera e)
        {
            var status = false;
            string empid = configuration.GetSection("empid").Value;

            if (e.IdCalendarioFeriado > 0)
            {
                var v = _context.FeriadoCabecera.Where(a => a.IdCalendarioFeriado == e.IdCalendarioFeriado).FirstOrDefault();
                if (v != null)
                {
                    v.Empid = empid;
                    v.Descripcion = e.Descripcion;
                }
            }
            else
            {
                e.Empid = empid;
                _context.FeriadoCabecera.Add(e);
            }

            _context.SaveChanges();
            status = true;

            var jsonResult = new { status = status, id= e.IdCalendarioFeriado, texto = e.Descripcion };
            return Json(jsonResult);
        }

        [HttpPost]
        public JsonResult BorrarCalendario(int idCalendarioFeriado)
        {
            var status = false;
            var errorFK = false;

            try
            {
                foreach (var item in _context.Feriado.Where(a => a.IdCalendarioFeriado == idCalendarioFeriado).ToList())
                {
                    _context.Feriado.Remove(item);                    
                    status = true;
                }
                _context.FeriadoCabecera.Remove(_context.FeriadoCabecera.Where(a => a.IdCalendarioFeriado == idCalendarioFeriado).FirstOrDefault());
                _context.SaveChanges();
                status = true;
            }
            catch (DbUpdateException ex)
            {
                var sqlException = ex.GetBaseException() as SqlException;
                if (sqlException != null)
                {
                    if (sqlException.Errors.Count > 0)
                    {
                        switch (sqlException.Errors[0].Number)
                        {
                            case 547: // Foreign Key violation
                                errorFK = true;
                                break;
                            default:
                                throw;
                        }
                    }
                }
                else
                {
                    throw;
                }
                status = false;
            }

            var jsonResult = new { status = status, errorFK = errorFK };
            return Json(jsonResult);
        }       

        [HttpPost]
        public JsonResult GrabarFeriado(TrnFeriado e)
        {
            var status = false;
            string empid = configuration.GetSection("empid").Value;

            if (e.EventID > 0)
            {
                var v = _context.Feriado.Where(a => a.EventID == e.EventID).FirstOrDefault();
                if (v != null)
                {
                    v.IdCalendarioFeriado = e.IdCalendarioFeriado;
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
                _context.Feriado.Add(e);
            }

            _context.SaveChanges();
            status = true;

            var jsonResult = new { status = status };
            return Json(jsonResult);
        }

        [HttpPost]
        public JsonResult BorrarFeriado(int eventID)
        {
            var status = false;

            ePlaceDBContext context = new ePlaceDBContext();
            //context.TrnBoca()
            var v = _context.Feriado.Where(a => a.EventID == eventID).FirstOrDefault();
            if (v != null)
            {
                _context.Feriado.Remove(v);
                _context.SaveChanges();
                status = true;
            }

            var jsonResult = new { status = status };
            return Json(jsonResult);
        }

        public string Getdato(int eventID)
        {
            var v = _context.Feriado.Where(a => a.Empid == configuration.GetSection("empid").Value && a.EventID == eventID).FirstOrDefault();
            if (v != null)
            {
                return v.Empid;
            }
            else
            {
                return "";
            }

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

        [HttpPost]
        public string TraerDiasLaborables()
        {
            var v = _context.customizacion.Where(a => a.Empid == configuration.GetSection("empid").Value).FirstOrDefault();
            if (v != null)
                return v.DiasLaborables;
            else
                return "";
        }
    }
}
