using System.Data;
using Dapper;

/// <summary>
/// Custom Dapper type handler that maps between <see cref="DateOnly"/> and database date values.
/// </summary>
public class DateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
{
    /// <summary>
    /// Parses a database value into a <see cref="DateOnly"/> instance.
    /// </summary>
    /// <param name="value">The database value to parse.</param>
    /// <returns>A <see cref="DateOnly"/> representation of the value.</returns>
    public override DateOnly Parse(object value)
    {
        return DateOnly.FromDateTime((DateTime)value);
    }

    /// <summary>
    /// Sets the value of a database parameter from a <see cref="DateOnly"/> instance.
    /// </summary>
    /// <param name="parameter">The database parameter to set.</param>
    /// <param name="value">The <see cref="DateOnly"/> value to assign.</param>
    public override void SetValue(IDbDataParameter parameter, DateOnly value)
    {
        parameter.DbType = DbType.Date;
        parameter.Value = value.ToDateTime(TimeOnly.MinValue);
    }
}
