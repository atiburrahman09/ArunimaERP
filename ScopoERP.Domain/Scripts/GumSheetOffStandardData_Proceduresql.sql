USE [ERP]
GO
/****** Object:  StoredProcedure [dbo].[GetOffStandardValueForCalculation]    Script Date: 11/6/2017 4:47:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[GetOffStandardValueForCalculation] @empCardNo nvarchar(20),@completedDate date, @specNo nvarchar(50)
AS

Select CAST((j.BaseRate/60) as float) as V3, 
	   dbo.getV6V2(@empCardNo,@completedDate,DATEADD(Day, -7, @completedDate),@specNo) as V6,dbo.getV6V2(@empCardNo,@completedDate,DATEADD(Day, -28, @completedDate),@specNo) as V2 ,dbo.getO2() as O2,
	   j.BaseRate as BaseRate, j.MaxPaid as MaxPaid
from JobClasses J 
INNER JOIN Operations O ON J.JobClassID = O.JobClassID
INNER JOIN Specs S on O.OperationID= S.OperationID
LEFT JOIN gumsheets G ON S.SpecID = G.SpecID AND G.EmployeeCardNo = @empCardNo
LEFT JOIN gumSheetOffStandards GF on GF.EmployeeCardNo = G.EmployeeCardNo
WHERE 
S.SpecNo = @specNo
GROUP BY J.BaseRate, J.MaxPaid


--EXEC GetOffStandardValueForCalculation '3214071','11/1/2017 12:00:00 AM','84001'