using Microsoft.AspNetCore.Http.Connections;
using Microsoft.EntityFrameworkCore;
using NJsonSchema.Generation.TypeMappers;
using PruebaIngresoBibliotecario.Api.Models;
using PruebaIngresoBibliotecario.Api.Models.Response;
using PruebaIngresoBibliotecario.Api.Services.Intefaces;
using PruebaIngresoBibliotecario.Infrastructure;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaIngresoBibliotecario.Api.Services
{
    public class PrestamoServices : IPrestamoServices
    {
        public readonly PersistenceContext _persistenceContext;

        public PrestamoServices(PersistenceContext persistence)
        {
            _persistenceContext = persistence;
        }

        public async Task<PrestamoResponse> CrearPrestamo(PrestamoRequest prestamoRequest)
        {
            var Prestamo = new PrestamoModel
            {
                Id = Guid.NewGuid().ToString(),
                Isbn = prestamoRequest.Isbn.ToString(),
                IdentificacionUsuario = prestamoRequest.IdentificacionUsuario,
                TipoUsuario = prestamoRequest.TipoUsuario,
                FechaMaximaDevolucion = await ValidarFechaDevolucion(prestamoRequest.TipoUsuario)
            };

            var Creado = await _persistenceContext.Prestamos.AddAsync(Prestamo);
            await _persistenceContext.SaveChangesAsync();

            return new PrestamoResponse(Guid.Parse(Creado.Entity.Id), Creado.Entity.FechaMaximaDevolucion);

        }

        public async Task<ResponseError> ValidarInvitado(PrestamoRequest prestamoRequest)
        {
            var Consultado = await _persistenceContext.Prestamos.Where(x => x.IdentificacionUsuario == prestamoRequest.IdentificacionUsuario && x.TipoUsuario == 3).FirstOrDefaultAsync();

            if (Consultado is not null)
            {
                return new ResponseError($"El usuario con identificacion {prestamoRequest.IdentificacionUsuario} ya tiene un libro prestado por lo cual no se le puede realizar otro prestamo");
            }
            else
            {
                return new ResponseError("");
            }
        }

        public async Task<PrestamoModel> ListarPrestamo(string id)
        {
            var Registro = await _persistenceContext.Prestamos.Where(x => x.Id == id).FirstOrDefaultAsync();
            return Registro;
        }

        public async Task<ResponseError> ValidarPrestamo(string id)
        {
            var Registro = await _persistenceContext.Prestamos.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (Registro is null)
            {
                return new ResponseError($"El prestamo con id {id} no existe");
            }
            else
            {
                return new ResponseError($"");
            }
        }

        private async Task<DateTime> ValidarFechaDevolucion(int TipoUsuario)
        {
            DateTime date = DateTime.Now.Date;
            int Dias = TipoUsuario == 1 ? 10 : TipoUsuario == 2 ? 8 : TipoUsuario == 3 ? 7 : 0;
            for (int i = 0; i <= Dias; i++)
            {
                if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                {
                    Dias = Dias+1;
                }
                date = date.AddDays(1);
            }

            DateTime DateEnd = DateTime.Now.Date;
            DateEnd = DateEnd.AddDays(Dias);

            return DateEnd;
        }
    }
}
