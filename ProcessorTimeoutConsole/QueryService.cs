using System;

namespace ProcessorTimeoutConsole
{
    public class QueryService
    {
        public string CreateQueryTextForBrokenQuery()
        {
            DateTime lastCompleted = new DateTime(2018, 02, 01); //Works            
            //DateTime lastCompleted = new DateTime(1900, 01, 01); //Works
            //lastCompleted = lastCompleted.AddMinutes(-10);
            //DateTime? lastCompleted = null; //Works
            //DateTime lastCompleted = new DateTime(); //Breaks


            return "SELECT " +
                            "Jobs.JobRID, " +
                            "left(Jobs.JobID, 12) as SiteNumber,  " +
                            "Left(Jobs.JobID,5)+'/'+Substring(Jobs.JobID,6,3)+'/'+Substring(Jobs.JobID,9,4) as NewJobNumber, " +
                            "Acts.ActID as activity, " +
                            "Acts.Name AS Name, " +
                            "coalesce(Vnds.VndID, 'YNH00') as Vendor_ID, " +
                            "iif(coalesce(JobPOs.Status, '') in ('',NULL, 'Hold'),'H', 'Y') as po_type,  " +
                            "JobPOs.DateReleased as Release_Date, " +
                            "JobPOs.DatePaid as Payment_Date, " +
                            "JobPOs.DateCancelled as Cancelled_Date, " +
                            "JobPOs.RefNumber as Invoice, " +
                            "iif(JobPOs.RAssnToJobPORID <> 0, 0, JobPOs.AmtPaid) as Payment_Amount, " +
                            "iif(JobPOs.RAssnToJobPORID <> 0, 0, JobPOs.AmtSubTotal) as Subtotal, " +
                            "iif(JobPOs.RAssnToJobPORID <> 0, 0, JobPOs.AmtTax) as Tax, " +
                            "iif(JobPOs.DateCancelled > '1900-01-01' and (select count(*) from JobPOs jpos where JobPOs.JobRID = jpos.JobRID and JobPOs.ActRID = jpos.ActRID and jpos.DateCancelled = '1900-01-01') > 0 , 0, JobPOs.AmtTotal) as Total, " +
                            "iif(JobPOs.DateCancelled > '1900-01-01' and (select count(*) from JobPOs jpos where JobPOs.JobRID = jpos.JobRID and JobPOs.ActRID = jpos.ActRID and jpos.DateCancelled = '1900-01-01') > 0 , 0, JobPOs.AmtTotal) as EGMAmount, " +

                            "JobPOs.CheckID as Check_No, " +
                            "0 as VPO_Yes_No, " +
                            "Left(Jobs.JobID,3) as Community, " +
                            "Substring(Jobs.JobID,4,2) as Product, " +
                            "Substring(Jobs.JobID,6,3) as Building, " +
                            "Substring(Jobs.JobID,9,4) as Unit, " +
                            "JobPOs.AmtTaxable as Taxable_Amount, " +
                            "Jobs.JobID as Job_No, " +
                            "JobPOs.DateApproved as ApprovePaymentDate, " +
                            "JobPOs.TaxPercentage as TaxRate, " +
                            "0 as eMeasurementPO, " +
                            "j1.DateComplByVnd as eSubmittalDate, " +
                            "JobPOs.JobPOID as SapphirePONumber, " +
                            "'JobPOs' as SapphireObjID, " +
                            "JobPOs.JobPORID as SapphireObjRID  " +
                        "FROM " +
                        "    Jobs " +
                        "INNER JOIN JobPOs " +
                        "    ON Jobs.JobRID = JobPOs.JobRID " +
                        "INNER JOIN Acts " +
                        "   ON JobPOs.ActRID = Acts.ActRID " +
                        " LEFT OUTER JOIN Vnds " +
                        "     ON JobPOs.VndRID = Vnds.VndRID " +
                        "INNER JOIN JobSchActs j1 " +
                        "   ON JobPOs.JobActRID = j1.JobActRID " +
                        "   AND j1.VndRID = JobPOs.VndRID " +
                        "WHERE " +
                        "    (JobPOs.Status IN ('Approved', 'Released', 'Hold', 'Cancelled', 'Completed', 'WorkInProgress')  OR JobPOs.Status IS NULL) AND " +
                        "   convert(varchar,jobpos.jobrid)+convert(varchar,jobpos.actrid) not in (select convert(varchar,jobcstacts.jobrid)+convert(varchar,jobcstacts.actrid) from jobcstacts where jobpos.jobrid = jobcstacts.jobrid) " +
                        "AND " +
                        "(JobPOs.LastUpdated >  " + "'" + lastCompleted.ToString() + "'" + " or JobPOs.LastUpdated is null) " +
                        "order by newjobnumber, activity;";
        }

        public string CreateQueryTextForWorkingQueryString()
        {
            DateTime lastCompleted = new DateTime(2018, 06, 01); //Works
            return "SELECT " +
                                "Jobs.JobRID, " +
                                "left(Jobs.JobID, 12) as SiteNumber, " +
                                "Left(Jobs.JobID,5)+'/'+Substring(Jobs.JobID,6,3)+'/'+Substring(Jobs.JobID,9,4) as NewJobNumber, " +
                                "Acts.ActID as activity, " +
                                "Acts.Name AS Name, " +
                                "coalesce(Vnds.VndID, 'YNH00') as Vendor_ID, " +
                                "iif(coalesce(JobPOs.Status, '') in ('',NULL, 'Hold'),'H', 'Y') as po_type,  " +
                                "JobPOs.DateReleased as Release_Date, " +
                                "JobPOs.DateCancelled as Cancelled_Date, " +
                                "JobPOs.RefNumber as Invoice, " +
                                "iif(JobPOs.RAssnToJobPORID <> 0, 0, JobPOs.AmtPaid) as Payment_Amount, " +
                                "iif(JobPOs.RAssnToJobPORID <> 0, 0, JobPOs.AmtSubTotal) as Subtotal, " +
                                "iif(JobPOs.RAssnToJobPORID <> 0, 0, JobPOs.AmtTax) as Tax, " +
                                "iif(JobPOs.DateCancelled > '1900-01-01' and (select count(*) from JobPOs jpos where JobPOs.JobRID = jpos.JobRID and JobPOs.ActRID = jpos.ActRID and jpos.DateCancelled = '1900-01-01') > 0 , 0, coalesce(JobPOs.AmtTotal,JobCstActs.BudgetAmt)) as Total, " +
                                "iif( (select count(*) from JobPOs jpos where JobPOs.JobRID = jpos.JobRID and JobPOs.ActRID = jpos.ActRID) <= 1, coalesce(JobCstActs.BudgetAmt, JobPOs.AmtTotal), " +
                                    "iif(JobPOs.JobPORID = (select min(jpos4.JobPORID) from JobPOs jpos4 where JobPOs.JobRID = jpos4.JobRID and JobPOs.ActRID = jpos4.ActRID), coalesce(JobCstActs.BudgetAmt, JobPOs.AmtTotal), 0)) " +
                                    "as EGMAmount, " +
                                "0 as VPO_Yes_No, " +
                                "Comms.CommunityID as Community, " +
                                "Substring(Jobs.JobID,4,2) as Product, " +
                                "Substring(Jobs.JobID,6,3) as Building, " +
                                "Substring(Jobs.JobID,9,4) as Unit," +
                                "JobPOs.AmtTaxable as Taxable_Amount, " +
                                "Jobs.JobID as Job_No, " +
                                "JobPOs.DateApproved as ApprovePaymentDate, " +
                                "JobPOs.TaxPercentage as TaxRate, " +
                                "0 as eMeasurementPO," +
                                "j1.DateComplByVnd as eSubmittalDate, " +
                                "JobPOs.JobPOID as SapphirePONumber, " +
                                "iif(JobPOs.DateReleased is null,'JobCstActs', 'JobPOs') as SapphireObjID, " +
                                "iif(JobPOs.DateReleased is null,JobCstActs.JobCstActRID, JobPOs.JobPORID) as SapphireObjRID  " +
                            "FROM " +
                            "    Jobs " +
                            "INNER JOIN Lots  " +
                            "    ON Jobs.LotRID = Lots.LotRID " +
                            "INNER JOIN Communities Comms " +
                            "    ON Lots.CommunityRID = Comms.CommunityRID " +
                            "INNER JOIN JobCstHdrs " +
                            "    ON Jobs.JobRID = JobCstHdrs.JobRID " +
                            "INNER JOIN JobCstActs " +
                            "    ON JobCstHdrs.JobCstHdrRID = JobCstActs.JobCstHdrRID " +
                            "INNER JOIN Acts " +
                            "    ON JobCstActs.ActRID = Acts.ActRID " +
                            "LEFT OUTER JOIN JobPOs " +
                            "    ON JobCstActs.JobRID = JobPOs.JobRID " +
                            "    AND JobCstActs.ActRID = JobPOs.ActRID " +
                            "LEFT OUTER JOIN Vnds " +
                            "    ON JobPOs.VndRID = Vnds.VndRID " +
                            "LEFT JOIN JobSchActs j1 " +
                            "   ON j1.JobActRID = JobPOs.JobActRID   " +
                            "   AND j1.VndRID = JobPOs.VndRID  " +
                            "LEFT JOIN Vnds v1 " +
                            "    ON j1.VndRID = v1.VndRID " +
                            "WHERE " +
                                "(JobCstHdrs.jobcsthdrrid in (select max(jobcsthdrrid) " +
                                 "from JobCstHdrs j3 " +
                                 "where j3.status in ('Current') " +
                                 "group by Name) " +
                                " AND coalesce(JobPOs.AmtTotal,JobCstActs.BudgetAmt) > 0.0 " +
                                "AND " +
                                "(JobPOs.Status IN ('Approved', 'Released', 'Hold', 'Cancelled', 'Completed', 'WorkInProgress')  OR JobPOs.Status IS NULL)) AND " +
                                    "Jobs.DataProcStatus = 'Completed' AND " +
                                    "(JobPOs.LastUpdated >  " + "'" + lastCompleted.ToString() + "'" + " or JobCstActs.LastUpdated >  " + "'" + lastCompleted.ToString() + "'" + " or JobPOs.LastUpdated is null) " +
                                    "order by newjobnumber, activity; ";
        }
    }
}
