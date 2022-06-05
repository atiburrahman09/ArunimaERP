using ScopoERP.Common.ViewModel;
using ScopoERP.Domain.Models;
using ScopoERP.Domain.Repositories;
using ScopoERP.Store.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Store.BLL
{
    public class ChalanLogic
    {
        private UnitOfWork unitOfWork;
        private chalanexport chalanExport;
        private shipment shipment;
        private ShipmentLogic shipmentLogic;

        public ChalanLogic(UnitOfWork unitOfWork, ShipmentLogic shipmentLogic)
        {
            this.unitOfWork = unitOfWork;
            this.shipmentLogic = shipmentLogic;
        }

        public bool CreateChalan(ChalanViewModel chalanVM)
        {
            chalanExport = new chalanexport
            {
                ChalanNo = chalanVM.ChalanNo,
                ChalanDate = chalanVM.ChalanDate,
                VehicleNo = chalanVM.VehicleNo,
                DriverName = chalanVM.DriverName,
                MobileNo = chalanVM.MobileNo,
                ShippedBy = chalanVM.ShippedBy,
                SealNo = chalanVM.SealNo,
                UserID = chalanVM.UserID,
                SetupDate = chalanVM.SetupDate
            };


            if (chalanVM.ShipmentList != null)
            {
                foreach (var item in chalanVM.ShipmentList)
                {
                    shipment = new shipment
                    {
                        PurchaseOrderID = item.PurchaseOrderID,

                        ChalanID = item.ChalanID,
                        ChalanQuantity = item.ChalanQuantity,
                        ChalanDate = chalanVM.ChalanDate,
                        CBM = item.CBM,
                        CartoonQuantity = item.CartoonQuantity,

                        UserID = chalanVM.UserID,
                        SetupDate = chalanVM.SetupDate
                    };

                    chalanExport.shipment.Add(shipment);
                }

                unitOfWork.ChalanExportRepository.Insert(chalanExport);

                unitOfWork.Save();
                return true;
            }

            else { return false; }
        }

        public void UpdateChalan(ChalanViewModel chalanVM)
        {
            chalanExport = new chalanexport
            {
                ChalanID = chalanVM.ChalanID,
                ChalanNo = chalanVM.ChalanNo,
                ChalanDate = chalanVM.ChalanDate,
                VehicleNo = chalanVM.VehicleNo,
                DriverName = chalanVM.DriverName,
                MobileNo = chalanVM.MobileNo,
                ShippedBy = chalanVM.ShippedBy,
                SealNo = chalanVM.SealNo,
                UserID = chalanVM.UserID,
                SetupDate = chalanVM.SetupDate
            };

            unitOfWork.ChalanExportRepository.Update(chalanExport);

            if (chalanVM.ShipmentList != null)
            {
                foreach (var item in chalanVM.ShipmentList)
                {
                    shipment = unitOfWork.ShipmentRepository.Get()
                                .Where(x => x.ChalanID == chalanVM.ChalanID 
                                    && x.PurchaseOrderID == item.PurchaseOrderID)
                                    .FirstOrDefault();

                    shipment = shipment ?? new shipment();

                    shipment.PurchaseOrderID = item.PurchaseOrderID;

                    shipment.ChalanID = chalanVM.ChalanID;
                    shipment.ChalanQuantity = item.ChalanQuantity;
                    shipment.ChalanDate = chalanVM.ChalanDate;
                    shipment.CBM = item.CBM;
                    shipment.CartoonQuantity = item.CartoonQuantity;

                    shipment.UserID = chalanVM.UserID;
                    shipment.SetupDate = chalanVM.SetupDate;

                    unitOfWork.ShipmentRepository.Insert(shipment);
                }
            }

            var shipmentList = shipmentLogic.GetAllShipmentByChalan(chalanVM.ChalanID);

            foreach (var item in shipmentList)
            {
                unitOfWork.ShipmentRepository.Delete(new shipment { ShipmentID = item.ShipmentID });
            }

            unitOfWork.Save();
        }

        public IQueryable<ChalanViewModel> GetAllChalan()
        {
            var result = (from s in unitOfWork.ChalanExportRepository.Get()
                          select new ChalanViewModel
                          {
                              ChalanID = s.ChalanID,
                              ChalanNo = s.ChalanNo,
                              ChalanDate = s.ChalanDate,
                              VehicleNo = s.VehicleNo,
                              DriverName = s.DriverName,
                              MobileNo = s.MobileNo,
                              ShippedBy = s.ShippedBy,
                              SealNo = s.SealNo,

                              UserID = s.UserID,
                              SetupDate = s.SetupDate
                          }).AsQueryable();

            return result;
        }

        public ChalanViewModel GetChalanByID(int id)
        {
            var result = (from s in unitOfWork.ChalanExportRepository.Get()
                          where s.ChalanID == id
                          select new ChalanViewModel
                          {
                              ChalanID = s.ChalanID,
                              ChalanNo = s.ChalanNo,
                              ChalanDate = s.ChalanDate,
                              VehicleNo = s.VehicleNo,
                              DriverName = s.DriverName,
                              MobileNo = s.MobileNo,
                              ShippedBy = s.ShippedBy,
                              SealNo = s.SealNo,

                              UserID = s.UserID,
                              SetupDate = s.SetupDate
                          }).SingleOrDefault();

            return result;
        }

        public List<DropDownListViewModel> GetChalanDropDown()
        {
            var result = (from s in unitOfWork.ChalanExportRepository.Get()
                          select new DropDownListViewModel
                          {
                              Text = s.ChalanNo,
                              Value = s.ChalanID
                          }).ToList();

            return result;
        }

        public bool IsUniqueChalan(string chalanNo, Nullable<int> chalanID = null)
        {
            IQueryable<int> result;

            if (chalanID == null)
            {
                result = from s in unitOfWork.ChalanExportRepository.Get()
                         where s.ChalanNo == chalanNo
                         select s.ChalanID;
            }
            else
            {
                result = from s in unitOfWork.ChalanExportRepository.Get()
                         where s.ChalanNo == chalanNo & s.ChalanID != chalanID
                         select s.ChalanID;
            }

            if (result.Count() > 0)
            {
                return false;
            }
            return true;
        }
    }
}
