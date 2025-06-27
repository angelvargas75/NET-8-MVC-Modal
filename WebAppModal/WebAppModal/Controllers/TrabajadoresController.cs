using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebAppModal.Data;
using WebAppModal.Models;

namespace WebAppModal.Controllers
{
    public class TrabajadoresController : Controller
    {
        private readonly AppDbContext _context;
        public TrabajadoresController(AppDbContext context)
        {
                _context = context;
        }

        public async Task<IActionResult> Index(string sexoFiltro = null)
        {
            try
            {
                // Cargar departamentos para el modal
                ViewBag.Departamentos = new SelectList(
                    await _context.Departamento.ToListAsync(), "Id", "NombreDepartamento");

                // Uso con LinQ
                //var query = _context.Trabajadores
                //.Include(t => t.IdDepartamentoNavigation)
                //.Include(t => t.IdProvinciaNavigation)
                //.Include(t => t.IdDistritoNavigation)
                //.AsQueryable();
                //if (!string.IsNullOrEmpty(sexoFiltro) && (sexoFiltro == "M" || sexoFiltro == "F"))
                //{
                //    query = query.Where(t => t.Sexo == sexoFiltro);
                //}
                //var trabajadores = await query.ToListAsync();

                // Uso con Store Procedure
                var param = new SqlParameter("@SexoFiltro", (object)sexoFiltro ?? DBNull.Value);

                var trabajadores = await _context
                    .Trabajadores
                    .FromSqlRaw("EXEC sp_ListarTrabajadores @SexoFiltro", param)
                    .ToListAsync();

                foreach (var trabajador in trabajadores)
                {
                    if (trabajador.IdDepartamento.HasValue)
                    {
                        trabajador.IdDepartamentoNavigation = await _context.Departamento
                            .FindAsync(trabajador.IdDepartamento);
                    }

                    if (trabajador.IdProvincia.HasValue)
                    {
                        trabajador.IdProvinciaNavigation = await _context.Provincia
                            .FindAsync(trabajador.IdProvincia);
                    }

                    if (trabajador.IdDistrito.HasValue)
                    {
                        trabajador.IdDistritoNavigation = await _context.Distrito
                            .FindAsync(trabajador.IdDistrito);
                    }
                }

                ViewBag.SexoFiltro = sexoFiltro;
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return PartialView("_FilasTabla", trabajadores);

                return View(trabajadores);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Ocurrió un error al cargar los trabajadores. {ex.Message}";
                return View(new List<Trabajadores>());
            }
        }

        public IActionResult Create()
        {
            ViewBag.Departamentos = new SelectList(_context.Departamento, "Id", "NombreDepartamento");
            ViewBag.Provincias = new SelectList(new List<Provincia>(), "Id", "NombreProvincia");
            ViewBag.Distritos = new SelectList(new List<Distrito>(), "Id", "NombreDistrito");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TipoDocumento,NumeroDocumento,Nombres,Sexo,IdDepartamento,IdProvincia,IdDistrito")] Trabajadores trabajador)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(trabajador);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Nuevo trabajador creado correctamente";
                    return RedirectToAction(nameof(Index));
                }
                ViewBag.Departamentos = new SelectList(_context.Departamento, "Id", "NombreDepartamento");
                ViewBag.Provincias = new SelectList(new List<Provincia>(), "Id", "NombreProvincia");
                ViewBag.Distritos = new SelectList(new List<Distrito>(), "Id", "NombreDistrito");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al crear el trabajador. {ex.Message}";
            }
            return View(trabajador);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var trabajador = await _context.Trabajadores
                .Include(t => t.IdDepartamentoNavigation)
                .Include(t => t.IdProvinciaNavigation)
                .Include(t => t.IdDistritoNavigation)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (trabajador == null) return NotFound();

            ViewBag.Departamentos = new SelectList(
                await _context.Departamento.ToListAsync(),
                "Id",
                "NombreDepartamento",
                trabajador.IdDepartamento);

            ViewBag.Provincias = new SelectList(
                await _context.Provincia
                    .Where(p => p.IdDepartamento == trabajador.IdDepartamento)
                    .ToListAsync(),
                "Id",
                "NombreProvincia",
                trabajador.IdProvincia);

            ViewBag.Distritos = new SelectList(
                await _context.Distrito
                    .Where(d => d.IdProvincia == trabajador.IdProvincia)
                    .ToListAsync(),
                "Id",
                "NombreDistrito",
                trabajador.IdDistrito);

            return PartialView("_FormTrabajador", trabajador);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TipoDocumento,NumeroDocumento,Nombres,Sexo,IdDepartamento,IdProvincia,IdDistrito")] Trabajadores trabajador)
        {
            try
            {
                if (id != trabajador.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(trabajador);
                        await _context.SaveChangesAsync();
                        TempData["SuccessMessage"] = "Trabajador actualizado correctamente";
                    }
                    catch (Exception ex)
                    {
                        if (!TrabajadoresExists(trabajador.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            TempData["ErrorMessage"] = $"Error al editar el trabajador. {ex.Message}";
                            return RedirectToAction(nameof(Index));
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al editar el trabajador. {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdDepartamento"] = new SelectList(_context.Departamento, "Id", "Id", trabajador.IdDepartamento);
            ViewData["IdDistrito"] = new SelectList(_context.Distrito, "Id", "Id", trabajador.IdDistrito);
            ViewData["IdProvincia"] = new SelectList(_context.Provincia, "Id", "Id", trabajador.IdProvincia);
            return View(trabajador);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trabajador = await _context.Trabajadores
                .Include(t => t.IdDepartamentoNavigation)
                .Include(t => t.IdProvinciaNavigation)
                .Include(t => t.IdDistritoNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trabajador == null)
            {
                return NotFound();
            }

            return PartialView("DeleteModal", trabajador);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var trabajador = await _context.Trabajadores.FindAsync(id);
                if (trabajador == null)
                {
                    TempData["ErrorMessage"] = "Trabajador no encontrado";
                    return RedirectToAction(nameof(Index));
                }

                _context.Trabajadores.Remove(trabajador);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Trabajador eliminado correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al eliminar el trabajador. {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        private bool TrabajadoresExists(int id)
        {
            return _context.Trabajadores.Any(e => e.Id == id);
        }


        [HttpGet]
        public async Task<IActionResult> GetProvinciasByDepartamento(int departamentoId)
        {
            try
            {
                var provincias = await _context.Provincia
                .Where(p => p.IdDepartamento == departamentoId)
                .Select(p => new { id = p.Id, nombreProvincia = p.NombreProvincia })
                .ToListAsync();
                return Json(provincias);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error al cargar provincias. {ex.Message}" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetDistritosByProvincia(int provinciaId)
        {
            try
            {
                var distritos = await _context.Distrito
                .Where(d => d.IdProvincia == provinciaId)
                .Select(d => new { id = d.Id, nombreDistrito = d.NombreDistrito })
                .ToListAsync();
                return Json(distritos);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error al cargar distritos. {ex.Message}" });
            }
        }
    }
}
