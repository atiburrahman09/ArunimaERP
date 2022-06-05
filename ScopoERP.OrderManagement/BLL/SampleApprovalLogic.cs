using ScopoERP.Domain.Models;
using ScopoERP.Domain.Repositories;
using ScopoERP.OrderManagement.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.OrderManagement.BLL
{
    public class SampleApprovalLogic
    {
        private UnitOfWork unitOfWork;
        private sampleapproval sampleapprove;

        public SampleApprovalLogic(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public List<SampleTypeViewModel> GetAllSample()
        {
            var result = (from s in unitOfWork.SampleTypeRepository.Get()
                          orderby s.SampleTypeID descending
                          select new SampleTypeViewModel
                          {
                              SampleTypeID = s.SampleTypeID,
                              SampleTypeName = s.SampleTypeName
                          }).ToList();

            return result;
        }

        public void SaveApprove(SampleApprovalViewModel sampleApproveVM)
        {
            foreach (var item in sampleApproveVM.ApprovalList)
            {

                sampleapproval sampleapprove = new sampleapproval
                {
                    StyleID = sampleApproveVM.StyleID,
                    ApprovalSerialNo = item.ApprovalSerialNo,
                    Validity = item.ValidityTime,
                    SampleTypeID = item.SampleTypeID,
                    Color = item.Color,
                    Size = item.Size,
                    Quantity = item.Quantity,
                    ApproximateSentDate = item.ApproximateSentDate , 
                    SentDate = item.SentDate,
                    ApproveDate = item.ApproveDate,
                    ApprovalStatus = item.ApprovalStatus,
                    CourierName = item.CourierName,
                    CourierNo = item.CourierNo,
                    ApprovalThrough = item.ApprovalThrough,
                    Remarks = item.Remarks,
                    SetDate = DateTime.Now,
                    UserID  = sampleApproveVM.UserID
                };
                unitOfWork.SampleApprovalRepository.Insert(sampleapprove);
            }
            unitOfWork.Save();
        }

        public void UpdateApprove(SampleApprovalViewModel sampleApproveVM)
        {
            foreach (var item in sampleApproveVM.ApprovalList)
            {
                unitOfWork.SampleApprovalRepository.RawQuery("DELETE FROM sampleapproval WHERE SampleApprovalID = '"+ item.SampleApprovalID +"'");
            }

            foreach(var item in sampleApproveVM.ApprovalList)
            {
                this.sampleapprove = new sampleapproval
                {
                    StyleID = sampleApproveVM.StyleID,
                    ApprovalSerialNo = item.ApprovalSerialNo,
                    Validity = item.ValidityTime,
                    SampleTypeID = item.SampleTypeID,
                    Color = item.Color,
                    Size = item.Size,
                    Quantity = item.Quantity,
                    ApproximateSentDate = item.ApproximateSentDate,
                    SentDate = item.SentDate,
                    ApproveDate = item.ApproveDate,
                    ApprovalStatus = item.ApprovalStatus,
                    CourierName = item.CourierName,
                    CourierNo = item.CourierNo,
                    ApprovalThrough = item.ApprovalThrough,
                    Remarks = item.Remarks,
                    SetDate = DateTime.Now,
                    UserID = sampleApproveVM.UserID
                };
                unitOfWork.SampleApprovalRepository.Insert(sampleapprove);
            }
            unitOfWork.Save();
        }

        public void RemoveSampleApprove(int sampleApprovalID)
        {
            unitOfWork.SampleApprovalRepository.RawQuery("DELETE FROM sampleapproval WHERE SampleApprovalID = '" + sampleApprovalID + "'");
            unitOfWork.Save();
        }

        public List<ApprovalViewModel> GetAllSampleApprove(int styleID)
        {
            var result = (from s in unitOfWork.SampleApprovalRepository.Get()
                          where s.StyleID == styleID
                          orderby s.SampleTypeID descending
                          select new ApprovalViewModel
                          {
                              SampleApprovalID=s.SampleApprovalID,
                              SampleTypeID = s.SampleTypeID,
                              Color = s.Color,
                              Size = s.Size,
                              Quantity = s.Quantity,
                              ApproximateSentDate = s.ApproximateSentDate,
                              SentDate = s.SetDate,
                              ApproveDate = s.ApproveDate,
                              ApprovalStatus = s.ApprovalStatus,
                              CourierName = s.CourierName,
                              CourierNo = s.CourierNo,
                              ApprovalThrough = s.ApprovalThrough,
                              Remarks = s.Remarks,
                              ValidityTime = s.Validity,
                              ApprovalSerialNo = s.ApprovalSerialNo

                          }).ToList();

            return result;
        }


        
    }
}
