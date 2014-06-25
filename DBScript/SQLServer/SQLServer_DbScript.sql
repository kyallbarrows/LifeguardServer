USE [parichaytest]
GO
/****** Object:  Table [dbo].[aspnet_role]    Script Date: 02/23/2012 15:34:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[aspnet_role](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NULL,
	[LoweredName] [nvarchar](255) NULL,
	[Description] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[aspnet_profiles]    Script Date: 02/23/2012 15:34:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[aspnet_profiles](
	[UserId] [int] NOT NULL,
	[PropertyNames] [nvarchar](255) NULL,
	[PropertyValuesString] [nvarchar](255) NULL,
	[PropertyValuesBinary] [varbinary](8000) NULL,
	[LastActivityDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[aspnet_membershipuser]    Script Date: 02/23/2012 15:34:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[aspnet_membershipuser](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NULL,
	[LoweredName] [nvarchar](255) NULL,
	[Description] [nvarchar](255) NULL,
	[Password] [nvarchar](255) NULL,
	[PasswordFormat] [int] NULL,
	[PasswordSalt] [nvarchar](255) NULL,
	[Email] [nvarchar](255) NULL,
	[LoweredEmail] [nvarchar](255) NULL,
	[PasswordQuestion] [nvarchar](255) NULL,
	[PasswordAnswer] [nvarchar](255) NULL,
	[Comments] [nvarchar](255) NULL,
	[IsApproved] [bit] NULL,
	[IsLockedOut] [bit] NULL,
	[CreationDate] [datetime] NULL,
	[LastActivityDate] [datetime] NULL,
	[LastLoginDate] [datetime] NULL,
	[LastLockedOutDate] [datetime] NULL,
	[LastPasswordChangeDate] [datetime] NULL,
	[FailedPwdAttemptCnt] [int] NULL,
	[FailedPwdAttemptWndStart] [datetime] NULL,
	[FailedPwdAnsAttemptCnt] [int] NULL,
	[FailedPwdAnsAttemptWndStart] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[aspnet_application]    Script Date: 02/23/2012 15:34:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[aspnet_application](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NULL,
	[LoweredName] [nvarchar](255) NULL,
	[Description] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[aspnet_actions]    Script Date: 02/23/2012 15:34:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[aspnet_actions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ControllerName] [nvarchar](255) NOT NULL,
	[ActionName] [nvarchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[lookup_request_type]    Script Date: 02/23/2012 15:34:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[lookup_request_type](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[lookup_country]    Script Date: 02/23/2012 15:34:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[lookup_country](
	[Id] [nvarchar](255) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[lookup_alerttype]    Script Date: 02/23/2012 15:34:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[lookup_alerttype](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ParamCount] [int] NOT NULL,
	[support_email] [nvarchar](255) NULL,
	[Template] [nvarchar](255) NULL,
	[Name] [nvarchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[member_details]    Script Date: 02/23/2012 15:34:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[member_details](
	[P_UserId] [int] NOT NULL,
	[VERSION] [datetime] NOT NULL,
	[P_EMAIL] [nvarchar](255) NOT NULL,
	[S_EMAIL] [nvarchar](255) NULL,
	[SURNM] [nvarchar](255) NULL,
	[POSTAL_C] [nvarchar](255) NULL,
	[HPHONE_N] [nvarchar](255) NULL,
	[INSTITUTE] [nvarchar](255) NULL,
	[TEL_N] [nvarchar](255) NULL,
	[NICKNM] [nvarchar](255) NULL,
	[GENDER_C] [nvarchar](255) NULL,
	[ADDR] [nvarchar](255) NULL,
	[CTRY_C] [nvarchar](255) NULL,
	[GIVENNM] [nvarchar](255) NULL,
	[TITLE_C] [nvarchar](255) NULL,
	[PicId] [int] NULL,
	[ShowPrvInfo] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[P_UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[member_groups]    Script Date: 02/23/2012 15:34:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[member_groups](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[About] [nvarchar](255) NULL,
	[Url] [nvarchar](255) NULL,
	[OwnerId] [int] NOT NULL,
	[Visibility] [int] NULL,
	[IsAutoApprove] [int] NULL,
	[CanMembersInvite] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedOn] [datetime] NULL,
	[AvatarId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[system_log]    Script Date: 02/23/2012 15:34:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[system_log](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NOT NULL,
	[Level] [nvarchar](255) NOT NULL,
	[Logger] [nvarchar](255) NOT NULL,
	[Message] [text] NOT NULL,
	[Thread] [nvarchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[member_uploads]    Script Date: 02/23/2012 15:34:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[member_uploads](
	[UNIQ_N] [int] IDENTITY(1,1) NOT NULL,
	[SECTION_NO] [nvarchar](255) NULL,
	[RANK] [nvarchar](255) NULL,
	[FILE_CONTENT_T] [nvarchar](255) NULL,
	[FILE_SIZE] [int] NULL,
	[CUNIQ_N] [int] NULL,
	[CO_UNIQ_N] [int] NULL,
	[FILE_DETAIL] [varbinary](8000) NULL,
	[CONF_C] [int] NULL,
	[ATTACHMT] [nvarchar](255) NULL,
	[CREATE_D] [datetime] NULL,
	[P_UNIQ_N] [int] NULL,
	[P_UserId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[UNIQ_N] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[member_requests]    Script Date: 02/23/2012 15:34:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[member_requests](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Version] [datetime] NOT NULL,
	[Guid] [nvarchar](255) NULL,
	[CreateDate] [datetime] NULL,
	[Type] [int] NOT NULL,
	[Target_PageId] [int] NULL,
	[Status] [int] NULL,
	[SenderId] [int] NULL,
	[RecipientId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[member_messages]    Script Date: 02/23/2012 15:34:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[member_messages](
	[MessageId] [int] IDENTITY(1,1) NOT NULL,
	[IsPrivate] [int] NOT NULL,
	[ImageFilename] [nvarchar](255) NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[Type] [int] NOT NULL,
	[Source] [nvarchar](255) NOT NULL,
	[Text] [nvarchar](255) NOT NULL,
	[ThumbnailFilename] [nvarchar](255) NULL,
	[SenderId] [int] NULL,
	[RecipientId] [int] NULL,
	[ParentId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[MessageId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[member_invitations]    Script Date: 02/23/2012 15:34:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[member_invitations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Version] [datetime] NOT NULL,
	[Guid] [nvarchar](255) NULL,
	[Email] [nvarchar](255) NULL,
	[Became_User_Id] [int] NULL,
	[CreateDate] [datetime] NULL,
	[Type] [int] NOT NULL,
	[Target_PageId] [int] NULL,
	[Status] [int] NULL,
	[Senderid] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[member_groupmessages]    Script Date: 02/23/2012 15:34:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[member_groupmessages](
	[MessageId] [int] IDENTITY(1,1) NOT NULL,
	[ImageFilename] [nvarchar](255) NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[Source] [nvarchar](255) NOT NULL,
	[Type] [int] NOT NULL,
	[IsPrivate] [int] NOT NULL,
	[Text] [nvarchar](255) NOT NULL,
	[ThumbnailFilename] [nvarchar](255) NULL,
	[GroupId] [int] NULL,
	[SenderId] [int] NULL,
	[RecipientId] [int] NULL,
	[Parentid] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[MessageId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[member_groupmembers]    Script Date: 02/23/2012 15:34:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[member_groupmembers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NotifyOnPending] [int] NOT NULL,
	[Status] [int] NULL,
	[NotifyOnJoin] [int] NOT NULL,
	[NotifyOnReply] [int] NOT NULL,
	[NotifyOnMessage] [int] NOT NULL,
	[Role] [int] NOT NULL,
	[NotifyOnLeave] [int] NOT NULL,
	[UserId] [int] NULL,
	[GroupId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[member_friends]    Script Date: 02/23/2012 15:34:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[member_friends](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[IsFavorite] [int] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[UserId] [int] NULL,
	[FriendId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[member_blog]    Script Date: 02/23/2012 15:34:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[member_blog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Version] [datetime] NOT NULL,
	[BLOG_TEXT] [nvarchar](255) NULL,
	[P_UserId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[member_alert]    Script Date: 02/23/2012 15:34:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[member_alert](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Version] [datetime] NOT NULL,
	[IsHidden] [int] NULL,
	[Message] [nvarchar](255) NULL,
	[Alert_type_Id] [int] NULL,
	[P_UserId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[member_about]    Script Date: 02/23/2012 15:34:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[member_about](
	[P_UserId] [int] NOT NULL,
	[ABOUT_TEXT] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[P_UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[aspnet_roleactions]    Script Date: 02/23/2012 15:34:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[aspnet_roleactions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PermissionType] [int] NOT NULL,
	[RoleId] [int] NULL,
	[UserId] [int] NULL,
	[ActionId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[aspnet_applicationuserrole]    Script Date: 02/23/2012 15:34:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[aspnet_applicationuserrole](
	[ApplicationId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ApplicationId] ASC,
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[aspnet_applicationuser]    Script Date: 02/23/2012 15:34:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[aspnet_applicationuser](
	[ApplicationId] [int] NULL,
	[UserId] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[aspnet_applicationrole]    Script Date: 02/23/2012 15:34:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[aspnet_applicationrole](
	[ApplicationId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ApplicationId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  ForeignKey [FKC695F909241F7860]    Script Date: 02/23/2012 15:34:53 ******/
ALTER TABLE [dbo].[aspnet_applicationrole]  WITH CHECK ADD  CONSTRAINT [FKC695F909241F7860] FOREIGN KEY([RoleId])
REFERENCES [dbo].[aspnet_role] ([Id])
GO
ALTER TABLE [dbo].[aspnet_applicationrole] CHECK CONSTRAINT [FKC695F909241F7860]
GO
/****** Object:  ForeignKey [FKC695F9097DCA6BB2]    Script Date: 02/23/2012 15:34:53 ******/
ALTER TABLE [dbo].[aspnet_applicationrole]  WITH CHECK ADD  CONSTRAINT [FKC695F9097DCA6BB2] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[aspnet_application] ([Id])
GO
ALTER TABLE [dbo].[aspnet_applicationrole] CHECK CONSTRAINT [FKC695F9097DCA6BB2]
GO
/****** Object:  ForeignKey [FK_aspnet_applicationuser_aspnet_application]    Script Date: 02/23/2012 15:34:53 ******/
ALTER TABLE [dbo].[aspnet_applicationuser]  WITH CHECK ADD  CONSTRAINT [FK_aspnet_applicationuser_aspnet_application] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[aspnet_application] ([Id])
GO
ALTER TABLE [dbo].[aspnet_applicationuser] CHECK CONSTRAINT [FK_aspnet_applicationuser_aspnet_application]
GO
/****** Object:  ForeignKey [FK_aspnet_applicationuser_aspnet_membershipuser]    Script Date: 02/23/2012 15:34:53 ******/
ALTER TABLE [dbo].[aspnet_applicationuser]  WITH CHECK ADD  CONSTRAINT [FK_aspnet_applicationuser_aspnet_membershipuser] FOREIGN KEY([UserId])
REFERENCES [dbo].[aspnet_membershipuser] ([Id])
GO
ALTER TABLE [dbo].[aspnet_applicationuser] CHECK CONSTRAINT [FK_aspnet_applicationuser_aspnet_membershipuser]
GO
/****** Object:  ForeignKey [FKA0AA9674241F7860]    Script Date: 02/23/2012 15:34:53 ******/
ALTER TABLE [dbo].[aspnet_applicationuserrole]  WITH CHECK ADD  CONSTRAINT [FKA0AA9674241F7860] FOREIGN KEY([RoleId])
REFERENCES [dbo].[aspnet_role] ([Id])
GO
ALTER TABLE [dbo].[aspnet_applicationuserrole] CHECK CONSTRAINT [FKA0AA9674241F7860]
GO
/****** Object:  ForeignKey [FKA0AA96747DCA6BB2]    Script Date: 02/23/2012 15:34:53 ******/
ALTER TABLE [dbo].[aspnet_applicationuserrole]  WITH CHECK ADD  CONSTRAINT [FKA0AA96747DCA6BB2] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[aspnet_application] ([Id])
GO
ALTER TABLE [dbo].[aspnet_applicationuserrole] CHECK CONSTRAINT [FKA0AA96747DCA6BB2]
GO
/****** Object:  ForeignKey [FKA0AA9674C9869715]    Script Date: 02/23/2012 15:34:53 ******/
ALTER TABLE [dbo].[aspnet_applicationuserrole]  WITH CHECK ADD  CONSTRAINT [FKA0AA9674C9869715] FOREIGN KEY([UserId])
REFERENCES [dbo].[aspnet_membershipuser] ([Id])
GO
ALTER TABLE [dbo].[aspnet_applicationuserrole] CHECK CONSTRAINT [FKA0AA9674C9869715]
GO
/****** Object:  ForeignKey [FKC7565717241F7860]    Script Date: 02/23/2012 15:34:53 ******/
ALTER TABLE [dbo].[aspnet_roleactions]  WITH CHECK ADD  CONSTRAINT [FKC7565717241F7860] FOREIGN KEY([RoleId])
REFERENCES [dbo].[aspnet_role] ([Id])
GO
ALTER TABLE [dbo].[aspnet_roleactions] CHECK CONSTRAINT [FKC7565717241F7860]
GO
/****** Object:  ForeignKey [FKC7565717A47A53E4]    Script Date: 02/23/2012 15:34:53 ******/
ALTER TABLE [dbo].[aspnet_roleactions]  WITH CHECK ADD  CONSTRAINT [FKC7565717A47A53E4] FOREIGN KEY([ActionId])
REFERENCES [dbo].[aspnet_actions] ([Id])
GO
ALTER TABLE [dbo].[aspnet_roleactions] CHECK CONSTRAINT [FKC7565717A47A53E4]
GO
/****** Object:  ForeignKey [FKC7565717C9869715]    Script Date: 02/23/2012 15:34:53 ******/
ALTER TABLE [dbo].[aspnet_roleactions]  WITH CHECK ADD  CONSTRAINT [FKC7565717C9869715] FOREIGN KEY([UserId])
REFERENCES [dbo].[aspnet_membershipuser] ([Id])
GO
ALTER TABLE [dbo].[aspnet_roleactions] CHECK CONSTRAINT [FKC7565717C9869715]
GO
/****** Object:  ForeignKey [FK_member_about_member_details]    Script Date: 02/23/2012 15:34:53 ******/
ALTER TABLE [dbo].[member_about]  WITH CHECK ADD  CONSTRAINT [FK_member_about_member_details] FOREIGN KEY([P_UserId])
REFERENCES [dbo].[member_details] ([P_UserId])
GO
ALTER TABLE [dbo].[member_about] CHECK CONSTRAINT [FK_member_about_member_details]
GO
/****** Object:  ForeignKey [FKA61605F44EB2486E]    Script Date: 02/23/2012 15:34:53 ******/
ALTER TABLE [dbo].[member_alert]  WITH CHECK ADD  CONSTRAINT [FKA61605F44EB2486E] FOREIGN KEY([P_UserId])
REFERENCES [dbo].[member_details] ([P_UserId])
GO
ALTER TABLE [dbo].[member_alert] CHECK CONSTRAINT [FKA61605F44EB2486E]
GO
/****** Object:  ForeignKey [FKA61605F4799F0B5C]    Script Date: 02/23/2012 15:34:53 ******/
ALTER TABLE [dbo].[member_alert]  WITH CHECK ADD  CONSTRAINT [FKA61605F4799F0B5C] FOREIGN KEY([Alert_type_Id])
REFERENCES [dbo].[lookup_alerttype] ([Id])
GO
ALTER TABLE [dbo].[member_alert] CHECK CONSTRAINT [FKA61605F4799F0B5C]
GO
/****** Object:  ForeignKey [FK393EAD754EB2486E]    Script Date: 02/23/2012 15:34:53 ******/
ALTER TABLE [dbo].[member_blog]  WITH CHECK ADD  CONSTRAINT [FK393EAD754EB2486E] FOREIGN KEY([P_UserId])
REFERENCES [dbo].[member_details] ([P_UserId])
GO
ALTER TABLE [dbo].[member_blog] CHECK CONSTRAINT [FK393EAD754EB2486E]
GO
/****** Object:  ForeignKey [FKB7E2223013E07F59]    Script Date: 02/23/2012 15:34:53 ******/
ALTER TABLE [dbo].[member_friends]  WITH CHECK ADD  CONSTRAINT [FKB7E2223013E07F59] FOREIGN KEY([FriendId])
REFERENCES [dbo].[member_details] ([P_UserId])
GO
ALTER TABLE [dbo].[member_friends] CHECK CONSTRAINT [FKB7E2223013E07F59]
GO
/****** Object:  ForeignKey [FKB7E222308E834188]    Script Date: 02/23/2012 15:34:53 ******/
ALTER TABLE [dbo].[member_friends]  WITH CHECK ADD  CONSTRAINT [FKB7E222308E834188] FOREIGN KEY([UserId])
REFERENCES [dbo].[member_details] ([P_UserId])
GO
ALTER TABLE [dbo].[member_friends] CHECK CONSTRAINT [FKB7E222308E834188]
GO
/****** Object:  ForeignKey [FK1296620414F82871]    Script Date: 02/23/2012 15:34:53 ******/
ALTER TABLE [dbo].[member_groupmembers]  WITH CHECK ADD  CONSTRAINT [FK1296620414F82871] FOREIGN KEY([GroupId])
REFERENCES [dbo].[member_groups] ([Id])
GO
ALTER TABLE [dbo].[member_groupmembers] CHECK CONSTRAINT [FK1296620414F82871]
GO
/****** Object:  ForeignKey [FK129662048E834188]    Script Date: 02/23/2012 15:34:53 ******/
ALTER TABLE [dbo].[member_groupmembers]  WITH CHECK ADD  CONSTRAINT [FK129662048E834188] FOREIGN KEY([UserId])
REFERENCES [dbo].[member_details] ([P_UserId])
GO
ALTER TABLE [dbo].[member_groupmembers] CHECK CONSTRAINT [FK129662048E834188]
GO
/****** Object:  ForeignKey [FK66A7191C14F82871]    Script Date: 02/23/2012 15:34:53 ******/
ALTER TABLE [dbo].[member_groupmessages]  WITH CHECK ADD  CONSTRAINT [FK66A7191C14F82871] FOREIGN KEY([GroupId])
REFERENCES [dbo].[member_groups] ([Id])
GO
ALTER TABLE [dbo].[member_groupmessages] CHECK CONSTRAINT [FK66A7191C14F82871]
GO
/****** Object:  ForeignKey [FK66A7191C9595A232]    Script Date: 02/23/2012 15:34:53 ******/
ALTER TABLE [dbo].[member_groupmessages]  WITH CHECK ADD  CONSTRAINT [FK66A7191C9595A232] FOREIGN KEY([MessageId])
REFERENCES [dbo].[member_groupmessages] ([MessageId])
GO
ALTER TABLE [dbo].[member_groupmessages] CHECK CONSTRAINT [FK66A7191C9595A232]
GO
/****** Object:  ForeignKey [FK66A7191CAC2636CA]    Script Date: 02/23/2012 15:34:53 ******/
ALTER TABLE [dbo].[member_groupmessages]  WITH CHECK ADD  CONSTRAINT [FK66A7191CAC2636CA] FOREIGN KEY([Parentid])
REFERENCES [dbo].[member_groupmessages] ([MessageId])
GO
ALTER TABLE [dbo].[member_groupmessages] CHECK CONSTRAINT [FK66A7191CAC2636CA]
GO
/****** Object:  ForeignKey [FK66A7191CC91AD59E]    Script Date: 02/23/2012 15:34:53 ******/
ALTER TABLE [dbo].[member_groupmessages]  WITH CHECK ADD  CONSTRAINT [FK66A7191CC91AD59E] FOREIGN KEY([SenderId])
REFERENCES [dbo].[member_details] ([P_UserId])
GO
ALTER TABLE [dbo].[member_groupmessages] CHECK CONSTRAINT [FK66A7191CC91AD59E]
GO
/****** Object:  ForeignKey [FK66A7191CF2BC27A7]    Script Date: 02/23/2012 15:34:53 ******/
ALTER TABLE [dbo].[member_groupmessages]  WITH CHECK ADD  CONSTRAINT [FK66A7191CF2BC27A7] FOREIGN KEY([RecipientId])
REFERENCES [dbo].[member_details] ([P_UserId])
GO
ALTER TABLE [dbo].[member_groupmessages] CHECK CONSTRAINT [FK66A7191CF2BC27A7]
GO
/****** Object:  ForeignKey [FKB4A909E0C91AD59E]    Script Date: 02/23/2012 15:34:53 ******/
ALTER TABLE [dbo].[member_invitations]  WITH CHECK ADD  CONSTRAINT [FKB4A909E0C91AD59E] FOREIGN KEY([Senderid])
REFERENCES [dbo].[member_details] ([P_UserId])
GO
ALTER TABLE [dbo].[member_invitations] CHECK CONSTRAINT [FKB4A909E0C91AD59E]
GO
/****** Object:  ForeignKey [FK7DAAAE2717D74E71]    Script Date: 02/23/2012 15:34:53 ******/
ALTER TABLE [dbo].[member_messages]  WITH CHECK ADD  CONSTRAINT [FK7DAAAE2717D74E71] FOREIGN KEY([MessageId])
REFERENCES [dbo].[member_messages] ([MessageId])
GO
ALTER TABLE [dbo].[member_messages] CHECK CONSTRAINT [FK7DAAAE2717D74E71]
GO
/****** Object:  ForeignKey [FK7DAAAE272E64DA89]    Script Date: 02/23/2012 15:34:53 ******/
ALTER TABLE [dbo].[member_messages]  WITH CHECK ADD  CONSTRAINT [FK7DAAAE272E64DA89] FOREIGN KEY([ParentId])
REFERENCES [dbo].[member_messages] ([MessageId])
GO
ALTER TABLE [dbo].[member_messages] CHECK CONSTRAINT [FK7DAAAE272E64DA89]
GO
/****** Object:  ForeignKey [FK7DAAAE27C91AD59E]    Script Date: 02/23/2012 15:34:53 ******/
ALTER TABLE [dbo].[member_messages]  WITH CHECK ADD  CONSTRAINT [FK7DAAAE27C91AD59E] FOREIGN KEY([SenderId])
REFERENCES [dbo].[member_details] ([P_UserId])
GO
ALTER TABLE [dbo].[member_messages] CHECK CONSTRAINT [FK7DAAAE27C91AD59E]
GO
/****** Object:  ForeignKey [FK7DAAAE27F2BC27A7]    Script Date: 02/23/2012 15:34:53 ******/
ALTER TABLE [dbo].[member_messages]  WITH CHECK ADD  CONSTRAINT [FK7DAAAE27F2BC27A7] FOREIGN KEY([RecipientId])
REFERENCES [dbo].[member_details] ([P_UserId])
GO
ALTER TABLE [dbo].[member_messages] CHECK CONSTRAINT [FK7DAAAE27F2BC27A7]
GO
/****** Object:  ForeignKey [FK56EEF5EEC91AD59E]    Script Date: 02/23/2012 15:34:53 ******/
ALTER TABLE [dbo].[member_requests]  WITH CHECK ADD  CONSTRAINT [FK56EEF5EEC91AD59E] FOREIGN KEY([SenderId])
REFERENCES [dbo].[member_details] ([P_UserId])
GO
ALTER TABLE [dbo].[member_requests] CHECK CONSTRAINT [FK56EEF5EEC91AD59E]
GO
/****** Object:  ForeignKey [FK56EEF5EEF2BC27A7]    Script Date: 02/23/2012 15:34:53 ******/
ALTER TABLE [dbo].[member_requests]  WITH CHECK ADD  CONSTRAINT [FK56EEF5EEF2BC27A7] FOREIGN KEY([RecipientId])
REFERENCES [dbo].[member_details] ([P_UserId])
GO
ALTER TABLE [dbo].[member_requests] CHECK CONSTRAINT [FK56EEF5EEF2BC27A7]
GO
/****** Object:  ForeignKey [FK4DA9DE9C4EB2486E]    Script Date: 02/23/2012 15:34:53 ******/
ALTER TABLE [dbo].[member_uploads]  WITH CHECK ADD  CONSTRAINT [FK4DA9DE9C4EB2486E] FOREIGN KEY([P_UserId])
REFERENCES [dbo].[member_details] ([P_UserId])
GO
ALTER TABLE [dbo].[member_uploads] CHECK CONSTRAINT [FK4DA9DE9C4EB2486E]
GO
