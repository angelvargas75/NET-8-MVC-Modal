using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppModal.Models;

public partial class Trabajadores
{
    [Key]
    public int Id { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [Display(Name = "Tipo de Documento")]
    public string? TipoDocumento { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [Display(Name = "Numero de Documento")]
    public string? NumeroDocumento { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [Display(Name = "Nombres")]
    public string? Nombres { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [Display(Name = "Sexo")]
    public string? Sexo { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [Display(Name = "Departamento")]
    public int? IdDepartamento { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [Display(Name = "Provincia")]
    public int? IdProvincia { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [Display(Name = "Distrito")]
    public int? IdDistrito { get; set; }

    [ForeignKey("IdDepartamento")]
    [InverseProperty("Trabajadores")]
    public virtual Departamento? IdDepartamentoNavigation { get; set; }

    [ForeignKey("IdDistrito")]
    [InverseProperty("Trabajadores")]
    public virtual Distrito? IdDistritoNavigation { get; set; }

    [ForeignKey("IdProvincia")]
    [InverseProperty("Trabajadores")]
    public virtual Provincia? IdProvinciaNavigation { get; set; }

}
