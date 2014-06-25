CREATE TABLE `member_details` (
  `P_UserId` int(11) NOT NULL,
  `P_EMAIL` varchar(50) NOT NULL,
  `NICKNM` varchar(10) DEFAULT NULL,
  `SURNM` varchar(20) DEFAULT NULL,
  `GIVENNM` varchar(60) DEFAULT NULL,
  `TITLE_C` char(5) DEFAULT NULL,
  `GENDER_C` char(1) DEFAULT NULL,
  `S_EMAIL` varchar(50) DEFAULT NULL,
  `INSTITUTE` varchar(100) DEFAULT NULL,
  `ADDR` varchar(80) DEFAULT NULL,
  `POSTAL_C` varchar(20) DEFAULT NULL,
  `TEL_N` varchar(30) DEFAULT NULL,
  `HPHONE_N` varchar(30) DEFAULT NULL,
  `CTRY_C` char(3) DEFAULT NULL,
  `version` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `PicId` int(11) DEFAULT NULL,
  `ShowPrvInfo` int(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`P_UserId`),
  CONSTRAINT `fk_membr_partics_membershipuser` FOREIGN KEY (`P_UserId`) REFERENCES `aspnet_membershipuser` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;


CREATE TABLE `member_alert` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `P_UserId` int(11) DEFAULT NULL,
  `Alert_type_Id` int(11) DEFAULT NULL,
  `IsHidden` int(11) DEFAULT NULL,
  `Message` varchar(255) DEFAULT NULL,
  `Version` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  KEY `FK_Alert_Alerttype` (`Alert_type_Id`),
  KEY `FK_Alert_UserId` (`P_UserId`),
  CONSTRAINT `FK_Alert_UserId` FOREIGN KEY (`P_UserId`) REFERENCES `member_details` (`P_UserId`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8;



CREATE TABLE `member_messages` (
  `MessageId` int(11) NOT NULL AUTO_INCREMENT,
  `ParentId` int(11) DEFAULT NULL,
  `Text` varchar(250) NOT NULL,
  `ImageFilename` varchar(150) DEFAULT NULL,
  `ThumbnailFilename` varchar(150) DEFAULT NULL,
  `SenderId` int(11) NOT NULL,
  `RecipientId` int(11) DEFAULT NULL,
  `IsPrivate` bit(1) NOT NULL,
  `Type` int(11) NOT NULL,
  `Source` varchar(250) NOT NULL,
  `CreatedOn` datetime NOT NULL,
  `ModifiedOn` datetime NOT NULL,
  PRIMARY KEY (`MessageId`),
  KEY `FK_Msg_SenderId` (`SenderId`),
  KEY `FK_Msg_RecipientId` (`RecipientId`),
  CONSTRAINT `fk_msg_recipientid` FOREIGN KEY (`RecipientId`) REFERENCES `member_details` (`P_UserId`),
  CONSTRAINT `fk_msg_senderid` FOREIGN KEY (`SenderId`) REFERENCES `member_details` (`P_UserId`)
) ENGINE=InnoDB AUTO_INCREMENT=27 DEFAULT CHARSET=latin1;



CREATE TABLE `member_friends` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UserId` int(11) NOT NULL,
  `FriendId` int(11) NOT NULL,
  `IsFavorite` bit(1) NOT NULL,
  `CreatedOn` datetime NOT NULL,
  `ModifiedOn` datetime NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_Friend_Users` (`UserId`),
  KEY `fk_friend_friendId` (`FriendId`),
  CONSTRAINT `FK_Friend_Users` FOREIGN KEY (`UserId`) REFERENCES `member_details` (`P_UserId`),
  CONSTRAINT `fk_friend_friendId` FOREIGN KEY (`FriendId`) REFERENCES `member_details` (`P_UserId`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1;


CREATE TABLE `member_groups` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Url` varchar(50) DEFAULT NULL,
  `Name` varchar(100) DEFAULT NULL,
  `About` varchar(500) DEFAULT NULL,
  `Visibility` int(11) DEFAULT NULL,
  `OwnerId` int(11) NOT NULL,
  `IsAutoApprove` bit(1) DEFAULT NULL,
  `CanMembersInvite` bit(1) DEFAULT NULL,
  `CreatedOn` datetime DEFAULT NULL,
  `ModifiedOn` datetime DEFAULT NULL,
  `AvatarId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1;


CREATE TABLE `member_groupmembers` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UserId` int(11) NOT NULL,
  `GroupId` int(11) NOT NULL,
  `Role` int(11) NOT NULL,
  `Status` int(11) DEFAULT NULL,
  `NotifyOnMessage` bit(1) NOT NULL,
  `NotifyOnReply` bit(1) NOT NULL,
  `NotifyOnLeave` bit(1) NOT NULL,
  `NotifyOnJoin` bit(1) NOT NULL,
  `NotifyOnPending` bit(1) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_GrpMembers_Users` (`UserId`),
  KEY `FK_GrpMmbrs_Grp` (`GroupId`),
  CONSTRAINT `FK_GrpMembers_Users` FOREIGN KEY (`UserId`) REFERENCES `member_details` (`P_UserId`),
  CONSTRAINT `FK_GrpMmbrs_Grp` FOREIGN KEY (`GroupId`) REFERENCES `member_groups` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


CREATE TABLE `member_groupmessages` (
  `MessageId` int(11) NOT NULL AUTO_INCREMENT,
  `ParentId` int(11) DEFAULT NULL,
  `GroupId` int(11) NOT NULL,
  `Text` varchar(250) NOT NULL,
  `ImageFilename` varchar(150) DEFAULT NULL,
  `ThumbnailFilename` varchar(150) DEFAULT NULL,
  `SenderId` int(11) NOT NULL,
  `RecipientId` int(11) DEFAULT NULL,
  `IsPrivate` bit(1) NOT NULL,
  `Type` int(11) NOT NULL,
  `CreatedOn` datetime NOT NULL,
  `ModifiedOn` datetime NOT NULL,
  `Source` varchar(20) NOT NULL,
  PRIMARY KEY (`MessageId`),
  KEY `FK_GrpMsg_Grp` (`GroupId`),
  KEY `FK_GrpMsg_SenderId` (`SenderId`),
  KEY `FK_GrpMsg_RecipientId` (`RecipientId`),
  CONSTRAINT `fk_groupId` FOREIGN KEY (`GroupId`) REFERENCES `member_groups` (`Id`),
  CONSTRAINT `fk_recipientId` FOREIGN KEY (`RecipientId`) REFERENCES `member_details` (`P_UserId`),
  CONSTRAINT `fk_senderId` FOREIGN KEY (`SenderId`) REFERENCES `member_details` (`P_UserId`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=latin1;



CREATE TABLE `member_requests` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `SenderId` int(11) NOT NULL,
  `Guid` varchar(50) DEFAULT NULL,
  `RecipientId` int(11) DEFAULT NULL,
  `CreateDate` datetime DEFAULT NULL,
  `Version` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `Type` int(11) DEFAULT NULL,
  `Target_PageId` int(11) DEFAULT NULL,
  `Status` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`),
  KEY `fk_MemberRequest_SenderId` (`SenderId`),
  KEY `fk_member_request_RecipientId` (`RecipientId`),
  CONSTRAINT `fk_MemberRequest_SenderId` FOREIGN KEY (`SenderId`) REFERENCES `member_details` (`P_UserId`),
  CONSTRAINT `fk_member_request_RecipientId` FOREIGN KEY (`RecipientId`) REFERENCES `member_details` (`P_UserId`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;



CREATE TABLE `member_invitations` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `SenderId` int(11) NOT NULL,
  `Email` varchar(255) DEFAULT NULL,
  `Guid` varchar(50) DEFAULT NULL,
  `Became_User_Id` int(11) DEFAULT NULL,
  `CreateDate` datetime DEFAULT NULL,
  `Version` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `Type` int(11) DEFAULT NULL,
  `Target_PageId` int(11) DEFAULT NULL,
  `Status` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`),
  KEY `fk_MemberInvites_SenderId` (`SenderId`),
  CONSTRAINT `fk_MemberInvites_SenderId` FOREIGN KEY (`SenderId`) REFERENCES `member_details` (`P_UserId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;


CREATE TABLE `member_uploads` (
  `UNIQ_N` int(6) NOT NULL AUTO_INCREMENT,
  `P_UNIQ_N` int(6) DEFAULT NULL,
  `CONF_C` int(11) DEFAULT NULL,
  `SECTION_NO` char(3) DEFAULT NULL,
  `ATTACHMT` varchar(200) DEFAULT NULL,
  `RANK` varchar(10) DEFAULT NULL,
  `CUNIQ_N` int(11) DEFAULT NULL,
  `FILE_CONTENT_T` varchar(50) DEFAULT NULL,
  `FILE_SIZE` int(10) DEFAULT NULL,
  `FILE_DETAIL` longblob,
  `CREATE_D` datetime DEFAULT NULL,
  `CO_UNIQ_N` int(6) DEFAULT NULL,
  `P_UserId` int(11) NOT NULL,
  PRIMARY KEY (`UNIQ_N`),
  KEY `fk_attachment_owner` (`P_UserId`),
  CONSTRAINT `fk_attachment_owner` FOREIGN KEY (`P_UserId`) REFERENCES `member_details` (`P_UserId`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8;



CREATE TABLE `member_about` (
  `P_UserId` int(11) NOT NULL,
  `ABOUT_TEXT` text CHARACTER SET latin1,
  PRIMARY KEY (`P_UserId`),
  CONSTRAINT `PK_FK_PUserId_memberpartics` FOREIGN KEY (`P_UserId`) REFERENCES `member_details` (`P_UserId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;



CREATE TABLE `member_blog` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `P_UserId` int(11) NOT NULL,
  `BLOG_TEXT` text CHARACTER SET latin1 NOT NULL,
  `Version` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  KEY `PK_FK_PUserId_memberdetails` (`P_UserId`),
  CONSTRAINT `PK_FK_PUserId_memberdetails` FOREIGN KEY (`P_UserId`) REFERENCES `member_details` (`P_UserId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;


CREATE TABLE `lookup_alerttype` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(20) NOT NULL,
  `Template` varchar(255) DEFAULT NULL,
  `ParamCount` int(11) NOT NULL DEFAULT '0',
  `support_email` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;


CREATE TABLE `lookup_request_type` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;


CREATE TABLE `lookup_country` (
  `Id` varchar(10) NOT NULL,
  `Name` varchar(40) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


