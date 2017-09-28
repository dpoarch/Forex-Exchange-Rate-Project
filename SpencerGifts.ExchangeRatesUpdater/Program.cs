using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using SpencerGifts.ExchangeRatesUpdater.DAL;

namespace SpencerGifts.ExchangeRatesUpdater
{
   class Program
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
     

        static int Main(string[] args)
        {
           
            string exchangeRate = string.Empty;
            string dateUpdated = string.Empty;

            //Prepare parameters
            try
            {

                foreach (string arg in args)
                {

                    int commandIndex = arg.IndexOf(":");
                    string command = arg.Substring(0, commandIndex).ToUpper();

                    switch (command)
                    {
                        case "EXCHANGERATE":
                            exchangeRate = arg.Substring(commandIndex + 1, arg.Length - commandIndex - 1);
                            break;
                        case "DATEUPDATED":
                            dateUpdated = arg.Substring(commandIndex + 1, arg.Length - commandIndex - 1);
                            break;
                        default:
                            // do other stuff...
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Debug(ex);
                _logger.ErrorException("[" + DateTime.Now.ToString() + "]", ex);
            }

             
               
            
            //Start Updating
            try
            {

                _logger.Info("Exchange Rate Started");

                var connectionManagerDataSection = ConfigurationManager.GetSection(ConnectionManagerDataSection.SectionName) as ConnectionManagerDataSection;
                if (connectionManagerDataSection != null)
                {
                    foreach (ConnectionManagerEndpointElement endpointElement in connectionManagerDataSection.ConnectionManagerEndpoints)
                    {
                       if (endpointElement.ExecuteQuery)
                        {
                            DBHelper _dbHelper = new DBHelper(endpointElement.Name);    
                            DateTime oDate = Convert.ToDateTime(dateUpdated);
                            DBParameter param1 = new DBParameter(endpointElement.RateParam, exchangeRate);
                            DBParameter param2 = new DBParameter(endpointElement.DateParam, oDate.ToString(endpointElement.DateFormat));

                            DBParameterCollection paramCollection = new DBParameterCollection();
                            IDbTransaction transaction = _dbHelper.BeginTransaction();
                            paramCollection.Add(param1);
                            paramCollection.Add(param2);
                            string message = "";
                            string updateCommand = endpointElement.Query;
                            try
                            {
                                message = _dbHelper.ExecuteNonQuery(updateCommand, paramCollection, transaction) > 0 ? "Exchange Rate updated successfully." : "Error in updating record.";
                                _dbHelper.CommitTransaction(transaction);
                                _logger.Info(endpointElement.Name + "Database Updated Successfully.");
                            }
                            catch (Exception err)
                            {
                                _dbHelper.RollbackTransaction(transaction);
                            }

                        }

                    }
                }

                _logger.Info("Exchange Rate Ended");

                return 1;
            }
            catch (Exception ex)
            {
                _logger.Info("Exchange Rate Stoped");
                _logger.ErrorException("Exception: ", ex);
                return 0;
            }


        }
    }
}
