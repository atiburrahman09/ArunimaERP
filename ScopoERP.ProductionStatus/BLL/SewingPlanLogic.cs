using ScopoERP.Common.ViewModel;
using ScopoERP.Domain.Models;
using ScopoERP.Domain.Repositories;
using ScopoERP.ProductionStatus.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.ProductionStatus.BLL
{
    public class SewingPlanLogic
    {
        private UnitOfWork unitOfWork;
        private POEmployeeMapping poEmployeeMapping;

        public SewingPlanLogic(UnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }


        public IEnumerable<ProductionPlanViewModel> GetAll()
        {
            var data = unitOfWork.ProductionPlanningRepository.Get()
                .Select(x => new ProductionPlanViewModel
                {
                    FloorLineID = x.FloorLineID,
                    ProductionPlanningID = x.PoductionPlanningID,
                    Capacity = x.Capacity,
                    EndDate = x.EndDate,
                    StartDate = x.StartDate,
                    PoStyleID = x.PoStyleID,
                    Quantity = x.Quantity,
                    FloorName = x.floorline.Floor,
                    PurchaseOrderNo = x.postyle.PoNo
                }).AsEnumerable();

            return data;
        }

        public ProductionPlanViewModel GetById(int? id)
        {
            var data = unitOfWork.ProductionPlanningRepository.Get()
                .Where(x => x.PoductionPlanningID == id)
                .Select(x => new ProductionPlanViewModel
                {
                    FloorLineID = x.floorline.FloorLineId,
                    ProductionPlanningID = x.PoductionPlanningID,
                    Capacity = x.Capacity,
                    EndDate = x.EndDate,
                    StartDate = x.StartDate,
                    PoStyleID = x.PoStyleID,
                    Quantity = x.Quantity,
                    FloorName = x.floorline.Floor,
                    StyleID = x.postyle.StyleId
                })
                .SingleOrDefault();
            return data;
        }

        public IEnumerable<DropDownListViewModel> GetFLoorLineDropdown()
        {
            var data = unitOfWork.ProductionFloorRepository.Get()
                .Select(x => new DropDownListViewModel
                {
                    Text = x.Floor,
                    Value = x.FloorLineId
                }).AsEnumerable();
            return data;
        }

        public void Update(ProductionPlanViewModel model)
        {
            var prPlan = unitOfWork.ProductionPlanningRepository
                    .GetById(model.ProductionPlanningID);
            prPlan.Capacity = model.Capacity;
            prPlan.EndDate = model.EndDate;
            prPlan.StartDate = model.StartDate;
            prPlan.PoStyleID = model.PoStyleID;
            prPlan.Quantity = model.Quantity;
            prPlan.FloorLineID = model.FloorLineID;

            unitOfWork.ProductionPlanningRepository.Update(prPlan);
            unitOfWork.Save();
        }

        public void Create(ProductionPlanViewModel model)
        {
            var prPlan = new productionplanning()
            {
                Capacity = model.Capacity,
                EndDate = model.EndDate,
                StartDate = model.StartDate,
                PoStyleID = model.PoStyleID,
                Quantity = model.Quantity,
                FloorLineID = model.FloorLineID,
            };
            unitOfWork.ProductionPlanningRepository.Insert(prPlan);
            unitOfWork.Save();
        }

        public void Delete(int? id)
        {
            unitOfWork.ProductionPlanningRepository.Delete(unitOfWork.ProductionPlanningRepository.GetById(id));
            unitOfWork.Save();
        }

        public object GetEmployeeList(string floor)
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT E.CardNo AS Text,E.EmployeeName AS ValueString, E.EmployeeID AS Value ");
            query.Append("FROM ScopoHR..Employees E ");
            query.Append("INNER JOIN ScopoHR..ProductionFloorLines P ON E.ProductionFloorLineID = P.ProductionFloorLineID ");
            query.Append("WHERE P.Floor = '" + floor.ToString() + "' ");
            query.Append("AND E.IsActive =1");

            var res = unitOfWork.ProductionPlanningRepository.SelectQuery<DropDownListViewModel>(query.ToString()).ToList();
            return res;

        }

        public object GetProductionPlanningData(int? id)
        {
            var data = unitOfWork.ProductionPlanningRepository.Get()
               .Where(x => x.PoductionPlanningID == id)
               .Select(x => new ProductionPlanViewModel
               {
                   FloorLineID = x.floorline.FloorLineId,
                   ProductionPlanningID = x.PoductionPlanningID,
                   Capacity = x.Capacity,
                   EndDate = x.EndDate,
                   StartDate = x.StartDate,
                   PoStyleID = x.PoStyleID,
                   Quantity = x.Quantity,
                   FloorName = x.floorline.Floor,
                   StyleID = x.postyle.StyleId,
                   StyleNo = x.postyle.styleinfo.StyleNo,
                   PoNo = x.postyle.PoNo
               })
               .SingleOrDefault();
            return data;
        }

        public object GetPoEmployeeMappingData(int? id)
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT E.CardNo AS Text, E.EmployeeName AS ValueString,E.EmployeeID AS Value ");
            query.Append("FROM POEmployeeMappings P ");
            query.Append("INNER JOIN ScopoHR..Employees E ON E.EmployeeID = P.EmployeeID ");
            query.Append("WHERE P.ProductionPlanningID='" + id +"'");


            var res = unitOfWork.POEmployeeMappingRepository.SelectQuery<DropDownListViewModel>(query.ToString()).ToList();
            return res;
        }

        public void SavePOEmployeeMapping(List<PoEmployeeMappingViewModel> model)
        {
            unitOfWork.POEmployeeMappingRepository.RawQuery("DELETE FROM POEmployeeMappings WHERE ProductionPlanningID='" + model[0].ProductionPlanningID + "'");

            for (var i = 0; i < model.Count(); i++)
            {
                poEmployeeMapping = new POEmployeeMapping
                {
                    ProductionPlanningID = model[i].ProductionPlanningID,
                    EmployeeID = model[i].EmployeeID
                };
                unitOfWork.POEmployeeMappingRepository.Insert(poEmployeeMapping);
            }
            unitOfWork.Save();

        }
    }
}
