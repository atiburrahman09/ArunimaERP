using ScopoERP.Domain.Models;
using ScopoERP.Domain.Repositories;
using ScopoERP.Finance.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Finance.BLL
{
    public class RealizationLogic
    {
        private UnitOfWork unitOfWork;
        private realization realization;

        public RealizationLogic(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public List<RealizationViewModel> GetAllRealization()
        {
            List<RealizationViewModel> realizationList = (from a in unitOfWork.RealizationAccountRepository.Get()
                                                          join r in unitOfWork.RealizationRepository.Get() on a.RealizationAccountID equals r.AccountID
                                                          join b in unitOfWork.BankForwardingRepository.Get() on r.BankForwardingID equals b.BankForwardingID
                                                          group r by new { b.BankForwardingNo, b.FDBPNo } into s
                                                          select new RealizationViewModel
                                                          {
                                                              BankForwardingNo = s.Key.BankForwardingNo,
                                                              FDBPNo = s.Key.FDBPNo,
                                                              RealizationDate = s.FirstOrDefault().RealizationDate,
                                                              CurrencyRate = s.FirstOrDefault().CurrencyRate,
                                                              TotalRealizedValue = s.Sum(x=>x.Amount)
                                                          }).ToList();

            return realizationList;
        }

        public List<RealizationViewModel> GetAllRealization(int bankForwardingID, int accountType)
        {
            var result = (from a in unitOfWork.RealizationAccountRepository.Get()
                         join rz in unitOfWork.RealizationRepository.Get() on a.RealizationAccountID equals rz.AccountID into rg
                         from r in rg.Where(x=>x.BankForwardingID == bankForwardingID).DefaultIfEmpty()
                          where a.RealizationAccountType == accountType || a.RealizationAccountType == 3
                          select new RealizationViewModel
                          {
                              RealizationID = r.RealizationID,
                              BankForwardingID = bankForwardingID,
                              RealizationDate = r.RealizationDate,
                              AccountID = a.RealizationAccountID,
                              AccountName = a.RealizationAccountName,
                              AccountNo = a.RealizationAccountNo,
                              Amount = r.Amount,
                              CurrencyRate = r.CurrencyRate
                          }).ToList();

            return result;
        }

        public List<RealizationViewModel> GetAllRealizationSummary()
        {
            var result = (from r in unitOfWork.RealizationRepository.Get()
                          join b in unitOfWork.BankForwardingRepository.Get() on r.BankForwardingID equals b.BankForwardingID
                          group r by new { b.BankForwardingID, b.FDBPNo } into c
                          select new RealizationViewModel
                          {
                              FDBPNo = c.Key.FDBPNo,
                              TotalRealizedValue = c.Sum(x => x.Amount)
                          }).ToList();

            return result;
        }

        public void SaveRealization(List<RealizationViewModel> realizationVMList)
        {
            if (realizationVMList.Count != 0)
            {
                int bankForwardingID = realizationVMList[0].BankForwardingID ?? 0;

                var existingRealizationList = unitOfWork.RealizationRepository
                                            .Get()
                                            .Where(x => x.BankForwardingID == bankForwardingID).ToList();

                if (existingRealizationList.Count != 0)
                {
                    foreach (var item in existingRealizationList)
                    {
                        unitOfWork.RealizationRepository.Delete(item);
                    }
                }
                

                foreach (var item in realizationVMList)
                {
                    realization = new realization
                    {
                        BankForwardingID = realizationVMList[0].BankForwardingID ?? 0,
                        RealizationDate = realizationVMList[0].RealizationDate ?? DateTime.Now,
                        AccountID = item.AccountID,
                        Amount = item.Amount ?? 0,
                        CurrencyRate = realizationVMList[0].CurrencyRate,
                        UserID = realizationVMList[0].UserID ?? 0,
                        SetDate = DateTime.Now
                    };

                    unitOfWork.RealizationRepository.Insert(realization);
                }
                unitOfWork.Save();
            }
        }
    }
}
