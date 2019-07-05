﻿/*********************************************************************************************************************************************************************************************
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
using System.Data.SqlClient;
using System.Data;

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

        public IActionResult Index(string empid)
        {
            configuration.GetSection("empid").Value = empid;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public JsonResult ObtenerTurnos()
        {
            List<Object> eventos = new List<Object>();
            var feriados = _context.Feriado.ToList();
            var turnos = _context.Turno.ToList();
            eventos.AddRange(feriados);
            eventos.AddRange(turnos);

            return Json(eventos);
        }

        [HttpPost]
        public JsonResult GrabarTurno(TrnTurno e)
        {
            var status = false;
            string empid = configuration.GetSection("empid").Value;
            //VER DE PASAR ESTO AL MODELO
            Conexion cn = new Conexion();
            SqlCommand sqlcommand = cn.GetCommand("TRN_VerificarFeriado");           
            sqlcommand.Parameters.AddWithValue("@FechaDesde", e.Start);
            DataTable dt = cn.Execute(sqlcommand);
            //SI NO ES FERIADO DEJO GRABAR
            if (dt.Rows[0]["esFeriado"].ToString() == "0")
            {
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
            }
            else status = false;
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

        public string Getdato(int eventID)
        {
            var v = _context.Turno.Where(a => a.EventID == eventID).FirstOrDefault();
            if (v != null)
                return v.Empid;
            else
                return "";
        }       
    }
}
