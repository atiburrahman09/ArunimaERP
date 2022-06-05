namespace ScopoERP.Domain.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ScopoContext : DbContext
    {
        public ScopoContext()
            : base("name=ScopoContext")
        {
        }

        public virtual DbSet<account> account { get; set; }
        public virtual DbSet<advancedcm> advancedcm { get; set; }
        public virtual DbSet<Applications> Applications { get; set; }
        public virtual DbSet<approval> approval { get; set; }
        public virtual DbSet<backtobacklc> backtobacklc { get; set; }
        public virtual DbSet<bank> bank { get; set; }
        public virtual DbSet<bankforwarding> bankforwarding { get; set; }
        public virtual DbSet<bl> bl { get; set; }
        public virtual DbSet<bldetails> bldetails { get; set; }
        public virtual DbSet<booking> booking { get; set; }
        public virtual DbSet<bookinghistory> bookinghistory { get; set; }
        public virtual DbSet<buyerinfo> buyerinfo { get; set; }
        public virtual DbSet<chalan> chalan { get; set; }
        public virtual DbSet<chalanexport> chalanexport { get; set; }
        public virtual DbSet<consumptionunit> consumptionunit { get; set; }
        public virtual DbSet<customerinfo> customerinfo { get; set; }
        public virtual DbSet<department> department { get; set; }
        public virtual DbSet<devision> devision { get; set; }
        public virtual DbSet<excessbooking> excessbooking { get; set; }
        public virtual DbSet<factory> factory { get; set; }
        public virtual DbSet<floorline> floorline { get; set; }
        public virtual DbSet<initialcostsheet> initialcostsheet { get; set; }
        public virtual DbSet<inventoryissue> inventoryissue { get; set; }
        public virtual DbSet<inventoryreceive> inventoryreceive { get; set; }
        public virtual DbSet<invoice> invoice { get; set; }
        public virtual DbSet<item> item { get; set; }
        public virtual DbSet<itemcategory> itemcategory { get; set; }
        public virtual DbSet<jobinfo> jobinfo { get; set; }
        public virtual DbSet<machine> machine { get; set; }
        public virtual DbSet<machinecategory> machinecategory { get; set; }
        public virtual DbSet<Memberships> Memberships { get; set; }
        public virtual DbSet<paymenttype> paymenttype { get; set; }
        public virtual DbSet<piinfo> piinfo { get; set; }
        public virtual DbSet<pocostsheet> pocostsheet { get; set; }
        public virtual DbSet<postyle> postyle { get; set; }
        public virtual DbSet<productiondailyreport> productiondailyreport { get; set; }
        public virtual DbSet<productionplanning> productionplanning { get; set; }
        public virtual DbSet<Profiles> Profiles { get; set; }
        public virtual DbSet<purchaserequisition> purchaserequisition { get; set; }
        public virtual DbSet<purchaserequisitiondetails> purchaserequisitiondetails { get; set; }
        public virtual DbSet<purchaserequisitioninstallment> purchaserequisitioninstallment { get; set; }
        public virtual DbSet<realization> realization { get; set; }
        public virtual DbSet<realizationaccount> realizationaccount { get; set; }
        public virtual DbSet<requisition> requisition { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<sampleapproval> sampleapproval { get; set; }
        public virtual DbSet<sampletype> sampletype { get; set; }
        public virtual DbSet<season> season { get; set; }
        public virtual DbSet<shipment> shipment { get; set; }
        public virtual DbSet<sizecolor> sizecolor { get; set; }
        public virtual DbSet<sr> sr { get; set; }
        public virtual DbSet<styleimage> styleimage { get; set; }
        public virtual DbSet<styleinfo> styleinfo { get; set; }
        public virtual DbSet<subcontract> subcontract { get; set; }
        public virtual DbSet<supplier> supplier { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<worksheets> worksheets { get; set; }
        public virtual DbSet<useraccount> useraccount { get; set; }
        public DbSet<coupon> coupon { get; set; }
        public DbSet<cuttingPlan> cuttingPlan { get; set; }
        public DbSet<gumsheet> gumsheet { get; set; }

        public DbSet<StyleOperation> StyleOperation { get; set; }
        public DbSet<Operation> StandardOperations { get; set; }
        public DbSet<JobClass> JobClasses { get; set; }
        public DbSet<Bundle> Bundles { get; set; }
        public DbSet<supervisor> Supervisors { get; set; }
        public DbSet<OperationCategory> OperationCategories { get; set; }
        public DbSet<Spec> Specs { get; set; }
        public DbSet<TrainingCurve> TrainingCurves { get; set; }
        public DbSet<EmployeeCapability> EmployeeCapabilities { get; set; }
        public DbSet<EmployeeRate> EmployeeRates { get; set; }
        public DbSet<gumSheetOffStandard> GumSheetOffStandards { get; set; }

        public virtual DbSet<ChartOfAccount> ChartOfAccount { get; set; }
        public virtual DbSet<Budget> Budget { get; set; }
        public virtual DbSet<Expense> Expense { get; set; }
        public virtual DbSet<BankPRCTracking> BankPRCTrackings { get; set; }
        public virtual DbSet<TimeLine> Timelines { get; set; }
        public virtual DbSet<POEmployeeMapping> POEmployeeMappings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<ScopoContext>(null);
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<account>()
                .Property(e => e.AccountName)
                .IsUnicode(false);

            modelBuilder.Entity<account>()
                .HasMany(e => e.requisition)
                .WithRequired(e => e.account)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<account>()
                .HasMany(e => e.styleinfo)
                .WithRequired(e => e.account)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<account>()
                .HasMany(e => e.useraccount)
                .WithRequired(e => e.account)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<advancedcm>()
                .Property(e => e.PIValue)
                .HasPrecision(20, 5);

            modelBuilder.Entity<advancedcm>()
                .Property(e => e.ConversionRate)
                .HasPrecision(10, 4);

            modelBuilder.Entity<advancedcm>()
                .Property(e => e.ReceivedAmount)
                .HasPrecision(18, 4);

            modelBuilder.Entity<advancedcm>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<Applications>()
                .HasMany(e => e.Memberships)
                .WithRequired(e => e.Applications)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Applications>()
                .HasMany(e => e.Roles)
                .WithRequired(e => e.Applications)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Applications>()
                .HasMany(e => e.Users)
                .WithRequired(e => e.Applications)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<approval>()
                .Property(e => e.SampleType)
                .IsUnicode(false);

            modelBuilder.Entity<approval>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<backtobacklc>()
                .Property(e => e.BackToBackLCValue)
                .HasPrecision(15, 5);

            modelBuilder.Entity<backtobacklc>()
                .HasMany(e => e.bl)
                .WithOptional(e => e.backtobacklc)
                .WillCascadeOnDelete();

            modelBuilder.Entity<bank>()
                .Property(e => e.BankName)
                .IsUnicode(false);

            modelBuilder.Entity<bank>()
                .Property(e => e.BankAddress)
                .IsUnicode(false);

            modelBuilder.Entity<bankforwarding>()
                .Property(e => e.FDBPNo)
                .IsUnicode(false);

            modelBuilder.Entity<bl>()
                .Property(e => e.AcceptanceValue)
                .HasPrecision(20, 5);

            modelBuilder.Entity<bl>()
                .HasMany(e => e.bldetails)
                .WithRequired(e => e.bl)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<bldetails>()
                .Property(e => e.InvoiceQuantity)
                .HasPrecision(18, 4);

            modelBuilder.Entity<bldetails>()
                .Property(e => e.ReceivedQuantity)
                .HasPrecision(18, 4);

            modelBuilder.Entity<booking>()
                .Property(e => e.ItemSize)
                .IsUnicode(false);

            modelBuilder.Entity<booking>()
                .Property(e => e.ItemColor)
                .IsUnicode(false);

            modelBuilder.Entity<booking>()
                .Property(e => e.TotalQuantity)
                .HasPrecision(18, 4);

            modelBuilder.Entity<booking>()
                .Property(e => e.UnitPrice)
                .HasPrecision(10, 5);

            modelBuilder.Entity<bookinghistory>()
                .Property(e => e.ItemSize)
                .IsUnicode(false);

            modelBuilder.Entity<bookinghistory>()
                .Property(e => e.ItemColor)
                .IsUnicode(false);

            modelBuilder.Entity<bookinghistory>()
                .Property(e => e.TotalQuantity)
                .HasPrecision(18, 4);

            modelBuilder.Entity<bookinghistory>()
                .Property(e => e.UnitPrice)
                .HasPrecision(10, 5);

            modelBuilder.Entity<buyerinfo>()
                .Property(e => e.BuyerName)
                .IsUnicode(false);

            modelBuilder.Entity<buyerinfo>()
                .HasMany(e => e.styleinfo)
                .WithRequired(e => e.buyerinfo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<chalan>()
                .Property(e => e.ChalanNo)
                .IsUnicode(false);

            modelBuilder.Entity<chalan>()
                .Property(e => e.ReceivedBy)
                .IsUnicode(false);

            modelBuilder.Entity<chalan>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<chalan>()
                .HasMany(e => e.inventoryreceive)
                .WithRequired(e => e.chalan)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<chalanexport>()
                .Property(e => e.ChalanNo)
                .IsUnicode(false);

            modelBuilder.Entity<chalanexport>()
                .Property(e => e.VehicleNo)
                .IsUnicode(false);

            modelBuilder.Entity<chalanexport>()
                .Property(e => e.DriverName)
                .IsUnicode(false);

            modelBuilder.Entity<chalanexport>()
                .Property(e => e.MobileNo)
                .IsUnicode(false);

            modelBuilder.Entity<chalanexport>()
                .Property(e => e.ShippedBy)
                .IsUnicode(false);

            modelBuilder.Entity<chalanexport>()
                .Property(e => e.SealNo)
                .IsUnicode(false);

            modelBuilder.Entity<consumptionunit>()
                .Property(e => e.UnitName)
                .IsUnicode(false);

            modelBuilder.Entity<consumptionunit>()
                .HasMany(e => e.booking)
                .WithRequired(e => e.consumptionunit)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<consumptionunit>()
                .HasMany(e => e.initialcostsheet)
                .WithRequired(e => e.consumptionunit)
                .HasForeignKey(e => e.ConsumptionUnitId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<consumptionunit>()
                .HasMany(e => e.initialcostsheet1)
                .WithRequired(e => e.consumptionunit1)
                .HasForeignKey(e => e.ConversionUnit)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<consumptionunit>()
                .HasMany(e => e.worksheets)
                .WithRequired(e => e.consumptionunit)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<customerinfo>()
                .Property(e => e.CustomerName)
                .IsUnicode(false);

            modelBuilder.Entity<customerinfo>()
                .HasMany(e => e.styleinfo)
                .WithRequired(e => e.customerinfo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<department>()
                .Property(e => e.DepartmentName)
                .IsUnicode(false);

            modelBuilder.Entity<department>()
                .HasMany(e => e.purchaserequisition)
                .WithRequired(e => e.department)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<devision>()
                .Property(e => e.DevisionName)
                .IsUnicode(false);

            modelBuilder.Entity<devision>()
                .HasMany(e => e.styleinfo)
                .WithRequired(e => e.devision)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<excessbooking>()
                .Property(e => e.ItemColor)
                .IsUnicode(false);

            modelBuilder.Entity<excessbooking>()
                .Property(e => e.ItemSize)
                .IsUnicode(false);

            modelBuilder.Entity<excessbooking>()
                .Property(e => e.TotalQuantity)
                .HasPrecision(20, 3);

            modelBuilder.Entity<excessbooking>()
                .Property(e => e.TotalPrice)
                .HasPrecision(20, 5);

            modelBuilder.Entity<factory>()
                .Property(e => e.FactoryName)
                .IsUnicode(false);

            modelBuilder.Entity<factory>()
                .HasMany(e => e.postyle)
                .WithRequired(e => e.factory)
                .WillCascadeOnDelete(false);
           
            modelBuilder.Entity<factory>()
                .HasMany(e => e.subcontract)
                .WithRequired(e => e.factory)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<floorline>()
                .Property(e => e.Floor)
                .IsUnicode(false);

            modelBuilder.Entity<floorline>()
                .Property(e => e.Line)
                .IsUnicode(false);

            modelBuilder.Entity<floorline>()
                .Property(e => e.Devision)
                .IsUnicode(false);

            modelBuilder.Entity<initialcostsheet>()
                .Property(e => e.Consumption)
                .HasPrecision(20, 10);

            modelBuilder.Entity<initialcostsheet>()
                .Property(e => e.Wastage)
                .HasPrecision(10, 5);

            modelBuilder.Entity<initialcostsheet>()
                .Property(e => e.ActualPrice)
                .HasPrecision(20, 10);

            modelBuilder.Entity<initialcostsheet>()
                .Property(e => e.OfferToBuyer)
                .HasPrecision(20, 10);

            modelBuilder.Entity<initialcostsheet>()
                .Property(e => e.ConversionQuantity)
                .HasPrecision(20, 10);

            modelBuilder.Entity<initialcostsheet>()
                .Property(e => e.ActualConsumption)
                .HasPrecision(20, 10);

            modelBuilder.Entity<inventoryissue>()
                .Property(e => e.IssuedQuantity)
                .HasPrecision(18, 4);

            modelBuilder.Entity<inventoryreceive>()
                .Property(e => e.ReceivedQuantity)
                .HasPrecision(18, 4);

            modelBuilder.Entity<invoice>()
                .Property(e => e.InvoiceNo)
                .IsUnicode(false);

            modelBuilder.Entity<invoice>()
                .Property(e => e.InvoiceValue)
                .HasPrecision(20, 10);

            modelBuilder.Entity<invoice>()
                .Property(e => e.BL)
                .IsUnicode(false);

            modelBuilder.Entity<invoice>()
                .Property(e => e.BLToBeEndorsedTo)
                .IsUnicode(false);

            modelBuilder.Entity<invoice>()
                .Property(e => e.EXP)
                .IsUnicode(false);

            modelBuilder.Entity<invoice>()
                .Property(e => e.FCR)
                .IsUnicode(false);

            modelBuilder.Entity<invoice>()
                .Property(e => e.FDBP_No)
                .IsUnicode(false);

            modelBuilder.Entity<invoice>()
                .Property(e => e.Courier)
                .IsUnicode(false);

            modelBuilder.Entity<invoice>()
                .Property(e => e.ShippingBill)
                .IsUnicode(false);

            modelBuilder.Entity<invoice>()
                .Property(e => e.PortOfLoading)
                .IsUnicode(false);

            modelBuilder.Entity<invoice>()
                .Property(e => e.FinalDestination)
                .IsUnicode(false);

            modelBuilder.Entity<invoice>()
                .Property(e => e.CountryName)
                .IsUnicode(false);

            modelBuilder.Entity<item>()
                .Property(e => e.ItemCode)
                .IsUnicode(false);

            modelBuilder.Entity<item>()
                .Property(e => e.ItemDescription)
                .IsUnicode(false);

            modelBuilder.Entity<item>()
                .HasMany(e => e.booking)
                .WithRequired(e => e.item)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<item>()
                .HasMany(e => e.initialcostsheet)
                .WithRequired(e => e.item)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<item>()
                .HasMany(e => e.worksheets)
                .WithRequired(e => e.item)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<itemcategory>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<itemcategory>()
                .HasMany(e => e.initialcostsheet)
                .WithRequired(e => e.itemcategory)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<itemcategory>()
                .HasMany(e => e.itemcategory1)
                .WithOptional(e => e.itemcategory2)
                .HasForeignKey(e => e.ParentCategoryId);

            modelBuilder.Entity<jobinfo>()
                .Property(e => e.JobNo)
                .IsUnicode(false);

            modelBuilder.Entity<jobinfo>()
                .Property(e => e.ContractNo)
                .IsUnicode(false);

            modelBuilder.Entity<jobinfo>()
                .Property(e => e.ExtraContractNo)
                .IsUnicode(false);

            modelBuilder.Entity<jobinfo>()
                .Property(e => e.AdvancedCMPercentage)
                .HasPrecision(7, 4);

            modelBuilder.Entity<jobinfo>()
                .HasMany(e => e.advancedcm)
                .WithRequired(e => e.jobinfo)
                .HasForeignKey(e => e.JobID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<jobinfo>()
                .HasMany(e => e.backtobacklc)
                .WithOptional(e => e.jobinfo)
                .HasForeignKey(e => e.JobID);

            modelBuilder.Entity<jobinfo>()
                .HasMany(e => e.excessbooking)
                .WithRequired(e => e.jobinfo)
                .HasForeignKey(e => e.JobID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<jobinfo>()
                .HasMany(e => e.piinfo)
                .WithOptional(e => e.jobinfo)
                .HasForeignKey(e => e.LoanFromJobID);

            modelBuilder.Entity<jobinfo>()
                .HasMany(e => e.postyle)
                .WithOptional(e => e.jobinfo)
                .HasForeignKey(e => e.JobId);

            modelBuilder.Entity<jobinfo>()
                .HasMany(e => e.requisition)
                .WithRequired(e => e.jobinfo)
                .HasForeignKey(e => e.JobID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<machine>()
                .Property(e => e.MachineCode)
                .IsUnicode(false);


            modelBuilder.Entity<machinecategory>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<machinecategory>()
                .HasMany(e => e.machine)
                .WithRequired(e => e.machinecategory)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<machinecategory>()
                .HasMany(e => e.machinecategory1)
                .WithOptional(e => e.machinecategory2)
                .HasForeignKey(e => e.ParentCategoryID);

            modelBuilder.Entity<paymenttype>()
                .Property(e => e.PaymentTitle)
                .IsUnicode(false);

            modelBuilder.Entity<paymenttype>()
                .HasMany(e => e.backtobacklc)
                .WithOptional(e => e.paymenttype)
                .HasForeignKey(e => e.LCTypeID);

            modelBuilder.Entity<piinfo>()
                .Property(e => e.ReferenceNo)
                .IsUnicode(false);

            modelBuilder.Entity<piinfo>()
                .Property(e => e.PINo)
                .IsUnicode(false);

            modelBuilder.Entity<piinfo>()
                .HasMany(e => e.advancedcm)
                .WithRequired(e => e.piinfo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<piinfo>()
                .HasMany(e => e.excessbooking)
                .WithRequired(e => e.piinfo)
                .HasForeignKey(e => e.ProformaInvoiceID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<pocostsheet>()
                .Property(e => e.CostSheetNo)
                .IsUnicode(false);

            modelBuilder.Entity<postyle>()
                .Property(e => e.PoNo)
                .IsUnicode(false);

            modelBuilder.Entity<postyle>()
                .Property(e => e.Fob)
                .HasPrecision(15, 10);

            modelBuilder.Entity<postyle>()
                .Property(e => e.AgreedCm)
                .HasPrecision(12, 7);

            modelBuilder.Entity<postyle>()
                .Property(e => e.DCCode)
                .IsUnicode(false);

            modelBuilder.Entity<postyle>()
                .Property(e => e.SubContractRate)
                .HasPrecision(10, 5);

            modelBuilder.Entity<postyle>()
                .Property(e => e.FactoryCM)
                .HasPrecision(12, 7);

            modelBuilder.Entity<postyle>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<postyle>()
                .Property(e => e.CostSheetNo)
                .IsUnicode(false);

            modelBuilder.Entity<postyle>()
                .HasMany(e => e.booking)
                .WithRequired(e => e.postyle)
                .HasForeignKey(e => e.PurchaseOrderID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<postyle>()
                .HasMany(e => e.productionplanning)
                .WithRequired(e => e.postyle)
                .WillCascadeOnDelete(false);
            
            modelBuilder.Entity<postyle>()
                .HasMany(e => e.shipment)
                .WithRequired(e => e.postyle)
                .HasForeignKey(e => e.PurchaseOrderID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<postyle>()
                .HasMany(e => e.subcontract)
                .WithRequired(e => e.postyle)
                .HasForeignKey(e => e.PurchaseOrderID);

            modelBuilder.Entity<postyle>()
                .HasMany(e => e.worksheets)
                .WithRequired(e => e.postyle)
                .WillCascadeOnDelete(false);
            
            modelBuilder.Entity<productiondailyreport>()
                .Property(e => e.Floor)
                .IsUnicode(false);

            modelBuilder.Entity<productiondailyreport>()
                .Property(e => e.Line)
                .IsUnicode(false);

            modelBuilder.Entity<productiondailyreport>()
                .Property(e => e.Color)
                .IsUnicode(false);

            modelBuilder.Entity<purchaserequisition>()
                .Property(e => e.RequisitionNo)
                .IsUnicode(false);

            modelBuilder.Entity<purchaserequisition>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<purchaserequisition>()
                .HasMany(e => e.purchaserequisitiondetails)
                .WithOptional(e => e.purchaserequisition)
                .WillCascadeOnDelete();

            modelBuilder.Entity<purchaserequisitiondetails>()
                .Property(e => e.ProductDescription)
                .IsUnicode(false);

            modelBuilder.Entity<purchaserequisitiondetails>()
                .Property(e => e.Quantity)
                .HasPrecision(18, 4);

            modelBuilder.Entity<purchaserequisitiondetails>()
                .Property(e => e.UnitPrice)
                .HasPrecision(18, 4);

            modelBuilder.Entity<purchaserequisitioninstallment>()
                .Property(e => e.Amount)
                .HasPrecision(18, 4);

            modelBuilder.Entity<purchaserequisitioninstallment>()
                .Property(e => e.PayableAmount)
                .HasPrecision(18, 4);

            modelBuilder.Entity<realization>()
                .Property(e => e.Amount)
                .HasPrecision(18, 4);

            modelBuilder.Entity<realization>()
                .Property(e => e.CurrencyRate)
                .HasPrecision(10, 4);

            modelBuilder.Entity<realizationaccount>()
                .Property(e => e.RealizationAccountName)
                .IsUnicode(false);

            modelBuilder.Entity<realizationaccount>()
                .Property(e => e.RealizationAccountNo)
                .IsUnicode(false);

            modelBuilder.Entity<requisition>()
                .Property(e => e.RequisitionNo)
                .IsUnicode(false);

            modelBuilder.Entity<Roles>()
                .HasMany(e => e.Users)
                .WithMany(e => e.Roles)
                .Map(m => m.ToTable("UsersInRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<sampleapproval>()
                .Property(e => e.ApprovalSerialNo)
                .IsUnicode(false);

            modelBuilder.Entity<sampletype>()
                .Property(e => e.SampleTypeName)
                .IsUnicode(false);

            modelBuilder.Entity<season>()
                .Property(e => e.SeasonName)
                .IsUnicode(false);

            modelBuilder.Entity<season>()
                .HasMany(e => e.postyle)
                .WithRequired(e => e.season)
                .WillCascadeOnDelete(false);
            
            modelBuilder.Entity<shipment>()
                .Property(e => e.CBM)
                .HasPrecision(10, 4);

            modelBuilder.Entity<shipment>()
                .Property(e => e.CartoonQuantity)
                .HasPrecision(10, 4);

            modelBuilder.Entity<shipment>()
                .Property(e => e.InvoiceFOB)
                .HasPrecision(18, 4);

            modelBuilder.Entity<shipment>()
                .Property(e => e.Destination)
                .IsUnicode(false);

            modelBuilder.Entity<sizecolor>()
                .Property(e => e.Size)
                .IsUnicode(false);

            modelBuilder.Entity<sizecolor>()
                .Property(e => e.Color)
                .IsUnicode(false);

            modelBuilder.Entity<sizecolor>()
                .Property(e => e.FOB)
                .HasPrecision(15, 10);

            modelBuilder.Entity<sr>()
                .Property(e => e.SRNo)
                .IsUnicode(false);

            modelBuilder.Entity<sr>()
                .Property(e => e.IssuedBy)
                .IsUnicode(false);

            modelBuilder.Entity<sr>()
                .Property(e => e.ReceiverName)
                .IsUnicode(false);

            modelBuilder.Entity<sr>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<sr>()
                .HasMany(e => e.inventoryissue)
                .WithRequired(e => e.sr)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<styleimage>()
                .Property(e => e.ImageUrl)
                .IsUnicode(false);

            modelBuilder.Entity<styleinfo>()
                .Property(e => e.StyleNo)
                .IsUnicode(false);

            modelBuilder.Entity<styleinfo>()
                .Property(e => e.StyleDescription)
                .IsUnicode(false);

            modelBuilder.Entity<styleinfo>()
                .Property(e => e.Sam)
                .HasPrecision(5, 1);

            modelBuilder.Entity<styleinfo>()
                .Property(e => e.BodyStyle)
                .IsUnicode(false);

            modelBuilder.Entity<styleinfo>()
                .Property(e => e.Item)
                .IsUnicode(false);

            modelBuilder.Entity<styleinfo>()
                .Property(e => e.Febrication)
                .IsUnicode(false);

            modelBuilder.Entity<styleinfo>()
                .HasMany(e => e.initialcostsheet)
                .WithRequired(e => e.styleinfo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<subcontract>()
                .Property(e => e.SubContractNo)
                .IsUnicode(false);

            modelBuilder.Entity<subcontract>()
                .Property(e => e.SubContractRate)
                .HasPrecision(10, 8);

            modelBuilder.Entity<subcontract>()
                .Property(e => e.CommercialPercentage)
                .HasPrecision(10, 4);

            modelBuilder.Entity<subcontract>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<supplier>()
                .Property(e => e.SupplierName)
                .IsUnicode(false);

            modelBuilder.Entity<supplier>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<supplier>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<supplier>()
                .Property(e => e.ContactNumber)
                .IsUnicode(false);

            modelBuilder.Entity<Users>()
                .HasOptional(e => e.Memberships)
                .WithRequired(e => e.Users);

            modelBuilder.Entity<Users>()
                .HasOptional(e => e.Profiles)
                .WithRequired(e => e.Users);

            modelBuilder.Entity<worksheets>()
                .Property(e => e.Size)
                .IsUnicode(false);

            modelBuilder.Entity<worksheets>()
                .Property(e => e.Color)
                .IsUnicode(false);

            modelBuilder.Entity<worksheets>()
                .Property(e => e.ItemSize)
                .IsUnicode(false);

            modelBuilder.Entity<worksheets>()
                .Property(e => e.ItemColor)
                .IsUnicode(false);

            modelBuilder.Entity<worksheets>()
                .Property(e => e.Consumption)
                .HasPrecision(10, 5);

            modelBuilder.Entity<worksheets>()
                .Property(e => e.Wastage)
                .HasPrecision(10, 5);

            modelBuilder.Entity<worksheets>()
                .Property(e => e.UnitPrice)
                .HasPrecision(10, 5);

            modelBuilder.Entity<worksheets>()
                .Property(e => e.TotalQuantity)
                .HasPrecision(18, 4);
        }
    }
}
