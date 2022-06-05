using ScopoERP.Common.ViewModel;
using ScopoERP.Domain.Models;
using ScopoERP.Domain.Repositories;
using ScopoERP.OrderManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.OrderManagement.BLL
{
    public class SizeColorLogic
    {
        private UnitOfWork unitOfWork;
        private sizecolor sizeColor;

        public SizeColorLogic(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void CreateSizeColor(List<SizeColorViewModel> sizeColorList)
        {
            int purchaseOrderID = sizeColorList[0].PoStyleID;

            var temp = (from s in unitOfWork.SizeColorRepository.Get()
                        where s.PoStyleId == purchaseOrderID
                        select s.SizeColorId).ToList();

            if (temp.Count != 0)
            {
                foreach(var sizeColorID in temp)
                {
                    unitOfWork.SizeColorRepository.Delete(new sizecolor { SizeColorId = sizeColorID });
                }
            }

            foreach (var item in sizeColorList)
            {
                foreach (var s in item.SizeQuantity)
                {
                    sizeColor = new sizecolor
                    {
                        PoStyleId = item.PoStyleID,
                        Color = item.Color,
                        Size = s.Size,
                        Quantity = s.Quantity,
                        FOB = s.FOB
                    };
                    unitOfWork.SizeColorRepository.Insert(sizeColor);
                }
            }

            unitOfWork.Save();
            
            //decimal _fobFromSizeColor = this.CalculateFOBFromSizeColor(purchaseOrderID);

            //if (_fobFromSizeColor != 0)
            //{
            //    var po = unitOfWork.PurchaseOrderRepository.GetById(purchaseOrderID);
            //    po.Fob = _fobFromSizeColor;

            //    unitOfWork.PurchaseOrderRepository.Update(po);
            //}

            //unitOfWork.Save();
        }
        
        public List<SizeColorViewModel> GetSizeColorByPurchaseOrderID(int purchaseOrderID)
        {
            Decimal _fob = unitOfWork.PurchaseOrderRepository.Get()
                            .Where(x => x.PoStyleId == purchaseOrderID)
                            .Select(x => x.Fob)
                            .SingleOrDefault();

            var sizeColorList = (from s in unitOfWork.SizeColorRepository.Get()
                                 where s.PoStyleId == purchaseOrderID
                                 select s).ToList();

            List<SizeColorViewModel> result = new List<SizeColorViewModel>();

            var colorList = (from s in sizeColorList
                             select s.Color).Distinct().ToList();

            foreach (var color in colorList)
            {
                var sizeQuantity = (from s in sizeColorList
                                    where s.Color == color
                                    select new SizeQuantity { Size = s.Size, Quantity = s.Quantity, FOB = s.FOB ?? _fob }).ToList();

                result.Add(new SizeColorViewModel { PoStyleID = purchaseOrderID, Color = color, SizeQuantity = sizeQuantity });
            }
          

            return result;
        }

        public List<SizeColorDetailsViewModel> GetSizeColorDetailsByPurchaseOrderID(int purchaseOrderID)
        {
            var result = (from s in unitOfWork.SizeColorRepository.Get()
                          where s.PoStyleId == purchaseOrderID
                          select new SizeColorDetailsViewModel
                          {
                              Color = s.Color,
                              Size = s.Size,
                              Quantity = s.Quantity
                          }).ToList();

            return result;
        }

        public List<SizeColorDetailsViewModel> GetSizeColorByFormula(int purchaseOrderID, int formula)
        {
            // Get All Size Color
            List<SizeColorDetailsViewModel> sizeColorList = GetSizeColorDetailsByPurchaseOrderID(purchaseOrderID);

            // Color Wise
            if (formula == 1)
            {
                return (from c in sizeColorList
                        group c by c.Color into s
                        select new SizeColorDetailsViewModel
                        {
                            Color = s.Key,
                            Size = "",
                            Quantity = s.Sum(x => x.Quantity)
                        }).ToList();
            }
            // Size wise
            else if (formula == 2)
            {
                return (from c in sizeColorList
                        group c by c.Size into s
                        select new SizeColorDetailsViewModel
                        {
                            Color = "",
                            Size = s.Key,
                            Quantity = s.Sum(x => x.Quantity)
                        }).ToList();
            }
            // Size & Color wise
            else if (formula == 3)
            {
                return sizeColorList;
            }
            // N/A
            else
            {
                int total = (from c in sizeColorList
                             select c.Quantity).Sum();

                List<SizeColorDetailsViewModel> _TotalQuantity = new List<SizeColorDetailsViewModel>();

                _TotalQuantity.Add(new SizeColorDetailsViewModel { Color = "", Size = "", Quantity = total });

                return _TotalQuantity;
            }
        }

        public bool IsSizeColorExists(int purchaseOrderID) 
        {
            var result = (from c in unitOfWork.SizeColorRepository.Get()
                          where c.PoStyleId == purchaseOrderID
                          select c);
            if (result.Count() > 0)
                return true;

            return false;    
        }

        public void CopySizeColor(int fromPurchaseOrderID, int toPurchaseOrderID)
        {
            var result = (from c in unitOfWork.SizeColorRepository.Get()
                          where c.PoStyleId == fromPurchaseOrderID
                          select new SizeColorDetailsViewModel 
                          {
                              Color = c.Color,
                              Quantity = c.Quantity,
                              Size = c.Size
                          }).ToList();

            foreach (var i in result)
            {
                sizeColor = new sizecolor
                {
                    PoStyleId = toPurchaseOrderID,
                    Color = i.Color,
                    Size = i.Size,
                    Quantity = i.Quantity
                };
                unitOfWork.SizeColorRepository.Insert(sizeColor);
            }

            unitOfWork.Save();
        }

        public Decimal CalculateFOBFromSizeColor(int purchaseOrderID)
        {
            int _orderQuantity = unitOfWork.PurchaseOrderRepository.Get()
                                .Where(x => x.PoStyleId == purchaseOrderID)
                                .Select(x => x.OrderQuantity)
                                .SingleOrDefault();

            decimal? _totalFOB = unitOfWork.SizeColorRepository.Get()
                                .Where(x => x.PoStyleId == purchaseOrderID)
                                .Sum(x => x.FOB * x.Quantity);

            if (_totalFOB == null)
                return 0;

            return (_totalFOB / _orderQuantity) ?? 0;
        }
    }
}
