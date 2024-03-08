using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Storage.Infrastructure.DAL
{
    internal sealed class StorageDbContext
    {
        private readonly IConfiguration _configuration;

        public StorageDbContext(IConfiguration configuration) 
        {
            _configuration = configuration;
        }

        public IDbConnection CreateConnection()
        {
            // TODO: Move checking to different place, now I don't know where
            var dbDirectory = _configuration.GetConnectionString("dbDirectory");
            if (!Directory.Exists(dbDirectory))
            {
                Directory.CreateDirectory(dbDirectory);
            }
            return new SqliteConnection(_configuration.GetConnectionString("SQLite"));
        }

        public async Task InitializeDatabase()
        {
            var productCreateDbQuery = """
                CREATE TABLE IF NOT EXISTS PRODUCT
                (
                    ID            integer
                        constraint PRODUCT_pk
                            unique
                                on conflict replace,
                    SKU           TEXT,
                    NAME          TEXT,
                    EAN           TEXT,
                    PRODUCER_NAME TEXT,
                    CATEGORY      integer,
                    IS_WIRE       integer,
                    SHIPPING_TIME TEXT,
                    AVAILABLE     integer,
                    IS_VENDOR     integer,
                    DEFAULT_IMAGE TEXT
                );
                """;
            var inventoryCreateDbQuery = """
                CREATE TABLE IF NOT EXISTS INVENTORY
                (
                    PRODUCT_ID    integer,
                    STOCK_QTY     integer,
                    SALE_UNIT     TEXT,
                    SHIPPING_COST REAL
                );
                """;
            var priceCreateDbQuery = """
                CREATE TABLE IF NOT EXISTS PRICE
                (
                    PRODUCT_ID                 integer,
                    NET_PRICE_FOR_UNIT_OF_SALE REAL
                );
                """;

            using (var connection = CreateConnection())
            {
                await connection.ExecuteAsync(productCreateDbQuery);
                await connection.ExecuteAsync(inventoryCreateDbQuery);
                await connection.ExecuteAsync(priceCreateDbQuery);
            }
        }
    }
}
