using ScopoERP.Common.ViewModel;
using ScopoERP.Domain.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Common.BLL
{
    public class DashboardLogic
    {
        private UnitOfWork unitOfWork;
        private DashboardViewModel dashboardViewModel;

        public DashboardLogic(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

      

        public object GetTotalOfOrderInvoicePI()
        {
            int totalOrderNumber = GetTotalOrderNumber();
            int totalInvoiceNumber = GetTotalInvoiceNumber();
            int totalPI = GetTotalPI();

            dashboardViewModel = new DashboardViewModel
            {
                TotalOrder=totalOrderNumber,
                TotalInvoice=totalInvoiceNumber,
                TotalPI=totalPI
            };
            return dashboardViewModel;

        }

        public ShipmentPerDay GetShipmentPerDayDataSet()
        {
            
            Random random = new Random();
            var data = new ShipmentPerDay();
            for (int i = 0; i < 30; i++)
            {
                data.Amounts.Add(random.Next(300, 222222));
                data.Dates.Add(i + 1);                
            }

            return data;
        }




        private int GetTotalPI()
        {
            var total = (from p in unitOfWork.PIRepository.Get()
                         select p
                       ).Count();
            return total;
        }

        public int GetTotalInvoiceNumber()
        {
            var total = (from I in unitOfWork.ExportInvoiceRepository.Get()
                         select I
                        ).Count();
            return total;
        }

        public ArrayList GetSampleCountByApSentDate()
        {
            DateTime d = DateTime.Now.AddDays(30);
            var data = (from p in unitOfWork.SampleApprovalRepository.Get()
                        where p.ApproximateSentDate >= DateTime.Now && p.ApproximateSentDate <= d
                        group p by p.ApproximateSentDate into g
                        select new
                        {
                            ApproximateSentDate = g.Key,
                            SampleApprovalCount = g.Count()
                        }).ToList();

            ArrayList al = new ArrayList();
            al.AddRange(data);
            return al;
        }

        public ArrayList GetShipmentDateTotalOrder()
        {
            DateTime d = DateTime.Now.AddDays(30);
            var data = (from p in unitOfWork.PurchaseOrderRepository.Get()
                        where p.ExitDate >= DateTime.Now && p.ExitDate <= d
                        group p by p.ExitDate into g
                        orderby g.Sum(x => x.OrderQuantity)
                        select new
                        {
                            ShipExitDate = g.Key,
                            ShipOrderQuanSum = g.Sum(x => x.OrderQuantity)
                        }).ToList();

            /* var groups = data
            .GroupBy(n => n.ExitDate)
            .Select(m => new
            {
                MetricName = m.Key,
                MetricCount = m.Count()
            }
            )
            .OrderBy(m => m.MetricName)
            .ToList();*/

            ArrayList al = new ArrayList();
            al.AddRange(data);
            return al;
        }

        public object GetDashboardData()
        {
            dashboardViewModel = new DashboardViewModel();
            dashboardViewModel.TotalContract = GetTotalContract();
            dashboardViewModel.TotalOrder = GetTotalOrderNumber();
            dashboardViewModel.CurrentShipment = GetCurrentShipment();
            dashboardViewModel.UpcomingShipment = GetUpcomingShipment();

            return dashboardViewModel;

        }

        private int GetUpcomingShipment()
        {
            var res = (from p in unitOfWork.PurchaseOrderRepository.Get()
                       where p.ExitDate > DateTime.Now
                       select p).Count();
            return res;
        }

        private int GetCurrentShipment()
        {
            DateTime firstDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime lastDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1);

            var res = (from s in unitOfWork.ShipmentRepository.Get()
                       where s.ChalanDate >= firstDate && s.ChalanDate <= lastDate
                       select s).Count();
            return res;
        }

        private int GetTotalContract()
        {
            var res = (from j in unitOfWork.JobRepository.Get()
                       select j).Count();
            return res;
        }

        public int GetTotalOrderNumber()
        {
            var total = (from p in unitOfWork.PurchaseOrderRepository.Get()
                         select p
                        ).Count();
            return total;

        }

        public ArrayList GetPICountByPIDate() {
            DateTime d = DateTime.Now.AddDays(30);
            /*  var data = (from p in unitOfWork.PIRepository.Get()
                          where p.PIDate >= DateTime.Now && p.PIDate <= d
                          group p by p.PIDate into g
                          select new
                          {
                              PIDate = g.Key,
                              PICount = g.Count(),
                             //PINoList = string.Join(",", g.Select(i => i.PINo))
                              //PINO = string.Concat(g.Select(i => i.PINo).ToArray())

                          }).ToList();*/

            var data = (from p in unitOfWork.PIRepository.Get()
                        where p.PIDate >= DateTime.Now && p.PIDate <= d
                        group p by p.PIDate into g
                        select g)
                        .AsEnumerable()
                        .Select(c => new 
                        {
                            PIDate = c.Key,
                            PICount = c.Count(),
                            PINoList = String.Join(", ", c.Select(i => i.PINo))
                        })
                             .ToList();

            ArrayList al = new ArrayList();
            al.AddRange(data);
            return al;
        }

        public ArrayList GetShipmentDates()
        {
            DateTime d = DateTime.Now.AddDays(30);
            var data = (from p in unitOfWork.PurchaseOrderRepository.Get()
                                                 where p.ExitDate >= DateTime.Now && p.ExitDate <= d
                                                 select new
                                                 {
                                                     ExitDate = p.ExitDate,
                                                     PurchaseOrderNo = p.PoNo
                                                 }).ToList();
            ArrayList al = new ArrayList();
            al.AddRange(data);

            return al;
        }
    }
}
