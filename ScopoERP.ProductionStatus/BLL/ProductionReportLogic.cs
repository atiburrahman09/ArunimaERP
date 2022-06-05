using ScopoERP.Domain.Repositories;
using ScopoERP.ProductionStatus.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ScopoERP.ProductionStatus.BLL
{
    public class ProductionReportLogic
    {
        private UnitOfWork unitOfWork;
        public ProductionReportLogic(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public ProductionReportViewModel GetSewingReport()
        {
            //return new ProductionReportViewModel
            //{
            //    ExpectedSewing = 5200,
            //    ActualSewing = 3600
            //};

            var date = DateTime.Now;
            var data = (from A in unitOfWork.ProductionStatusRepository.Get()
                        join B in unitOfWork.ProductionPlanningRepository.Get()
                        on A.PoStyleId equals B.PoStyleID
                        where DbFunctions.TruncateTime(A.Date) == DbFunctions.TruncateTime(date)
                        select new ProductionReportViewModel
                        {
                            ActualSewing = B.Capacity,
                            ExpectedSewing = (int)A.TodaySewing
                        }).SingleOrDefault();
            return data;
        }
    }
}
