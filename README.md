# 🧑‍💻 Mantenimiento de Trabajadores en ASP.NET Core MVC

Este proyecto es el resultado del desarrollo de una prueba técnica para el puesto de **Programador Junior .NET Core**. Consiste en una aplicación web construida con **ASP.NET Core MVC 8** bajo el enfoque Database First de Entity Framework que permite gestionar un listado de trabajadores, incluyendo operaciones CRUD (Crear, Leer, Actualizar, Eliminar), todo con una experiencia moderna basada en **modales** y funcionalidades con **AJAX**.

---

## 📋 Requisitos Técnicos

- ASP.NET Core (versión mínima: **.NET 5**)
- Entity Framework Core
- SQL Server (Base de datos: `TrabajadoresPrueba`)
- Bootstrap 5
- jQuery y Select2
- Control de versiones con Git y GitHub

---

## 💻 Tecnologías utilizadas

| Componente       | Tecnología           |
|------------------|----------------------|
| Backend          | ASP.NET Core MVC 8   |
| ORM              | Entity Framework Core 7 |
| Base de Datos    | SQL Server 2019     |
| Frontend         | Bootstrap 5 + jQuery + Ajax |
| Componentes UI   | Select2, FontAwesome |
| Validaciones     | DataAnnotations      |

---

## 🧩 Funcionalidades Implementadas

✅ **Listado de Trabajadores**  
- Se utiliza un procedimiento almacenado (`sp_ListarTrabajadores`) para consultar la base de datos.  
- Las filas se pintan según el sexo: azul para **masculino** y naranja para **femenino**.

✅ **Crear Trabajador**  
- Implementado mediante un modal con validaciones cliente/servidor.  
- Soporta la selección dependiente de Departamento, Provincia y Distrito.  
- Se usa Select2 para una mejor experiencia en los `dropdowns`.

✅ **Editar Trabajador**  
- Mediante modal reutilizando el mismo formulario parcial.  
- Se conserva la selección dinámica de ubigeo.  
- Incluye validaciones completas.

✅ **Eliminar Trabajador**  
- Modal con mensaje de confirmación:  
  *"¿Está seguro que desea eliminar al trabajador?"*

✅ **Filtro por Sexo (Bonus)**  
- Filtrado mediante `AJAX` sin recargar la página.  
- Utiliza el mismo método `Index` del controlador.

✅ **Select2 en todos los combos**  
- Mejora la experiencia de búsqueda en los campos de ubicación.

---

## 🛠️ Estructura del Proyecto

- **Controllers/TrabajadoresController.cs**  
  Contiene toda la lógica CRUD.

- **Views/Trabajadores/**  
  - `Index.cshtml`: vista principal.  
  - `CreateModal.cshtml`, `EditModal.cshtml`, `DeleteModal.cshtml`: modales.  
  - `_FormTrabajador.cshtml`: vista parcial reutilizable.  
  - `_FilasTrabajadores.cshtml`: plantilla para pintar los datos por AJAX.

- **wwwroot/js/site.js**  
  Contiene la lógica con jQuery y AJAX.

- **Stored Procedures**  
  - `sp_ListarTrabajadores`: lista los trabajadores con filtro opcional por sexo.

---

## ⚙️ Instalación y Uso

1. Clonar el repositorio:
   ```bash
   git clone https://github.com/angelvargas75/Prueba-Tecnica-Myper-2.git
   cd Prueba-Tecnica-Myper-2

2. Configurar la cadena de conexión en appsettings.json apuntando a tu instancia de SQL Server.

3. Ejecutar el script de base de datos en SQL Server.

4. Ejecutar la aplicación:

   ```bash
   dotnet run

---

##  Consideraciones
- La validación del formulario utiliza anotaciones [Required] y mensajes personalizados.
- Se manejan errores del servidor y se notifican mediante TempData y alertas.
- Al cerrar el modal sin guardar, se limpia correctamente el formulario.

---

##  Capturas

<p align="center">
  <img src="https://i.ibb.co/xS7xbXnr/1-index.png" alt="1-index" border="0" />
</p>

<p align="center">
  <img src="https://i.ibb.co/39133yRD/2-create.png" alt="2-create" border="0">
</p>

<p align="center">
  <img src="https://i.ibb.co/MDb84pqw/3-edit.png" alt="3-edit" border="0">
</p>

<p align="center">
  <img src="https://i.ibb.co/W4k7MBZJ/4-delete.png" alt="4-delete" border="0">
</p>

---

## 🎯 Reconocimientos

**A Myper** por esta oportunidad técnica que me permitió demostrar mis capacidades en:
- Arquitectura MVC con ASP.NET Core
- Implementación de requerimientos complejos
- Soluciones eficientes con Entity Framework

*"Valoro el tiempo invertido en evaluar mi trabajo y espero haber cumplido con sus expectativas técnicas."*  

---

## 🧑‍💼 Autor
**Desarrollado por:** [Angel Vargas](https://angeldevportfolio.netlify.app/)
