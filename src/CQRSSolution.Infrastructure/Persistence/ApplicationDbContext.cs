using CQRSSolution.Application.Interfaces;
using CQRSSolution.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace CQRSSolution.Infrastructure.Persistence;

/// <summary>
///     Represents the application's database context, implementing <see cref="IApplicationDbContext" />.
/// </summary>
public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ApplicationDbContext" /> class.
    /// </summary>
    /// <param name="options">The options to be used by a DbContext.</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    /// <inheritdoc />
    public DbSet<Order> Orders { get; set; } = null!;

    /// <inheritdoc />
    public DbSet<OrderItem> OrderItems { get; set; } = null!;

    /// <inheritdoc />
    public DbSet<OutboxMessage> OutboxMessages { get; set; } = null!;

    /// <inheritdoc />
    public DbSet<Customer> Customers { get; set; }

    /// <summary>
    ///     Configures the schema needed for the identity framework.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply all entity configurations from the current assembly
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    /// <summary>
    /// Override of SaveChangesAsync to potentially dispatch domain events if MediatR is used for that.
    /// For this project, domain events are primarily for creating OutboxMessages within the same transaction,
    /// so direct event dispatching here might be redundant if not carefully designed.
    /// </summary>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Add any custom logic before saving, e.g., updating audit properties
        // var result = await base.SaveChangesAsync(cancellationToken);
        // Dispatch Domain Events if any (after successfully saving to DB and before committing transaction)
        // await _mediator.DispatchDomainEventsAsync(this);
        return await base.SaveChangesAsync(cancellationToken);
    }
}