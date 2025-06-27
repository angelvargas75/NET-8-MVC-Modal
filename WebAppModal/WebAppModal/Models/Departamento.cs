using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppModal.Models;

public partial class Departamento
{
    [Key]
    public int Id { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? NombreDepartamento { get; set; }

    [InverseProperty("IdDepartamentoNavigation")]
    public virtual ICollection<Provincia> Provincia { get; set; } = new List<Provincia>();

    [InverseProperty("IdDepartamentoNavigation")]
    public virtual ICollection<Trabajadores> Trabajadores { get; set; } = new List<Trabajadores>();
}
