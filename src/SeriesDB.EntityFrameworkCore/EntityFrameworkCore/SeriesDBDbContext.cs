using Microsoft.EntityFrameworkCore;
using SeriesDB.ListasDeSeguimiento;
using SeriesDB.Series;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;

namespace SeriesDB.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ConnectionStringName("Default")]
public class SeriesDBDbContext :
    AbpDbContext<SeriesDBDbContext>,
    IIdentityDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */

    public DbSet<Serie> Series { get; set; }
    public DbSet<Temporada> Temporadas { get; set; }
    public DbSet<Episodio> Episodios { get; set; }
    public DbSet<ListaDeSeguimiento> ListasDeSeguimiento { get; set; }
    #region Entities from the modules

    /* Notice: We only implemented IIdentityProDbContext 
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityProDbContext .
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    // Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }
    public DbSet<IdentitySession> Sessions { get; set; }

    #endregion

    public SeriesDBDbContext(DbContextOptions<SeriesDBDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureFeatureManagement();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureBlobStoring();
        
        /* Configure your own tables/entities inside here */

        //builder.Entity<YourEntity>(b =>
        //{
        //    b.ToTable(SeriesDBConsts.DbTablePrefix + "YourEntities", SeriesDBConsts.DbSchema);
        //    b.ConfigureByConvention(); //auto configure for the base class props
        //    //...
        //});

        builder.Entity<Serie>(b =>
        {
            b.ToTable(SeriesDBConsts.DbTablePrefix + "Series", SeriesDBConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.Property(x => x.Titulo).IsRequired().HasMaxLength(128);
            b.Property(x => x.Generos).IsRequired().HasMaxLength(128);
            b.Property(x => x.Sinopsis).IsRequired().HasMaxLength(300);
            b.Property(x => x.FechaEstreno).IsRequired().HasMaxLength(128);
            b.Property(x => x.Duracion).IsRequired().HasMaxLength(128);
            b.Property(x => x.Clasificacion).IsRequired().HasMaxLength(128);
            b.Property(x => x.Idiomas).IsRequired().HasMaxLength(128);
            b.Property(x => x.Directores).IsRequired().HasMaxLength(128);
            b.Property(x => x.Escritores).IsRequired().HasMaxLength(128);
            b.Property(x => x.Actores).IsRequired().HasMaxLength(128);
            b.Property(x => x.Poster).IsRequired().HasMaxLength(128);
            b.Property(x => x.Pais).IsRequired().HasMaxLength(128);
            b.Property(x => x.ImdbId).IsRequired().HasMaxLength(128);
            b.Property(x => x.ImdbCalificacion).IsRequired().HasMaxLength(128);
            b.Property(x => x.ImdbVotos).IsRequired(); // No HasMaxLength for int
            b.Property(x => x.Tipo).IsRequired().HasMaxLength(128);
            b.Property(x => x.TotalTemporadas).IsRequired(); // No HasMaxLength for int

            // Relación con Temporadas
            b.HasMany(s => s.Temporadas)
             .WithOne(t => t.Serie)
             .HasForeignKey(t => t.SerieID)
             .OnDelete(DeleteBehavior.Cascade)
             .IsRequired();
        });

        builder.Entity<Temporada>(b =>
        {
            b.ToTable(SeriesDBConsts.DbTablePrefix + "Temporadas",
                SeriesDBConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.Property(x => x.Titulo).IsRequired().HasMaxLength(128);
            b.Property(x => x.FechaLanzamiento).IsRequired().HasMaxLength(128);
            b.Property(x => x.NroTemporada).IsRequired();

            // Relación con Serie
            b.HasOne(t => t.Serie)
             .WithMany(s => s.Temporadas)
             .HasForeignKey(t => t.SerieID)
             .OnDelete(DeleteBehavior.Cascade)
             .IsRequired();

            // Relación con Episodios
            b.HasMany(t => t.Episodios)
             .WithOne(e => e.Temporada)
             .HasForeignKey(e => e.TemporadaID)
             .OnDelete(DeleteBehavior.Cascade)
             .IsRequired();
        });

        builder.Entity<Episodio>(b =>
        {
            b.ToTable(SeriesDBConsts.DbTablePrefix + "Episodios",
                SeriesDBConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.Property(x => x.NroEpisodio).IsRequired();
            b.Property(x => x.Titulo).IsRequired().HasMaxLength(128);
            b.Property(x => x.Duracion).IsRequired().HasMaxLength(128);
            b.Property(x => x.Resumen).IsRequired().HasMaxLength(128);
            b.Property(x => x.FechaEstreno).IsRequired();
            b.Property(x => x.Directores).IsRequired().HasMaxLength(128);
            b.Property(x => x.Escritores).IsRequired().HasMaxLength(128);

            // Relación con Temporada
            b.HasOne(e => e.Temporada)
             .WithMany(t => t.Episodios)
             .HasForeignKey(e => e.TemporadaID)
             .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<ListaDeSeguimiento>(b =>
        {
            b.ToTable(SeriesDBConsts.DbTablePrefix + "ListasDeSeguimiento",
                SeriesDBConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.Property(x => x.FechaModificacion).IsRequired();
             
            // Relación con Serie
            b.HasMany(ls => ls.Series)
             .WithOne();

            // Relación con el Usuario (IdentityUser)
            b.HasOne<IdentityUser>()
             .WithMany()
             .HasForeignKey(u => u.UsuarioId)
             .OnDelete(DeleteBehavior.Cascade)
             .IsRequired();
        });

    }
}
