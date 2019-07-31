/*********************************************************************************************************************************************************************************************
* Módulo                : Turnos
* Versión               : v2.0.7
* Descripción           : Módulo de gestión y reserva de Turnos
* Autor                 : Fernando Burza
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
using System.Data;

namespace Turnos.Controllers
{
    /// <summary>
    /// Clase encargada del handling de la lógica de negocio del Calendario de Turnos
    /// </summary>
    public class TurnosPlantaController : Controller
    {
        private readonly TurnosContext _context;
        private IConfiguration configuration;

        public TurnosPlantaController(TurnosContext context, IConfiguration configuration)
        {
            _context = context;
            this.configuration = configuration;
        }

        public IActionResult Index(string empid)
        {
            empid = "AR3050450732";
            ePlaceDBContext context = new ePlaceDBContext();
            if (empid == null || empid == "")
                empid = configuration.GetSection("empid").Value;
            configuration.GetSection("empid").Value = empid;

            TrnProveedores proveedor = new TrnProveedores(configuration);
            proveedor.emp_id = empid;
            List<TrnProveedores> proveedores = proveedor.TraerProveedoresPorEmpID();
            //List<TrnProvMapSocio> a = new _context.TrnProvMapSocio.Where(a => a.Emp_Id == empid).ToList();
            ViewData["Proveedores"] = new SelectList(proveedores, "prov_id", "nombre");
            ViewData["IdTipoBoca"] = new SelectList(context.TrnBocaTipo.Where(a => a.Empid == empid).ToList(), "IdTipoBoca", "Nombre");
            ViewData["IdTransporteTipo"] = new SelectList(context.TrnTransporteTipo.Where(a => a.Empid == empid).ToList(), "IdTransporteTipo", "Nombre");
            ViewData["Codigo"] = new SelectList(_context.TrnUsuarioPlanta.Where(a => a.User_Id == empid).ToList(), "Codigo", "Descripcion");

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public JsonResult ObtenerTurnosPlanta(string idPlanta)
        {
            List<Object> eventos = new List<Object>();

            List<TrnFeriado> feriados = new List<TrnFeriado>();
            List<TrnTurno> turnos = new List<TrnTurno>();

            TrnTurno trnFeriado = new TrnTurno(configuration);
            trnFeriado.Empid = configuration.GetSection("empid").Value;
            //Toma los feriados por planta y a su vez que el idcalendarioferiado del evento corresponda con el asignado a la boca 
            feriados = trnFeriado.ObtenerFeriados(idPlanta);

            TrnTurno trnTurno = new TrnTurno(configuration);
            //Toma los turnos asignados a todos los proveedores, por tipo de boca, y que pertenezcan a la planta elegida
            trnTurno.Empid = configuration.GetSection("empid").Value;
            turnos = trnTurno.ObtenerTurnosPlanta(idPlanta);

            eventos.AddRange(feriados);
            eventos.AddRange(turnos);

            return Json(eventos);

        }

        [HttpPost]
        public JsonResult GrabarTurno(TrnTurno e, string idPlanta)
        {
            var status = false;
            var errorFeriado = false;
            var errorBoca = true;
            var errorEvento = true;
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
                    //Si entra quiere decir que encuentra boca por lo tanto no existe error en bocas
                    errorBoca = false;
                    //DE TODAS LAS BOCAS ME FIJO EN CUALQUIERA QUE NO HAYA EVENTOS PARA LA FECHA Y HORA SELECCIONADA
                    //ENTONCES SI NO EXISTE UN EVENTO PARA ESA BOCA Y FECHA/HORA SELECCIONADA TOMO EL ID
                    if (!trnTurno.ExisteEvento(empid, idPlanta, e.EventID, item.IdBoca, e.IdTipoBoca, e.Start, e.End))
                    {
                        idBoca = item.IdBoca;
                        errorEvento = false;
                        break;
                    }
                }

                if (idBoca != 0 && !errorEvento)
                {
                    if (e.EventID > 0)
                    {
                        var v = _context.Turno.Where(a => a.EventID == e.EventID).FirstOrDefault();
                        if (v != null)
                        {
                            v.Empid = empid;
                            v.Provid = e.Provid;
                            v.Subject = e.Subject;
                            v.Start = e.Start;
                            v.End = e.End;
                            v.Description = e.Description;
                            v.IsFullDay = e.IsFullDay;
                            v.ThemeColor = TraerColorPorEstados(e);
                            v.IdTransporteTipo = e.IdTransporteTipo;
                            v.TransporteTipo = e.TransporteTipo;
                            v.KGPrevistos = e.KGPrevistos;
                            v.PalletsPrevistos = e.PalletsPrevistos;
                            v.IdTipoBoca = e.IdTipoBoca;
                            v.IdBoca = idBoca;
                            v.ConfirmadoProveedor = e.ConfirmadoProveedor;
                            v.ConfirmadoAdherente = e.ConfirmadoAdherente;
                        }
                    }
                    else
                    {
                        e.Empid = empid;
                        e.IdBoca = idBoca;
                        e.ThemeColor = TraerColorPorEstados(e);
                        _context.Turno.Add(e);
                    }

                    _context.SaveChanges();
                    status = true;
                    ViewBag.Message = "El turno se guardó correctamente!";
                    ViewBag.ResultMessageCss = "alert-success";
                }
            }
            else
            {
                errorFeriado = true;
            }
            var jsonResult = new { status = status, errorFeriado = errorFeriado, errorEvento = errorEvento, errorBoca = errorBoca };
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
        public JsonResult ConfirmarTurno(int eventID)
        {
            var status = false;

            var v = _context.Turno.Where(a => a.EventID == eventID).FirstOrDefault();
            if (v != null)
            {
                v.ConfirmadoAdherente = true;
                if (v.ConfirmadoProveedor && v.ConfirmadoAdherente)
                    v.ThemeColor = "#6F8908"; //Verde oscuro
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
        public string TraerHorarioMinimoPorBoca(short dia, string idPlanta)
        {
            ePlaceDBContext context = new ePlaceDBContext();
            TrnBoca trnBoca = new TrnBoca(configuration);
            trnBoca.Empid = configuration.GetSection("empid").Value;
            trnBoca.IdPlanta = idPlanta;
            return trnBoca.TraerHorarioMinimoPorBoca(dia);
        }

        [HttpPost]
        public string TraerHorarioMaximoPorBoca(short dia, string idPlanta)
        {
            ePlaceDBContext context = new ePlaceDBContext();
            TrnBoca trnBoca = new TrnBoca(configuration);
            trnBoca.Empid = configuration.GetSection("empid").Value;
            trnBoca.IdPlanta = idPlanta;
            return trnBoca.TraerHorarioMaximoPorBoca(dia);
        }

        [HttpPost]
        //SE TOMA EL VALOR MINIMO ENTRE TODAS LAS BOCAS DEPENDIENDO EL TIPO SELECCIONADO
        public string TraerCantidadMinutosSegmento(string idPlanta)
        {
            ePlaceDBContext context = new ePlaceDBContext();
            TrnBoca trnBoca = new TrnBoca(configuration);
            trnBoca.IdPlanta = idPlanta;
            trnBoca.Empid = configuration.GetSection("empid").Value;
            return TimeSpan.FromMinutes(trnBoca.TraerSegmentoMinimo()).ToString();
        }

        public string TraerNombreProveedor(string empid)
        {
            TrnUsuarioMaestros trnUsuarioMaestros = _context.TrnUsuarioMaestros.Where(a => a.usr_Id == empid).FirstOrDefault();
            return trnUsuarioMaestros.nombre.Trim() + ": ";
        }


        [HttpPost]
        public DateTime TraerFechaDesdeEvento(int eventID)
        {
            TrnTurno trnTurno = _context.Turno.Where(a => a.EventID == eventID).FirstOrDefault();
            if (trnTurno != null)
                return trnTurno.Start;
            else
                return DateTime.Now;

        }

        public JsonResult ObtenerReferenciaDeBocas(string idPlanta)
        {
            List<TrnBoca> bocas = new List<TrnBoca>();
            bocas = _context.TrnBoca.Where(a => a.IdPlanta == idPlanta && a.Empid == configuration.GetSection("empid").Value).ToList();

            return Json(bocas);
        }

        public string TraerColorPorIdBoca(int IdBoca)
        {
            
            TrnBoca boca = _context.TrnBoca.Where(a => a.IdBoca == IdBoca && a.Empid == configuration.GetSection("empid").Value).FirstOrDefault();            
            return boca.color;
        }

        public string TraerCodigoPorTipoBoca(int IdTipoBoca)
        {
            TrnBocaTipo tipoBoca = new TrnBocaTipo();
            tipoBoca = _context.TrnBocaTipo.Where(a => a.IdTipoBoca == IdTipoBoca && a.Empid == configuration.GetSection("empid").Value).FirstOrDefault();
            return tipoBoca.Codigo.Trim();
        }

        private string TraerColorPorEstados(TrnTurno e)
        {
            string color = "";
            TrnBocaTipo trnTipoBoca = _context.TrnBocaTipo.Where(a => a.IdTipoBoca == e.IdTipoBoca && a.Empid == configuration.GetSection("empid").Value).FirstOrDefault();

            if (trnTipoBoca.Codigo.Trim() == "R")
                color = "orange";
            else
                color = "yellow";

            return color;
        }

    }
}
