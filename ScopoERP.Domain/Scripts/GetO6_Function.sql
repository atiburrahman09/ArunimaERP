USE [ERP]
GO
/****** Object:  UserDefinedFunction [dbo].[GetV6V2]    Script Date: 11/6/2017 4:48:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER FUNCTION [dbo].[GetV6V2](@empCardNo nvarchar(50), @completedDate date,@fromDate date, @specNo as nvarChar(50))
RETURNS float
AS
BEGIN
DECLARE @V3 float = 
	(Select  ((Sum(g.EGP)+Sum(gf.EGP))/7)/(Sum(g.ClockedTime)/7)
from gumsheets g
LEFT JOIN gumSheetOffStandards gf on gf.EmployeeCardNo= g.EmployeeCardNo
LEFT JOIN Specs s on g.SpecID= s.SpecID
LEFT JOIN Operations o on s.OperationID=o.OperationID
LEFT JOIN JobClasses j on o.JobClassID=j.JobClassID

where g.EmployeeCardNo = @empCardNo AND CAST(g.CompletedDate as Date) BETWEEN CAST(@fromDate as DATE) AND CAST(@completedDate as DATE)
AND s.SpecNo=@specNo
)
RETURN @V3
END;
