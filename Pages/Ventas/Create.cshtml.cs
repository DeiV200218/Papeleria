using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Papeleria.Data;
using Papeleria.Models;
using Papeleria.ViewModels;

namespace Papeleria.Pages.Ventas
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;

        public CreateModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public CrearTransaccionViewModel VentaVM { get; set; } = new();

        public List<Producto> ProductosDisponibles { get; set; } = new();

        public async Task OnGetAsync()
        {
            ProductosDisponibles = await _context.Producto.ToListAsync();

            if (VentaVM.Detalles == null || !VentaVM.Detalles.Any())
            {
                VentaVM.Detalles.Add(new DetalleVentaInput());
            }
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var transaccion = new Transaccion
            {
                Fecha = VentaVM.Fecha,
                Total = VentaVM.Total,
                Detalles = VentaVM.Detalles.Select(d => new DetalleTransaccion
                {
                    ProductoId = d.ProductoId,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario
                }).ToList(),
                TipoTransaccionId = 1
            };

            foreach (var item in transaccion.Detalles)
            {
                var producto = await _context.Producto.FindAsync(item.ProductoId);

                if (producto == null)
                {
                    ModelState.AddModelError(string.Empty, $"El producto con ID {item.ProductoId} no existe.");
                    continue;
                }

                if (item.Cantidad > producto.Stock)
                {
                    ModelState.AddModelError(string.Empty, $"Stock insuficiente para '{producto.Nombre}'. Disponible: {producto.Stock}, solicitado: {item.Cantidad}.");
                }
            }


            foreach (var item in transaccion.Detalles)
            {
                var producto = await _context.Producto.FindAsync(item.ProductoId);

                if (transaccion.TipoTransaccionId == 1)
                {
                    producto.Stock -= item.Cantidad;
                }
            }

            _context.Transaccion.Add(transaccion);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}
