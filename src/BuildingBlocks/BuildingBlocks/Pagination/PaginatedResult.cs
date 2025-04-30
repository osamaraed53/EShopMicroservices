using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BuildingBlocks.Pagination;

public record PaginatedResult<TEntity>(int PageIndex,int PageSize , long Count , IEnumerable<TEntity> Data) where TEntity : class 
{
    public int PageIndex { get; } = PageIndex;
    public int PageSize { get; } = PageSize;
    public long Count { get; } = Count;
    public IEnumerable<TEntity> Data { get; } = Data;
}

