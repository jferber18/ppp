using Microsoft.AspNetCore.Mvc;
using PruebaIngresoBibliotecario.Api.Models;
using PruebaIngresoBibliotecario.Api.Services.Intefaces;
using System.Threading.Tasks;

namespace PruebaIngresoBibliotecario.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrestamoController : ControllerBase
    {
        public readonly IPrestamoServices _prestamoServices;

        public PrestamoController(IPrestamoServices prestamoServices)
        {
            _prestamoServices = prestamoServices;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var Validacion = await _prestamoServices.ValidarPrestamo(id);
                if (Validacion.mensaje != "")
                {
                    return NotFound(Validacion);
                }

                var registro = await _prestamoServices.ListarPrestamo(id);

                return Ok(registro);

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(PrestamoRequest prestamo)
        {
            try
            {
                var validacion = await _prestamoServices.ValidarInvitado(prestamo);
                if (validacion.mensaje != "")
                {
                    return BadRequest(validacion);
                }

                var registro = await _prestamoServices.CrearPrestamo(prestamo);
                return Ok(registro);

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

    }
}
