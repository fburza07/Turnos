using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Turnos.Models;
using Microsoft.AspNetCore.Http;
using System.Web;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore.Storage;

namespace Turnos.Controllers
{
    public class NuevoCalendarioFeriadoController : Controller
    {
        private readonly ePlaceDBContext _context;
        private IConfiguration configuration;
        public NuevoCalendarioFeriadoController(ePlaceDBContext context, IConfiguration configuration)
        {
            _context = context;
            this.configuration = configuration;
        }

        public IActionResult NuevoCalendarioFeriado()
        {
            TrnCalendarioFeriadoVista trnCalendarioFeriadoVista = new TrnCalendarioFeriadoVista();
            trnCalendarioFeriadoVista.trnCalendarioFeriado = new TrnCalendarioFeriado();
            trnCalendarioFeriadoVista.detalle = new List<TrnCalendarioFeriadoDetalle>();
            HttpContext.Session.SetString("trnCalendarioFeriadoVista", JsonConvert.SerializeObject(trnCalendarioFeriadoVista));

            return View(trnCalendarioFeriadoVista);
        }

        [HttpPost]
        public IActionResult NuevoCalendarioFeriado(TrnCalendarioFeriadoVista trnCalendarioFeriadoVista)
        {
            if (ModelState.IsValid)
            {
                trnCalendarioFeriadoVista = JsonConvert.DeserializeObject<TrnCalendarioFeriadoVista>(HttpContext.Session.GetString("trnCalendarioFeriadoVista"));

                if (trnCalendarioFeriadoVista.detalle.Count > 0)
                {
                    using (var context = new ePlaceDBContext())
                    {
                        using (IDbContextTransaction transaction = context.Database.BeginTransaction())
                        {
                            try
                            {                                
                                string descripcion = Request.Form["trnCalendarioFeriado.Descripcion"];
                                TrnCalendarioFeriado trnCalendarioFeriado = new TrnCalendarioFeriado
                                {
                                    Descripcion = descripcion,
                                    Empid = configuration.GetSection("empid").Value,
                                };
                                _context.Add(trnCalendarioFeriado);
                                _context.SaveChanges();

                                int lastCalendarioFeriadoID = _context.TrnCalendarioFeriado.ToList().Select(C => C.IdCalendarioFeriado).Max();

                                foreach (var item in trnCalendarioFeriadoVista.detalle)
                                {
                                    TrnCalendarioFeriadoDetalle trnCalendarioFeriadoDetalle = new TrnCalendarioFeriadoDetalle
                                    {
                                        IdCalendarioFeriado = lastCalendarioFeriadoID,
                                        Descripcion = item.Descripcion,
                                        Empid = item.Empid,
                                        Fecha = item.Fecha,
                                        DiaCompleto = item.DiaCompleto,
                                        HoraDesde = item.HoraDesde,
                                        HoraHasta = item.HoraHasta
                                    };
                                    _context.Add(trnCalendarioFeriadoDetalle);
                                }
                                _context.SaveChanges();

                                transaction.Commit();

                                trnCalendarioFeriadoVista.trnCalendarioFeriado = new TrnCalendarioFeriado();
                                trnCalendarioFeriadoVista.detalle = new List<TrnCalendarioFeriadoDetalle>();
                                HttpContext.Session.SetString("trnCalendarioFeriadoVista", JsonConvert.SerializeObject(trnCalendarioFeriadoVista));

                                ViewBag.Message = "Se guardó correctamente";
                                ViewBag.ResultMessageCss = "alert-success";
                            }
                            catch (Exception ex)
                            {
                                ViewBag.Message = "Hubo un error al guardar los datos!";
                                ViewBag.ResultMessageCss = "alert-danger";

                                transaction.Rollback();
                                throw ex;
                            }
                            finally
                            {
                                ModelState.Clear();
                            }
                        }
                    }
                }
                else
                {
                    ViewBag.Message = "Aún no ha ingresado elementos en el detalle!";
                    ViewBag.ResultMessageCss = "alert-danger";
                }
            }

            return View(trnCalendarioFeriadoVista);
        }
        [HttpGet]
        public IActionResult AddDetail()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddDetail(TrnCalendarioFeriadoDetalle trnCalendarioFeriadoDetalle)
        {
            TrnCalendarioFeriadoVista trnCalendarioFeriadoVista = JsonConvert.DeserializeObject<TrnCalendarioFeriadoVista>(HttpContext.Session.GetString("trnCalendarioFeriadoVista"));
            DateTime? date = null;
            trnCalendarioFeriadoDetalle = new TrnCalendarioFeriadoDetalle()
            {
                IdCalendarioFeriado = 0,
                Empid = configuration.GetSection("empid").Value,
                Fecha = DateTime.Parse(Request.Form["Fecha"]),
                Descripcion = Request.Form["Descripcion"],
                DiaCompleto = Request.Form["DiaCompleto"] == "checked" ? "S" : "N",
                HoraDesde = string.IsNullOrWhiteSpace(Request.Form["HoraDesde"]) ? date : DateTime.Parse(Request.Form["HoraDesde"]),
                HoraHasta = string.IsNullOrWhiteSpace(Request.Form["HoraHasta"]) ? date : DateTime.Parse(Request.Form["HoraHasta"]),
            };
            trnCalendarioFeriadoVista.detalle.Add(trnCalendarioFeriadoDetalle);

            HttpContext.Session.SetString("trnCalendarioFeriadoVista", JsonConvert.SerializeObject(trnCalendarioFeriadoVista));

            return View("NuevoCalendarioFeriado", trnCalendarioFeriadoVista);
        }

    }
}