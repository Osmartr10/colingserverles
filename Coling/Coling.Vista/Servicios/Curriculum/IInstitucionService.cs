using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coling.Vista.Modelos;
namespace Coling.Vista.Servicios.Curriculum
{
    public interface IInstitucionService
    {
        Task<List<Institucion>> ListaInstituciones();
    }
}
