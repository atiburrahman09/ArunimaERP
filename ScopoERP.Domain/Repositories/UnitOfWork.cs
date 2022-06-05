using ScopoERP.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScopoERP.Domain.Repositories
{
    public class UnitOfWork
    {
        private ScopoContext db;

        public UnitOfWork(ScopoContext db)
        {
            this.db = db;
        }
        
        public void Save()
        {
             db.SaveChanges();
        }
        
        private IRepository<account> accountRepo;
        public IRepository<account> AccountRepository
        {
            get
            {
                if (this.accountRepo == null)
                {
                    this.accountRepo = new Repository<account>(db);
                }
                return accountRepo;
            }
        }
        
        private IRepository<buyerinfo> buyerRepo;
        public IRepository<buyerinfo> BuyerRepository
        {
            get
            {
                if (this.buyerRepo == null)
                {
                    this.buyerRepo = new Repository<buyerinfo>(db);
                }
                return buyerRepo;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private IRepository<customerinfo> customerRepo;
        public IRepository<customerinfo> CustomerRepository
        {
            get
            {
                if (this.customerRepo == null)
                {
                    this.customerRepo = new Repository<customerinfo>(db);
                }
                return customerRepo;
            }
        }

        /// <summary>
        /// Supplier
        /// </summary>
        private IRepository<supplier> supplierRepo;
        public IRepository<supplier> SupplierRepository
        {
            get
            {
                if (this.supplierRepo == null)
                {
                    this.supplierRepo = new Repository<supplier>(db);
                }
                return supplierRepo;
            }
        }

        /// <summary>
        /// Style
        /// </summary>
        private IRepository<styleinfo> styleRepo;
        public IRepository<styleinfo> StyleRepository
        {
            get
            {
                if (this.styleRepo == null)
                {
                    this.styleRepo = new Repository<styleinfo>(db);
                }
                return styleRepo;
            }
        }

        /// <summary>
        /// Purchase Order
        /// </summary>
        private IRepository<postyle> purchaseOrderRepo;
        public IRepository<postyle> PurchaseOrderRepository
        {
            get
            {
                if (this.purchaseOrderRepo == null)
                {
                    this.purchaseOrderRepo = new Repository<postyle>(db);
                }
                return purchaseOrderRepo;
            }
        }

        /// <summary>
        /// Size Color
        /// </summary>
        private IRepository<sizecolor> sizeColorRepo;
        public IRepository<sizecolor> SizeColorRepository
        {
            get
            {
                if (this.sizeColorRepo == null)
                {
                    this.sizeColorRepo = new Repository<sizecolor>(db);
                }
                return sizeColorRepo;
            }
        }

        /// <summary>
        /// Division
        /// </summary>
        private IRepository<devision> divisionRepo;
        public IRepository<devision> DivisionRepository
        {
            get
            {
                if (this.divisionRepo == null)
                {
                    this.divisionRepo = new Repository<devision>(db);
                }
                return divisionRepo;
            }
        }

        /// <summary>
        /// Factory
        /// </summary>
        private IRepository<factory> factoryRepo;
        public IRepository<factory> FactoryRepository
        {
            get
            {
                if (this.factoryRepo == null)
                {
                    this.factoryRepo = new Repository<factory>(db);
                }
                return factoryRepo;
            }
        }

        /// <summary>
        /// Season
        /// </summary>
        private IRepository<season> seasonRepo;
        public IRepository<season> SeasonRepository
        {
            get
            {
                if (this.seasonRepo == null)
                {
                    this.seasonRepo = new Repository<season>(db);
                }
                return seasonRepo;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private IRepository<jobinfo> jobRepo;
        public IRepository<jobinfo> JobRepository
        {
            get
            {
                if (this.jobRepo == null)
                {
                    this.jobRepo = new Repository<jobinfo>(db);
                }
                return jobRepo;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private IRepository<invoice> exportInvoiceRepo;
        public IRepository<invoice> ExportInvoiceRepository
        {
            get
            {
                if (this.exportInvoiceRepo == null)
                {
                    this.exportInvoiceRepo = new Repository<invoice>(db);
                }
                return exportInvoiceRepo;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private IRepository<productiondailyreport> productionStatusRepo;
        public IRepository<productiondailyreport> ProductionStatusRepository
        {
            get
            {
                if (this.productionStatusRepo == null)
                {
                    this.productionStatusRepo = new Repository<productiondailyreport>(db);
                }
                return productionStatusRepo;
            }
        }

        /// <summary>
        /// Item
        /// </summary>
        private IRepository<item> itemRepo;
        public IRepository<item> ItemRepository
        {
            get
            {
                if (this.itemRepo == null)
                {
                    this.itemRepo = new Repository<item>(db);
                }
                return itemRepo;
            }
        }

        /// <summary>
        /// Item Category
        /// </summary>
        private IRepository<itemcategory> itemCategoryRepo;
        public IRepository<itemcategory> ItemCategoryRepository
        {
            get
            {
                if (this.itemCategoryRepo == null)
                {
                    this.itemCategoryRepo = new Repository<itemcategory>(db);
                }
                return itemCategoryRepo;
            }
        }

        /// <summary>
        /// Consumption Unit
        /// </summary>
        private IRepository<consumptionunit> consumptionUnitRepo;
        public IRepository<consumptionunit> ConsumptionUnitRepository
        {
            get
            {
                if (this.consumptionUnitRepo == null)
                {
                    this.consumptionUnitRepo = new Repository<consumptionunit>(db);
                }
                return consumptionUnitRepo;
            }
        }

        /// <summary>
        /// Costsheet
        /// </summary>
        private IRepository<initialcostsheet> costsheetRepo;
        public IRepository<initialcostsheet> CostsheetRepository
        {
            get
            {
                if (this.costsheetRepo == null)
                {
                    this.costsheetRepo = new Repository<initialcostsheet>(db);
                }
                return costsheetRepo;
            }
        }

        /// <summary>
        /// Worksheet
        /// </summary>
        private IRepository<worksheets> worksheetRepo;
        public IRepository<worksheets> WorksheetRepository
        {
            get
            {
                if (this.worksheetRepo == null)
                {
                    this.worksheetRepo = new Repository<worksheets>(db);
                }
                return worksheetRepo;
            }
        }

        /// <summary>
        /// Booking
        /// </summary>
        private IRepository<booking> bookingRepo;
        public IRepository<booking> BookingRepository
        {
            get
            {
                if (this.bookingRepo == null)
                {
                    this.bookingRepo = new Repository<booking>(db);
                }
                return bookingRepo;
            }
        }

        /// <summary>
        /// Booking History
        /// </summary>
        private IRepository<bookinghistory> bookingHistoryRepo;
        public IRepository<bookinghistory> BookingHistoryRepository
        {
            get
            {
                if (this.bookingHistoryRepo == null)
                {
                    this.bookingHistoryRepo = new Repository<bookinghistory>(db);
                }
                return bookingHistoryRepo;
            }
        }

        /// <summary>
        /// PI
        /// </summary>
        private IRepository<piinfo> piRepo;
        public IRepository<piinfo> PIRepository
        {
            get
            {
                if (this.piRepo == null)
                {
                    this.piRepo = new Repository<piinfo>(db);
                }
                return piRepo;
            }
        }

        /// <summary>
        /// Advance CM
        /// </summary>
        private IRepository<advancedcm> advancedCMRepo;
        public IRepository<advancedcm> AdvancedCMRepository
        {
            get
            {
                if (this.advancedCMRepo == null)
                {
                    this.advancedCMRepo = new Repository<advancedcm>(db);
                }
                return advancedCMRepo;
            }
        }

        /// <summary>
        /// Machine
        /// </summary>
        private IRepository<machine> machineRepo;
        public IRepository<machine> MachineRepository
        {
            get
            {
                if (this.machineRepo == null)
                {
                    this.machineRepo = new Repository<machine>(db);
                }
                return machineRepo;
            }
        }

        /// <summary>
        /// Machine Category
        /// </summary>
        private IRepository<machinecategory> machineCategoryRepo;
        public IRepository<machinecategory> MachineCategoryRepository
        {
            get
            {
                if (this.machineCategoryRepo == null)
                {
                    this.machineCategoryRepo = new Repository<machinecategory>(db);
                }
                return machineCategoryRepo;
            }
        }

        /// <summary>
        /// Back To Back LC
        /// </summary>
        private IRepository<backtobacklc> backToBackLCRepo;
        public IRepository<backtobacklc> BackToBackLCRepository
        {
            get
            {
                if (this.backToBackLCRepo == null)
                {
                    this.backToBackLCRepo = new Repository<backtobacklc>(db);
                }
                return backToBackLCRepo;
            }
        }

        /// <summary>
        /// Payment/LC Type
        /// </summary>
        private IRepository<paymenttype> lcTypeRepo;
        public IRepository<paymenttype> LCTypeRepository
        {
            get
            {
                if (this.lcTypeRepo == null)
                {
                    this.lcTypeRepo = new Repository<paymenttype>(db);
                }
                return lcTypeRepo;
            }
        }

        /// <summary>
        /// Floor Line
        /// </summary>
        private IRepository<floorline> productionFloorRepo;
        public IRepository<floorline> ProductionFloorRepository
        {
            get
            {
                if (this.productionFloorRepo == null)
                {
                    this.productionFloorRepo = new Repository<floorline>(db);
                }
                return productionFloorRepo;
            }
        }

        /// <summary>
        /// Machine Category
        /// </summary>
        private IRepository<productionplanning> productionPlanningRepo;
        public IRepository<productionplanning> ProductionPlanningRepository
        {
            get
            {
                if (this.productionPlanningRepo == null)
                {
                    this.productionPlanningRepo = new Repository<productionplanning>(db);
                }
                return productionPlanningRepo;
            }
        }

        /// <summary>
        /// Export Chalan
        /// </summary>
        private IRepository<chalanexport> chalanExportRepo;
        public IRepository<chalanexport> ChalanExportRepository
        {
            get
            {
                if (this.chalanExportRepo == null)
                {
                    this.chalanExportRepo = new Repository<chalanexport>(db);
                }
                return chalanExportRepo;
            }
        }

        /// <summary>
        /// Shipment
        /// </summary>
        private IRepository<shipment> shipmentRepo;
        public IRepository<shipment> ShipmentRepository
        {
            get
            {
                if (this.shipmentRepo == null)
                {
                    this.shipmentRepo = new Repository<shipment>(db);
                }
                return shipmentRepo;
            }
        }

        /// <summary>
        /// User
        /// </summary>
        private IRepository<Users> userRepo;
        public IRepository<Users> UserRepository
        {
            get
            {
                if (this.userRepo == null)
                {
                    this.userRepo = new Repository<Users>(db);
                }
                return userRepo;
            }
        }

        /// <summary>
        /// User Account
        /// </summary>
        private IRepository<useraccount> userAccountRepo;
        public IRepository<useraccount> UserAccountRepository
        {
            get
            {
                if (this.userAccountRepo == null)
                {
                    this.userAccountRepo = new Repository<useraccount>(db);
                }
                return userAccountRepo;
            }
        }

        /// <summary>
        /// Shipment
        /// </summary>
        private IRepository<pocostsheet> pocostsheetRepo;
        public IRepository<pocostsheet> POCostsheetRepository
        {
            get
            {
                if (this.pocostsheetRepo == null)
                {
                    this.pocostsheetRepo = new Repository<pocostsheet>(db);
                }
                return pocostsheetRepo;
            }
        }

        /// <summary>
        /// BL
        /// </summary>
        private IRepository<bl> blRepo;
        public IRepository<bl> BLRepository
        {
            get
            {
                if (this.blRepo == null)
                {
                    this.blRepo = new Repository<bl>(db);
                }
                return blRepo;
            }
        }

        /// <summary>
        /// BL Details
        /// </summary>
        private IRepository<bldetails> blDetailsRepo;
        public IRepository<bldetails> BLDetailsRepository
        {
            get
            {
                if (this.blDetailsRepo == null)
                {
                    this.blDetailsRepo = new Repository<bldetails>(db);
                }
                return blDetailsRepo;
            }
        }

        /// <summary>
        /// Requisition
        /// </summary>
        private IRepository<requisition> requisitionRepo;
        public IRepository<requisition> RequisitionRepository
        {
            get
            {
                if (this.requisitionRepo == null)
                {
                    this.requisitionRepo = new Repository<requisition>(db);
                }
                return requisitionRepo;
            }
        }

        /// <summary>
        /// Excess Booking
        /// </summary>
        private IRepository<excessbooking> excessBookingRepo;
        public IRepository<excessbooking> ExcessBookingRepository
        {
            get
            {
                if (this.excessBookingRepo == null)
                {
                    this.excessBookingRepo = new Repository<excessbooking>(db);
                }
                return excessBookingRepo;
            }
        }

        /// <summary>
        /// Bank Forwarding
        /// </summary>
        private IRepository<bankforwarding> bankForwardingRepo;
        public IRepository<bankforwarding> BankForwardingRepository
        {
            get
            {
                if (this.bankForwardingRepo == null)
                {
                    this.bankForwardingRepo = new Repository<bankforwarding>(db);
                }
                return bankForwardingRepo;
            }
        }

        /// <summary>
        /// Sample Type
        /// </summary>
        private IRepository<sampletype> sampleTypeRepo;
        public IRepository<sampletype> SampleTypeRepository
        {
            get
            {
                if (this.sampleTypeRepo == null)
                {
                    this.sampleTypeRepo = new Repository<sampletype>(db);
                }
                return sampleTypeRepo;
            }
        }

        /// <summary>
        /// Sample Type
        /// </summary>
        private IRepository<sampleapproval> sampleApprovalRepo;
        public IRepository<sampleapproval> SampleApprovalRepository
        {
            get
            {
                if (this.sampleApprovalRepo == null)
                {
                    this.sampleApprovalRepo = new Repository<sampleapproval>(db);
                }
                return sampleApprovalRepo;
            }
        }

        /// <summary>
        /// Realization
        /// </summary>
        private IRepository<realization> realizationRepo;
        public IRepository<realization> RealizationRepository
        {
            get
            {
                if (this.realizationRepo == null)
                {
                    this.realizationRepo = new Repository<realization>(db);
                }
                return realizationRepo;
            }
        }

        /// <summary>
        /// Realization
        /// </summary>
        private IRepository<realizationaccount> realizationAccountRepo;
        public IRepository<realizationaccount> RealizationAccountRepository
        {
            get
            {
                if (this.realizationAccountRepo == null)
                {
                    this.realizationAccountRepo = new Repository<realizationaccount>(db);
                }
                return realizationAccountRepo;
            }
        }

        /// <summary>
        /// Bank
        /// </summary>
        private IRepository<bank> bankRepo;
        public IRepository<bank> BankRepository
        {
            get
            {
                if (this.bankRepo == null)
                {
                    this.bankRepo = new Repository<bank>(db);
                }
                return bankRepo;
            }
        }

        /// <summary>
        /// Role
        /// </summary>
        private IRepository<Roles> roleRepo;
        public IRepository<Roles> RoleRepository
        {
            get
            {
                if (this.roleRepo == null)
                {
                    this.roleRepo = new Repository<Roles>(db);
                }
                return roleRepo;
            }
        }

        /// <summary>
        /// Sub Contract
        /// </summary>
        private IRepository<subcontract> subContractRepo;
        public IRepository<subcontract> SubContractRepository
        {
            get
            {
                if (this.subContractRepo == null)
                {
                    this.subContractRepo = new Repository<subcontract>(db);
                }
                return subContractRepo;
            }
        }


        /// <summary>
        /// Purchase Requisition
        /// </summary>
        private IRepository<purchaserequisition> purchaseRequisitionRepo;
        public IRepository<purchaserequisition> PurchaseRequisitionRepository
        {
            get
            {
                if (this.purchaseRequisitionRepo == null)
                {
                    this.purchaseRequisitionRepo = new Repository<purchaserequisition>(db);
                }
                return purchaseRequisitionRepo;
            }
        }


        /// <summary>
        /// Purchase Requisition Details
        /// </summary>
        private IRepository<purchaserequisitiondetails> purchaseRequisitionDetailsRepo;
        public IRepository<purchaserequisitiondetails> PurchaseRequisitionDetailsRepository
        {
            get
            {
                if (this.purchaseRequisitionDetailsRepo == null)
                {
                    this.purchaseRequisitionDetailsRepo = new Repository<purchaserequisitiondetails>(db);
                }
                return purchaseRequisitionDetailsRepo;
            }
        }


        /// <summary>
        /// Purchase Requisition Installment
        /// </summary>
        private IRepository<purchaserequisitioninstallment> purchaseRequisitionInstallmentRepo;
        public IRepository<purchaserequisitioninstallment> PurchaseRequisitionInstallmentRepository
        {
            get
            {
                if (this.purchaseRequisitionInstallmentRepo == null)
                {
                    this.purchaseRequisitionInstallmentRepo = new Repository<purchaserequisitioninstallment>(db);
                }
                return purchaseRequisitionInstallmentRepo;
            }
        }


        /// <summary>
        /// Department
        /// </summary>
        private IRepository<department> departmentRepo;
        public IRepository<department> DepartmentRepository
        {
            get
            {
                if (this.departmentRepo == null)
                {
                    this.departmentRepo = new Repository<department>(db);
                }
                return departmentRepo;
            }
        }
        
        private IRepository<StyleOperation> styleOperationRepo;
        public IRepository<StyleOperation> StyleOperationRepository
        {
            get
            {
                if (this.styleOperationRepo == null)
                {
                    this.styleOperationRepo = new Repository<StyleOperation>(db);
                }

                return styleOperationRepo;
            }
        }

        private IRepository<Operation> standardOperationRepo;
        public IRepository<Operation> StandardOperationRepository
        {
            get
            {
                if (this.standardOperationRepo == null)
                {
                    this.standardOperationRepo = new Repository<Operation>(db);
                }
                return standardOperationRepo;
            }
        }
        
        private IRepository<JobClass> jobClassRepo;
        public IRepository<JobClass> JobClassRepository
        {
            get
            {
                if (this.jobClassRepo == null)
                {
                    this.jobClassRepo = new Repository<JobClass>(db);
                }
                return jobClassRepo;
            }
        }


        private IRepository<cuttingPlan> cuttingPlanRepo;
        public IRepository<cuttingPlan> cuttingPlanRepository
        {
            get
            {
                if (this.cuttingPlanRepo == null)
                {
                    this.cuttingPlanRepo = new Repository<cuttingPlan>(db);
                }
                return cuttingPlanRepo;
            }
        }

        private IRepository<coupon> couponRepo;
        public IRepository<coupon> CouponRepository
        {
            get
            {
                if (this.couponRepo == null)
                {
                    this.couponRepo = new Repository<coupon>(db);
                }
                return couponRepo;
            }
        }
        
        private IRepository<Bundle> bundleRepo;
        public IRepository<Bundle> BundleRepository
        {
            get
            {
                if (this.bundleRepo == null)
                {
                    this.bundleRepo = new Repository<Bundle>(db);
                }
                return bundleRepo;
            }
        }

        private IRepository<gumsheet> gumsheetRepo;
        public IRepository<gumsheet> GumSheetRepository
        {
            get
            {
                if (this.gumsheetRepo == null)
                {
                    this.gumsheetRepo = new Repository<gumsheet>(db);
                }
                return gumsheetRepo;
            }
        }
        private IRepository<supervisor> supervisorRepo;
        public IRepository<supervisor> SupervisorRepository
        {
            get
            {
                if (this.supervisorRepo == null)
                {
                    this.supervisorRepo = new Repository<supervisor>(db);
                }
                return supervisorRepo;
            }
        }

        private IRepository<inventoryissue> inventoryissueRepo;
        public IRepository<inventoryissue> InventoryissueRepository
        {
            get
            {
                if (this.inventoryissueRepo == null)
                {
                    this.inventoryissueRepo = new Repository<inventoryissue>(db);
                }
                return inventoryissueRepo;
            }
        }

        private IRepository<sr> srRepo;
        public IRepository<sr> SrRepository
        {
            get
            {
                if (this.srRepo == null)
                {
                    this.srRepo = new Repository<sr>(db);
                }
                return srRepo;
            }
        }

        private IRepository<OperationCategory> operationCategoryRepo;
        public IRepository<OperationCategory> OperationCategoryRepository
        {
            get
            {
                if (this.operationCategoryRepo == null)
                {
                    this.operationCategoryRepo = new Repository<OperationCategory>(db);
                }
                return operationCategoryRepo;
            }
        }
        private IRepository<Spec> specRepo;
        public IRepository<Spec> SpecRepository
        {
            get
            {
                if (this.specRepo == null)
                {
                    this.specRepo = new Repository<Spec>(db);
                }
                return specRepo;
            }
        }

        private IRepository<TrainingCurve> trainingCurveRepo;
        public IRepository<TrainingCurve> TrainingCurveRepository
        {
            get
            {
                if (this.trainingCurveRepo == null)
                {
                    this.trainingCurveRepo = new Repository<TrainingCurve>(db);
                }
                return trainingCurveRepo;
            }
        }
        private IRepository<EmployeeCapability> employeeCapabilityRepo;
        public IRepository<EmployeeCapability> EmployeeCapabilityRepository
        {
            get
            {
                if (this.employeeCapabilityRepo == null)
                {
                    this.employeeCapabilityRepo = new Repository<EmployeeCapability>(db);
                }
                return employeeCapabilityRepo;
            }
        }
        private IRepository<EmployeeRate> employeeRateRepo;
        public IRepository<EmployeeRate> EmployeeRateRepository
        {
            get
            {
                if (this.employeeRateRepo == null)
                {
                    this.employeeRateRepo = new Repository<EmployeeRate>(db);
                }
                return employeeRateRepo;
            }
        }

        private IRepository<gumSheetOffStandard> gumSheetOffStandardRepo;
        public IRepository<gumSheetOffStandard> GumSheetOffStandardRepository
        {
            get
            {
                if (this.gumSheetOffStandardRepo == null)
                {
                    this.gumSheetOffStandardRepo = new Repository<gumSheetOffStandard>(db);
                }
                return gumSheetOffStandardRepo;
            }
        }

        private IRepository<ChartOfAccount> chartOfAccountRepo;
        public IRepository<ChartOfAccount> ChartOfAccountRepository
        {
            get
            {
                if (this.chartOfAccountRepo == null)
                {
                    this.chartOfAccountRepo = new Repository<ChartOfAccount>(db);
                }
                return chartOfAccountRepo;
            }
        }

        private IRepository<Budget> budgetRepo;
        public IRepository<Budget> BudgetRepository
        {
            get
            {
                if (this.budgetRepo == null)
                {
                    this.budgetRepo = new Repository<Budget>(db);
                }
                return budgetRepo;
            }
        }

        private IRepository<Expense> expenseRepo;
        public IRepository<Expense> ExpenseRepository
        {
            get
            {
                if (this.expenseRepo == null)
                {
                    this.expenseRepo = new Repository<Expense>(db);
                }
                return expenseRepo;
            }
        }

        private IRepository<BankPRCTracking> bankPRCTrackingRepo;
        public IRepository<BankPRCTracking> BankPRCTrackingRepository
        {
            get
            {
                if (this.bankPRCTrackingRepo == null)
                {
                    this.bankPRCTrackingRepo = new Repository<BankPRCTracking>(db);
                }
                return bankPRCTrackingRepo;
            }
        }
        private IRepository<TimeLine> timeLineRepo;
        public IRepository<TimeLine> TimeLineRepository
        {
            get
            {
                if (this.timeLineRepo == null)
                {
                    this.timeLineRepo = new Repository<TimeLine>(db);
                }
                return timeLineRepo;
            }
        }
        private IRepository<POEmployeeMapping> poEmployeeMappingRepo;
        public IRepository<POEmployeeMapping> POEmployeeMappingRepository
        {
            get
            {
                if (this.poEmployeeMappingRepo == null)
                {
                    this.poEmployeeMappingRepo = new Repository<POEmployeeMapping>(db);
                }
                return poEmployeeMappingRepo;
            }
        }
    }
}
