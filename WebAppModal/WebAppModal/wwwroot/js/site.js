// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$(document).ready(function () {
    // Manejar clic en botón editar
    $(document).on('click', '.btn-edit', function () {
        var id = $(this).data('id');

        $('#editModal01 .modal-body').html(`
            <div class="text-center py-4">
                <div class="spinner-border text-primary" role="status">
                    <span class="visually-hidden">Cargando...</span>
                </div>
            </div>
        `);

        $('#editModal01').modal('show');

        $.get(`/Trabajadores/Edit/${id}`, function (data) {
            $('#editModal01 .modal-body').html(data);
            $('#editModal01 form').attr('action', `/Trabajadores/Edit/${id}`);
            $.validator.unobtrusive.parse('#editModal01 form');
            inicializarSelect2EnModal('#editModal01');  // <- aqui se utiliza la funcion del select2
            $('#editModal01').modal('show');
        }).fail(function () {
            $('#editModal01 .modal-body').html(`
                <div class="alert alert-danger">
                    Error al cargar los datos del trabajador
                </div>
            `);
        });
    });


    // Manejar clic en botón Crear
    $(document).on('click', '[data-bs-target="#createModal01"]', function () {
        setTimeout(() => {
            inicializarSelect2EnModal('#createModal01');
        }, 200); 
    });


    // Manejar clic en boton Eliminar
    $(document).on('click', '.btn-delete', function () {
        const id = $(this).data('id');

        // Configurar la acción del formulario
        $('#deleteModal form').attr('action', '/Trabajadores/Delete/' + id);

        // Obtener y mostrar datos directamente desde la tabla
        const row = $(this).closest('tr');
        const nombre = row.find('td:eq(2)').text(); // Columna de nombres
        const documento = row.find('td:eq(1)').text(); // Columna de documento
        const ubicacion = row.find('td:eq(4)').text() + ' / ' +
            row.find('td:eq(5)').text() + ' / ' +
            row.find('td:eq(6)').text();

        // Llenar el modal con los datos
        $('#deleteModal .modal-body').html(`
        <p>¿Está seguro que desea eliminar al trabajador?</p>
        
        <div class="d-flex align-items-center mb-3">
            <div class="flex-grow-1 ms-3">
                <h6 class="mb-1">${nombre}</h6>
                <p class="mb-1 text-muted">${documento}</p>
                <p class="mb-0 text-muted small">${ubicacion}</p>
            </div>
        </div>
        
        <div class="alert alert-warning mb-0">
            <i class="fas fa-exclamation-triangle me-2"></i>Acción irreversible
        </div>
    `);
        // Mostrar el modal
        $('#deleteModal').modal('show');
    });

    // Dropdowns dependientes (provincias y distritos)
    $(document).on('change', '#IdDepartamento', function () {
        var departamentoId = $(this).val();
        var $provincia = $(this).closest('form').find('#IdProvincia');
        var $distrito = $(this).closest('form').find('#IdDistrito');

        $provincia.empty().append('<option value="">-- Cargando... --</option>');
        $distrito.empty().append('<option value="">-- Seleccione provincia --</option>');

        if (departamentoId) {
            $.getJSON('/Trabajadores/GetProvinciasByDepartamento', { departamentoId: departamentoId }, function (data) {
                $provincia.empty().append('<option value="">-- Seleccione --</option>');
                $.each(data, function (index, item) {
                    $provincia.append(new Option(item.nombreProvincia, item.id));
                });
            });
        }
    });

    $(document).on('change', '#IdProvincia', function () {
        var provinciaId = $(this).val();
        var $distrito = $(this).closest('form').find('#IdDistrito');

        $distrito.empty().append('<option value="">-- Cargando... --</option>');

        if (provinciaId) {
            $.getJSON('/Trabajadores/GetDistritosByProvincia', { provinciaId: provinciaId }, function (data) {
                $distrito.empty().append('<option value="">-- Seleccione --</option>');
                $.each(data, function (index, item) {
                    $distrito.append(new Option(item.nombreDistrito, item.id));
                });
            });
        }
    });

    // Cerrar alertas automáticamente
    setTimeout(function () {
        $('#successAlert').alert('close');
    }, 3000);

    // Inicializar Select2
    $('.select2').select2();
    $(document).on('select2:open', () => {
        setTimeout(() => {
            document.querySelector('.select2-search__field').focus();
        }, 0);
    });

    function inicializarSelect2EnModal(modalId) {
        $(`${modalId} .select2`).select2({
            dropdownParent: $(modalId),
            width: '100%'
        });
    }


    // Carga de la tabla en ajax
    $(document).ready(function () {
        $('#sexoFiltro').on('change', function () {
            var sexo = $(this).val();
            $.ajax({
                url: '/Trabajadores/Index',
                type: 'GET',
                data: { sexoFiltro: sexo },
                success: function (data) {
                    $('#tabla-trabajadores tbody').html(data);
                },
                error: function () {
                    alert('Error al filtrar trabajadores.');
                }
            });
        });
    });


    // Al cerrar el modal de crear, limpia todos los campos
    $('#createModal01').on('hidden.bs.modal', function () {
        const $form = $(this).find('form');

        $form[0].reset(); // limpia los inputs
        $form.find('.select2').val(null).trigger('change'); // limpia selects select2
        $form.find('.text-danger').text(''); // limpia errores de validación
    });

});