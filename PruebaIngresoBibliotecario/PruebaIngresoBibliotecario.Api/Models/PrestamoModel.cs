using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PruebaIngresoBibliotecario.Api.Models
{
    public class PrestamoModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Isbn { get; set; }
        [Required]
        [MaxLength(10)]
        public string IdentificacionUsuario { get; set; }
        [Required]
        [Range(0, 9)]
        public int TipoUsuario { get; set; }
        [Required]
        public DateTime FechaMaximaDevolucion { get; set; }
    }

    public record PrestamoRequest(
        [Required]
        Guid Isbn,
        [Required]
        [MaxLength(10)]
        string IdentificacionUsuario,

        [Required]
        [Range(0,9)]
        int TipoUsuario
        );

    public record PrestamoResponse(
        [Required]
        Guid Id,
        [Required]
        DateTime FechaMaximaDevolucion
        );
}
