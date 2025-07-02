using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Papeleria.Models;
using System.Diagnostics.Metrics;

namespace Papeleria.Pages
{
    public class IndexModel : PageModel
    {
        private readonly Papeleria.Data.AppDbContext _context;

        public IndexModel(Papeleria.Data.AppDbContext context)
        {
            _context = context;
        }

        public decimal TotalVentasMes { get; set; }
        public decimal TotalComprasMes { get; set; }
        public int ProductosVendidos { get; set; }
        public int ProductosBajoStock { get; set; }

        public List<string> Meses { get; set; } = new();
        public List<decimal> TotalesVentasMensuales { get; set; } = new();
        public List<decimal> TotalesComprasMensuales { get; set; } = new();

        public List<string> NombresProveedores { get; set; } = new();
        public List<decimal> TotalesPorProveedor { get; set; } = new();

        public List<string> NombresProductos { get; set; } = new();
        public List<int> CantidadesVendidas { get; set; } = new();

        public List<Producto> ProductosCriticos { get; set; } = new();

        public async Task OnGetAsync()
        {
            await CargarResumenMensual();
            await CargarGraficoMensual();
            await CargarComprasPorProveedor();
            await CargarProductosMasVendidos();
        }
        private async Task CargarResumenMensual()
        {
            var ahora = DateTime.Now;

            TotalVentasMes = await _context.Transaccion
                .Where(t => t.TipoTransaccionId == 1 &&
                            t.Fecha.Month == ahora.Month &&
                            t.Fecha.Year == ahora.Year)
                .SumAsync(t => t.Total);

            TotalComprasMes = await _context.Transaccion
                .Where(t => t.TipoTransaccionId == 2 &&
                            t.Fecha.Month == ahora.Month &&
                            t.Fecha.Year == ahora.Year)
                .SumAsync(t => t.Total);

            ProductosVendidos = await _context.DetalleTransaccion
                .Where(d => d.Venta.TipoTransaccionId == 1)
                .SumAsync(d => d.Cantidad);

            ProductosBajoStock = await _context.Producto
                .CountAsync(p => p.Stock <= 10);

            ProductosCriticos = await _context.Producto
                .Where(p => p.Stock <= 10)
                .OrderBy(p => p.Stock)
                .Take(6) // Opcional: solo los más urgentes
                .ToListAsync();
        }
        private async Task CargarGraficoMensual()
        {
            var ventas = await _context.Transaccion
                .Where(t => t.TipoTransaccionId == 1)
                .GroupBy(t => new { t.Fecha.Year, t.Fecha.Month })
                .Select(g => new { Mes = $"{g.Key.Month:00}/{g.Key.Year}", Total = g.Sum(x => x.Total) })
                .ToListAsync();

            var compras = await _context.Transaccion
                .Where(t => t.TipoTransaccionId == 2)
                .GroupBy(t => new { t.Fecha.Year, t.Fecha.Month })
                .Select(g => new { Mes = $"{g.Key.Month:00}/{g.Key.Year}", Total = g.Sum(x => x.Total) })
                .ToListAsync();

            var todosLosMeses = ventas.Select(v => v.Mes)
                .Union(compras.Select(c => c.Mes))
                .Distinct()
                .OrderBy(m => m)
                .ToList();

            Meses = todosLosMeses;
            TotalesVentasMensuales = todosLosMeses
                .Select(m => ventas.FirstOrDefault(v => v.Mes == m)?.Total ?? 0).ToList();
            TotalesComprasMensuales = todosLosMeses
                .Select(m => compras.FirstOrDefault(c => c.Mes == m)?.Total ?? 0).ToList();
        }
        private async Task CargarComprasPorProveedor()
        {
            var compras = _context.DetalleTransaccion
                .Where(d => d.Venta.TipoTransaccionId == 2 && d.Producto.Proveedor != null)
                .GroupBy(d => d.Producto.Proveedor.Nombre)
                .Select(g => new {
                    Nombre = g.Key,
                    Total = g.Sum(x => x.Cantidad * x.PrecioUnitario)
                })
                .AsEnumerable() // Fuerza la ejecución en memoria
                .OrderByDescending(x => x.Total)
                .ToList();

            NombresProveedores = compras.Select(p => p.Nombre).ToList();
            TotalesPorProveedor = compras.Select(p => p.Total).ToList();
        }
        private async Task CargarProductosMasVendidos()
        {
            var topProductos = await _context.DetalleTransaccion
                .Where(d => d.Venta.TipoTransaccionId == 1)
                .GroupBy(d => d.Producto.Nombre)
                .Select(g => new {
                    Nombre = g.Key,
                    Cantidad = g.Sum(x => x.Cantidad)
                })
                .OrderByDescending(x => x.Cantidad)
                .Take(6)
                .ToListAsync();

            NombresProductos = topProductos.Select(p => p.Nombre).ToList();
            CantidadesVendidas = topProductos.Select(p => p.Cantidad).ToList();
        }
    }
}
