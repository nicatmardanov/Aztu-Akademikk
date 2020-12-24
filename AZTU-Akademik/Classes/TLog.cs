using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AZTU_Akademik.Models;

namespace AZTU_Akademik.Classes
{
    public class TLog
    {
        public static async Task Log(string tableName, string descp, long refId, byte operation, int user_id, string IpAdress, string additionalInformation)
        {
            using AztuAkademikContext aztuAkademik = new AztuAkademikContext();
            Log _log = new Log
            {
                TableName = tableName,
                IpAddress = IpAdress,
                Description = descp,
                AdditionalInformation = additionalInformation,
                CreateDate = DateTime.UtcNow.AddHours(4),
                RefId = refId,
                UserId = user_id,
                OperationId = operation
            };

            await aztuAkademik.Log.AddAsync(_log).ConfigureAwait(false);
            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
        }


        public static async Task Log(string tableName, string descp, int[] refIds, byte operation, int user_id, string IpAdress, string additionalInformation)
        {
            using AztuAkademikContext aztuAkademik = new AztuAkademikContext();

            Log _log;
            for (int i = 0; i < refIds.Count(); i++)
            {
                _log = new Log
                {
                    TableName = tableName,
                    IpAddress = IpAdress,
                    Description = descp,
                    AdditionalInformation = additionalInformation,
                    CreateDate = DateTime.UtcNow.AddHours(4),
                    RefId = refIds[i],
                    UserId = user_id,
                    OperationId = operation
                };

                await aztuAkademik.Log.AddAsync(_log).ConfigureAwait(false);
                await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
            }

        }

        public static async Task Log(string tableName, string descp, long[] refIds, byte operation, int user_id, string IpAdress, string additionalInformation)
        {
            using AztuAkademikContext aztuAkademik = new AztuAkademikContext();

            Log _log;
            for (int i = 0; i < refIds.Count(); i++)
            {
                _log = new Log
                {
                    TableName = tableName,
                    IpAddress = IpAdress,
                    Description = descp,
                    AdditionalInformation = additionalInformation,
                    CreateDate = DateTime.UtcNow.AddHours(4),
                    RefId = refIds[i],
                    UserId = user_id,
                    OperationId = operation
                };

                await aztuAkademik.Log.AddAsync(_log).ConfigureAwait(false);
                await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
            }

        }


    }
}
