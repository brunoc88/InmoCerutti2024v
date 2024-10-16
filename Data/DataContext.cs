using Microsoft.EntityFrameworkCore;

public class DataContext : DbContext{
    public DataContext(DbContextOptions<DataContext> options): base(options){

    }
     // Define los DbSet para tus entidades. Aquí un ejemplo:
        public DbSet<Propietario> Propietario { get; set; }
        public DbSet<Inmueble> inmuebles {get;set;}
        public DbSet<TipoInmueble> tipoInmuebles {get;set;}
        // Agrega más DbSet para otras entidades según tu modelo de datos
}