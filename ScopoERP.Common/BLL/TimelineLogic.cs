using ScopoERP.Domain.Models;
using ScopoERP.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScopoERP.Common.ViewModel;

namespace ScopoERP.Common.BLL
{
    public class TimelineLogic
    {
        private UnitOfWork unitOfWork;
        private TimeLine timeLine;

        public TimelineLogic(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public object GetAllTimelineByPOID(int purchaseOrderID)
        {
            var res = (from t in unitOfWork.TimeLineRepository.Get()
                       where t.PurchaseOrderID==purchaseOrderID
                       select t).ToList();

            return res;
        }

        public void SaveTimeLine(TimelineViewModel timelineVM)
        {
            timeLine = new TimeLine
            {
                Description = timelineVM.Description,
                ExpectedDate =Convert.ToDateTime(timelineVM.ExpectedDate),
                ProvableDate = Convert.ToDateTime(timelineVM.ProvableDate),
                LastModified = DateTime.Now,
                ModifiedBy= timelineVM.ModifiedBy,
                PurchaseOrderID=timelineVM.PurchaseOrderID
            };
            unitOfWork.TimeLineRepository.Insert(timeLine);
            unitOfWork.Save();
        }
        

        public void RemoveTimeline(int timelineID)
        {
            unitOfWork.TimeLineRepository.RawQuery("DELETE FROM Timelines WHERE TimelineID = '" + timelineID + "'");
            unitOfWork.Save();
        }

        public void UpdateTimeline(TimelineViewModel timelineVM)
        {
            timeLine = new TimeLine
            {
                TimeLineID = timelineVM.TimeLineID,
                Description = timelineVM.Description,
                ExpectedDate = Convert.ToDateTime(timelineVM.ExpectedDate),
                ProvableDate = Convert.ToDateTime(timelineVM.ProvableDate),
                LastModified = DateTime.Now,
                ModifiedBy = timelineVM.ModifiedBy,
                PurchaseOrderID = timelineVM.PurchaseOrderID
            };
            unitOfWork.TimeLineRepository.Update(timeLine);
            unitOfWork.Save();
        }
    }
}
