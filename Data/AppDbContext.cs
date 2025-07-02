using Microsoft.EntityFrameworkCore;
using Papeleria.Models;

namespace Papeleria.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Producto> Producto { get; set; }
        public DbSet<Transaccion> Transaccion { get; set; }
        public DbSet<DetalleTransaccion> DetalleTransaccion { get; set; }
        public DbSet<Proveedor> Proveedor { get; set; } = default!;
        public DbSet<TipoTransaccion> TipoTransaccion { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relación DetalleTransaccion → Producto (clave foránea)
            modelBuilder.Entity<DetalleTransaccion>()
                .HasOne(d => d.Producto)
                .WithMany() // o .WithMany(p => p.Detalles) si agregas la colección en Producto
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.Restrict); // evita que se borre un producto en uso

            // Relación DetalleTransaccion → Venta (clave foránea)
            modelBuilder.Entity<DetalleTransaccion>()
                .HasOne(d => d.Venta)
                .WithMany(v => v.Detalles)
                .HasForeignKey(d => d.VentaId)
                .OnDelete(DeleteBehavior.Cascade); // borra los detalles si se borra la venta

            // Relación Producto → Proveedor
            modelBuilder.Entity<Producto>()
                .HasOne(p => p.Proveedor)
                .WithMany(p => p.Productos)
                .HasForeignKey(p => p.ProveedorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TipoTransaccion>().HasData(
                new TipoTransaccion { Id = 1, Nombre = "Venta" }
            );
            modelBuilder.Entity<TipoTransaccion>().HasData(
               new TipoTransaccion { Id = 2, Nombre = "Compra" }
           );

            // Relación Venta → TipoTrasaccion (clave foránea)
            modelBuilder.Entity<Transaccion>()
                .HasOne(d => d.TipoTransaccion)
                .WithMany(d => d.Transacciones)
                .HasForeignKey(d => d.TipoTransaccionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
