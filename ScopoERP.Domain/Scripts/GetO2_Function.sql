USE [ERP]
GO
/****** Object:  UserDefinedFunction [dbo].[getO2]    Script Date: 11/6/2017 4:47:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER FUNCTION [dbo].[getO2]()
RETURNS float
AS
BEGIN
DECLARE @O2 float = 
	(SELECT BaseRate/60
from JobClasses

where JobClassName='Plan_Min'
)
RETURN @O2
END;

--select BaseRate/60 from JobClasses where JobClassName = 'Plan_Min'