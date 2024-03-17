using Coling.API.Curriculum.Contratos.Repositorio;
using Coling.API.Curriculum.Modelo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Coling.API.Curriculum.EndPoints
{
    public class InstitucionFunction
    {
        private readonly ILogger<InstitucionFunction> _logger;
        private readonly IInstitucionRepositorio repos;

        public InstitucionFunction(ILogger<InstitucionFunction> logger, IInstitucionRepositorio repos)
        {
            _logger = logger;
            this.repos = repos;
        }

        [Function("InsertarInstitucion")]
        [OpenApiOperation("Listarspec", "InsertarInstitucion", Description = "Sirve para listar todas las instituciones")]
        [OpenApiRequestBody("application/json", typeof(Institucion),
           Description = "Institucion modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json",
            bodyType: typeof(Institucion),
            Description = "Mostrara la institucion creada")]
        public async Task<HttpResponseData> InsertarInstitucion([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                var registro = await req.ReadFromJsonAsync<Institucion>() ?? throw new Exception("Debe ingresar una institucion con todos sus datos");
                registro.RowKey = Guid.NewGuid().ToString();
                registro.Timestamp = DateTime.UtcNow;
                bool sw = await repos.Create(registro);
                if (sw)
                {
                    respuesta = req.CreateResponse(HttpStatusCode.OK);
                    return respuesta;
                }
                else 
                { 
                    respuesta = req.CreateResponse(HttpStatusCode.BadRequest); 
                    return respuesta;
                }
            }
            catch (Exception)
            {
                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }

        [Function("ListarInstitucion")]
        [OpenApiOperation("Listarspec", "ListarInstitucion", Description ="Sirve para listar todas las instituciones") ]
        [OpenApiResponseWithBody(statusCode:HttpStatusCode.OK, contentType:"application/json", 
            bodyType: typeof(List<Institucion>),
            Description ="Mostrara una lista de instituciones")]
        public async Task<HttpResponseData> ListarInstitucion([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                var lista = repos.GetAll();
                respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(lista.Result);
                return respuesta;
            }
            catch (Exception)
            {
                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }

        [Function("ListaNombres")]
        [OpenApiOperation("Listarspec", "ListaNombres", Description = "Sirve para listar todas las instituciones")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json",
            bodyType: typeof(List<string>),
            Description = "Mostrara una lista de instituciones")]
        public async Task<HttpResponseData> ListaNombres([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                List<string> cadenas = new List<string>();
                cadenas.Add("UPDS");
                cadenas.Add("UAJMS");
                cadenas.Add("UNO");
                respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(cadenas);

                return respuesta;
                //return new OkObjectResult(cadenas);
            }
            catch (Exception)
            {
                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;

                //return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
