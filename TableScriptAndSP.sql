CREATE DATABASE LOANLEADDB
USE LOANLEADDB
GO
/****** Object:  Table [dbo].[ContactDetails]    Script Date: 1/29/2021 1:22:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContactDetails](
	[ContactId] [int] IDENTITY(1,1) NOT NULL,
	[LeadId] [int] NULL,
	[ContactType] [varchar](50) NULL,
	[ContactName] [varchar](100) NULL,
	[Dob] [datetime] NULL,
	[Gender] [varchar](10) NULL,
	[Email] [varchar](50) NULL,
	[ContactNumber] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[ContactId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LoanLead]    Script Date: 1/29/2021 1:22:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoanLead](
	[LeadId] [int] IDENTITY(1,1) NOT NULL,
	[LoanAmount] [int] NULL,
	[LeadSource] [varchar](50) NULL,
	[CommunicationMode] [varchar](50) NULL,
	[Status] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[LeadId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[sp_getLead]    Script Date: 1/29/2021 1:22:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[sp_getLead](
@leadId int
) as
begin

select LL.LoanAmount,LL.LeadSource,LL.CommunicationMode,LL.Status,
(select CD.LeadId,CD.ContactType,CD.ContactName,CD.Dob,CD.Gender,CD.Email,CD.ContactNumber from ContactDetails CD
inner join LoanLead LL on CD.LeadId = LL.LeadId for json path
) as 'ContactDetails'
from LoanLead LL where LL.LeadId = @leadId
for json path
end
GO
/****** Object:  StoredProcedure [dbo].[sp_saveLeadInfo]    Script Date: 1/29/2021 1:22:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[sp_saveLeadInfo](
@LoanAmount int,
@LeadSource varchar(100),
@CommunicationMode varchar(50),
@Status varchar(50),
@leadId int output
)
as
begin

insert into LoanLead(LoanAmount,LeadSource,CommunicationMode,Status) values(@LoanAmount,@LeadSource,@CommunicationMode,@Status)

select @leadId = @@IDENTITY
end
GO
