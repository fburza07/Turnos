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
            ePlaceDBContext context = new ePlaceDBContext();

            ViewData["IdTipoBoca"] = new SelectList(context.TrnBocaTipo, "IdTipoBoca", "Codigo");
            ViewData["IdTransporteTipo"] = new SelectList(context.TrnTransporteTipo, "IdTransporteTipo", "Nombre");

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

        public JsonResult ObtenerTurnos(int idTipoBoca)
        {

            List<Object> eventos = new List<Object>();

            var feriados = _context.Feriado.ToList();
            var turnos = _context.Turno.Where(a => a.IdTipoBoca == idTipoBoca).ToList();

            eventos.AddRange(feriados);
            eventos.AddRange(turnos);

            return Json(eventos);

        }

        [HttpPost]
        public JsonResult GrabarTurno(TrnTurno e)
        {
            var status = false;
            var errorFeriado = false;
            var errorEvento = false;            
            int idBoca = 0;
            string empid = configuration.GetSection("empid").Value;
            TrnFeriado trnFeriado = new TrnFeriado(configuration);
            TrnTurno trnTurno = new TrnTurno(configuration);
            //REVISAR SI NO ES FERIADO EL DIA EN EL CUAL QUIERE INGRESAR EL TURNO
            if (!trnFeriado.EsFeriado(empid, e.Start, e.End))
            {
                //VERIFICAR Y BUSCAR BOCA DISPONIBLE CON EL TIPO DE BOCA SELECCIONADO
                //PRIMERO BUSCO TODAS LAS BOCAS DISPONIBLES PARA EL EMPID Y EL TIPO DE BOCA SELECCIONADO
                List<TrnBoca> trnBocasPorTipo = _context.TrnBoca.Where(a => a.IdTipoBoca == e.IdTipoBoca && a.Empid == empid).ToList();                
                foreach (TrnBoca item in trnBocasPorTipo)
                {
                    //DE TODAS LAS BOCAS ME FIJO EN CUALQUIERA QUE NO HAYA EVENTOS PARA LA FECHA Y HORA SELECCIONADA
                    //ENTONCES SI NO EXISTE UN EVENTO PARA ESA BOCA Y FECHA/HORA SELECCIONADA TOMO EL ID
                    if (!trnTurno.ExisteEvento(empid, e.EventID, item.IdBoca, e.IdTipoBoca, e.Start, e.End))
                    {
                        idBoca = item.IdBoca;
                    }
                }
                if (idBoca != 0)
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
                            v.IdTransporteTipo = e.IdTransporteTipo;
                            v.TransporteTipo = e.TransporteTipo;
                            v.KGPrevistos = e.KGPrevistos;
                            v.PalletsPrevistos = e.PalletsPrevistos;
                            v.IdTipoBoca = e.IdTipoBoca;
                            v.IdBoca = idBoca;
                        }
                    }
                    else
                    {
                        e.Empid = empid;
                        e.IdBoca = idBoca;
                        _context.Turno.Add(e);
                    }

                    _context.SaveChanges();
                    status = true;
                    ViewBag.Message = "El turno se guardó correctamente!";
                    ViewBag.ResultMessageCss = "alert-success";
                }
                else
                {
                    errorEvento = true;
                }
            }
            else
            {
                errorFeriado = true;
            }
            var jsonResult = new { status = status, errorFeriado = errorFeriado, errorEvento = errorEvento };
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

        [HttpPost]
        public string Getdato(int eventID)
        {
            var v = _context.Turno.Where(a => a.EventID == eventID).FirstOrDefault();
            if (v != null)
                return v.Empid;
            else
                return "";

        }

        [HttpPost]
        public string TraerDiasPrevision()
        {
            var v = _context.customizacion.Where(a => a.Empid == configuration.GetSection("empid").Value).FirstOrDefault();
            if (v != null)
                return v.HorarioMaximo.ToLongTimeString();
            else
                return "";
        }

        [HttpPost]
        public string TraerHorarioMinimoPorBoca(short dia)
        {
            ePlaceDBContext context = new ePlaceDBContext();
            TrnBoca trnBoca = new TrnBoca(configuration);
            trnBoca.Empid = configuration.GetSection("empid").Value;
            return trnBoca.TraerHorarioMinimoPorBoca(dia);
        }

        [HttpPost]
        public string TraerHorarioMaximoPorBoca(short dia)
        {
            ePlaceDBContext context = new ePlaceDBContext();
            TrnBoca trnBoca = new TrnBoca(configuration);
            trnBoca.Empid = configuration.GetSection("empid").Value;
            return trnBoca.TraerHorarioMaximoPorBoca(dia);
        }

        [HttpPost]
        //SE TOMA EL VALOR MINIMO ENTRE TODAS LAS BOCAS DEPENDIENDO EL TIPO SELECCIONADO
        public string TraerCantidadMinutosSegmento(byte idTipoBoca)
        {
            ePlaceDBContext context = new ePlaceDBContext();
            TrnBoca trnBoca = new TrnBoca(configuration);
            trnBoca.Empid = configuration.GetSection("empid").Value;
            trnBoca.IdTipoBoca = idTipoBoca;
            return TimeSpan.FromMinutes(trnBoca.TraerSegmentoMinimo()).ToString();
        }

    }
}
