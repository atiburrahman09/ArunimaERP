USE [ERP]
GO
/****** Object:  StoredProcedure [dbo].[GetPostSheetData]    Script Date: 11/22/2017 11:50:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[GetPostSheetData]
(
	@fromDate datetime,
	@toDate datetime
)
AS
BEGIN
	SET NOCOUNT ON;

	--#############################################################################################################################################
	-- OffStandard Data
	--#############################################################################################################################################
	CREATE TABLE #OffStandard
	(
		[SpecID] [int] NOT NULL,
		[OperationID] [int] NOT NULL,
		[Section] [int] NOT NULL,
		[WorkingDate] [datetime] NOT NULL,
		[EmployeeCardNo] [nvarchar](max) NULL,
		[NW] [nvarchar](max) NULL,
		[NWEGP] [float] NULL,
		[NWDuration] [float] NULL,
		[MT] [nvarchar](max) NULL,
		[MTEGP] [float] NULL,
		[MTDuration] [float] NULL,
		[TB] [nvarchar](max) NULL,
		[TBEGP] [float] NULL,
		[TBDuration] [float] NULL,
		[MISC] [nvarchar](max) NULL,
		[MISCEGP] [float] NULL,
		[MISCDuration] [float] NULL,	
		[BU] [nvarchar](max) NULL,
		[BUEGP] [float] NULL,
		[BUDuration] [float] NULL,
		[AL] [nvarchar](max) NULL,
		[ALEGP] [float] NULL,
		[ALDuration] [float] NULL,
		[LC] [nvarchar](max) NULL,
		[LCEGP] [float] NULL,
		[LCDuration] [float] NULL
	)

	INSERT INTO #OffStandard (SpecID, OperationID, Section,WorkingDate,EmployeeCardNo,LC,LCDuration,LCEGP) 
	(
		SELECT SpecID,OperationID,Section,WorkingDate,EmployeeCardNo,'LC',EGP,Duration 
		FROM gumSheetOffStandards
		WHERE NonStandCode='LC'
		AND CAST(WorkingDate as DATE) BETWEEN CAST(@fromDate as DATE) AND CAST(@toDate as DATE) 
	)

	UPDATE O SET MT='MT',MTEGP=g.EGP , MTDuration=g.Duration
	FROM #OffStandard O
	INNER JOIN gumSheetOffStandards g ON O.SpecID=g.SpecID 
	AND O.Section=g.Section 
	AND O.OperationID= g.OperationID 
	AND O.EmployeeCardNo = g.EmployeeCardNo
	AND O.WorkingDate = g.WorkingDate
	WHERE g.NonStandCode='MT'

	UPDATE O SET TB='TB',TBEGP=g.EGP , TBDuration=g.Duration
	FROM #OffStandard O
	INNER JOIN gumSheetOffStandards g ON O.SpecID=g.SpecID 
	AND O.Section=g.Section 
	AND O.OperationID= g.OperationID 
	AND O.EmployeeCardNo = g.EmployeeCardNo
	AND O.WorkingDate = g.WorkingDate
	WHERE g.NonStandCode='TB'

	UPDATE O SET [MISC]='MISC',[MISCEGP]=g.EGP , [MISCDuration]=g.Duration
	FROM #OffStandard O
	INNER JOIN gumSheetOffStandards g ON O.SpecID=g.SpecID 
	AND O.Section=g.Section 
	AND O.OperationID= g.OperationID 
	AND O.EmployeeCardNo = g.EmployeeCardNo
	AND O.WorkingDate = g.WorkingDate
	WHERE g.NonStandCode='MISC(CC)'

	UPDATE O SET [MISC]='MISC',[MISCEGP]+=g.EGP , [MISCDuration]+=g.Duration
	FROM #OffStandard O
	INNER JOIN gumSheetOffStandards g ON O.SpecID=g.SpecID 
	AND O.Section=g.Section 
	AND O.OperationID= g.OperationID 
	AND O.EmployeeCardNo = g.EmployeeCardNo
	AND O.WorkingDate = g.WorkingDate
	WHERE g.NonStandCode='MISC(OPC)'

	UPDATE O SET BU='BU',BUEGP=g.EGP , BUDuration=g.Duration
	FROM #OffStandard O
	INNER JOIN gumSheetOffStandards g ON O.SpecID=g.SpecID 
	AND O.Section=g.Section 
	AND O.OperationID= g.OperationID 
	AND O.EmployeeCardNo = g.EmployeeCardNo
	AND O.WorkingDate = g.WorkingDate
	WHERE g.NonStandCode='BU'

	UPDATE O SET AL='AL',ALEGP=g.EGP , ALDuration=g.Duration
	FROM #OffStandard O
	INNER JOIN gumSheetOffStandards g ON O.SpecID=g.SpecID 
	AND O.Section=g.Section 
	AND O.OperationID= g.OperationID 
	AND O.EmployeeCardNo = g.EmployeeCardNo
	AND O.WorkingDate = g.WorkingDate
	WHERE g.NonStandCode='AL'

	UPDATE O SET NW='NW',NWEGP=g.EGP , NWDuration=g.Duration
	FROM #OffStandard O
	INNER JOIN gumSheetOffStandards g ON O.SpecID=g.SpecID 
	AND O.Section=g.Section 
	AND O.OperationID= g.OperationID 
	AND O.EmployeeCardNo = g.EmployeeCardNo
	AND O.WorkingDate = g.WorkingDate
	WHERE g.NonStandCode='NW'

	--SELECT * FROM #OffStandard
	
	--#############################################################################################################################################
	-- Inserting Earnings Data From gumsheet and OffStandard Temp Table Into #Earnings Temp Table
	--#############################################################################################################################################

	CREATE TABLE #Earnings
	(
		[SpecID] [int] NOT NULL,
		[OperationID] [int] NOT NULL,
		[Section] [int] NOT NULL,
		[WorkingDate] [datetime] NOT NULL,
		[EmployeeCardNo] [nvarchar](max) NULL,
		[PM] [float] null,
		[EM] [float] null,
		[SAH] [float] null
	)
	
	INSERT INTO #Earnings (SpecID,OperationID,Section,WorkingDate,EmployeeCardNo,PM,EM,SAH)
	SELECT O.SpecID,O.OperationID,O.Section,O.WorkingDate,O.EmployeeCardNo,
	(ISNULL(SUM(g.EGP), 0) + ISNULL(SUM(O.NWEGP), 0) + ISNULL(SUM(O.MTEGP), 0) + ISNULL(SUM(O.MISCEGP), 0)) AS PM,
	SUM(g.EGP) AS EM,
	SUM(g.Duration) AS SAH
	FROM gumsheets g
	INNER JOIN #OffStandard O ON  CAST(g.CompletedDate as DATE)= CAST(O.WorkingDate as DATE)
	AND g.EmployeeCardNo=O.EmployeeCardNo
	AND g.SpecID=O.SpecID
	AND g.Section=O.Section
	AND CAST(g.CompletedDate as DATE) = CAST(O.WorkingDate as DATE)
	GROUP BY O.SpecID,O.OperationID,O.Section,O.WorkingDate,O.EmployeeCardNo

	--SELECT * FROM #Earnings
	
	--#############################################################################################################################################
	-- Inserting Efficiency Data From gumsheet , Earnings and OffStandard Temp Table Into #Efficiency Temp Table
	--#############################################################################################################################################
	
	CREATE TABLE #Efficiency
	(
		[SpecID] [int] NOT NULL,
		[OperationID] [int] NOT NULL,
		[Section] [int] NOT NULL,
		[WorkingDate] [datetime] NOT NULL,
		[EmployeeCardNo] [nvarchar](max) NULL,
		[P] [float] null,
		[E] [float] null,
		[O] [float] null,
		[TH] [float] null,
		[SH] [float] null
	)

	INSERT INTO #Efficiency (SpecID,OperationID,Section,WorkingDate,EmployeeCardNo,P,E,O,TH,SH)
	SELECT DISTINCT ER.SpecID,ER.OperationID,ER.Section,ER.WorkingDate,ER.EmployeeCardNo,
	(ER.PM / (g.ClockedTime*j.BaseRate))*100 AS P, 
	(ER.SAH / (g.ClockedTime - (IIF(O.NWDuration IS NULL,0,O.NWDuration) + IIF(O.MTDuration IS NULL,0,O.MTDuration) + IIF(O.[MISCDuration] IS NULL,0,O.[MISCDuration]))/60))*100 AS E,
	(ER.SAH / g.ClockedTime)*100 AS O,
	g.ClockedTime AS TH,
	(g.ClockedTime - ((IIF(O.NWDuration IS NULL,0,O.NWDuration) + IIF(O.MTDuration IS NULL,0,O.MTDuration) + IIF(O.[MISCDuration] IS NULL,0,O.[MISCDuration])))/60) AS SH
	FROM #Earnings ER
	LEFT JOIN #OffStandard O ON ER.SpecID=O.SpecID AND ER.OperationID=O.OperationID AND ER.Section=O.Section AND CAST(ER.WorkingDate AS DATE) = CAST(O.WorkingDate AS DATE) AND ER.EmployeeCardNo = O.EmployeeCardNo
	INNER JOIN gumsheets g ON CAST(g.CompletedDate as DATE)= CAST(O.WorkingDate as DATE)  AND g.EmployeeCardNo=O.EmployeeCardNo  AND g.SpecID=O.SpecID AND g.Section=O.Section
	INNER JOIN Operations OP ON O.OperationID=Op.OperationID
	INNER JOIN JobClasses j ON OP.JobClassID = j.JobClassID

	--SELECT * FROM #Efficiency

	--#############################################################################################################################################
	-- Retriving Employee Data
	--#############################################################################################################################################
	CREATE TABLE #EmployeeInfo
	(
		[EmployeeCardNo][nvarchar](max) NOT NULL,
		[EmployeeName][nvarchar](max) NULL,				
		[OTRate][float] NULL,[JoiningDate][Date] NULL
	)

	--INSERT INTO #EmployeeInfo (EmployeeName,EmployeeCardNo,JoiningDate,BasicSalary,GrossSalary,OTRate) VALUES('A','3214071','2001-01-01',8500,15000,20.50)
	--INSERT INTO #EmployeeInfo (EmployeeName,EmployeeCardNo,JoiningDate,BasicSalary,GrossSalary,OTRate) VALUES('B','1440331','2001-01-01',8500,15000,20.50)

	INSERT INTO #EmployeeInfo
	SELECT EMP.CardNo as EmployeeCardNo, EMP.EmployeeName,
	(SELECT SM.Amount/104 FROM ScopoHR..SalaryMappings SM WHERE SM.SalaryTypeID = 1 AND SM.EmployeeID = EMP.EmployeeID) AS OTRate,
	EMP.JoinDate
	FROM ScopoHR..Employees EMP
	WHERE EMP.CardNo = '3214071' OR EMP.CardNo='1440331'

	
	--#############################################################################################################################################
	-- Inserting Summary Data
	--#############################################################################################################################################
	CREATE TABLE #Summary
	(
		[SpecID] [int] NOT NULL,
		[OperationID] [int] NOT NULL,
		[Section] [int] NOT NULL,
		[WorkingDate] [datetime] NOT NULL,
		[EmployeeCardNo] [nvarchar](max) NULL,
		[EmployeeName] [nvarchar](max) null,
		[OT] [float] null,
		[OTTH] [float] null,
		[PP] [float] null,
		[NP] [float] null,
		[CSD] [float] null,
		[TotalPaid] [float] null,
		[JoiningDate] [date] null,
	)

	INSERT INTO #Summary
	SELECT A.SpecID,A.OperationID,A.Section,A.WorkingDate, E.EmployeeCardNo,E.EmployeeName, ISNULL((SUM(B.TH - 8) * SUM(E.OTRate)),0) as OT, ISNULL(SUM(B.TH-8),0) as OTTH,NULL AS PP , (SUM(A.PM) / SUM(B.TH)) AS NP, (SUM(A.PM) / SUM(A.EM)) AS CSD, ((SUM(B.TH - 8) * SUM(E.OTRate)) + SUM(A.PM)) AS TotalPaid, E.JoiningDate
	FROM
	#EmployeeInfo E
	INNER JOIN #Earnings A ON E.EmployeeCardNo=A.EmployeeCardNo
	INNER JOIN #Efficiency B ON A.EmployeeCardNo=B.EmployeeCardNo AND A.SpecID = B.SpecID AND A.Section = B.Section AND A.WorkingDate = B.WorkingDate AND A.OperationID = B.OperationID
	GROUP BY A.SpecID,A.OperationID,A.Section,A.WorkingDate, E.EmployeeCardNo,E.EmployeeName,E.JoiningDate
	

	--######################################################################################################################################
	--Final Select Statement
	--######################################################################################################################################

	SELECT DISTINCT A.SpecID,A.OperationID,A.Section,A.WorkingDate,A.EmployeeCardNo, A.E,A.O,A.P,A.TH,A.SH, 
	B.PM,B.EM,B.SAH,
	C.NW,C.NWDuration,C.NWEGP,C.MT,C.MTDuration,C.MTEGP,C.[MISC],C.[MISCDuration],C.[MISCEGP],
	C.TB,C.TBDuration,C.TBEGP,C.BU,C.BUDuration,C.BUEGP,C.AL,C.ALDuration,C.ALEGP,C.LC,C.LCDuration,C.LCEGP ,
	S.EmployeeName,S.JoiningDate,S.NP,S.OT,S.OTTH,S.PP,S.TotalPaid,S.CSD,
	SV.SupervisorName AS Supervisor,
	O.OperationName
	FROM #Efficiency A 
	INNER JOIN #Earnings B ON A.EmployeeCardNo=B.EmployeeCardNo AND A.SpecID = B.SpecID AND A.Section = B.Section AND A.WorkingDate = B.WorkingDate AND A.OperationID = B.OperationID
	INNER JOIN #OffStandard C ON C.EmployeeCardNo=B.EmployeeCardNo AND C.SpecID = B.SpecID AND C.Section = B.Section AND C.WorkingDate = B.WorkingDate AND C.OperationID = B.OperationID
	INNER JOIN #Summary S ON C.EmployeeCardNo=S.EmployeeCardNo AND C.SpecID = S.SpecID AND C.Section = S.Section AND C.WorkingDate = S.WorkingDate AND C.OperationID = S.OperationID
	INNER JOIN StyleOperations SO ON B.SpecID=SO.SpecID AND B.Section=SO.SectionNo
	INNER JOIN supervisors SV ON SO.SupervisorID = SV.SupervisorID
	INNER JOIN Operations O ON S.OperationID = O.OperationID

	--EXEC [GetPostSheetData] '2017-11-01','2017-11-02'

	DROP TABLE #OffStandard
	DROP TABLE #Earnings
	DROP TABLE #Efficiency
END


--EXEC [GetPostSheetData] '2017-11-01','2017-11-04'

