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

        public IActionResult Index(string provid)
        {
            //provid = "AR3071542751";
            ePlaceDBContext context = new ePlaceDBContext();
            if (provid == null || provid == "")
                provid = configuration.GetSection("provid").Value;
            configuration.GetSection("provid").Value = provid;

            //Los turnos se ingresan por proveedor pero los datos de boca transporte y planta usan el adherente
            //busco adherente para traer estos datos
            TrnProvMapSocio adherente = _context.TrnProvMapSocio.Where(a => a.Prov_Id == provid).FirstOrDefault();
            configuration.GetSection("empid").Value = adherente.Emp_Id;

            ViewData["IdTipoBoca"] = new SelectList(context.TrnBocaTipo.Where(a => a.Empid == adherente.Emp_Id).ToList(), "IdTipoBoca", "Nombre");
            ViewData["IdTransporteTipo"] = new SelectList(context.TrnTransporteTipo.Where(a => a.Empid == adherente.Emp_Id && a.Activo == true).ToList(), "IdTransporteTipo", "Nombre");
            ViewData["Codigo"] = new SelectList(_context.TrnUsuarioPlanta.Where(a => a.User_Id == adherente.Emp_Id).ToList(), "Codigo", "Descripcion");

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
            
        public JsonResult ObtenerTurnos(byte idTipoBoca, string idPlanta, string estado)
        {
            List<Object> eventos = new List<Object>();

            List<TrnFeriado> feriados = new List<TrnFeriado>();
            List<TrnTurno> turnos = new List<TrnTurno>();

            TrnTurno trnFeriado = new TrnTurno(configuration);
            trnFeriado.Empid = configuration.GetSection("empid").Value;
            //Toma los feriados por planta y a su vez que el idcalendarioferiado del evento corresponda con el asignado a la boca 
            feriados = trnFeriado.ObtenerFeriados(idPlanta);

            TrnTurno trnTurno = new TrnTurno(configuration);
            trnTurno.Empid = configuration.GetSection("empid").Value;
            trnTurno.Provid = configuration.GetSection("provid").Value;
            //Toma los turnos asignados a un proveedor, por tipo de boca, y que pertenezcan a la planta
            turnos = trnTurno.ObtenerTurnos(idTipoBoca, idPlanta, estado);

            eventos.AddRange(feriados);
            eventos.AddRange(turnos);
            //eventos.AddRange(HorariosOcupados(idTipoBoca, codigo));

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
            string provid = configuration.GetSection("provid").Value;
            TrnFeriado trnFeriado = new TrnFeriado(configuration);
            TrnTurno trnTurno = new TrnTurno(configuration);            

            //REVISAR SI NO ES FERIADO EL DIA EN EL CUAL QUIERE INGRESAR EL TURNO
            if (!trnFeriado.EsFeriado(empid, e.Start, e.End))
            {
                //VERIFICAR Y BUSCAR BOCA DISPONIBLE CON EL TIPO DE BOCA SELECCIONADO
                //PRIMERO BUSCO TODAS LAS BOCAS DISPONIBLES PARA EL TIPO DE BOCA SELECCIONADO
                List<TrnBoca> trnBocasPorTipo = _context.TrnBoca.Where(a => a.IdTipoBoca == e.IdTipoBoca && a.Empid == empid && a.Estado == true).ToList();
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
                            v.Provid = provid;
                            v.Empid = empid;
                            v.Subject = e.Subject;
                            v.Start = e.Start;
                            v.End = e.End;
                            v.Description = e.Description;
                            v.IsFullDay = e.IsFullDay;
                            v.ThemeColor = TraerColorPorEstados(e);
                            v.IdTransporteTipo = e.IdTransporteTipo;
                            v.TransporteTipo = _context.TransporteTipo.Where(a => a.IdTransporteTipo == v.IdTransporteTipo && v.Empid == e.Empid).FirstOrDefault().Nombre;
                            v.KGPrevistos = e.KGPrevistos;
                            v.PalletsPrevistos = e.PalletsPrevistos;
                            v.IdTipoBoca = e.IdTipoBoca;
                            v.IdBoca = idBoca;
                            v.ConfirmadoProveedor = e.ConfirmadoProveedor;
                        }
                    }
                    else
                    {
                        e.TransporteTipo = e.TransporteTipo == null ? "" : e.TransporteTipo;
                        e.ThemeColor = TraerColorPorEstados(e);
                        e.Empid = empid;
                        e.Provid = provid;
                        e.IdBoca = idBoca;
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
            var jsonResult = new { status = status, errorFeriado = errorFeriado, errorEvento = errorEvento, errorBoca = errorBoca, start = e.Start };
            return Json(jsonResult);
        }

        private string TraerColorPorEstados(TrnTurno e)
        {
            string color = "";
            TrnBocaTipo trnTipoBoca = _context.TrnBocaTipo.Where(a => a.IdTipoBoca == e.IdTipoBoca && a.Empid == configuration.GetSection("empid").Value).FirstOrDefault();

            if (trnTipoBoca.Codigo.Trim() == "D")
                color = "lightGreen";
            else if (trnTipoBoca.Codigo.Trim() == "R" && !e.ConfirmadoAdherente)
                color = "#FA9C99";   //Rojo claro         
            
            return color;
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

            var jsonResult = new { status = status, start = v.Start };
            return Json(jsonResult);
        }

        [HttpPost]
        public JsonResult ConfirmarTurno(int eventID)
        {
            var status = false;

            var v = _context.Turno.Where(a => a.EventID == eventID).FirstOrDefault();
            if (v != null)
            {
                v.ConfirmadoProveedor = true;
                if (v.ConfirmadoProveedor && v.ConfirmadoAdherente)
                    v.ThemeColor = "#6F8908"; //Verde oscuro

                _context.SaveChanges();
                status = true;
            }

            var jsonResult = new { status = status, start = v.Start };
            return Json(jsonResult);
        }

        [HttpPost]
        public JsonResult CancelarTurno(int eventID)
        {
            var status = false;

            var v = _context.Turno.Where(a => a.EventID == eventID).FirstOrDefault();

            if (v != null)
            {
             
                v.ConfirmadoProveedor = false;
                v.ThemeColor = this.TraerColorPorEstados(v);

                _context.SaveChanges();
                status = true;
            }

            var jsonResult = new { status = status, start = v.Start };
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

        [HttpPost]
        public int TraerDiasPrevision(int idTipoBoca)
        {
            var v = _context.TrnBoca.Where(a => a.Empid == configuration.GetSection("empid").Value && a.IdTipoBoca == idTipoBoca && a.Estado == true);
            
            if (v != null)
                return (from x in v select x.DiasPrevision).Min();
            else
                return 0;
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
        public string TraerCantidadMinutosSegmento(byte idTipoBoca, string idPlanta)
        {
            ePlaceDBContext context = new ePlaceDBContext();
            TrnBoca trnBoca = new TrnBoca(configuration);
            trnBoca.IdPlanta = idPlanta;
            trnBoca.IdTipoBoca = idTipoBoca;
            trnBoca.Empid = configuration.GetSection("empid").Value;
            return TimeSpan.FromMinutes(trnBoca.TraerSegmentoMinimoPorBoca()).ToString();
        }

        [HttpPost]
        //SI ES TIPO T (CARGA Y DESCARGA) SOLO PUEDEN VERSE LOS DATOS DE LOS TURNOS PERO NO EDITARLOS
        public JsonResult PermiteEdicion(byte idTipoBoca)
        {
            bool permite = true;
            TrnBocaTipo trnTipoBoca = _context.TrnBocaTipo.Where(a => a.IdTipoBoca == idTipoBoca && a.Empid == configuration.GetSection("empid").Value).FirstOrDefault();            
            if (trnTipoBoca.Codigo.Trim() == "T")
                permite = false;

            var jsonResult = new { permite = permite };
            return Json(jsonResult);
        }

        [HttpPost]
        //VERIFICA DISPONIBILIDAD DE BOCA SIEMPRE Y CUANDO EL TIPO DE BOCA NO SEA T (ESTA ULTIMA VALIDACION LA REALIZA ANTES DE VERIFICAR ESTO)           
        public JsonResult VerificarBocaDisponibleporTipo(byte idTipoBoca)
        {
            bool bocaDisponible = false;

            TrnBoca trnBoca = _context.TrnBoca.Where(a => a.IdTipoBoca == idTipoBoca && a.Empid == configuration.GetSection("empid").Value && a.Estado == true).FirstOrDefault();
            if (trnBoca != null && trnBoca.IdBoca != 0)
                bocaDisponible = true;

            var jsonResult = new { bocaDisponible = bocaDisponible };
            return Json(jsonResult);
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

    }
}
