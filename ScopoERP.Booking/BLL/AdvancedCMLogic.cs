using ScopoERP.MaterialManagement.ViewModel;
using ScopoERP.Domain.Models;
using ScopoERP.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.MaterialManagement.BLL
{
    public class AdvancedCMLogic
    {
        private UnitOfWork unitOfWork;
        private advancedcm advancedCM;
        private piinfo piInfo;

        private BookingLogic bookingLogic;

        public AdvancedCMLogic(UnitOfWork unitOfWork, BookingLogic bookingLogic)
        {
            this.unitOfWork = unitOfWork;
            this.bookingLogic = bookingLogic;
        }

        public void CreateAdvancedCM(AdvancedCMViewModel advancedCMVM)
        {
            piInfo = new piinfo
            {
                PINo = advancedCMVM.PINo,
                ReferenceNo = bookingLogic.GetNewReferenceNo(),
                PIDate = advancedCMVM.PIDate,
                SupplierID = advancedCMVM.SupplierID,
                Status = 1
            };

            advancedCM = new advancedcm
            {
                JobID = advancedCMVM.JobID,
                PIID = advancedCMVM.PIID,
                PIValue = advancedCMVM.PIValue,
                UDStatus = advancedCMVM.UDStatus,
                ConversionRate = advancedCMVM.ConversionRate,
                ReceivedAmount = advancedCMVM.ReceivedAmount,
                ReceivedDate = advancedCMVM.ReceivedDate,
                Remarks = advancedCMVM.Remarks,
                UserID = advancedCMVM.UserID,
                SetupDate = advancedCMVM.SetupDate,
                piinfo = piInfo
            };

            unitOfWork.AdvancedCMRepository.Insert(advancedCM);
            unitOfWork.Save();
        }

        public void UpdateAdvancedCM(AdvancedCMViewModel advancedCMVM)
        {
            advancedCM = new advancedcm
            {
                AdvancedCMID = advancedCMVM.AdvancedCMID,
                JobID = advancedCMVM.JobID,
                PIID = advancedCMVM.PIID,
                PIValue = advancedCMVM.PIValue,
                UDStatus = advancedCMVM.UDStatus,
                ConversionRate = advancedCMVM.ConversionRate,
                ReceivedAmount = advancedCMVM.ReceivedAmount,
                ReceivedDate = advancedCMVM.ReceivedDate,
                Remarks = advancedCMVM.Remarks,
                UserID = advancedCMVM.UserID,
                SetupDate = advancedCMVM.SetupDate,
            };

            unitOfWork.AdvancedCMRepository.Update(advancedCM);

            piInfo = unitOfWork.PIRepository.Get().SingleOrDefault(x => x.PIID == advancedCMVM.PIID);
            
            if(piInfo != null)
            {
                piInfo.PINo = advancedCMVM.PINo;
                piInfo.PIDate = advancedCMVM.PIDate;

                unitOfWork.PIRepository.Update(piInfo);
            }
            
            unitOfWork.Save();
        }

        public IQueryable<AdvancedCMViewModel> GetAllAdvancedCM()
        {
            var result = (from a in unitOfWork.AdvancedCMRepository.Get()
                          join p in unitOfWork.PIRepository.Get() on a.PIID equals p.PIID
                          join x in unitOfWork.BackToBackLCRepository.Get() on p.BackToBackLCID equals x.BackToBackLCID into y
                          join s in unitOfWork.SupplierRepository.Get() on p.SupplierID equals s.SupplierId
                          join j in unitOfWork.JobRepository.Get() on a.JobID equals j.JobInfoId
                          from b in y.DefaultIfEmpty()
                          select new AdvancedCMViewModel
                          {
                              AdvancedCMID = a.AdvancedCMID,
                              JobID = a.JobID,
                              JobNo = j.JobNo,

                              PIDate = p.PIDate,
                              PIID = p.PIID,
                              PINo = p.PINo,
                              PIValue = a.PIValue,

                              SupplierID = s.SupplierId,
                              SupplierName = s.SupplierName,

                              UDStatus = a.UDStatus,

                              ConversionRate = a.ConversionRate,
                              ReceivableAmount = a.ConversionRate * a.PIValue,
                              ReceivedAmount = a.ReceivedAmount,
                              ReceivedDate = a.ReceivedDate,
                              DifferenceFromReceivable = a.ConversionRate * a.PIValue - a.ReceivedAmount,

                              Remarks = a.Remarks,

                              BackToBackLC = b.BackToBackLC1,
                              BackToBackLCDate = b.BackToBackLCDate,

                              UserID = a.UserID,
                              SetupDate = a.SetupDate
                          }).AsQueryable();

            return result;
        }

        public AdvancedCMViewModel GetAdvancedCMByID(int id)
        {
            var result = (from a in unitOfWork.AdvancedCMRepository.Get()
                          join p in unitOfWork.PIRepository.Get() on a.PIID equals p.PIID
                          join s in unitOfWork.SupplierRepository.Get() on p.SupplierID equals s.SupplierId
                          join j in unitOfWork.JobRepository.Get() on a.JobID equals j.JobInfoId
                          where a.AdvancedCMID == id
                          select new AdvancedCMViewModel
                          {
                              AdvancedCMID = a.AdvancedCMID,
                              JobID = a.JobID,
                              JobNo = j.JobNo,

                              PIDate = p.PIDate,
                              PIID = p.PIID,
                              PINo = p.PINo,
                              PIValue = a.PIValue,

                              SupplierID = s.SupplierId,
                              SupplierName = s.SupplierName,

                              UDStatus = a.UDStatus,

                              ConversionRate = a.ConversionRate,
                              ReceivableAmount = a.ConversionRate * a.PIValue,
                              ReceivedAmount = a.ReceivedAmount,
                              ReceivedDate = a.ReceivedDate,
                              DifferenceFromReceivable = a.ConversionRate * a.PIValue - a.ReceivedAmount,

                              Remarks = a.Remarks,

                              UserID = a.UserID,
                              SetupDate = a.SetupDate
                          }).SingleOrDefault();

            return result;
        }

        public decimal? GetPIValueFromAdvancedCM(int piID)
        {
            decimal? piValue = (from s in unitOfWork.AdvancedCMRepository.Get()
                                where s.PIID == piID
                                select s.PIValue).FirstOrDefault();

            return piValue;
        }
    }
}
