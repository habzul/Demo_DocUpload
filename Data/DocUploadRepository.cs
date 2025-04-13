using Dapper;
using eRehat.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
namespace Demo_DocUpload.Data
{
    public class DocUploadRepository
    {
        private readonly string _connectionString;

        public DocUploadRepository(string connectionString)
        {
            _connectionString = connectionString;

        }

        public async Task<IEnumerable<DocUpload>> GetAllDocumentsAsync(int book_id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@book_id", book_id, DbType.Int32);
                // Call the stored procedure
                var sql = "GET_DOC_UPLOAD"; // Stored procedure name
                return await connection.QueryAsync<DocUpload>(sql, parameters, commandType: CommandType.StoredProcedure, commandTimeout: 300);
            }
        }

        public async Task<bool> UpdateDocumentPathAsync(int book_id, string doc_id, string filePath)
        {
            using (var connection = new SqlConnection(_connectionString))
            {

                var parameters = new DynamicParameters();
                parameters.Add("@book_id", book_id, DbType.Int32);
                parameters.Add("@doc_id", doc_id, DbType.String);
                parameters.Add("@filePath", filePath, DbType.String);
                var result = await connection.ExecuteAsync("ADD_DOC_UPLOAD", parameters,
                    commandType: CommandType.StoredProcedure);
                return result > 0;

            }
        }
    }
}
