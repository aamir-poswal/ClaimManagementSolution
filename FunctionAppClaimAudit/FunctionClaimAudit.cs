// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;

namespace FunctionAppClaimAudit
{
    public static class FunctionClaimAudit
    {

        [FunctionName("FunctionClaimAudit")]
        public static async System.Threading.Tasks.Task RunAsync([EventGridTrigger]EventGridEvent eventGridEvent, ILogger log)
        {
            try
            {
                log.LogInformation($"request received with subject {eventGridEvent.Subject}");

                var claimAudit = JsonConvert.DeserializeObject<ClaimAudit>(eventGridEvent.Data.ToString());

                log.LogInformation($"Claim Id {claimAudit.Id}");

                var sqlConnectionStringClaimAuditDB = @Environment.GetEnvironmentVariable("SQLConnectionStringClaimAuditDB", EnvironmentVariableTarget.Process);

                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStringClaimAuditDB))
                {
                    await sqlConnection.OpenAsync();
                    string sqlInsertClaimAuditQuery = "INSERT INTO ClaimAudit (ClaimId, Name, Year, DamageCost, Type, RequestName,CreatedAt) " + "VALUES (@ClaimId, @Name, @Year, @DamageCost, @Type, @RequestName,@CreatedAt) SELECT @Id = SCOPE_IDENTITY()";

                    using (SqlCommand sqlCommand = new SqlCommand(sqlInsertClaimAuditQuery, sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("ClaimId", claimAudit.Id);
                        sqlCommand.Parameters.AddWithValue("Name", claimAudit.Name);
                        sqlCommand.Parameters.AddWithValue("Year", claimAudit.Year);
                        sqlCommand.Parameters.AddWithValue("DamageCost", claimAudit.DamageCost);
                        sqlCommand.Parameters.AddWithValue("Type", claimAudit.Type);
                        sqlCommand.Parameters.AddWithValue("RequestName", claimAudit.RequestName);
                        sqlCommand.Parameters.AddWithValue("CreatedAt", claimAudit.CreatedAt);
                        sqlCommand.Parameters.Add("@Id", SqlDbType.Int).Direction = ParameterDirection.Output;

                        sqlCommand.ExecuteNonQuery();
                        
                        sqlConnection.Close();
                    }

                }

                log.LogInformation($"request completed with subject eventGridEvent.Subject {eventGridEvent.Subject}");
            }
            catch (Exception exception)
            {
                log.LogInformation($"exception occured for request with EventType {eventGridEvent.EventType} {exception.Message}");
                log.LogError(exception.ToString());
                throw exception;
            }
        }
    }
}
