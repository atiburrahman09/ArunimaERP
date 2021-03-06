USE [ERP]
GO
/****** Object:  StoredProcedure [dbo].[GetGhumSheetData]    Script Date: 11/6/2017 4:47:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[GetGhumSheetData] @empCardNo nvarchar(20),@completedDate date
AS

SELECT A.OffDURATION as OffStandardDuration, A.OffEGP as OffStandardEarn, B.DURATION as ProductionTotalDuration, B.EGP as ProductionEarn, B.ClockedTime FROM
  (SELECT GS.EmployeeCardNo, GS.WorkingDate, SUM(DURATION) OffDURATION, SUM(EGP) OffEGP FROM [gumSheetOffStandards] GS 
  GROUP BY GS.EmployeeCardNo, GS.WorkingDate) A
  INNER JOIN 
  (SELECT GS.EmployeeCardNo, GS.CompletedDate, GS.ClockedTime, SUM(DURATION) DURATION, SUM(EGP) EGP FROM [gumsheets] GS 
  GROUP BY GS.EmployeeCardNo, GS.CompletedDate, GS.ClockedTime) B 
  ON A.EmployeeCardNo = B.EmployeeCardNo AND A.WorkingDate = B.CompletedDate

--EXEC GetGhumSheetData '3214071','11/4/2017 12:00:00 AM'