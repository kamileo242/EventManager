using System;
using System.Linq.Expressions;
using AutoMapper;
using EventManager.Datalayer;
using EventManager.Datalayer.Dbos;
using EventManager.Models;

public class DboConverter : IDboConverter
{
  private readonly IMapper mapper;

  public DboConverter()
  {
    mapper = CreateMapper();
  }

  public TResult Convert<TResult>(object source)
  {
    return mapper.Map<TResult>(source);
  }

  public Expression<Func<TDestination, bool>> ConvertExpression<TSource, TDestination>(Expression<Func<TSource, bool>> expression)
  {
    var converter = new ExpressionConverter<TSource, TDestination>();
    return converter.Convert(expression);
  }

  private IMapper CreateMapper()
  {
    var configuration = new MapperConfiguration(cfg =>
    {
      cfg.ClearPrefixes();
      cfg.AllowNullCollections = true;

      cfg.CreateMap<User, UserDbo>().ReverseMap();
      cfg.CreateMap<Address, AddressDbo>().ReverseMap();
      cfg.CreateMap<Event, EventDbo>().ReverseMap();
      cfg.CreateMap<UserEvent, UserEventDbo>().ReverseMap();
    });

    return configuration.CreateMapper();
  }
}
