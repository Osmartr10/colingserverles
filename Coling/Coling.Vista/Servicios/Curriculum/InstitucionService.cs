using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coling.Shared;
using Coling.Vista.Modelos;
using Newtonsoft.Json;
namespace Coling.Vista.Servicios.Curriculum
{
    public class InstitucionService: IInstitucionService
    {
        string url = "http://localhost:7227";
        string endPoint = "";
        HttpClient client = new HttpClient();

        public async Task<List<Institucion>> ListaInstituciones()
        {
            endPoint = "api/ListarInstitucion";
            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.GetAsync(endPoint);
            List<Institucion> result = new List<Institucion>();
            if (response.IsSuccessStatusCode)
            {
                string respuestaCuerpo = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<List<Institucion>>(respuestaCuerpo);
            }
            return result;
        }

    }
}
