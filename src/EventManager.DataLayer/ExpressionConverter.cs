using System;
using System.Linq.Expressions;

public class ExpressionConverter<TSource, TDestination> : ExpressionVisitor
{
  private readonly ParameterExpression parameter;

  public ExpressionConverter()
  {
    parameter = Expression.Parameter(typeof(TDestination), "dest");
  }

  public Expression<Func<TDestination, bool>> Convert(Expression<Func<TSource, bool>> expression)
  {
    var body = Visit(expression.Body);
    return Expression.Lambda<Func<TDestination, bool>>(body, parameter);
  }

  protected override Expression VisitMember(MemberExpression node)
  {
    if (node.Expression is ParameterExpression)
    {
      var sourceMember = node.Member;
      var destinationMember = typeof(TDestination).GetProperty(sourceMember.Name);
      if (destinationMember != null)
      {
        return Expression.Property(parameter, destinationMember);
      }
    }
    return base.VisitMember(node);
  }

  protected override Expression VisitParameter(ParameterExpression node)
  {
    return parameter;
  }
}
