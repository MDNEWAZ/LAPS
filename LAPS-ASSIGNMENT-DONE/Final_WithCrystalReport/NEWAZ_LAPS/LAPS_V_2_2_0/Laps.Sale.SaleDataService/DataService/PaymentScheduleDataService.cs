using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.Sale;
using Utilities;

namespace Laps.Sale.SaleDataService.DataService
{
    public class PaymentScheduleDataService
    {
        public string SaveInstallment(List<Installment> objInstallmentList, int saleId, string invoice, int saleType, CommonConnection connection)
        {
            var res = "";
            try
            {
                var qBuilder = new StringBuilder();
                var entryDate = DateTime.Now.ToString("dd-MMM-yyyy");
                if (objInstallmentList != null && objInstallmentList.Count > 0)
                {
                    //if (saleType == 1)
                    //{

                    SaveInstallmentIntoTemp(invoice, connection);


                    var newMargedInstallments = MargeInstallments(objInstallmentList, invoice);

                    var installmentsForInsert = newMargedInstallments.Where(i => i.InstallmentId == 0);
                    foreach (var installment in installmentsForInsert)
                    {
                        DateTime adate = Convert.ToDateTime(installment.DueDate);
                        var dueDate = adate.ToString("dd-MMM-yyyy");
                        var query =
                            string.Format(@"Insert Into Sale_Installment([SInvoice],[ProductNo],[Number],[Amount],[Status],[DueDate],[EntryDate],[Updated],[Flag])
                                   Values('{0}','{1}',{2},{3},{4},'{5}','{6}','{7}',{8});", invoice, "",
                                installment.Number, installment.Amount, installment.Status, dueDate, entryDate, "",
                                installment.Flag);
                        qBuilder.Append(query);

                    }

                    var installmentsForUpdate = newMargedInstallments.Where(i => i.InstallmentId > 0);
                    foreach (var installment in installmentsForUpdate)
                    {
                        DateTime adate = Convert.ToDateTime(installment.DueDate);
                        var dueDate = adate.ToString("dd-MMM-yyyy");
                        var query =
                            string.Format(
                                @"Update Sale_Installment set [Amount]={0},[DueDate]='{1}' where InstallmentId={2}", installment.Amount, dueDate, installment.InstallmentId);
                        qBuilder.Append(query);

                    }

                    //                    }
                    //                    else
                    //                    {
                    //                        foreach (var installment in objInstallmentList)
                    //                        {
                    //                            DateTime adate = Convert.ToDateTime(installment.DueDate);
                    //                            var dueDate = adate.ToString("dd-MMM-yyyy");
                    //                            var query =
                    //                                string.Format(@"Insert Into Sale_Installment([SInvoice],[ProductNo],[Number],[Amount],[Status],[DueDate],[EntryDate],[Updated],[Flag])
                    //                                   Values('{0}','{1}',{2},{3},{4},'{5}','{6}','{7}',{8});", invoice, "",
                    //                                    installment.Number, installment.Amount, installment.Status, dueDate, entryDate, "",
                    //                                    installment.Flag);
                    //                            qBuilder.Append(query);

                    //                        }
                    //                    }



                    if (qBuilder.ToString() != "")
                    {
                        var sql = "Begin " + qBuilder + " End;";
                        connection.ExecuteNonQuery(sql);
                        res = Operation.Success.ToString();
                    }

                }
            }
            catch (Exception)
            {
                throw new Exception("Error! During Saving Installment");
            }
            return res;
        }

        private static void SaveInstallmentIntoTemp(string invoice, CommonConnection connection)
        {
            string deleteQuery = string.Format(@"Delete From Sale_Installment_Temp Where SInvoice='{0}'", invoice);
            connection.ExecuteNonQuery(deleteQuery);

            string insertTempInstallment =
                string.Format(@"INSERT INTO Sale_Installment_Temp( InstallmentId,[SInvoice] ,[ProductNo],[Number],[Amount],[Status],[DueDate],[EntryDate],[Updated],[Flag])
                    Select  InstallmentId, [SInvoice],[ProductNo],[Number],[Amount],[Status],
                    [DueDate],[EntryDate],[Updated],[Flag] From Sale_Installment Where SInvoice='{0}'", invoice);
            connection.ExecuteNonQuery(insertTempInstallment);
        }

        private List<Installment> MargeInstallments(List<Installment> objInstallmentList, string invoice)
        {
            var newMargedInstallments = new List<Installment>();
            string query = string.Format(@"select * from Sale_Installment where SInvoice='{0}'", invoice);
            var data = Data<Installment>.DataSource(query);
            if (data.Count == 0)return objInstallmentList;
            var unpaidInstallment = data.Where(i => i.Status != 1).ToList();
            var partialPaidInstallment = data.Where(i => i.Status == 2);
            
            if (unpaidInstallment.Count() <= objInstallmentList.Count)
            {
                int insNo = 2;
                int first = 0;
                foreach (var installment in unpaidInstallment)
                {
                    if (first > 0)
                    {
                        installment.Amount = installment.Amount + objInstallmentList[insNo - 2].Amount;
                        installment.DueDate = objInstallmentList[insNo - 2].DueDate;
                        installment.EntryDate = objInstallmentList[insNo - 2].EntryDate;
                        insNo = insNo + 1;
                    }
                    else
                    {
                        first++;

                    }
                    newMargedInstallments.Add(installment);
                }
                for (int i = insNo-1; i <= objInstallmentList.Count; i++)
                {
                    objInstallmentList[i - 1].Number = i+1;
                    newMargedInstallments.Add(objInstallmentList[i - 1]);
                }
            }
            else
            {
                int insNo = 2;
                int first = 0;
                foreach (var installment in unpaidInstallment)
                {
                    if (first > 0)
                    {
                        if (insNo > objInstallmentList.Count+1) break;
                        installment.Amount = installment.Amount + objInstallmentList[insNo - 2].Amount;
                        installment.DueDate = objInstallmentList[insNo - 2].DueDate;
                        installment.EntryDate = objInstallmentList[insNo - 2].EntryDate;
                        insNo = insNo + 1;
                    }
                    else
                    {
                        first++;
                    }

                    newMargedInstallments.Add(installment);
                }
                for (int i = insNo; i <= unpaidInstallment.Count(); i++)
                {
                    unpaidInstallment[i - 1].Number = i;
                    newMargedInstallments.Add(unpaidInstallment[i - 1]);
                }
            }
            return newMargedInstallments;

        }
    }
}
