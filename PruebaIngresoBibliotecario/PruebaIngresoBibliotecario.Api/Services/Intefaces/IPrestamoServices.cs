using PruebaIngresoBibliotecario.Api.Models;
using PruebaIngresoBibliotecario.Api.Models.Response;
using System.Threading.Tasks;

namespace PruebaIngresoBibliotecario.Api.Services.Intefaces
{
    public interface IPrestamoServices
    {
        Task<PrestamoResponse> CrearPrestamo(PrestamoRequest prestamoRequest);
        Task<ResponseError> ValidarInvitado(PrestamoRequest prestamoRequest);
        Task<PrestamoModel> ListarPrestamo(string id);
        Task<ResponseError> ValidarPrestamo(string id);
    }
}
