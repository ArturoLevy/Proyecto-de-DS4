using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Configuration;
using System.Web.Http;
using Proyecto_2.Models;
using Proyecto_2.Persistencia;

namespace Proyecto_2.Controllers
{
    public class OperacionesController : ApiController
    {
        private readonly Queries queries;
        public OperacionesController()
        {
            queries = new Queries();
        }
        [HttpGet]
        public IHttpActionResult ObtenerTodos()
        {
            try
            {
                List<CalculadoraCientifica> calculadoras = queries.ObtenerTodos();

                return Ok(calculadoras);
            }
            catch (System.Exception ex)
            {
                return InternalServerError(ex);
            }
        }
      
        [HttpPost]
        public IHttpActionResult AddOperacion([FromBody] CalculadoraCientifica operacion)
        {
            try
            {
                queries.AddOperacion(operacion.Numero1,operacion.Operacion,operacion.Numero2, operacion.Resultado);

          
                return Ok("Operación agregada correctamente");
            }
            catch (System.Exception ex)
            {
    
                return InternalServerError(ex);
            }
        }


        [HttpGet]
        [Route("api/calculadoracientifica/suma")]
        public IHttpActionResult Suma([FromBody] CalculadoraCientifica request)
        {
            try
            {
                decimal resultado = request.Numero1 + request.Numero2;
                return Ok(new { Resultado = resultado });
            }
            catch (System.Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpGet]
        [Route("api/calculadoracientifica/buscar")]
        public IHttpActionResult BuscarPorOperacion(string Operacion)
        {
            try
            {
                string decodedOperacion1 = System.Web.HttpUtility.UrlDecode(Operacion);

                Console.WriteLine($"Operacion recibida: {Operacion}");
                if (Operacion == "+")
                {
                    Operacion = "+";
                }

                var operaciones = queries.GetOperationsByOperacion(decodedOperacion1);

                if (operaciones == null || operaciones.Count == 0)
                {
                    return NotFound(); 
                }
                return Ok(operaciones);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en BuscarPorOperacion: {ex}");
                return InternalServerError(ex);
            }
        }


    }

}
