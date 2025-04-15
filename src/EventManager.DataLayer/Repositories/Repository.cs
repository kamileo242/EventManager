using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManager.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Datalayer
{
  /// <summary>
  /// Abstrakcyjna klasa repozytorium implementująca generyczny interfejs repozytorium.
  /// </summary>
  /// <typeparam name="T">Typ encji, na której operuje repozytorium.</typeparam>
  public abstract class Repository<TModel, TDbo> : IRepository<TModel>
    where TModel : class, IEntity
    where TDbo : class
  {
    protected readonly EventManagerDbContext context;
    protected readonly DbSet<TDbo> dbSet;
    private readonly IDboConverter dboConverter;

    protected Repository(EventManagerDbContext context, IDboConverter dboConverter)
    {
      this.context = context ?? throw new ArgumentNullException(nameof(context));
      this.dboConverter = dboConverter ?? throw new ArgumentNullException(nameof(dboConverter));
      dbSet = context.Set<TDbo>();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TModel>> GetAllAsync()
    {
      var result = await dbSet.ToListAsync();

      return result.Select(s => dboConverter.Convert<TModel>(s)).ToList();
    }


    /// <inheritdoc />
    public async Task<TModel> GetByIdAsync(Guid id)
    {
      var entity = await dbSet.FindAsync(id);

      return entity != null
        ? dboConverter.Convert<TModel>(entity)
        : null;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TModel>> FindAsync(Expression<Func<TModel, bool>> predicate)
    {
      var convertedPredicate = dboConverter.ConvertExpression<TModel, TDbo>(predicate);

      var result = await dbSet.Where(convertedPredicate).ToListAsync();

      return result.Select(s => dboConverter.Convert<TModel>(s)).ToList();
    }


    /// <inheritdoc />
    public async Task AddAsync(TModel entity)
    {
      var result = dboConverter.Convert<TDbo>(entity);
      await dbSet.AddAsync(result);
      await context.SaveChangesAsync();

      await Task.CompletedTask;
    }

    /// <inheritdoc />
    public async Task UpdateAsync(TModel entity)
    {
      var existingEntity = await dbSet.FindAsync(entity.Id);

      var updatedEntity = dboConverter.Convert<TDbo>(entity);
      if (existingEntity != null)
      {
        context.Entry(existingEntity).CurrentValues.SetValues(updatedEntity);
        await context.SaveChangesAsync();
      }

      await Task.CompletedTask;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
      var entity = await dbSet.FindAsync(id);

      if (entity != null)
      {
        dbSet.Remove(entity);
        await context.SaveChangesAsync();
      }

      await Task.CompletedTask;
    }
  }
}
