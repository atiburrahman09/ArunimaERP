USE [ERP]
GO
/****** Object:  Table [dbo].[coupons]    Script Date: 11/6/2017 4:29:03 PM ******/
SET ANSI_NULLS ON
GO

DROP TABLE coupons
DROP TABLE EmployeeRates
DROP TABLE gumsheets
DROP TABLE gumSheetOffStandards
DROP TABLE TrainingCurves

SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[coupons](
	[CouponID] [int] IDENTITY(1,1) NOT NULL,
	[SerialNo] [int] NOT NULL,
	[BundleID] [int] NOT NULL,
	[OperationID] [int] NOT NULL,
	[SpecID] [int] NULL,
	[CuttingPlanID] [int] NOT NULL,
	[OperationCategoryID] [int] NOT NULL,
	[StyleOperationID] [int] NOT NULL,
	[PoStyleId] [int] NOT NULL,
	[JobClassID] [int] NOT NULL,
	[SupervisorID] [int] NULL,
	[Quantity] [int] NOT NULL,
	[Time] [real] NOT NULL,
	[Size] [nvarchar](max) NULL,
	[BaseRate] [decimal](18, 2) NOT NULL,
	[Value] [real] NULL,
	[CompletedDate] [datetime] NULL,
	[SectionNo] [int] NOT NULL,
	[EmployeeCardNo] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.coupons] PRIMARY KEY CLUSTERED 
(
	[CouponID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[EmployeeRates]    Script Date: 11/6/2017 4:29:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmployeeRates](
	[EmployeeRateID] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeCardNo] [nvarchar](max) NULL,
	[Curve] [nvarchar](max) NULL,
	[Stage] [int] NULL,
	[RTorNHorFL] [nvarchar](max) NULL,
	[Section] [int] NOT NULL,
	[SpecNo] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.EmployeeRates] PRIMARY KEY CLUSTERED 
(
	[EmployeeRateID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[gumSheetOffStandards]    Script Date: 11/6/2017 4:29:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[gumSheetOffStandards](
	[GumSheetOffStandardID] [int] IDENTITY(1,1) NOT NULL,
	[SpecID] [int] NOT NULL,
	[OperationID] [int] NOT NULL,
	[Section] [int] NOT NULL,
	[WorkingDate] [datetime] NOT NULL,
	[NonStandCode] [nvarchar](max) NULL,
	[Duration] [float] NOT NULL,
	[EGP] [float] NOT NULL,
	[Remark] [nvarchar](max) NULL,
	[EmployeeCardNo] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.gumSheetOffStandards] PRIMARY KEY CLUSTERED 
(
	[GumSheetOffStandardID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[gumsheets]    Script Date: 11/6/2017 4:29:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[gumsheets](
	[GumSheetID] [int] IDENTITY(1,1) NOT NULL,
	[CompletedDate] [datetime] NULL,
	[ClockedTime] [float] NOT NULL,
	[Section] [int] NOT NULL,
	[MachineTrouble] [bit] NOT NULL,
	[PayMethod] [bit] NOT NULL,
	[LearningCurve] [float] NULL,
	[Allowance] [float] NULL,
	[SpecID] [int] NULL,
	[EmployeeCardNo] [int] NULL,
	[BundleNo] [nvarchar](50) NOT NULL CONSTRAINT [DF__gumsheets__Bundl__4A6ECD84]  DEFAULT ((0)),
	[Duration] [float] NOT NULL CONSTRAINT [DF__gumsheets__Durat__4B62F1BD]  DEFAULT ((0)),
	[EGP] [float] NOT NULL CONSTRAINT [DF__gumsheets__EGP__4C5715F6]  DEFAULT ((0)),
 CONSTRAINT [PK_dbo.gumsheets] PRIMARY KEY CLUSTERED 
(
	[GumSheetID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TrainingCurves]    Script Date: 11/6/2017 4:29:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TrainingCurves](
	[TrainingCurveID] [int] IDENTITY(1,1) NOT NULL,
	[Stage] [int] NOT NULL,
	[Curve_1] [float] NULL,
	[Curve_1A] [float] NULL,
	[Curve_2] [float] NULL,
	[Curve_3] [float] NULL,
	[Curve_4] [float] NULL,
	[Curve_5] [float] NULL,
	[Curve_6] [float] NULL,
	[Curve_New] [float] NULL,
	[Curve] [int] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_dbo.TrainingCurves] PRIMARY KEY CLUSTERED 
(
	[TrainingCurveID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[coupons]  WITH CHECK ADD  CONSTRAINT [FK_dbo.coupons_dbo.Bundles_BundleID] FOREIGN KEY([BundleID])
REFERENCES [dbo].[Bundles] ([BundleID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[coupons] CHECK CONSTRAINT [FK_dbo.coupons_dbo.Bundles_BundleID]
GO
ALTER TABLE [dbo].[coupons]  WITH CHECK ADD  CONSTRAINT [FK_dbo.coupons_dbo.cuttingPlans_CuttingPlanID] FOREIGN KEY([CuttingPlanID])
REFERENCES [dbo].[cuttingPlans] ([CuttingPlanID])
GO
ALTER TABLE [dbo].[coupons] CHECK CONSTRAINT [FK_dbo.coupons_dbo.cuttingPlans_CuttingPlanID]
GO
ALTER TABLE [dbo].[coupons]  WITH CHECK ADD  CONSTRAINT [FK_dbo.coupons_dbo.Operations_OperationID] FOREIGN KEY([OperationID])
REFERENCES [dbo].[Operations] ([OperationID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[coupons] CHECK CONSTRAINT [FK_dbo.coupons_dbo.Operations_OperationID]
GO
ALTER TABLE [dbo].[coupons]  WITH CHECK ADD  CONSTRAINT [FK_dbo.coupons_dbo.StyleOperations_StyleOperationID] FOREIGN KEY([StyleOperationID])
REFERENCES [dbo].[StyleOperations] ([StyleOperationID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[coupons] CHECK CONSTRAINT [FK_dbo.coupons_dbo.StyleOperations_StyleOperationID]
GO
