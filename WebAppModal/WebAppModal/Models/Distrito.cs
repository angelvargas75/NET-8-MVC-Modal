using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppModal.Models;

public partial class Distrito
{
    [Key]
    public int Id { get; set; }

    public int? IdProvincia { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? NombreDistrito { get; set; }

    [ForeignKey("IdProvincia")]
    [InverseProperty("Distrito")]
    public virtual Provincia? IdProvinciaNavigation { get; set; }

    [InverseProperty("IdDistritoNavigation")]
    public virtual ICollection<Trabajadores> Trabajadores { get; set; } = new List<Trabajadores>();
}
