using PollyDapperRetryPolicy.Extensions;
using PollyDapperRetryPolicy.Models;

namespace PollyDapperRetryPolicy.Data;

public class ProductRepository : DbSession
{
    public async Task<Produto> GetById(int productId)
    {
        return (await Connection.QueryAsyncWithRetry<Produto>
            (
                sql: "SELECT ProductId, Name, ProductNumber, Color FROM Production.Product WHERE ProductId = @ProductId", 
                param: new {productId}
            )).FirstOrDefault();
    }
}